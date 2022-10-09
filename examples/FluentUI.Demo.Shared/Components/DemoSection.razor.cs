﻿using System.Reflection.Metadata;
using Microsoft.AspNetCore.Components;


namespace FluentUI.Demo.Shared.Components;
public partial class DemoSection : ComponentBase
{
    [Inject]
    private HttpClient Http { get; set; } = default!;

    [Parameter, EditorRequired]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public RenderFragment? Description { get; set; }

    /// <summary>
    /// The component for wich the example will be shown. Enter the name only. '.razor' will be added internally
    /// </summary>
    [Parameter, EditorRequired]
    public Type Component { get; set; } = default!;

    ///<summary>
    /// Any parameters that need to be supplied to the component
    /// </summary>
    [Parameter]
    public Dictionary<string, object>? ComponentParameters { get; set; }

    /// <summary>
    /// Any collocated (isolated) .cs, .css or .js files. Enter the extensions only
    /// Example: @(new[] { "css", "js" })
    /// </summary>
    [Parameter]
    public string[]? CollocatedFiles { get; set; }

    [Parameter]
    public bool IsNew { get; set; }

    private bool HasCode { get; set; } = false;

    private Dictionary<string, string> TabPanelsContent { get; set; } = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            HasCode = true;
            await SetCodeContentsAsync();

        }
    }

    protected async Task SetCodeContentsAsync()
    {
        try
        {
            foreach (string source in GetSources())
            {
                TabPanelsContent.Add(source, await Http.GetStringAsync($"_content/FluentUI.Demo.Shared/examples/{source}.txt"));
            }
        }
        catch
        {
            //Do Nothing
        }
    }

    IEnumerable<string> GetSources()
    {
        yield return $"{Component.Name}.razor";
        foreach (string ext in CollocatedFiles ?? Enumerable.Empty<string>())
        {
            yield return $"{Component.Name}.razor.{ext}";
        }
    }

    static string TabDisplayName(string tabName)
    {

        if (tabName.EndsWith(".cs"))
        {
            return "C#";
        }

        if (tabName.EndsWith(".razor"))
        {
            return "Razor";
        }

        if (tabName.EndsWith(".css"))
        {
            return "CSS";
        }

        if (tabName.EndsWith(".js"))
        {
            return "JavaScript";
        }

        return tabName;
    }

    static string? TabLanguageClass(string tabName)
    {
        if (tabName.EndsWith(".cs"))
        {
            return "language-csharp";
        }

        if (tabName.EndsWith(".razor"))
        {
            return "language-cshtml-razor";
        }

        if (tabName.EndsWith(".css"))
        {
            return "language-css";
        }
        if (tabName.EndsWith(".js"))
        {
            return "language-javascript";
        }

        return null;
    }
}
