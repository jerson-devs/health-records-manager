namespace HealthRecords.Application.Constants;

/// <summary>
/// Constantes para nombres de eventos de logging estructurado
/// </summary>
public static class LogEvents
{
    // Eventos de Autenticación
    public const string LoginAttempt = "LoginAttempt";
    public const string LoginSuccess = "LoginSuccess";
    public const string LoginFailedInvalidUser = "LoginFailedInvalidUser";
    public const string LoginFailedInvalidPassword = "LoginFailedInvalidPassword";
    public const string LoginError = "LoginError";
    public const string RefreshTokenAttempt = "RefreshTokenAttempt";
    public const string RefreshTokenSuccess = "RefreshTokenSuccess";
    public const string RefreshTokenFailed = "RefreshTokenFailed";
    public const string LogoutAttempt = "LogoutAttempt";
    public const string LogoutSuccess = "LogoutSuccess";
    public const string LogoutFailed = "LogoutFailed";

    // Eventos de Pacientes
    public const string PatientGetAll = "PatientGetAll";
    public const string PatientGetById = "PatientGetById";
    public const string PatientGetByIdWithRecords = "PatientGetByIdWithRecords";
    public const string PatientCreate = "PatientCreate";
    public const string PatientUpdate = "PatientUpdate";
    public const string PatientDelete = "PatientDelete";
    public const string PatientError = "PatientError";
    public const string PatientDuplicateEmail = "PatientDuplicateEmail";
    public const string PatientDuplicateDocument = "PatientDuplicateDocument";

    // Eventos de Historiales Médicos
    public const string MedicalRecordGetAll = "MedicalRecordGetAll";
    public const string MedicalRecordGetById = "MedicalRecordGetById";
    public const string MedicalRecordGetByPatientId = "MedicalRecordGetByPatientId";
    public const string MedicalRecordCreate = "MedicalRecordCreate";
    public const string MedicalRecordUpdate = "MedicalRecordUpdate";
    public const string MedicalRecordDelete = "MedicalRecordDelete";
    public const string MedicalRecordError = "MedicalRecordError";
    public const string MedicalRecordPatientNotFound = "MedicalRecordPatientNotFound";

    // Eventos Generales
    public const string MethodEntry = "MethodEntry";
    public const string MethodExit = "MethodExit";
    public const string ValidationError = "ValidationError";
    public const string UnhandledException = "UnhandledException";
}
