using HealthRecords.Application.Interfaces;
using HealthRecords.Application.Requests.Auth;
using HealthRecords.Application.Responses;
using HealthRecords.API.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace HealthRecords.API.Controllers;

/// <summary>
/// Controller para autenticación
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : BaseController
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthService authService,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Autentica un usuario y genera tokens JWT (access token y refresh token)
    /// </summary>
    /// <param name="request">Datos de login (username/email y password)</param>
    /// <returns>Tokens JWT y datos del usuario</returns>
    /// <response code="200">Login exitoso</response>
    /// <response code="400">Credenciales inválidas</response>
    [HttpPost("login")]
    [ValidateModel]
    [ProducesResponseType(typeof(ResponseDto<Application.Responses.Auth.LoginResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ResponseDto<Application.Responses.Auth.LoginResponse>>> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        if (result == null)
        {
            return Error<Application.Responses.Auth.LoginResponse>("Credenciales inválidas");
        }

        return Success(result, "Login exitoso");
    }

    /// <summary>
    /// Renueva el access token usando un refresh token válido
    /// </summary>
    /// <param name="request">Refresh token para renovar</param>
    /// <returns>Nuevos tokens JWT</returns>
    /// <response code="200">Token renovado exitosamente</response>
    /// <response code="400">Refresh token inválido o expirado</response>
    [HttpPost("refresh")]
    [ValidateModel]
    [ProducesResponseType(typeof(ResponseDto<Application.Responses.Auth.LoginResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ResponseDto<Application.Responses.Auth.LoginResponse>>> Refresh([FromBody] RefreshTokenRequest request)
    {
        var result = await _authService.RefreshTokenAsync(request.RefreshToken);

        if (result == null)
        {
            return Error<Application.Responses.Auth.LoginResponse>("Refresh token inválido o expirado");
        }

        return Success(result, "Token renovado exitosamente");
    }

    /// <summary>
    /// Cierra la sesión del usuario invalidando el refresh token
    /// </summary>
    /// <param name="request">Refresh token a invalidar</param>
    /// <returns>Resultado del logout</returns>
    /// <response code="200">Logout exitoso</response>
    /// <response code="400">Error al cerrar sesión</response>
    [HttpPost("logout")]
    [Authorize]
    [ValidateModel]
    [ProducesResponseType(typeof(ResponseDto<object>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ResponseDto<object>), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ResponseDto<object>>> Logout([FromBody] RefreshTokenRequest request)
    {
        var result = await _authService.LogoutAsync(request.RefreshToken);

        if (!result)
        {
            return Error<object>("Error al cerrar sesión");
        }

        return Success<object>(new { }, "Sesión cerrada exitosamente");
    }
}

