using DGPCE.Sigemad.Application.Dtos.DeclaracionesZAGEP;
using DGPCE.Sigemad.Application.Dtos.NotificacionesEmergencias;
using DGPCE.Sigemad.Application.Features.DeclaracionesZAGEP.Commands.ManageDeclaracionesZAGEP;
using DGPCE.Sigemad.Application.Features.NotificacionesEmergencias.Commands.ManageNotificacionEmergencia;
using DGPCE.Sigemad.Application.Resources;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Localization;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Tests.Features.NotificacionesEmergencias.Commands;
public class ManageNotificacionEmergenciaCommandValidatorTest
{
    private readonly Mock<IStringLocalizer<ValidationMessages>> _localizerMock;
    private readonly ManageNotificacionEmergenciaValidator _validator;

    public ManageNotificacionEmergenciaCommandValidatorTest()
    {
        _localizerMock = new Mock<IStringLocalizer<ValidationMessages>>();
        _localizerMock.Setup(l => l["IdSucesoObligatorio"]).Returns(new LocalizedString("IdSucesoObligatorio", "IdSuceso es obligatorio"));
        _localizerMock.Setup(l => l["IdTipoNotificacionObligatorio"]).Returns(new LocalizedString("IdTipoNotificacionObligatorio", "IdTipoNotificacion es obligatoria"));
        _localizerMock.Setup(l => l["FechaHoraNotificacionObligatorio"]).Returns(new LocalizedString("FechaHoraNotificacionObligatorio", "FechaHoraNotificacion es obligatoria"));
        _localizerMock.Setup(l => l["OrganosNotificadosObligatorio"]).Returns(new LocalizedString("OrganosNotificadosObligatorio", "OrganosNotificados es obligatoria"));
        _localizerMock.Setup(l => l["OrganosNotificadosMaxLength"]).Returns(new LocalizedString("OrganosNotificadosMaxLength", "La longitud máxima de los Organos Notificados es 510 caracteres"));

        _validator = new ManageNotificacionEmergenciaValidator(_localizerMock.Object);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenIdSucesoIsZero()
    {
        // Arrange
        var command = new ManageNotificacionEmergenciaCommand { IdSuceso = 0 };

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
        var command = new ManageNotificacionEmergenciaCommand
        {
            IdSuceso = 1,
            Detalles = new List<ManageNotificacionEmergenciaDto>
                {
                    new ManageNotificacionEmergenciaDto
                    {
                        OrganismoInternacional = "3"
                    }
                }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Detalles[0].IdTipoNotificacion")
              .WithErrorMessage("IdTipoNotificacion es obligatoria");
        result.ShouldHaveValidationErrorFor("Detalles[0].FechaHoraNotificacion")
              .WithErrorMessage("FechaHoraNotificacion es obligatoria");
    }


    [Fact]
    public void Validate_ShouldNotHaveError_WhenDetallesIsNullOrEmpty()
    {
        // Arrange
        var commandWithNullDetalles = new ManageNotificacionEmergenciaCommand
        {
            IdSuceso = 1,
            Detalles = null
        };

        var commandWithEmptyDetalles = new ManageNotificacionEmergenciaCommand
        {
            IdSuceso = 1,
            Detalles = new List<ManageNotificacionEmergenciaDto>()
        };

        // Act
        var resultWithNullDetalles = _validator.TestValidate(commandWithNullDetalles);
        var resultWithEmptyDetalles = _validator.TestValidate(commandWithEmptyDetalles);

        // Assert
        resultWithNullDetalles.ShouldNotHaveValidationErrorFor(c => c.Detalles);
        resultWithEmptyDetalles.ShouldNotHaveValidationErrorFor(c => c.Detalles);
    }
}

