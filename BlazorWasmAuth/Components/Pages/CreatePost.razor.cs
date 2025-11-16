using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.DTOs.User;
using System.Net.Http;
using System.Net.Http.Json;

namespace Frontend.Components.Pages;

public partial class CreatePost
{
    private MudForm? _form;
    private bool _isSubmitting = false;
    private PostModel _model = new();

    [Inject]
    private PostsServiceClient _serviceClient { get; set; }

    [Inject]
    private NavigationManager _navigationManager { get; set; }

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

            var response = await _serviceClient.CreatePostAsync(_model);

            if (response.IsSuccessStatusCode)
            {
                Snackbar.Add("Thank you for your good deed 💖", Severity.Success);
                ResetForm();
                _navigationManager.NavigateTo("posts");
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
        _model = new PostModel();
        _form?.ResetValidation();
    }

    public class PostModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}