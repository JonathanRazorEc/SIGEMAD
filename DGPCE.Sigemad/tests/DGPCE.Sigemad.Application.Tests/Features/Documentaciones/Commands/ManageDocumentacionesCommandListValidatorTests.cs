using DGPCE.Sigemad.Application.Dtos.Documentaciones;
using DGPCE.Sigemad.Application.Features.Documentaciones.Commands.ManageDocumentaciones;
using DGPCE.Sigemad.Application.Resources;
using FluentValidation.TestHelper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Tests.Features.Documentaciones.Commands;

public class ManageDocumentacionesCommandListValidatorTests
{
    private readonly ManageDocumentacionesCommandListValidator _validator;
    private readonly IStringLocalizer<ValidationMessages> _localizer;

    public ManageDocumentacionesCommandListValidatorTests()
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging() // Agrega esta línea para registrar ILoggerFactory
            .AddLocalization(options => options.ResourcesPath = "Resources")
            .BuildServiceProvider();

        var localizerFactory = serviceProvider.GetService<IStringLocalizerFactory>();
        _localizer = new StringLocalizer<ValidationMessages>(localizerFactory);

        _validator = new ManageDocumentacionesCommandListValidator(_localizer);
    }

    [Fact]
    public void Validate_IdSucesoIsZero_ShouldHaveValidationError()
    {
        // Arrange
        var command = new ManageDocumentacionesCommand { IdSuceso = 0 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.IdSuceso)
            .WithErrorMessage(_localizer["IdSucesoObligatorio"]);
    }

    [Fact]
    public void Validate_IdSucesoIsGreaterThanZero_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new ManageDocumentacionesCommand { IdSuceso = 1 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.IdSuceso);
    }

    [Fact]
    public void Validate_DetallesDocumentacionesIsEmpty_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new ManageDocumentacionesCommand { DetallesDocumentaciones = new List<DetalleDocumentacionDto>() };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.DetallesDocumentaciones);
    }

    [Fact]
    public void Validate_DetallesDocumentacionesIsValid_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new ManageDocumentacionesCommand
        {
            DetallesDocumentaciones = new List<DetalleDocumentacionDto>
            {
                new DetalleDocumentacionDto
                {
                    FechaHora = DateTime.Now,
                    FechaHoraSolicitud = DateTime.Now,
                    IdTipoDocumento = 1,
                    Descripcion = "Test"
                }
            }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.DetallesDocumentaciones);
    }

    [Fact]
    public void Validate_DetallesDocumentacionesIsInvalid_ShouldHaveValidationError()
    {
        // Arrange
        var command = new ManageDocumentacionesCommand
        {
            DetallesDocumentaciones = new List<DetalleDocumentacionDto>
            {
                new DetalleDocumentacionDto
                {
                    FechaHora = default,
                    FechaHoraSolicitud = default,
                    IdTipoDocumento = 0,
                    Descripcion = string.Empty
                }
            }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("DetallesDocumentaciones[0].FechaHora")
            .WithErrorMessage(_localizer["FechaHoraObligatorio"]);
        result.ShouldHaveValidationErrorFor("DetallesDocumentaciones[0].FechaHoraSolicitud")
            .WithErrorMessage(_localizer["FechaHoraSolicitud"]);
        result.ShouldHaveValidationErrorFor("DetallesDocumentaciones[0].IdTipoDocumento")
            .WithErrorMessage(_localizer["IdTipoDocumentoObligatorio"]);
        result.ShouldHaveValidationErrorFor("DetallesDocumentaciones[0].Descripcion")
            .WithErrorMessage(_localizer["DescripcionObligatorio"]);
    }
}

