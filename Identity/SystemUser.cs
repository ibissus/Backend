using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KompaniaPchor.Identity
{
    public class SystemUser : IdentityUser
    {
        public int? IdOsoby { get; set; }
        public string FirebaseToken { get; set; } = null;
    }
}
