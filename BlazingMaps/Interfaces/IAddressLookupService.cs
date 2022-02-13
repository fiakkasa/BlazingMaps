using BlazingMaps.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BlazingMaps.Interfaces;

public interface IAddressLookupService
{
    ValueTask<IEnumerable<AddressLookup>> Fetch(string? address, CancellationToken cancellationToken = default);
}
