using System;
using System.ComponentModel.DataAnnotations;

namespace WScoreApi.DTOs
{
    public record UserCreateDto(
        [Required, StringLength(100)] string Nome,
        [Required, EmailAddress, StringLength(150)] string Email
    );

    public record UserReadDto(Guid Id, string Nome, string Email);

    public record UserUpdateDto(
        [Required, StringLength(100)] string Nome,
        [Required, EmailAddress, StringLength(150)] string Email
    );
}
