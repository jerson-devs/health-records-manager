using HealthRecords.Domain.Models;
using HealthRecords.Domain.Repositories;
using HealthRecords.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace HealthRecords.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio para la entidad User.
/// Utiliza Entity Framework Core para acceso a datos.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Constructor que recibe el DbContext mediante inyección de dependencias
    /// </summary>
    /// <param name="context">Contexto de base de datos</param>
    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    /// <inheritdoc/>
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    /// <inheritdoc/>
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    /// <inheritdoc/>
    public Task<User> CreateAsync(User user)
    {
        user.CreatedAt = DateTime.UtcNow;
        _context.Users.Add(user);
        return Task.FromResult(user);
    }

    /// <inheritdoc/>
    public Task<User> UpdateAsync(User user)
    {
        _context.Users.Update(user);
        return Task.FromResult(user);
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        return await _context.Users
            .AnyAsync(u => u.Username == username);
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email);
    }
}

