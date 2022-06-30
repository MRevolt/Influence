using Cosmetic.Model.Enums.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Influence.UserApi.Controllers
{

    [Authorize(Policy = PolicyEnum.UserPolicy)]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
    }
}
