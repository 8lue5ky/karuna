using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Frontend.Components.Pages;

public partial class ContactCard
{
    [Parameter] 
    public string Title { get; set; } = "";
    
    [Parameter] 
    public string Text { get; set; } = "";
    
    [Parameter] 
    public string LinkText { get; set; } = "";
    
    [Parameter] 
    public string Link { get; set; } = "";
    
    [Parameter] 
    public string Icon { get; set; } = Icons.Material.Filled.Info;
    
    [Parameter] 
    public Color Color { get; set; } = MudBlazor.Color.Primary;

    [Parameter]
    public string? Class { get; set; }

    private string GetColorClass(Color color)
    {
        return color switch
        {
            Color.Primary => "var(--mud-palette-primary)",
            Color.Secondary => "var(--mud-palette-secondary)",
            Color.Tertiary => "var(--mud-palette-tertiary)",
            Color.Info => "var(--mud-palette-info)",
            Color.Success => "var(--mud-palette-success)",
            Color.Warning => "var(--mud-palette-warning)",
            Color.Error => "var(--mud-palette-error)",
            Color.Dark => "var(--mud-palette-dark)",
            _ => "var(--mud-palette-primary)"
        };
    }
}