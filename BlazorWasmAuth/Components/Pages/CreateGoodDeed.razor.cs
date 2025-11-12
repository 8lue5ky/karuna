using System.Net.Http.Json;
using MudBlazor;

namespace Frontend.Components.Pages;

public partial class CreateGoodDeed
{
    private MudForm? _form;
    private bool _isSubmitting = false;
    private GoodDeedModel _model = new();

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

            var response = await Http.PostAsJsonAsync("api/posts", _model);

            if (response.IsSuccessStatusCode)
            {
                Snackbar.Add("Thank you for your good deed 💖", Severity.Success);
                ResetForm();
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