using Microsoft.AspNetCore.Mvc.Rendering;
using WFEngine.Application.AuthorizationServer.Queries.GetUserEmailAndPassword;
using WFEngine.Domain.Authorization.Entities;

namespace WFEngine.Presentation.AuthorizationServer.ViewModels
{
    public class LoginViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
        public string SelectedTenantId { get; set; }
        public List<SelectListItem> Tenants { get; set; }

        public LoginViewModel()
        {
            Tenants = new List<SelectListItem>();
        }

        public void AddUser(User user)
        {
            Tenants = user
                 .Tenants
                 .Select(tenantUser => tenantUser.Tenant)
                 .Select(tenant => new SelectListItem
                 {
                     Text = tenant.TenantName,
                     Value = tenant.TenantId
                 })
                 .ToList();
        }

        public GetUserEmailAndPasswordQuery ToGetUserEmailAndPasswordQuery()
        {
            return new GetUserEmailAndPasswordQuery(Email, Password, RememberMe);
        }
    }
}
