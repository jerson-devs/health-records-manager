using System;
using System.ComponentModel.DataAnnotations;

namespace HealthRecords.Application.Requests.Patient;

/// <summary>
/// DTO para actualizar un paciente existente
/// </summary>
public class UpdatePatientRequest
{
    /// <summary>
    /// Nombre completo del paciente
    /// </summary>
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico del paciente
    /// </summary>
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de nacimiento del paciente
    /// </summary>
    [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
    [DataType(DataType.Date)]
    public DateTime FechaNacimiento { get; set; }

    /// <summary>
    /// Número de documento de identidad
    /// </summary>
    [Required(ErrorMessage = "El documento es requerido")]
    [StringLength(50, ErrorMessage = "El documento no puede exceder 50 caracteres")]
    public string Documento { get; set; } = string.Empty;
}

