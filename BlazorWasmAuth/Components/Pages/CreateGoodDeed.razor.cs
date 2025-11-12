using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.DTOs.User;
using System.Net.Http;
using System.Net.Http.Json;

namespace Frontend.Components.Pages;

public partial class CreateGoodDeed
{
    private MudForm? _form;
    private bool _isSubmitting = false;
    private GoodDeedModel _model = new();
    private HttpClient _httpClient;

    [Inject]
    private IHttpClientFactory _httpClientFactory { get; set; }

    [Inject]
    private NavigationManager _navigationManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _httpClient = _httpClientFactory.CreateClient("Auth");

        await base.OnInitializedAsync();
    }

    private async Task SubmitAsync()
    {
        if (_form is null)
        {
            return;
        }

        await _form.Validate();

        if (!_form.IsValid)
        {
            Snackbar.Add("Please fill in all required fields.", Severity.Warning);
            return;
        }

        try
        {
            _isSubmitting = true;

            var response = await _httpClient.PostAsJsonAsync("api/posts", _model);

            if (response.IsSuccessStatusCode)
            {
                Snackbar.Add("Thank you for your good deed 💖", Severity.Success);
                ResetForm();
                _navigationManager.NavigateTo("gooddeeds");
            }
            else
            {
                Snackbar.Add("Error posting your good deed.", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error: {ex.Message}", Severity.Error);
        }
        finally
        {
            _isSubmitting = false;
        }
    }

    private void ResetForm()
    {
        _model = new GoodDeedModel();
        _form?.ResetValidation();
    }

    public class GoodDeedModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}