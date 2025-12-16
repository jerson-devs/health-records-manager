using HealthRecords.Application.Interfaces;
using HealthRecords.Application.Mappers;
using HealthRecords.Application.Requests.Patient;
using HealthRecords.Application.Responses.Patient;
using HealthRecords.Domain.Models;
using Moq;
using System;
using System.Linq;
using Xunit;
using FluentAssertions;

namespace HealthRecords.Tests.Unit.Mappers;

/// <summary>
/// Tests unitarios para PatientMapper
/// </summary>
public class PatientMapperTests
{
    private readonly Mock<IDateTimeConverter> _mockDateTimeConverter;
    private readonly PatientMapper _mapper;

    public PatientMapperTests()
    {
        _mockDateTimeConverter = new Mock<IDateTimeConverter>();
        _mapper = new PatientMapper(_mockDateTimeConverter.Object);
    }

    [Fact]
    public void MapToResponse_ShouldMapAllProperties()
    {
        // Arrange
        var patient = new Patient
        {
            Id = 1,
            Nombre = "Juan Pérez",
            Email = "juan@test.com",
            FechaNacimiento = DateTime.UtcNow.AddYears(-30),
            Documento = "12345678",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var result = _mapper.MapToResponse(patient);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(patient.Id);
        result.Nombre.Should().Be(patient.Nombre);
        result.Email.Should().Be(patient.Email);
        result.FechaNacimiento.Should().Be(patient.FechaNacimiento);
        result.Documento.Should().Be(patient.Documento);
        result.CreatedAt.Should().Be(patient.CreatedAt);
        result.UpdatedAt.Should().Be(patient.UpdatedAt);
    }

    [Fact]
    public void MapToResponse_WhenPatientHasMedicalRecords_ShouldIncludeMedicalRecords()
    {
        // Arrange
        var patient = new Patient
        {
            Id = 1,
            Nombre = "Juan Pérez",
            MedicalRecords = new List<MedicalRecord>
            {
                new MedicalRecord { Id = 1, Fecha = DateTime.UtcNow, Diagnostico = "Resfriado", Medico = "Dr. García" }
            }
        };

        // Act
        var result = _mapper.MapToResponse(patient);

        // Assert
        result.MedicalRecords.Should().NotBeNull();
        result.MedicalRecords.Should().HaveCount(1);
        result.MedicalRecords!.First().Id.Should().Be(1);
    }

    [Fact]
    public void MapToEntity_ShouldConvertDateTimeToUtc()
    {
        // Arrange
        var request = new CreatePatientRequest
        {
            Nombre = "Juan Pérez",
            Email = "juan@test.com",
            FechaNacimiento = new DateTime(1990, 1, 1),
            Documento = "12345678"
        };

        var expectedUtc = DateTime.UtcNow;
        _mockDateTimeConverter.Setup(c => c.ToUtc(request.FechaNacimiento)).Returns(expectedUtc);

        // Act
        var result = _mapper.MapToEntity(request);

        // Assert
        result.Should().NotBeNull();
        result.Nombre.Should().Be(request.Nombre);
        result.Email.Should().Be(request.Email);
        result.Documento.Should().Be(request.Documento);
        _mockDateTimeConverter.Verify(c => c.ToUtc(request.FechaNacimiento), Times.Once);
    }

    [Fact]
    public void MapToEntity_UpdateRequest_ShouldUpdatePatientProperties()
    {
        // Arrange
        var patient = new Patient
        {
            Id = 1,
            Nombre = "Juan Pérez",
            Email = "juan@test.com",
            FechaNacimiento = DateTime.UtcNow.AddYears(-30),
            Documento = "12345678"
        };

        var request = new UpdatePatientRequest
        {
            Nombre = "Juan Carlos Pérez",
            Email = "juancarlos@test.com",
            FechaNacimiento = new DateTime(1990, 1, 1),
            Documento = "87654321"
        };

        var expectedUtc = DateTime.UtcNow;
        _mockDateTimeConverter.Setup(c => c.ToUtc(request.FechaNacimiento)).Returns(expectedUtc);

        // Act
        _mapper.MapToEntity(request, patient);

        // Assert
        patient.Nombre.Should().Be(request.Nombre);
        patient.Email.Should().Be(request.Email);
        patient.Documento.Should().Be(request.Documento);
        _mockDateTimeConverter.Verify(c => c.ToUtc(request.FechaNacimiento), Times.Once);
    }
}
