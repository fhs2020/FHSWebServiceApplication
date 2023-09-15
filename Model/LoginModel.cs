using System.ComponentModel.DataAnnotations;

namespace FHSWebServiceApplication.Model
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
