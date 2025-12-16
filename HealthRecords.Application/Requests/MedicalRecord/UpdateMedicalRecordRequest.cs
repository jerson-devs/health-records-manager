using System;
using System.ComponentModel.DataAnnotations;

namespace HealthRecords.Application.Requests.MedicalRecord;

/// <summary>
/// DTO para actualizar un historial médico existente
/// </summary>
public class UpdateMedicalRecordRequest
{
    /// <summary>
    /// Fecha del registro médico
    /// </summary>
    [Required(ErrorMessage = "La fecha es requerida")]
    [DataType(DataType.DateTime)]
    public DateTime Fecha { get; set; }

    /// <summary>
    /// Diagnóstico del paciente
    /// </summary>
    [Required(ErrorMessage = "El diagnóstico es requerido")]
    [StringLength(500, ErrorMessage = "El diagnóstico no puede exceder 500 caracteres")]
    public string Diagnostico { get; set; } = string.Empty;

    /// <summary>
    /// Tratamiento prescrito
    /// </summary>
    [Required(ErrorMessage = "El tratamiento es requerido")]
    [StringLength(1000, ErrorMessage = "El tratamiento no puede exceder 1000 caracteres")]
    public string Tratamiento { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del médico que atendió
    /// </summary>
    [Required(ErrorMessage = "El nombre del médico es requerido")]
    [StringLength(200, ErrorMessage = "El nombre del médico no puede exceder 200 caracteres")]
    public string Medico { get; set; } = string.Empty;
}

