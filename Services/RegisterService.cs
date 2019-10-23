using KompaniaPchor.DTO_Models;
using KompaniaPchor.Identity;
using KompaniaPchor.ORM_Models;
using KompaniaPchor.Repositories;
using KompaniaPchor.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KompaniaPchor.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly UserManager<SystemUser> _userManager;
        private readonly GenericRepo<Zolnierz> _zolnierzRepo;
        private readonly UserService _userService;

        public RegisterService(UserManager<SystemUser> userManager, GenericRepo<Zolnierz> zolnierzRepo,
            UserService userService)
        {
            _userManager = userManager;
            _zolnierzRepo = zolnierzRepo;
            _userService = userService;
        }
        public async Task<string> RegisterNewUser(DTO_RegisterForm registerForm)
        {
            var soldier = new Zolnierz(registerForm);
            var login = soldier.Imie.ToLower().Substring(0, 1) + soldier.Nazwisko.ToLower();

            _zolnierzRepo.Add(soldier);
            await _zolnierzRepo.SaveAsync();

            var user = await _userManager.FindByNameAsync(login);

            string tempLogin = login;

            if (user != null)
            {
                SystemUser tempUser;
                int i = 0;
                do
                {
                    i++;
                    tempLogin = login + i.ToString("00");
                    tempUser = await _userManager.FindByNameAsync(tempLogin);
                } while (tempUser != null);
            }

            await _userService.CreateAccountForSoldier(soldier, tempLogin, registerForm.Password);

            return tempLogin;
        }
    }
}
