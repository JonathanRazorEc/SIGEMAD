using DGPCE.Sigemad.Application.Dtos.ActivacionSistema;
using DGPCE.Sigemad.Application.Dtos.DeclaracionesZAGEP;
using DGPCE.Sigemad.Application.Features.ActivacionesSistemas.Commands;
using DGPCE.Sigemad.Application.Features.DeclaracionesZAGEP.Commands.ManageDeclaracionesZAGEP;
using DGPCE.Sigemad.Application.Resources;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Localization;
using Moq;


namespace DGPCE.Sigemad.Application.Tests.Features.ActivacionSistemas.Commands;

public class ManageActivacionSistemaCommandListValidatorTest
    {
        private readonly Mock<IStringLocalizer<ValidationMessages>> _localizerMock;
        private readonly ManageActivacionSistemaCommandValidator _validator;

        public ManageActivacionSistemaCommandListValidatorTest()
        {
            _localizerMock = new Mock<IStringLocalizer<ValidationMessages>>();
            _localizerMock.Setup(l => l["IdSucesoObligatorio"]).Returns(new LocalizedString("IdSucesoObligatorio", "IdSuceso es obligatorio"));
            _localizerMock.Setup(l => l["IdTipoSistemaEmergencia"]).Returns(new LocalizedString("IdTipoSistemaEmergencia", "IdTipoSistemaEmergencia es obligatorio"));
            _localizerMock.Setup(l => l["AutoridadMaxLength"]).Returns(new LocalizedString("AutoridadMaxLength", "La longitud máxima de la Autoridad es 510 caracteres"));

            _validator = new ManageActivacionSistemaCommandValidator(_localizerMock.Object);
        }
    


    [Fact]
    public void Validate_ShouldHaveError_WhenIdSucesoIsZero()
    {
        // Arrange
        var command = new ManageActivacionSistemaCommand { IdSuceso = 0 };

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
        var command = new ManageActivacionSistemaCommand
        {
            IdSuceso = 1,
            Detalles = new List<ManageActivacionSistemaDto>
                {
                    new ManageActivacionSistemaDto
                    {
                        Observaciones = ""
                    }
                }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Detalles[0].IdTipoSistemaEmergencia")
              .WithErrorMessage("IdTipoSistemaEmergencia es obligatorio");
    }


    [Fact]
    public void Validate_ShouldNotHaveError_WhenDetallesIsNullOrEmpty()
    {
        // Arrange
        var commandWithNullDetalles = new ManageActivacionSistemaCommand
        {
            IdSuceso = 1,
            Detalles = null
        };

        var commandWithEmptyDetalles = new ManageActivacionSistemaCommand
        {
            IdSuceso = 1,
            Detalles = new List<ManageActivacionSistemaDto>()
        };

        // Act
        var resultWithNullDetalles = _validator.TestValidate(commandWithNullDetalles);
        var resultWithEmptyDetalles = _validator.TestValidate(commandWithEmptyDetalles);

        // Assert
        resultWithNullDetalles.ShouldNotHaveValidationErrorFor(c => c.Detalles);
        resultWithEmptyDetalles.ShouldNotHaveValidationErrorFor(c => c.Detalles);
    }
}



