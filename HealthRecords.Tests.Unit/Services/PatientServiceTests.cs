using HealthRecords.Application.Interfaces;
using HealthRecords.Application.Interfaces.Mappers;
using HealthRecords.Application.Requests.Patient;
using HealthRecords.Application.Responses.Patient;
using HealthRecords.Application.Services;
using HealthRecords.Domain.Exceptions;
using HealthRecords.Domain.Models;
using HealthRecords.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace HealthRecords.Tests.Unit.Services;

/// <summary>
/// Tests unitarios para PatientService
/// </summary>
public class PatientServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IPatientRepository> _mockPatientRepository;
    private readonly Mock<IPatientMapper> _mockPatientMapper;
    private readonly Mock<ILogger<PatientService>> _mockLogger;
    private readonly PatientService _patientService;

    public PatientServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockPatientRepository = new Mock<IPatientRepository>();
        _mockPatientMapper = new Mock<IPatientMapper>();
        _mockLogger = new Mock<ILogger<PatientService>>();

        _mockUnitOfWork.Setup(u => u.Patients).Returns(_mockPatientRepository.Object);

        _patientService = new PatientService(
            _mockUnitOfWork.Object,
            _mockLogger.Object,
            _mockPatientMapper.Object
        );
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllPatients()
    {
        // Arrange
        var patients = new List<Patient>
        {
            new Patient { Id = 1, Nombre = "Juan Pérez", Email = "juan@test.com" },
            new Patient { Id = 2, Nombre = "María García", Email = "maria@test.com" }
        };

        var responses = patients.Select(p => new PatientResponse 
        { 
            Id = p.Id, 
            Nombre = p.Nombre, 
            Email = p.Email 
        }).ToList();

        _mockPatientRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(patients);
        _mockPatientMapper.Setup(m => m.MapToResponse(It.IsAny<Patient>()))
            .Returns<Patient>(p => responses.First(r => r.Id == p.Id));

        // Act
        var result = await _patientService.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        _mockPatientRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenPatientExists_ShouldReturnPatient()
    {
        // Arrange
        var patient = new Patient { Id = 1, Nombre = "Juan Pérez", Email = "juan@test.com" };
        var response = new PatientResponse { Id = 1, Nombre = "Juan Pérez", Email = "juan@test.com" };

        _mockPatientRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(patient);
        _mockPatientMapper.Setup(m => m.MapToResponse(patient)).Returns(response);

        // Act
        var result = await _patientService.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Nombre.Should().Be("Juan Pérez");
        _mockPatientRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenPatientDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        _mockPatientRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Patient?)null);

        // Act
        var result = await _patientService.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
        _mockPatientRepository.Verify(r => r.GetByIdAsync(999), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WhenEmailExists_ShouldThrowDuplicateEmailException()
    {
        // Arrange
        var request = new CreatePatientRequest
        {
            Nombre = "Juan Pérez",
            Email = "juan@test.com",
            FechaNacimiento = DateTime.UtcNow.AddYears(-30),
            Documento = "12345678"
        };

        _mockPatientRepository.Setup(r => r.ExistsByEmailAsync(request.Email)).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<DuplicateEmailException>(() => _patientService.CreateAsync(request));
        _mockPatientRepository.Verify(r => r.ExistsByEmailAsync(request.Email), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<System.Threading.CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenDocumentExists_ShouldThrowDuplicateDocumentException()
    {
        // Arrange
        var request = new CreatePatientRequest
        {
            Nombre = "Juan Pérez",
            Email = "juan@test.com",
            FechaNacimiento = DateTime.UtcNow.AddYears(-30),
            Documento = "12345678"
        };

        _mockPatientRepository.Setup(r => r.ExistsByEmailAsync(request.Email)).ReturnsAsync(false);
        _mockPatientRepository.Setup(r => r.ExistsByDocumentoAsync(request.Documento)).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<DuplicateDocumentException>(() => _patientService.CreateAsync(request));
        _mockPatientRepository.Verify(r => r.ExistsByDocumentoAsync(request.Documento), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<System.Threading.CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenValid_ShouldCreatePatient()
    {
        // Arrange
        var request = new CreatePatientRequest
        {
            Nombre = "Juan Pérez",
            Email = "juan@test.com",
            FechaNacimiento = DateTime.UtcNow.AddYears(-30),
            Documento = "12345678"
        };

        var patient = new Patient { Id = 1, Nombre = request.Nombre, Email = request.Email };
        var response = new PatientResponse { Id = 1, Nombre = request.Nombre, Email = request.Email };

        _mockPatientRepository.Setup(r => r.ExistsByEmailAsync(request.Email)).ReturnsAsync(false);
        _mockPatientRepository.Setup(r => r.ExistsByDocumentoAsync(request.Documento)).ReturnsAsync(false);
        _mockPatientMapper.Setup(m => m.MapToEntity(request)).Returns(patient);
        _mockPatientRepository.Setup(r => r.CreateAsync(It.IsAny<Patient>())).ReturnsAsync(patient);
        _mockPatientMapper.Setup(m => m.MapToResponse(patient)).Returns(response);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _patientService.CreateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        _mockPatientRepository.Verify(r => r.CreateAsync(It.IsAny<Patient>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<System.Threading.CancellationToken>()), Times.Once);
    }
}
