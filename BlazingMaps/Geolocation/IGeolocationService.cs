using System.Threading.Tasks;

namespace Darnton.Blazor.DeviceInterop.Geolocation;

/// <summary>
/// A wrapper around the device's Geolocation API services.
/// <see href="https://developer.mozilla.org/en-US/docs/Web/API/Geolocation_API"/>.
/// </summary>
public interface IGeolocationService
{
    /// <summary>
    /// A wrapper around the <see href="https://developer.mozilla.org/en-US/docs/Web/API/Geolocation/getCurrentPosition"/> function,
    /// used to get the current position of the device.
    /// </summary>
    /// <param name="options"><see cref="PositionOptions"/> used to modify the request.</param>
    /// <returns>The result of the geolocation request.</returns>
    Task<GeolocationResult> GetCurrentPosition(PositionOptions? options = default);
}
