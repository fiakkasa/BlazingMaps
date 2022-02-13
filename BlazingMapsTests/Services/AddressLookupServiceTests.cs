using BlazingMaps.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Contrib.HttpClient;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace BlazingMaps.Services.Tests;

public class AddressLookupServiceTests
{
    private readonly Mock<IHttpClientFactory> mockClientFactory;
    private readonly Mock<HttpMessageHandler> mockHandler;
    private readonly Mock<ILogger<AddressLookupService>> mockLogger;
    private readonly AddressLookupService service;

    public AddressLookupServiceTests()
    {
        mockHandler = new();
        mockClientFactory = new();
        mockClientFactory
            .Setup(m => m.CreateClient(It.IsAny<string>()))
            .Returns(() =>
            {
                var client = mockHandler.CreateClient();
                client.BaseAddress = new Uri("https://example.com");
                return client;
            });
        mockLogger = new();

        service = new(mockClientFactory.Object, mockLogger.Object);
    }

    [Fact]
    public async Task FetchSuccess()
    {
        mockHandler
            .SetupAnyRequest()
            .Returns(Task.FromResult(new HttpResponseMessage
            {
                Content = new StringContent(JsonSerializer.Serialize(new[] { new AddressLookup() })),
                StatusCode = HttpStatusCode.OK
            }));
        Assert.NotEmpty(await service.Fetch("test"));
        mockLogger.VerifyLog(m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
    }

    [Fact]
    public async Task FetchSuccessEmpty()
    {
        mockHandler
            .SetupAnyRequest()
            .Returns(Task.FromResult(new HttpResponseMessage
            {
                Content = new StringContent(JsonSerializer.Serialize(Enumerable.Empty<AddressLookup>())),
                StatusCode = HttpStatusCode.OK
            }));
        Assert.Empty(await service.Fetch("test"));
        mockLogger.VerifyLog(m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
    }

    [Fact]
    public async Task FetchSuccessEmptyToken()
    {
        mockHandler
            .SetupAnyRequest()
            .Returns(Task.FromResult(new HttpResponseMessage
            {
                Content = new StringContent(JsonSerializer.Serialize(Enumerable.Empty<AddressLookup>())),
                StatusCode = HttpStatusCode.OK
            }));
        Assert.Empty(await service.Fetch("  "));
        mockLogger.VerifyLog(m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
    }

    [Fact]
    public async Task FetchSuccessNullToken()
    {
        mockHandler
            .SetupAnyRequest()
            .Returns(Task.FromResult(new HttpResponseMessage
            {
                Content = new StringContent(JsonSerializer.Serialize(Enumerable.Empty<AddressLookup>())),
                StatusCode = HttpStatusCode.OK
            }));
        Assert.Empty(await service.Fetch(null));
        mockLogger.VerifyLog(m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
    }

    [Fact]
    public async Task FetchErrorWrongResponseType()
    {
        mockHandler
            .SetupAnyRequest()
            .Returns(Task.FromResult(new HttpResponseMessage
            {
                Content = new StringContent(JsonSerializer.Serialize(new[] { new MapOptionsEx() })),
                StatusCode = HttpStatusCode.OK
            }));
        Assert.NotEmpty(await service.Fetch("test"));
        mockLogger.VerifyLog(m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Never);
    }

    [Fact]
    public async Task FetchHttpError()
    {
        mockHandler
            .SetupAnyRequest()
            .Returns(Task.FromResult(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Forbidden
            }));
        Assert.Empty(await service.Fetch("test"));
        mockLogger.VerifyLog(m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
    }

    [Fact]
    public async Task FetchError()
    {
        mockClientFactory
            .Setup(m => m.CreateClient(It.IsAny<string>()))
            .Throws(new Exception("Splash!"));
        Assert.Empty(await service.Fetch("test"));
        mockLogger.VerifyLog(m => m.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
    }
}
