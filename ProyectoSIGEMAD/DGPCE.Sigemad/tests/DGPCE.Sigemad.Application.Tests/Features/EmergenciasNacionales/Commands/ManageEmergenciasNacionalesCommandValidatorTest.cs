using DGPCE.Sigemad.Application.Dtos.EmergenciasNacionales;
using DGPCE.Sigemad.Application.Features.EmergenciasNacionales.Commands.ManageEmergenciasNacionales;
using DGPCE.Sigemad.Application.Resources;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Localization;
using Moq;


namespace DGPCE.Sigemad.Application.Tests.Features.EmergenciasNacionales.Commands;
public class ManageEmergenciasNacionalesCommandValidatorTests
{
    private readonly ManageEmergenciasNacionalesCommandValidator _validator;
    private readonly Mock<IStringLocalizer<ValidationMessages>> _localizerMock;

    public ManageEmergenciasNacionalesCommandValidatorTests()
    {
        _localizerMock = new Mock<IStringLocalizer<ValidationMessages>>();
        _localizerMock.Setup(l => l["IdSucesoObligatorio"]).Returns(new LocalizedString("IdSucesoObligatorio", "El Id del suceso es obligatorio"));
        _localizerMock.Setup(l => l["FechaHoraSolicitud"]).Returns(new LocalizedString("FechaHoraSolicitud", "La fecha y hora de solicitud son obligatorias"));
        _localizerMock.Setup(l => l["AutoridadSolicitanteObligatorio"]).Returns(new LocalizedString("AutoridadSolicitanteObligatorio", "La autoridad solicitante es obligatoria"));
        _localizerMock.Setup(l => l["AutoridadSolicitanteMaxLength"]).Returns(new LocalizedString("AutoridadSolicitanteMaxLength", "La autoridad solicitante no puede exceder los 510 caracteres"));
        _localizerMock.Setup(l => l["DescripcionSolicitudObligatorio"]).Returns(new LocalizedString("DescripcionSolicitudObligatorio", "La descripción de la solicitud es obligatoria"));
        _localizerMock.Setup(l => l["DescripcionSolicitudMaxLength"]).Returns(new LocalizedString("DescripcionSolicitudMaxLength", "La descripción de la solicitud no puede exceder los 510 caracteres"));

        _validator = new ManageEmergenciasNacionalesCommandValidator(_localizerMock.Object);
    }

    [Fact]
    public void Should_Have_Error_When_IdSuceso_Is_Zero()
    {
        var model = new ManageEmergenciasNacionalesCommand { IdSuceso = 0 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.IdSuceso).WithErrorMessage("El Id del suceso es obligatorio");
    }

    [Fact]
    public void Should_Not_Have_Error_When_IdSuceso_Is_Greater_Than_Zero()
    {
        var model = new ManageEmergenciasNacionalesCommand { IdSuceso = 1 };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.IdSuceso);
    }

    [Fact]
    public void Should_Have_Error_When_EmergenciaNacional_Is_Invalid()
    {
        var model = new ManageEmergenciasNacionalesCommand
        {
            IdSuceso = 1,
            EmergenciaNacional = new ManageEmergenciaNacionalDto
            {
                Autoridad = "",
                DescripcionSolicitud = ""
            }
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor("EmergenciaNacional.FechaHoraSolicitud").WithErrorMessage("La fecha y hora de solicitud son obligatorias");
        result.ShouldHaveValidationErrorFor("EmergenciaNacional.Autoridad").WithErrorMessage("La autoridad solicitante es obligatoria");
        result.ShouldHaveValidationErrorFor("EmergenciaNacional.DescripcionSolicitud").WithErrorMessage("La descripción de la solicitud es obligatoria");
    }

    [Fact]
    public void Should_Not_Have_Error_When_EmergenciaNacional_Is_Valid()
    {
        var model = new ManageEmergenciasNacionalesCommand
        {
            IdSuceso = 1,
            EmergenciaNacional = new ManageEmergenciaNacionalDto
            {
                FechaHoraSolicitud = DateTime.Now,
                Autoridad = "Autoridad Valida",
                DescripcionSolicitud = "Descripción válida"
            }
        };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor("EmergenciaNacional.FechaHoraSolicitud");
        result.ShouldNotHaveValidationErrorFor("EmergenciaNacional.Autoridad");
        result.ShouldNotHaveValidationErrorFor("EmergenciaNacional.DescripcionSolicitud");
    }

    [Fact]
    public void Should_Have_Error_When_Autoridad_Exceeds_MaxLength()
    {
        var model = new ManageEmergenciasNacionalesCommand
        {
            IdSuceso = 1,
            EmergenciaNacional = new ManageEmergenciaNacionalDto
            {
                FechaHoraSolicitud = DateTime.Now,
                Autoridad = new string('A', 511),
                DescripcionSolicitud = "Descripción válida"
            }
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor("EmergenciaNacional.Autoridad").WithErrorMessage("La autoridad solicitante no puede exceder los 510 caracteres");
    }

    [Fact]
    public void Should_Have_Error_When_DescripcionSolicitud_Exceeds_MaxLength()
    {
        var model = new ManageEmergenciasNacionalesCommand
        {
            IdSuceso = 1,
            EmergenciaNacional = new ManageEmergenciaNacionalDto
            {
                FechaHoraSolicitud = DateTime.Now,
                Autoridad = "Autoridad Valida",
                DescripcionSolicitud = new string('D', 511)
            }
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor("EmergenciaNacional.DescripcionSolicitud").WithErrorMessage("La descripción de la solicitud no puede exceder los 510 caracteres");
    }

    [Fact]
    public void Should_Not_Have_Error_When_EmergenciaNacional_Is_Null()
    {
        var model = new ManageEmergenciasNacionalesCommand
        {
            IdSuceso = 1,
            EmergenciaNacional = null
        };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.EmergenciaNacional);
    }
}
