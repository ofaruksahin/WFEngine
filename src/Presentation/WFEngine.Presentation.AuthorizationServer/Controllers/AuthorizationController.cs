using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using WFEngine.Application.Common.Models;
using WFEngine.Presentation.AuthorizationServer.Exceptions;
using WFEngine.Presentation.AuthorizationServer.Extensions;
using WFEngine.Presentation.AuthorizationServer.ViewModels;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace WFEngine.Presentation.AuthorizationServer.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IOpenIddictScopeManager _scopeManager;
        public AuthorizationController(
            IMediator mediator,
            IOpenIddictScopeManager scopeManager)
        {
            _mediator = mediator;
            _scopeManager = scopeManager;
        }

        [HttpGet, Route("authorize")]
        public async Task<IActionResult> Index()
        {
            var request = HttpContext.GetOpenIddictServerRequest();

            if (request.IsClientCredentialsGrantType())
            {
                throw new UnsupportedAuthorizationFlowException();
            }
            else if (request.IsAuthorizationCodeFlow())
            {
                return View(new LoginViewModel());
            }
            else
            {
                throw new UnsupportedAuthorizationFlowException();
            }
        }

        [HttpPost, Route("authorize")]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel, string button)
        {
            ModelState.Clear();

            var query = loginViewModel.ToGetUserEmailAndPasswordQuery();
            var response = await _mediator.Send(query);

            if (button == "login")
            {
                if (!response.IsSuccess)
                {
                    MapModelState(response);
                    return View(new LoginViewModel());
                }

                loginViewModel.AddUser(response.Data);

                return View("~/Views/Authorization/SelectAccount.cshtml", loginViewModel);
            }

            var selectedTenant = response
                .Data
                .Tenants
                .Where(f => f.TenantId == loginViewModel.SelectedTenantId)
                .Select(f => f.Tenant)
                .FirstOrDefault();

            var request = HttpContext.GetOpenIddictServerRequest();

            var identity = new ClaimsIdentity(
                authenticationType: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                nameType: Claims.Name,
                roleType: Claims.Role);

            identity.AddClaim(new Claim(Claims.Subject, response.Data.Id.ToString()));
            identity.AddClaim(new Claim(Claims.Email, response.Data.Email));
            identity.AddClaim(new Claim("tenant_id", selectedTenant.TenantId));
            identity.SetScopes(request.GetScopes());
            identity.SetResources(await _scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());
            identity.SetDestinations(claim => new[] { Destinations.AccessToken, Destinations.IdentityToken});

            return new SignInResult(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties());
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }

        private void MapModelState<T>(ApiResponse<T> apiResponse)
            where T : class
        {
            foreach (var message in apiResponse.Messages)
            {
                ModelState.AddModelError(string.Empty, message);
            }
        }
    }
}