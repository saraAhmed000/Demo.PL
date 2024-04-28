using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class RegisterVieModel
    {
        public string FName { get; set; }
        public string LName { get; set; }


        [Required(ErrorMessage ="Eamil is Required !!")]
        [EmailAddress (ErrorMessage ="Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required (ErrorMessage ="Confirm password is required")]
        [Compare ("Password", ErrorMessage ="confirm password does not match Password")]
        [DataType(DataType.Password)]

        public string ConfirmPassword { get; set; }

        public bool IsAgree { get; set; }   

    }
}
