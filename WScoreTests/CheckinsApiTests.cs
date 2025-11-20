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
            // cria user
            var userResp = await _client.PostAsJsonAsync("/api/v1/users", new
            {
                nome = "A",
                email = "a@a.com"
            });

            var user = await userResp.Content.ReadFromJsonAsync<User>();

            // cria checkin
            var payload = new
            {
                userId = user!.Id,
                humor = 7,
                sono = 6,
                foco = 8,
                energia = 5,
                cargaTrabalho = 4
            };

            var resp = await _client.PostAsJsonAsync("/api/v1/checkins", payload);
            Assert.Equal(HttpStatusCode.Created, resp.StatusCode);

            var checkin = await resp.Content.ReadFromJsonAsync<Checkin>();

            Assert.NotNull(checkin);
            Assert.True(checkin!.Score > 0);
            Assert.Equal(user.Id, checkin.UserId);
        }
    }
}
