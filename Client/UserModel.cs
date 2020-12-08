using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Client
{
    public class UserModel
    {
        [Required(ErrorMessage = "Användare måste anges")]
        public string UserName { get; set; }
    }
}
