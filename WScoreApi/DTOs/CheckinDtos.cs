using System.ComponentModel.DataAnnotations;

namespace WScoreApi.DTOs;

public record CheckinCreateDto(
    [Required] Guid UserId,
    [Range(0,10)] int Humor,
    [Range(0,10)] int Sono,
    [Range(0,10)] int Foco,
    [Range(0,10)] int Energia,
    [Range(0,16)] int CargaTrabalho,
    DateTime? DataCheckin // opcional: se não vier, usamos UtcNow
);

public record CheckinReadDto(
    Guid Id,
    DateTime DataCheckin,
    int Humor,
    int Sono,
    int Foco,
    int Energia,
    int CargaTrabalho,
    int Score,
    Guid UserId
);
