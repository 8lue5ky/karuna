using System.ComponentModel.DataAnnotations;
using Frontend.Identity.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Frontend.Components.Identity;

public partial class Register
{
    private FormResult _formResult = new();

    [SupplyParameterFromForm] private InputModel Input { get; set; } = new();

    public async Task RegisterUserAsync()
    {
        _formResult = await Acct.RegisterAsync(Input.Email, Input.Password);

        if (_formResult.Succeeded)
        {
            Snackbar.Add("Registration successful!", Severity.Success);
        }
        else
        {
            Snackbar.Add("Registration failed.", Severity.Error);
        }
    }

    private sealed class InputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}