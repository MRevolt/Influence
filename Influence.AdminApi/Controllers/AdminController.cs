using Cosmetic.Model.Enums.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Influence.AdminApi.Controllers
{

    [Authorize(Policy = PolicyEnum.AdminPolicy)]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
    }
}
