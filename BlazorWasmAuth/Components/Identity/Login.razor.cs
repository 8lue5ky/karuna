using System.ComponentModel.DataAnnotations;
using Frontend.Identity.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Frontend.Components.Identity;

public partial class Login
{
    private FormResult _formResult = new();

    [SupplyParameterFromForm] private InputModel Input { get; set; } = new();

    [SupplyParameterFromQuery] private string? ReturnUrl { get; set; }

    public async Task LoginUser()
    {
        _formResult = await Acct.LoginAsync(Input.Email, Input.Password);

        if (_formResult.Succeeded)
        {
            Snackbar.Add("Login successful", Severity.Success);

            if (!string.IsNullOrEmpty(ReturnUrl))
                Navigation.NavigateTo(ReturnUrl);
            else
                Navigation.NavigateTo("/");
        }
        else
        {
            Snackbar.Add("Login failed", Severity.Error);
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
    }
}