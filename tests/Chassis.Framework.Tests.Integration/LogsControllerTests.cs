using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Chassis.Framework.Tests.Integration;

public class LogsControllerTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    
    [Fact]
    public async Task GetLogs_ReturnsOk()
    {
        // Arrange
        var endpoint = "/logs";
        var client = _factory.CreateClient();

        // Act
        var result = await client.GetAsync(endpoint);

        // Assert
        result.IsSuccessStatusCode.Should().BeTrue();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Content.Should().BeNull();
    }
}