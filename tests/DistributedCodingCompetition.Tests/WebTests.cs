using System.Net;

namespace DistributedCodingCompetition.Tests;

public class WebTests
{
    [Fact]
    public async Task GetWebResourceRootReturnsOkStatusCode()
    {
        // Arrange
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.DistributedCodingCompetition_AppHost>();
        await using var app = await appHost.BuildAsync();
        await app.StartAsync();

        // Act
        var httpClient = app.CreateHttpClient("webfrontend");
        var response = await httpClient.GetAsync("/");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetWebResourceRootReturnsHtmlContent()
    {
        // Arrange
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.DistributedCodingCompetition_AppHost>();
        await using var app = await appHost.BuildAsync();
        await app.StartAsync();

        // Act
        var httpClient = app.CreateHttpClient("webfrontend");
        var response = await httpClient.GetAsync("/");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains("<!DOCTYPE html>", content);
    }

    [Fact]
    public async Task GetWebResourceRootReturnsHtmlContentWithCorrectTitle()
    {
        // Arrange
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.DistributedCodingCompetition_AppHost>();
        await using var app = await appHost.BuildAsync();
        await app.StartAsync();

        // Act
        var httpClient = app.CreateHttpClient("webfrontend");
        var response = await httpClient.GetAsync("/");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains("<title>Distributed Coding Competition</title>", content);
    }

    [Fact]
    public async Task GetLoginPageReturnsOkStatusCode()
    {
        // Arrange
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.DistributedCodingCompetition_AppHost>();
        await using var app = await appHost.BuildAsync();
        await app.StartAsync();

        // Act
        var httpClient = app.CreateHttpClient("webfrontend");
        var response = await httpClient.GetAsync("/login");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("<!DOCTYPE html>", await response.Content.ReadAsStringAsync());
    }
}
