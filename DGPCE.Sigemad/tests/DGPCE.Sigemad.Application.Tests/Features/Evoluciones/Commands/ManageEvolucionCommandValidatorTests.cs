using DGPCE.Sigemad.Application.Features.DatosPrincipales.Commands;
using DGPCE.Sigemad.Application.Features.Parametros.Commands;
using DGPCE.Sigemad.Application.Features.Registros.Command.CreateRegistros;
using DGPCE.Sigemad.Application.Resources;
using FluentValidation.TestHelper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Tests.Features.Evoluciones.Commands;

public class ManageEvolucionCommandValidatorTests
{
    private readonly ManageParametroCommandValidator _validator;
    private readonly IStringLocalizer<ValidationMessages> _localizer;

    public ManageEvolucionCommandValidatorTests()
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging() // Agrega esta línea para registrar ILoggerFactory
            .AddLocalization(options => options.ResourcesPath = "Resources")
            .BuildServiceProvider();

        var localizerFactory = serviceProvider.GetService<IStringLocalizerFactory>();
        _localizer = new StringLocalizer<ValidationMessages>(localizerFactory);

        _validator = new ManageParametroCommandValidator(_localizer);
    }

    [Fact]
    public void Validate_IdSucesoIsZero_ShouldHaveValidationError()
    {
        // Arrange
        var command = new ManageParametroCommand { IdSuceso = 0 };

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
        var command = new ManageParametroCommand { IdSuceso = 1 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.IdSuceso);
    }

    [Fact]
    public void Validate_RegistroIsNull_ShouldHaveValidationError()
    {
        // Arrange
        var command = new ManageParametroCommand { Registro = null };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Registro)
            .WithErrorMessage(_localizer["RegistroObligatorio"]);
    }

    [Fact]
    public void Validate_DatoPrincipalIsNull_ShouldHaveValidationError()
    {
        // Arrange
        var command = new ManageParametroCommand { DatoPrincipal = null };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.DatoPrincipal)
            .WithErrorMessage(_localizer["DatoPrincipalObligatorio"]);
    }

    [Fact]
    public void Validate_ParametroIsNull_ShouldHaveValidationError()
    {
        // Arrange
        var command = new ManageParametroCommand { Parametro = null };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Parametro)
            .WithErrorMessage(_localizer["ParametroObligatorio"]);
    }

    [Fact]
    public void Validate_RegistroIsValid_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new ManageParametroCommand
        {
            Registro = new CreateRegistroCommand
            {
                FechaHoraEvolucion = DateTime.Now,
                IdEntradaSalida = 1,
                IdMedio = 1,
                RegistroProcedenciasDestinos = new List<int> { 1, 2 }
            }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Registro);
    }

    [Fact]
    public void Validate_DatoPrincipalIsValid_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new ManageParametroCommand
        {
            DatoPrincipal = new CreateDatoPrincipalCommand
            {
                FechaHora = DateTime.Now
            }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.DatoPrincipal);
    }

    [Fact]
    public void Validate_ParametroIsValid_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new ManageParametroCommand
        {
            Parametro = new CreateParametroCommand
            {
                IdEstadoIncendio = 1
            }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Parametro);
    }
}
