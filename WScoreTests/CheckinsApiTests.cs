using System.Net;
using System.Net.Http.Json;
using WScoreDomain.Entities;
using Xunit;

namespace WScoreTests
{
    public class CheckinsApiTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public CheckinsApiTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task PostCheckin_DeveCriarComScore()
        {
            var userResp = await _client.PostAsJsonAsync("/api/v1/users", new
            {
                nome = "A",
                email = "a@a.com"
            });

            var userEnvelope = await userResp.Content.ReadFromJsonAsync<ApiResponse<User>>();
            var user = userEnvelope!.Data!;

            var payload = new
            {
                userId = user.Id,
                humor = 7,
                sono = 6,
                foco = 8,
                energia = 5,
                cargaTrabalho = 4
            };

            var resp = await _client.PostAsJsonAsync("/api/v1/checkins", payload);

            Assert.Equal(HttpStatusCode.Created, resp.StatusCode);

            var envelope = await resp.Content.ReadFromJsonAsync<ApiResponse<Checkin>>();
            var checkin = envelope!.Data!;

            Assert.NotNull(checkin);
            Assert.True(checkin.Score > 0);
            Assert.Equal(user.Id, checkin.UserId);
        }
    }
}
