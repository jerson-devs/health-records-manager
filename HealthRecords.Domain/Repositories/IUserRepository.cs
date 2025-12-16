using HealthRecords.Domain.Models;
using System.Threading.Tasks;

namespace HealthRecords.Domain.Repositories;

/// <summary>
/// Interfaz del repositorio para la entidad User.
/// Define los métodos de acceso a datos para usuarios.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Obtiene un usuario por su ID
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <returns>Usuario encontrado o null</returns>
    Task<User?> GetByIdAsync(int id);

    /// <summary>
    /// Obtiene un usuario por su nombre de usuario
    /// </summary>
    /// <param name="username">Nombre de usuario</param>
    /// <returns>Usuario encontrado o null</returns>
    Task<User?> GetByUsernameAsync(string username);

    /// <summary>
    /// Obtiene un usuario por su email
    /// </summary>
    /// <param name="email">Email del usuario</param>
    /// <returns>Usuario encontrado o null</returns>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// Crea un nuevo usuario
    /// </summary>
    /// <param name="user">Usuario a crear</param>
    /// <returns>Usuario creado</returns>
    Task<User> CreateAsync(User user);

    /// <summary>
    /// Actualiza un usuario existente
    /// </summary>
    /// <param name="user">Usuario a actualizar</param>
    /// <returns>Usuario actualizado</returns>
    Task<User> UpdateAsync(User user);

    /// <summary>
    /// Verifica si existe un usuario con el username dado
    /// </summary>
    /// <param name="username">Username a verificar</param>
    /// <returns>True si existe</returns>
    Task<bool> ExistsByUsernameAsync(string username);

    /// <summary>
    /// Verifica si existe un usuario con el email dado
    /// </summary>
    /// <param name="email">Email a verificar</param>
    /// <returns>True si existe</returns>
    Task<bool> ExistsByEmailAsync(string email);
}

