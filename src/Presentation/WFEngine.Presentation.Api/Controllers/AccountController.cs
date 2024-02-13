using MediatR;
using Microsoft.AspNetCore.Mvc;
using WFEngine.Application.AuthorizationServer.Commands.Register;

namespace WFEngine.Presentation.Api.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            var response = await _mediator.Send(command);
            return Response(response);
        }
    }
}
