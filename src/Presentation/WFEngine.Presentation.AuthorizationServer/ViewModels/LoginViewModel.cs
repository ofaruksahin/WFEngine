using System.ComponentModel.DataAnnotations;

namespace WFEngine.Presentation.AuthorizationServer.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "MustBeEmail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
