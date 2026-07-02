using System.Collections.Concurrent;
using Api.Application.Abstractions;
using Api.Domain.Entities;
using Api.Domain.ValueObjects;

namespace Api.Infrastructure.Data.Repositories;

public sealed class InMemoryUsuarioRepository : IUsuarioRepository
{
    private readonly ConcurrentDictionary<Guid, Usuario> usuarios = new();

    public Task<Usuario?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        usuarios.TryGetValue(id, out var usuario);
        return Task.FromResult(usuario);
    }

    public Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken)
    {
        var exists = usuarios.Values.Any(usuario => usuario.Email == email);
        return Task.FromResult(exists);
    }

    public Task AddAsync(Usuario usuario, CancellationToken cancellationToken)
    {
        usuarios[usuario.Id] = usuario;
        return Task.CompletedTask;
    }
}