using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using WFEngine.Application.AuthorizationServer.Queries.GetUserClaimsForLogin;
using WFEngine.Application.Common.Models;
using WFEngine.Domain.Authorization.Entities;
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

            var getUserQuery = loginViewModel.ToGetUserEmailAndPasswordQuery();
            var getUserQueryResponse = await _mediator.Send(getUserQuery);

            if (button == "login")
            {
                if (!getUserQueryResponse.IsSuccess)
                {
                    MapModelState(getUserQueryResponse);
                    return View(new LoginViewModel());
                }

                loginViewModel.AddUser(getUserQueryResponse.Data);

                return View("~/Views/Authorization/SelectAccount.cshtml", loginViewModel);
            }

            if (!getUserQueryResponse.IsSuccess) throw new ClaimsNotFoundException();

            var getUserClaimsQuery = new GetUserClaimsForLoginQuery(loginViewModel.Email, loginViewModel.Password, loginViewModel.SelectedTenantId);
            var getUserClaimsResponse = await _mediator.Send(getUserClaimsQuery);

            if (!getUserClaimsResponse.IsSuccess) throw new ClaimsNotFoundException();

            return await SignIn(getUserClaimsResponse.Data);
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }

        #region Private Methods

        private void MapModelState<T>(ApiResponse<T> apiResponse)
            where T : class
        {
            foreach (var message in apiResponse.Messages)
            {
                ModelState.AddModelError(string.Empty, message);
            }
        }

        private async Task<IActionResult> SignIn(List<UserClaim> userClaims)
        {
            var request = HttpContext.GetOpenIddictServerRequest();

            var identity = new ClaimsIdentity(
                authenticationType: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                nameType: Claims.Name,
                roleType: Claims.Role);

            userClaims.ForEach(claim =>
            {
                if (claim.IsAddToken)
                {
                    identity.AddClaim(new Claim(claim.Name, claim.Value));
                }
            });

            identity.SetScopes(request.GetScopes());
            identity.SetResources(await _scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());
            identity.SetDestinations(claim => new[] { Destinations.AccessToken, Destinations.IdentityToken });

            return new SignInResult(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties());
        }

        #endregion
    }
}