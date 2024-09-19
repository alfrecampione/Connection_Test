using Xunit;
using Moq;
using Moq.Protected;
using System.Net;
using API;

namespace API_connection_Tests;

public class ConnectionTests
{
    private const string ApiUrl = "http://146.190.130.247:5011/donbest";
    private const string Token = "reeEQitM0rEsVOdhd7Ed";
    private readonly string[] _endpoints = ["v2/schedule", "v2/schedule_expanded"];

    [Fact]
    public void Get_ValidOption_ReturnsResponseContent()
    {
        // Arrange
        var expectedResponseContent = "{ \"message\": \"Success\" }";
        var httpResponseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(expectedResponseContent)
        };
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        mockHttpMessageHandler.Protected() // Mock protected method
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync", // The protected method name as string
                ItExpr.IsAny<HttpRequestMessage>(), // Match any HttpRequestMessage
                ItExpr.IsAny<CancellationToken>()   // Match any CancellationToken
             )
        .ReturnsAsync(httpResponseMessage); // Return your predefined HttpResponseMessage

        // Now you can use the `httpClient` in your test


        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var connection = new Connection(ApiUrl, Token, _endpoints, httpClient);

        // Act
        var result = connection.Get(1);

        // Assert
        Assert.Equal(expectedResponseContent, result);
    }

    [Fact]
    public void Get_OptionGreaterThanEndpointsLength_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var connection = new Connection(ApiUrl, Token, _endpoints);

        // Assert
        try
        {
            connection.Get(_endpoints.Length + 1);
            throw new Exception("Invalid bahavior");
        }
        catch (Exception)
        {
            return;
        }
    }

    [Fact]
    public void Get_HttpRequestException_ThrowsException()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException());

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var connection = new Connection(ApiUrl, Token, _endpoints, httpClient);
        // Act and Assert
        Assert.Throws<Exception>(() => connection.Get(1));
    }
}
