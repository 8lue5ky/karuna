using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Frontend.Components.Pages;

public partial class EditProfile
{
    private MudForm? _form;
    private MudFileUpload<IBrowserFile>? _fileUpload;
    private IBrowserFile? _selectedImage;
    private string? _profilePicturePreview = "https://cdn-icons-png.flaticon.com/512/847/847969.png";

    private UserProfile _profile = new()
    {
        Username = "GoodDeedHero",
        Email = "hero@example.com",
        Bio = "I love to help others.",
        Location = "Berlin, Germany",
        Gender = "Diverse"
    };

    private async Task OpenFilePicker()
    {
        if (_fileUpload == null)
        {
            Snackbar.Add("Upload not initialized.", Severity.Warning);
            return;
        }

        await _fileUpload.OpenFilePickerAsync();
    }

    private async Task ClearSelectedImage()
    {
        _selectedImage = null;
        _profilePicturePreview = "https://cdn-icons-png.flaticon.com/512/847/847969.png";

        if (_fileUpload != null)
        {
            await _fileUpload.ClearAsync();
        }

        StateHasChanged();
    }

    private async Task OnFileChanged(IBrowserFile? file)
    {
        if (file == null)
        {
            return;
        }

        _selectedImage = file;

        using var ms = new MemoryStream();
        await file.OpenReadStream(2_097_152).CopyToAsync(ms);
        var bytes = ms.ToArray();
        _profilePicturePreview = $"data:{file.ContentType};base64,{Convert.ToBase64String(bytes)}";

        StateHasChanged();
    }

    private async Task OnSaveClicked()
    {
        if (_form == null)
        {
            Snackbar.Add("Formular not initialized.", Severity.Error);
            return;
        }

        await _form.Validate();

        if (!_form.IsValid)
        {
            Snackbar.Add("Please check your entries.", Severity.Warning);
            return;
        }

        try
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(_profile.Username ?? string.Empty), "Username");
            content.Add(new StringContent(_profile.Email ?? string.Empty), "Email");
            content.Add(new StringContent(_profile.Bio ?? string.Empty), "Bio");
            content.Add(new StringContent(_profile.Location ?? string.Empty), "Location");
            content.Add(new StringContent(_profile.Gender ?? string.Empty), "Gender");

            if (_selectedImage != null)
            {
                var stream = _selectedImage.OpenReadStream(2_097_152);
                var streamContent = new StreamContent(stream);
                streamContent.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue(_selectedImage.ContentType);
                content.Add(streamContent, "ProfileImage", _selectedImage.Name);
            }

            var resp = await Http.PostAsync("api/profile/update", content);
            if (resp.IsSuccessStatusCode)
            {
                Snackbar.Add("Profile successfully updated!", Severity.Success);

                if (_form != null)
                {
                    await _form.ResetAsync();
                }
            }
            else
            {
                var txt = await resp.Content.ReadAsStringAsync();
                Snackbar.Add($"Error during save: {resp.StatusCode} — {txt}", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Unexpected error: {ex.Message}", Severity.Error);
        }
    }

    private async Task OnCancelClicked()
    {
        if (_form != null)
        {
            await _form.ResetAsync();
        }

        Snackbar.Add("Changes discarded.", Severity.Info);
    }

    public class UserProfile
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
    }
}