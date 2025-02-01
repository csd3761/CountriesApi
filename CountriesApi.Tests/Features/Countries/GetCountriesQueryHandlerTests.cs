using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CountriesApi.Application.Features.Countries;
using Moq;
using Moq.Protected;

namespace CountriesApi.Tests.Features.Countries
{
    public class GetCountriesQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ValidApiCall_ReturnsCountries()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri.ToString().StartsWith("https://restcountries.com/v3.1/all")
                    ),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(@"
                    [
                        {
                            ""name"": { ""common"": ""Switzerland"" },
                            ""capital"": [ ""Bern"" ],
                            ""borders"": [ ""AUT"", ""FRA"" ]
                        }
                    ]")
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://restcountries.com/v3.1/")
            };

            var httpClientFactory = new Mock<IHttpClientFactory>();
            httpClientFactory.Setup(x => x.CreateClient("CountriesClient"))
                .Returns(httpClient);

            var handler = new GetCountriesQueryHandler(httpClientFactory.Object);
            
            var result = await handler.Handle(new GetCountriesQuery(), CancellationToken.None);

            Assert.Single(result);
            Assert.Equal("Switzerland", result[0].CommonName);
            Assert.Equal("Bern", result[0].Capital);
            Assert.Equal(2, result[0].Borders.Count);
        }
    }
}
