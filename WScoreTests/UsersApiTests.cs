using System.Net;
using System.Net.Http.Json;
using WScoreDomain.Entities;
using Xunit;

namespace WScoreTests
{
    public class UsersApiTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public UsersApiTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task PostUser_DeveCriarComSucesso()
        {
            var payload = new { nome = "Teste", email = "teste@ws.com" };

            var resp = await _client.PostAsJsonAsync("/api/v1/users", payload);

            Assert.Equal(HttpStatusCode.Created, resp.StatusCode);

            var envelope = await resp.Content.ReadFromJsonAsync<ApiResponse<User>>();
            var created = envelope!.Data!;

            Assert.Equal("Teste", created.Nome);
            Assert.Equal("teste@ws.com", created.Email);
        }

        [Fact]
        public async Task GetUsers_Paginado_DeveRetornarItens()
        {
            await _client.PostAsJsonAsync("/api/v1/users", new
            {
                nome = "User Teste",
                email = "user@teste.com"
            });

            var resp = await _client.GetAsync("/api/v1/users/paginado?page=1&pageSize=10");

            var envelope = await resp.Content.ReadFromJsonAsync<ApiResponse<List<User>>>();
            var list = envelope!.Data!;

            Assert.True(list.Count > 0);
        }
    }
}
