using BlazingMaps.Models;

namespace BlazingMaps.Interfaces;

public interface IAddressLookupService
{
    ValueTask<IEnumerable<AddressLookup>> Fetch(string address, CancellationToken cancellationToken = default);
}
