using System.Net;
using System.Net.Http.Json;
using WScoreApi.DTOs;
using WScoreDomain.Entities;
using Xunit;

namespace WScoreTests;

public class UsersApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public UsersApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostUser_DeveCriarUsuarioComSucesso()
    {
        // ✅ use o DTO de criação (Nome/Email)
        var payload = new UserCreateDto(
            Nome: "Teste Usuário",
            Email: "teste@ws.com"
        );

        var resp = await _client.PostAsJsonAsync("/api/v1/users", payload);

        Assert.Equal(HttpStatusCode.Created, resp.StatusCode);

        // opcional: validar corpo de resposta
        var created = await resp.Content.ReadFromJsonAsync<UserReadDto>();
        Assert.NotNull(created);
        Assert.Equal("Teste Usuário", created!.Nome);
        Assert.Equal("teste@ws.com", created.Email);
        Assert.NotEqual(Guid.Empty, created.Id);
    }

    [Fact]
    public async Task GetUsers_DeveRetornarEnvelopePaginadoComItens()
    {
        // garante ao menos 1 usuário
        await _client.PostAsJsonAsync("/api/v1/users",
            new UserCreateDto("Usuario Teste", "user@teste.com"));

        // ✅ agora GET retorna PagedResponse<UserReadDto>
        var resp = await _client.GetAsync("/api/v1/users?page=1&pageSize=10");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var body = await resp.Content.ReadFromJsonAsync<PagedResponse<UserReadDto>>();
        Assert.NotNull(body);
        Assert.True(body!.Items.Any());
        Assert.True(body.Page >= 1);
        Assert.True(body.PageSize >= 1);
        Assert.True(body.TotalItems >= 1);
        Assert.True(body.TotalPages >= 1);
        Assert.NotNull(body.Links);
    }
}
