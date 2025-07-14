using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.ManageSucesoRelacionados;
using DGPCE.Sigemad.Application.Resources;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Localization;
using Moq;

namespace DGPCE.Sigemad.Application.Tests.Features.SucesosRelacionados.Commands.ManageSucesoRelacionados;

public class ManageSucesoRelacionadosCommandValidatorTests
{
    private readonly ManageSucesoRelacionadosCommandValidator _validator;
    private readonly Mock<IStringLocalizer<ValidationMessages>> _localizerMock;

    public ManageSucesoRelacionadosCommandValidatorTests()
    {
        _localizerMock = new Mock<IStringLocalizer<ValidationMessages>>();
        _localizerMock.Setup(l => l["IdSucesoObligatorio"]).Returns(new LocalizedString("IdSucesoObligatorio", "Id Suceso es obligatorio"));
        _validator = new ManageSucesoRelacionadosCommandValidator(_localizerMock.Object);
    }

    [Fact]
    public void Validate_IdSucesoIsZero_ShouldHaveValidationError()
    {
        // Arrange
        var command = new ManageSucesoRelacionadosCommand { IdSuceso = 0 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.IdSuceso)
            .WithErrorMessage("Id Suceso es obligatorio");
    }

    [Fact]
    public void Validate_IdSucesoIsGreaterThanZero_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new ManageSucesoRelacionadosCommand { IdSuceso = 1 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.IdSuceso);
    }
}
