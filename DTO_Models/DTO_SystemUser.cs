using KompaniaPchor.Identity;
using KompaniaPchor.ORM_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KompaniaPchor.DTO_Models
{
    public class DTO_SystemUser
    {
        /// <summary>Corresponds to Zolnierz.IdOsoby</summary>
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public ICollection<string> RoleNames { get; set; }
        public Zolnierz Soldier { get; set; }
        public DTO_SystemUser(SystemUser systemUser, string token, ICollection<string> roles, Zolnierz soldier)
        {
            UserId = systemUser.IdOsoby;
            UserName = systemUser.UserName;
            Token = token;
            RoleNames = roles;
            Soldier = soldier;
        }
    }
}
