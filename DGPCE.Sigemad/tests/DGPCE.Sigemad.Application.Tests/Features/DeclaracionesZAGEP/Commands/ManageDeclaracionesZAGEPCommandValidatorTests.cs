using FluentValidation.TestHelper;
using global::DGPCE.Sigemad.Application.Dtos.DeclaracionesZAGEP;
using global::DGPCE.Sigemad.Application.Features.DeclaracionesZAGEP.Commands.ManageDeclaracionesZAGEP;
using global::DGPCE.Sigemad.Application.Resources;
using Microsoft.Extensions.Localization;
using Moq;
using Xunit;

namespace DGPCE.Sigemad.Application.Tests.Features.DeclaracionesZAGEP.Commands
{
    public class ManageDeclaracionesZAGEPCommandValidatorTests
    {
        private readonly Mock<IStringLocalizer<ValidationMessages>> _localizerMock;
        private readonly ManageDeclaracionesZAGEPCommandValidator _validator;

        public ManageDeclaracionesZAGEPCommandValidatorTests()
        {
            _localizerMock = new Mock<IStringLocalizer<ValidationMessages>>();
            _localizerMock.Setup(l => l["IdSucesoObligatorio"]).Returns(new LocalizedString("IdSucesoObligatorio", "IdSuceso es obligatorio"));
            _localizerMock.Setup(l => l["FechaSolicitudObligatorio"]).Returns(new LocalizedString("FechaSolicitudObligatorio", "Fecha de solicitud es obligatoria"));
            _localizerMock.Setup(l => l["DenominacionObligatorio"]).Returns(new LocalizedString("DenominacionObligatorio", "Denominación es obligatoria"));
            _localizerMock.Setup(l => l["DenominacionZAGEPMaxLength"]).Returns(new LocalizedString("DenominacionZAGEPMaxLength", "La longitud máxima de la denominación es 510 caracteres"));

            _validator = new ManageDeclaracionesZAGEPCommandValidator(_localizerMock.Object);
        }

        [Fact]
        public void Validate_ShouldHaveError_WhenIdSucesoIsZero()
        {
            // Arrange
            var command = new ManageDeclaracionesZAGEPCommand { IdSuceso = 0 };

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
            var command = new ManageDeclaracionesZAGEPCommand
            {
                IdSuceso = 1,
                Detalles = new List<ManageDeclaracionZAGEPDto>
                {
                    new ManageDeclaracionZAGEPDto
                    {
                        Denominacion = ""
                    }
                }
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor("Detalles[0].FechaSolicitud")
                  .WithErrorMessage("Fecha de solicitud es obligatoria");
            result.ShouldHaveValidationErrorFor("Detalles[0].Denominacion")
                  .WithErrorMessage("Denominación es obligatoria");
        }

        [Fact]
        public void Validate_ShouldNotHaveError_WhenDetallesIsNullOrEmpty()
        {
            // Arrange
            var commandWithNullDetalles = new ManageDeclaracionesZAGEPCommand
            {
                IdSuceso = 1,
                Detalles = null
            };

            var commandWithEmptyDetalles = new ManageDeclaracionesZAGEPCommand
            {
                IdSuceso = 1,
                Detalles = new List<ManageDeclaracionZAGEPDto>()
            };

            // Act
            var resultWithNullDetalles = _validator.TestValidate(commandWithNullDetalles);
            var resultWithEmptyDetalles = _validator.TestValidate(commandWithEmptyDetalles);

            // Assert
            resultWithNullDetalles.ShouldNotHaveValidationErrorFor(c => c.Detalles);
            resultWithEmptyDetalles.ShouldNotHaveValidationErrorFor(c => c.Detalles);
        }
    }
}

