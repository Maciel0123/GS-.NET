// WScoreTests/CheckinsApiTests.cs
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace WScoreTests;

public class CheckinsApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CheckinsApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_Checkin_DeveCriarScoreECadastrar()
    {
        // 1) cria um usuário
        var userCreate = new { nome = "Teste", email = "t@t.com" };
        var userResp = await _client.PostAsJsonAsync("/api/v1/users", userCreate);
        userResp.EnsureSuccessStatusCode();
        var user = await userResp.Content.ReadFromJsonAsync<UserReadDto>();

        // 2) cria um checkin para esse usuário
        var checkin = new
        {
            userId = user!.Id,
            humor = 7,
            sono = 6,
            foco = 8,
            energia = 5,
            cargaTrabalho = 4
        };

        var resp = await _client.PostAsJsonAsync("/api/v1/checkins", checkin);

        Assert.Equal(HttpStatusCode.Created, resp.StatusCode);
    }

    private record UserReadDto(Guid Id, string Nome, string Email);
}
