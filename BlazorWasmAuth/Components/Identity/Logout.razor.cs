using Microsoft.AspNetCore.Components.Authorization;

namespace Frontend.Components.Identity;

public partial class Logout
{
    private AuthorizeView? _authView;

    protected override async Task OnInitializedAsync()
    {
        if (await Acct.CheckAuthenticatedAsync())
        {
            await Acct.LogoutAsync();
        }
    }
}