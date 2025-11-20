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

            var created = await resp.Content.ReadFromJsonAsync<User>();

            Assert.NotNull(created);
            Assert.Equal("Teste", created!.Nome);
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
            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

            var list = await resp.Content.ReadFromJsonAsync<List<User>>();

            Assert.NotNull(list);
            Assert.True(list.Count > 0);
        }
    }
}
