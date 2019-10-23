using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KompaniaPchor.Identity
{
    public class IdentityConfiguration
    {
        public const string UserRoleName = "User";
        public const string CompanyCommanderRoleName = "CompanyCommander";
        public const string PlatoonCommanderRoleName = "PlatoonCommander";
        public const string AssistantRoleName = "Assistant";

        public const string UserPolicyName = "UserPolicyName";
        public const string CompanyCommanderPolicyName = "CompanyCommanderPolicyName";
        public const string PlatoonCommanderPolicyName = "PlatoonCommanderPolicyName";
        public const string AssistantPolicyName = "AssistantPolicyName";


        public static IdentityRole UserRole = new IdentityRole
        {
            Name = UserRoleName
        };

        public static IdentityRole CompanyCommanderRole = new IdentityRole
        {
            Name = CompanyCommanderRoleName
        };

        public static IdentityRole PlatoonCommanderRole = new IdentityRole
        {
            Name = PlatoonCommanderRoleName
        };

        public static IdentityRole AssistantRole = new IdentityRole
        {
            Name = AssistantRoleName
        };

        public static Claim UserRoleClaim = new Claim(ClaimTypes.Role, UserRoleName);
        public static Claim CompanyCommanderRoleClaim = new Claim(ClaimTypes.Role, CompanyCommanderRoleName);
        public static Claim PlatoonCommanderRoleClaim = new Claim(ClaimTypes.Role, PlatoonCommanderRoleName);
        public static Claim AssistantRoleClaim = new Claim(ClaimTypes.Role, AssistantRoleName);

    }
}
