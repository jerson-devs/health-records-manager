using HealthRecords.Application.Services;
using System;
using Xunit;
using FluentAssertions;

namespace HealthRecords.Tests.Unit.Services;

/// <summary>
/// Tests unitarios para DateTimeConverter
/// </summary>
public class DateTimeConverterTests
{
    private readonly DateTimeConverter _converter;

    public DateTimeConverterTests()
    {
        _converter = new DateTimeConverter();
    }

    [Fact]
    public void ToUtc_WhenDateTimeIsUtc_ShouldReturnSameDateTime()
    {
        // Arrange
        var utcDateTime = DateTime.UtcNow;

        // Act
        var result = _converter.ToUtc(utcDateTime);

        // Assert
        result.Should().Be(utcDateTime);
        result.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Fact]
    public void ToUtc_WhenDateTimeIsLocal_ShouldConvertToUtc()
    {
        // Arrange
        var localDateTime = DateTime.Now;

        // Act
        var result = _converter.ToUtc(localDateTime);

        // Assert
        result.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Fact]
    public void ToUtc_WhenDateTimeIsUnspecified_ShouldConvertToUtc()
    {
        // Arrange
        var unspecifiedDateTime = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Unspecified);

        // Act
        var result = _converter.ToUtc(unspecifiedDateTime);

        // Assert
        result.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Fact]
    public void ToUtc_WhenNullableDateTimeIsNull_ShouldReturnNull()
    {
        // Arrange
        DateTime? nullDateTime = null;

        // Act
        var result = _converter.ToUtc(nullDateTime);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void ToUtc_WhenNullableDateTimeIsUtc_ShouldReturnSameDateTime()
    {
        // Arrange
        DateTime? utcDateTime = DateTime.UtcNow;

        // Act
        var result = _converter.ToUtc(utcDateTime);

        // Assert
        result.Should().Be(utcDateTime);
        result!.Value.Kind.Should().Be(DateTimeKind.Utc);
    }
}
