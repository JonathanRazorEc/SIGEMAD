using DGPCE.Sigemad.Application.Dtos.ConvocatoriasCECOD;
using DGPCE.Sigemad.Application.Dtos.DeclaracionesZAGEP;
using DGPCE.Sigemad.Application.Features.ConvocatoriasCECOD.Commands;
using DGPCE.Sigemad.Application.Features.DeclaracionesZAGEP.Commands.ManageDeclaracionesZAGEP;
using DGPCE.Sigemad.Application.Resources;
using DGPCE.Sigemad.Domain.Modelos;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Localization;
using Moq;


namespace DGPCE.Sigemad.Application.Tests.Features.ConvocatoriasCECOD.Commands;
public class ManageConvocatoriaCECODCommandValidatorTests
{
    private readonly Mock<IStringLocalizer<ValidationMessages>> _localizerMock;
    private readonly ManageConvocatoriaCECODCommandValidator _validator;

    public ManageConvocatoriaCECODCommandValidatorTests()
    {
        _localizerMock = new Mock<IStringLocalizer<ValidationMessages>>();
        _localizerMock.Setup(l => l["IdSucesoObligatorio"]).Returns(new LocalizedString("IdSucesoObligatorio", "IdSuceso es obligatorio"));
        _localizerMock.Setup(l => l["FechaInicioObligatorio"]).Returns(new LocalizedString("FechaInicioObligatorio", "Fecha de Inicio es obligatoria"));
        _localizerMock.Setup(l => l["LugarObligatorio"]).Returns(new LocalizedString("LugarObligatorio", "Lugar es obligatorio"));
        _localizerMock.Setup(l => l["LugarObligatorioMaxLength"]).Returns(new LocalizedString("LugarObligatorioMaxLength", "La longitud máxima de la denominación es 510 caracteres"));
        _localizerMock.Setup(l => l["ConvocadosObligatorio"]).Returns(new LocalizedString("ConvocadosObligatorio", "Convocados es obligatoria"));
        _localizerMock.Setup(l => l["ConvocadosMaxLength"]).Returns(new LocalizedString("ConvocadosMaxLength", "La longitud máxima de la denominación es 510 caracteres"));

        _validator = new ManageConvocatoriaCECODCommandValidator(_localizerMock.Object);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenIdSucesoIsZero()
    {
        // Arrange
        var command = new ManageConvocatoriaCECODCommand { IdSuceso = 0 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.IdSuceso)
              .WithErrorMessage("IdSuceso es obligatorio");
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenDetallesIsInvalid()
    {
        // Arrange
        var command = new ManageConvocatoriaCECODCommand
        {
            IdSuceso = 1,
            Detalles = new List<ManageConvocatoriaCECODDto>
                {
                    new ManageConvocatoriaCECODDto
                    {
                    }
                }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Detalles[0].FechaInicio")
              .WithErrorMessage("Fecha de Inicio es obligatoria");
        result.ShouldHaveValidationErrorFor("Detalles[0].Lugar")
              .WithErrorMessage("Lugar es obligatorio");
        result.ShouldHaveValidationErrorFor("Detalles[0].Convocados")
              .WithErrorMessage("Convocados es obligatoria");
    }



    [Fact]
    public void Validate_ShouldNotHaveError_WhenDetallesIsNullOrEmpty()
    {
        // Arrange
        var commandWithNullDetalles = new ManageConvocatoriaCECODCommand
        {
            IdSuceso = 1,
            Detalles = null
        };

        var commandWithEmptyDetalles = new ManageConvocatoriaCECODCommand
        {
            IdSuceso = 1,
            Detalles = new List<ManageConvocatoriaCECODDto>()
        };

        // Act
        var resultWithNullDetalles = _validator.TestValidate(commandWithNullDetalles);
        var resultWithEmptyDetalles = _validator.TestValidate(commandWithEmptyDetalles);

        // Assert
        resultWithNullDetalles.ShouldNotHaveValidationErrorFor(c => c.Detalles);
        resultWithEmptyDetalles.ShouldNotHaveValidationErrorFor(c => c.Detalles);
    }
}

