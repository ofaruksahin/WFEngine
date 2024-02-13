using MediatR;
using Microsoft.AspNetCore.Mvc;
using WFEngine.Application.Common;

namespace WFEngine.Presentation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected IMediator _mediator;

        protected BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected IActionResult Response(BaseResponse response)
        {
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
