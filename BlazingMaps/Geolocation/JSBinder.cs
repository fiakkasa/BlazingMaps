using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Darnton.Blazor.DeviceInterop;

internal class JSBinder
{
    internal IJSRuntime JSRuntime;
    private readonly string _importPath;
    private Task<IJSObjectReference>? _module;

    public JSBinder(IJSRuntime jsRuntime, string importPath)
    {
        JSRuntime = jsRuntime;
        _importPath = importPath;
    }

    internal Task<IJSObjectReference> GetModule() =>
        _module ??= JSRuntime.InvokeAsync<IJSObjectReference>("import", _importPath).AsTask();

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (_module is not { }) return;

        var module = await _module.ConfigureAwait(true);
        await module.DisposeAsync();
    }
}
