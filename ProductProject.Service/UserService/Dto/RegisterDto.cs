using System.ComponentModel.DataAnnotations;

namespace ProductProject.Service.UserService.Dto;

public class RegisterDto
{
    [Required]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }


    [Required]
    public string Password { get; set; }

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Repeating the password is different from the password.")]
    public string ConfirmPassword { get; set; }
}