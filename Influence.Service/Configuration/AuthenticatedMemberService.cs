using Microsoft.AspNetCore.Http;

namespace Influence.Service.Configuration
{
    public class AuthenticatedMemberService : IAuthenticatedMemberService
    {
        public AuthenticatedMemberService(IHttpContextAccessor httpContextAccessor)
        {
            string customervalue = httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "MemberId")?.Value;
            string rolevalue = httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "RoleId")?.Value;
            string unitvalue = httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "UnitId")?.Value;
            string companyvalue = httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "CompanyId")?.Value;
         
           UserId = GetIntClaim(customervalue);
           RoleId = GetIntClaim(rolevalue);
           UnitId = GetIntClaim(unitvalue);
           CompanyId = GetIntClaim(companyvalue);
            
          
        }

        public int? UserId { get; }
        public int? RoleId { get;  }
        public bool FullControl { get; set; }
        public int? UnitId { get;  }
        public int? CompanyId { get;  }

        public string Username { get; }
        public void SetFullControl(bool fullControl,bool superAdmin)
        {
            this.FullControl = fullControl;
            this.SuperAdmin = superAdmin;
        }
        public bool SuperAdmin { get; set; }
        private int? GetIntClaim(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                int b = 0;
                int.TryParse(value, out b);
                if (b != 0)
                    return b;
            }

            return null;
        }

    }
 
    public interface IAuthenticatedMemberService
    {
        int? UserId { get; }
        int? RoleId { get; }
        public string Username { get; }
        int? UnitId { get; }
        int? CompanyId { get; }
        public bool FullControl { get;  }
        public bool SuperAdmin { get; set; }
        void SetFullControl(bool fullControl,bool superAdmin);
    }
}
