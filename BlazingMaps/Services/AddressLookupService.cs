using BlazingMaps.Interfaces;
using BlazingMaps.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BlazingMaps.Services;

public class AddressLookupService : IAddressLookupService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<AddressLookupService> _logger;

    public AddressLookupService(IHttpClientFactory clientFactory, ILogger<AddressLookupService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async ValueTask<IEnumerable<AddressLookup>> Fetch(string address, CancellationToken cancellationToken = default)
    {
        if (address?.Trim() is not { Length: > 0 } value) return Enumerable.Empty<AddressLookup>();

        using var client = _clientFactory.CreateClient(Consts.AddressLookupClientName);

        try
        {
            return (
                await client
                    .GetFromJsonAsync<IEnumerable<AddressLookup>>(client.BaseAddress + value, cancellationToken)
                    .ConfigureAwait(false)
            )?
            .DistinctBy(x => x.Id)
            ?? Enumerable.Empty<AddressLookup>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to lookup for {address}", new[] { address });
            return Enumerable.Empty<AddressLookup>();
        }
    }
}
