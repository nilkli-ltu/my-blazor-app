using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Client
{
    public class UserModel
    {
        [Required]
        public string UserName { get; set; }
    }
}
