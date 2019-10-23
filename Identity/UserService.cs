using KompaniaPchor.DTO_Models;
using KompaniaPchor.ORM_Models;
using KompaniaPchor.Repositories;
using KompaniaPchor.Services;
using KompaniaPchor.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static KompaniaPchor.Identity.IdentityConfiguration;

namespace KompaniaPchor.Identity
{
    public class UserService
    {
        private UserManager<SystemUser> _userManager { get; }
        IPasswordHasher<SystemUser> _passwordHasher { get; }
        private SignInManager<SystemUser> _signInManager { get; }
        private GenericRepo<Zolnierz> _pchorRepo { get; }

        private ISoldierService _soldierService { get; }

        private IConfiguration _configuration { get; }

        private IFirebaseService _firebaseService { get; }

        public UserService(UserManager<SystemUser> userManager, IPasswordHasher<SystemUser> passwordHasher,IConfiguration configuration,
            SignInManager<SystemUser> signInManager, GenericRepo<Zolnierz> pchorRepo, ISoldierService soldierService, IFirebaseService firebaseService)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _signInManager = signInManager;
            _pchorRepo = pchorRepo;
            _soldierService = soldierService;
            _configuration = configuration;
            _firebaseService = firebaseService;
        }

        public async Task Init()
        {
            await CreateSU();

            /*var pchors = await _pchorRepo.Get().AsNoTracking().ToListAsync();
            pchors.ForEach(async p => await CreateAccountForSoldier(p));*/
        }

        public async Task<bool> CreateSU()
        {
            var newUser = new SystemUser
            {
                //Id = user.Id.ToString(),
                UserName = "SuperUser",
                //Email = user.Email
            };

            var created = await _userManager.CreateAsync(newUser, "aaa");

            if (!created.Succeeded)
            {
                string exceptionMessage = "";
                foreach (var error in created.Errors) exceptionMessage += error.Description += Environment.NewLine;
                throw new Exception(exceptionMessage);
            }

            //var newUser = await _userManager.FindByNameAsync("SuperUser");

            await _userManager.AddToRoleAsync(newUser, UserRoleName);
            await _userManager.AddToRoleAsync(newUser, CompanyCommanderRoleName);
            await _userManager.AddToRoleAsync(newUser, PlatoonCommanderRoleName);
            await _userManager.AddToRoleAsync(newUser, AssistantRoleName);

            return true;
        }

        public async Task<bool> CreateAccountForSoldier(Zolnierz zolnierz, string login = null, string password = null)
        {
            var newUser = new SystemUser
            {
                //Id = user.Id.ToString(),
                IdOsoby = zolnierz.IdOsoby,
                UserName = login ?? zolnierz.Imie.ToLower().Substring(0, 1) + zolnierz.Nazwisko.ToLower(),
                //Email = user.Email
            };

            var created = await _userManager.CreateAsync(newUser, password ?? "aaa");

            if (!created.Succeeded)
            {
                string exceptionMessage = "";
                //foreach (var error in created.Errors) exceptionMessage += error.Description += Environment.NewLine;
                foreach (var e in created.Errors) exceptionMessage += e.Description;
                throw new Exception(exceptionMessage);
            }

            var assign = await _userManager.AddToRoleAsync(newUser, UserRoleName);

            if (!assign.Succeeded)
            {
                string exceptionMessage = "";
                //foreach (var error in created.Errors) exceptionMessage += error.Description += Environment.NewLine;
                foreach (var e in assign.Errors) exceptionMessage += e.Description;
                throw new Exception(exceptionMessage);
            }

            return true;
        }

        public async Task<DTO_SystemUser> SingInAsync(string login, string password, string firebaseToken)
        {
            var user = await _userManager.FindByNameAsync(login);

            if (user != null)
            {
                await _signInManager.SignOutAsync();

                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if (!string.IsNullOrWhiteSpace(firebaseToken)){ await UpdateFirebaseToken(user, firebaseToken); }

                var ret = await AuthWithJWTAsync(user);

                if (result.Succeeded) return ret;
            }

            return null;
        }

        private async Task UpdateFirebaseToken(SystemUser user, string firebaseToken)
        {
            if(user.FirebaseToken != firebaseToken)
            {
                await _firebaseService.UpdateUserToken((int)user.IdOsoby, user.FirebaseToken, firebaseToken);

                user.FirebaseToken = firebaseToken;
                await _userManager.UpdateAsync(user);
            }
        }

        private async Task<DTO_SystemUser> AuthWithJWTAsync(SystemUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            byte[] key = _configuration == null
                ? Encoding.ASCII.GetBytes("54789952794d58a96eefa6789425ee6e7ff12a35bbc6bb5c1469895120adc255acb8bc334d8ee9z2e9512eeef5f421ff218afac123579aa1549eefecb55ab218")// Testing
                : Encoding.ASCII.GetBytes(_configuration["JWTsettings:Secret"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("User")) { claims.Add(new Claim(ClaimTypes.Role, UserRoleName)); }
            if (roles.Contains("CompanyCommander")) { claims.Add(new Claim(ClaimTypes.Role, CompanyCommanderRoleName)); }
            if (roles.Contains("PlatoonCommander")) { claims.Add(new Claim(ClaimTypes.Role, PlatoonCommanderRoleName)); }
            if (roles.Contains("Assistant")) { claims.Add(new Claim(ClaimTypes.Role, AssistantRoleName)); }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var soldier = await _soldierService.GetSoldierInfoAsync(user.IdOsoby);

            return new DTO_SystemUser(user, tokenHandler.WriteToken(token), roles, soldier);
        }
    }
}
