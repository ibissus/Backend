using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static KompaniaPchor.Identity.IdentityConfiguration;

namespace KompaniaPchor.Identity
{
    public class RoleService
    {
        private UserManager<SystemUser> _userManager { get; }
        private RoleManager<IdentityRole> _roleManager { get; }
        public RoleService(UserManager<SystemUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Init()
        {
            var res1 = await _roleManager.CreateAsync(UserRole);
            if (res1.Succeeded)
            {
                var res11 = await _roleManager.AddClaimAsync(UserRole, UserRoleClaim);
                if (!res11.Succeeded) throw new Exception("Couldn't add user claim");
            }
            else throw new Exception("Couldn't create user role");

            var res2 = await _roleManager.CreateAsync(CompanyCommanderRole);
            if (res2.Succeeded)
            {
                var res22 = await _roleManager.AddClaimAsync(CompanyCommanderRole, CompanyCommanderRoleClaim);
                if (!res22.Succeeded) throw new Exception("Couldn't add user claim");
            }
            else throw new Exception("Couldn't create user role");

            var res3 = await _roleManager.CreateAsync(PlatoonCommanderRole);
            if (res3.Succeeded)
            {
                var res33 = await _roleManager.AddClaimAsync(PlatoonCommanderRole, PlatoonCommanderRoleClaim);
                if (!res33.Succeeded) throw new Exception("Couldn't add user claim");
            }
            else throw new Exception("Couldn't create user role");

            var res4 = await _roleManager.CreateAsync(AssistantRole);
            if (res4.Succeeded)
            {
                var res44 = await _roleManager.AddClaimAsync(AssistantRole, AssistantRoleClaim);
                if (!res44.Succeeded) throw new Exception("Couldn't add user claim");
            }
            else throw new Exception("Couldn't create user role");
        }

        /// <summary>Add user to Company Commander Role</summary>
        public async Task<bool> AddToCCRole(SystemUser user)
        {
            var result = await _userManager.AddToRoleAsync(user, CompanyCommanderRoleName);
            return Verify(result);
        }

        /// <summary>Removes user from Company Commander Role</summary>
        public async Task<bool> RemoveFromCCRole(SystemUser user)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, CompanyCommanderRoleName);
            return Verify(result);
        }

        /// <summary>Add user to Platoon Commander Role</summary>
        public async Task<bool> AddToPCRole(SystemUser user)
        {
            var result = await _userManager.AddToRoleAsync(user, PlatoonCommanderRoleName);
            return Verify(result);
        }

        /// <summary>Removes user from Platoon Commander Role</summary>
        public async Task<bool> RemoveFromPCRole(SystemUser user)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, PlatoonCommanderRoleName);
            return Verify(result);
        }

        /// <summary>Add user to Assistant Role</summary>
        public async Task<bool> AddToAssRole(SystemUser user)
        {
            var result = await _userManager.AddToRoleAsync(user, AssistantRoleName);
            return Verify(result);
        }

        /// <summary>Removes user from Assistant Role</summary>
        public async Task<bool> RemoveFromAssRole(SystemUser user)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, AssistantRoleName);
            return Verify(result);
        }

        public bool Verify(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                string exceptionMessage = "";
                foreach (var error in result.Errors) exceptionMessage += error.Description += Environment.NewLine;
                throw new Exception(exceptionMessage);
            }

            return true;
        }

    }
}
