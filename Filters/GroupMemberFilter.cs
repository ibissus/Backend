using KompaniaPchor.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace KompaniaPchor.Filters
{
    public class GroupMemberFilter : IAuthorizationFilter
    {
        private readonly GroupType _inputGroupType;
        public GroupMemberFilter(GroupType input)
        {
            _inputGroupType = input;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if(_inputGroupType == GroupType.Company)
            {
                ICompanyService _companyService = context.HttpContext.RequestServices.GetService<ICompanyService>();

                object companyName = context.RouteData.Values["companyId"] ?? context.HttpContext.Request.Query["companyId"];
                int companyId = int.Parse(companyName.ToString());

                var result = _companyService.IsUserAssignedToCompany(companyId, context.HttpContext.User.Identity.Name).GetAwaiter().GetResult();

                if(result == false && context.HttpContext.User.Identity.Name != "SuperUser")
                {
                    context.Result = new JsonResult("User is not a member of this group") { StatusCode = 403 };
                    return;
                }
            }
            else if (_inputGroupType == GroupType.Platoon)
            {
                ICompanyService _companyService = context.HttpContext.RequestServices.GetService<ICompanyService>();
                IPlatoonService _platoonService = context.HttpContext.RequestServices.GetService<IPlatoonService>();

                var companyId = int.Parse(context.HttpContext.Request.Query["companyId"].ToString());
                var platoonId = int.Parse(context.HttpContext.Request.Query["platoonId"].ToString());

                var result = _platoonService.IsUserAssignedToPlatoon(companyId, platoonId, context.HttpContext.User.Identity.Name).GetAwaiter().GetResult();

                if (result == false && context.HttpContext.User.Identity.Name != "SuperUser")
                {
                    context.Result = new JsonResult("User is not a member of this group") { StatusCode = 403 };
                    return;
                }
            }
            else
            {
                context.Result = new JsonResult(null) { StatusCode = 403 };
                return;
            }
        }
    }

    /// <summary>Authorize access only for specified group members</summary>
    public class GroupMemberAttribute : TypeFilterAttribute
    {
        public GroupMemberAttribute(GroupType groupType) : base(typeof(GroupMemberFilter))
        {
            Arguments = new object[] { groupType };
        }
    }

    public enum GroupType
    {
        Company, Platoon
    };
}
