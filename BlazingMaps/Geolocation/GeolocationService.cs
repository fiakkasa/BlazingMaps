using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Darnton.Blazor.DeviceInterop.Geolocation;

/// <summary>
/// An implementation of <see cref="IGeolocationService"/> that provides
/// an interop layer for the device's Geolocation API.
/// </summary>
public class GeolocationService : IGeolocationService
{
    private JSBinder? _jsBinder;
    private JSBinder JsBinder => _jsBinder ??= new JSBinder(JSRuntime, "./js/Geolocation.js");
    private IJSRuntime JSRuntime { get; }

    /// <summary>
    /// Constructs a <see cref="GeolocationService"/> object.
    /// </summary>
    /// <param name="JSRuntime"></param>
    public GeolocationService(IJSRuntime JSRuntime)
    {
        this.JSRuntime = JSRuntime;
    }

    /// <inheritdoc/>
    public async Task<GeolocationResult> GetCurrentPosition(PositionOptions? options = default)
    {
        try
        {
            var module = await JsBinder.GetModule().ConfigureAwait(true);
            return await module.InvokeAsync<GeolocationResult>("Geolocation.getCurrentPosition", options).ConfigureAwait(true);
        }
        catch
        {
            return new() { Error = new() { Code = GeolocationPositionErrorCode.ERROR } };
        }
    }
}
