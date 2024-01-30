using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using WFEngine.Application.AuthorizationServer.Queries.GetUserClaimsForAuthorizationCodeFlow;
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

            if (!request.IsAuthorizationCodeFlow())
                throw new UnsupportedAuthorizationFlowException();

            return View(new LoginViewModel());
        }

        [HttpPost, Route("token")]
        public async Task<IActionResult> Token()
        {
            var request = HttpContext.GetOpenIddictServerRequest();

            if (request.IsAuthorizationCodeGrantType() || request.IsRefreshTokenGrantType())
            {
                var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                var identity = new ClaimsIdentity(result.Principal.Claims,
                     authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                     nameType: Claims.Name,
                     roleType: Claims.Role);

                identity.SetScopes(request.GetScopes());
                identity.SetResources(await _scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());
                identity.SetDestinations(claim => new[] { Destinations.AccessToken, Destinations.IdentityToken });

                return new SignInResult(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties());
            }
            else if (request.IsClientCredentialsGrantType())
            {
                return View();
            }

            throw new UnsupportedAuthorizationFlowException();
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

            var getUserClaimsQuery = new GetUserClaimsForAuthorizationCodeFlowQuery(loginViewModel.Email, loginViewModel.Password, loginViewModel.SelectedTenantId);
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