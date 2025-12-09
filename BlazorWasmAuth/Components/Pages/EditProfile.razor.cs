using Frontend.Identity.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Shared.DTOs.User;
using System.Net.Http.Json;
using System.Text.Json;

namespace Frontend.Components.Pages;

public partial class EditProfile
{
    private MudForm? _form;
    private MudFileUpload<IBrowserFile>? _fileUpload;
    private IBrowserFile? _selectedImage;
    private string? _profilePicturePreview = "https://cdn-icons-png.flaticon.com/512/847/847969.png";
    private FormResult _formResult = new();

    [Inject]
    private IHttpClientFactory _httpClientFactory { get; set; }

    private HttpClient _httpClient;

    private UserProfileDto _profile = new UserProfileDto();

    protected override async Task OnInitializedAsync()
    {
        _httpClient = _httpClientFactory.CreateClient("Auth");

        _profile = await GetUserProfile() ?? new UserProfileDto();

        _profilePicturePreview = GetThumbnailUrl();

        await base.OnInitializedAsync();
    }

    private string GetThumbnailUrl()
    {
        var backendUrl = Configuration["BackendUrl"];
        return $"{backendUrl}/uploads/users/{_profile.UserId}/profile.png";
    }

    private async Task<UserProfileDto?> GetUserProfile()
    {
        return await _httpClient.GetFromJsonAsync<UserProfileDto>($"api/profile");
    }

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
            content.Add(new StringContent(_profile?.DisplayName ?? string.Empty), "Username");
            content.Add(new StringContent(_profile?.Email ?? string.Empty), "Email");
            content.Add(new StringContent(_profile?.Bio ?? string.Empty), "Bio");
            content.Add(new StringContent(_profile?.Location ?? string.Empty), "Location");

            if (_selectedImage != null)
            {
                var stream = _selectedImage.OpenReadStream(2_097_152);
                var streamContent = new StreamContent(stream);
                streamContent.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue(_selectedImage.ContentType);
                content.Add(streamContent, "ProfileImage", _selectedImage.Name);
            }

            var resp = await _httpClient.PostAsync("api/profile/update", content);
            if (resp.IsSuccessStatusCode)
            {
                _formResult = new FormResult{ Succeeded = true};
                Snackbar.Add(@Localizer["ProfileUpdatedSuccessfully"], Severity.Success);
                return;
            }

            // body should contain details about why it failed
            var details = await resp.Content.ReadAsStringAsync();
            var problemDetails = JsonDocument.Parse(details);
            var errors = new List<string>();
            var errorList = problemDetails.RootElement.GetProperty("errors");

            foreach (var errorEntry in errorList.EnumerateObject())
            {
                if (errorEntry.Value.ValueKind == JsonValueKind.String)
                {
                    errors.Add(errorEntry.Value.GetString()!);
                }
                else if (errorEntry.Value.ValueKind == JsonValueKind.Array)
                {
                    errors.AddRange(
                        errorEntry.Value.EnumerateArray().Select(
                                e => e.GetString() ?? string.Empty)
                            .Where(e => !string.IsNullOrEmpty(e)));
                }
            }

            // return the error list
            _formResult= new FormResult
            {
                Succeeded = false,
                ErrorList = problemDetails == null ? ["An error occured."] : [.. errors]
            };

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
}