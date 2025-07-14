//using DGPCE.Sigemad.Application.Features.Incendios.Commands.CreateIncendios;
//using FluentValidation.TestHelper;
//using NetTopologySuite.Geometries;

//namespace DGPCE.Sigemad.Application.Tests.Features.Incendios.Commands;
//public class CreateIncendioCommandValidatorTests
//{
//    private readonly CreateIncendioCommandValidator _validator;

//    public CreateIncendioCommandValidatorTests()
//    {
//        _validator = new CreateIncendioCommandValidator();
//    }

//    [Fact]
//    public void Validator_WithValidRequest_ShouldNotHaveValidationErrors()
//    {
//        // Arrange
//        var command = new CreateIncendioCommand
//        {
//            IdTerritorio = 1,
//            IdPais = 1,
//            IdProvincia = 1,
//            IdMunicipio = 1,
//            Denominacion = "Incendio de prueba",
//            FechaInicio = DateTime.UtcNow,
//            IdTipoSuceso = 1,
//            IdClaseSuceso = 1,
//            IdEstadoSuceso = 1,
//            Comentarios = "Comentarios de prueba",
//            GeoPosicion = new Point(1, 1) { SRID = 4326 }
//        };

//        // Act
//        var result = _validator.TestValidate(command);

//        // Assert
//        result.ShouldNotHaveAnyValidationErrors();
//    }

//    [Fact]
//    public void Validator_WithEmptyIdTerritorio_ShouldHaveValidationError()
//    {
//        // Arrange
//        var command = new CreateIncendioCommand
//        {
//            IdPais = 1,
//            IdProvincia = 1,
//            IdMunicipio = 1,
//            Denominacion = "Incendio de prueba",
//            FechaInicio = DateTime.UtcNow,
//            IdTipoSuceso = 1,
//            IdClaseSuceso = 1,
//            IdEstadoSuceso = 1,
//            Comentarios = "Comentarios de prueba",
//            GeoPosicion = new Point(1, 1) { SRID = 4326 }
//        };

//        // Act
//        var result = _validator.TestValidate(command);

//        // Assert
//        result.ShouldHaveValidationErrorFor(c => c.IdTerritorio)
//            .WithErrorMessage("Es obligatorio y debe ser mayor a 0");
//    }

//    [Fact]
//    public void Validator_WithInvalidDenominacionLength_ShouldHaveValidationError()
//    {
//        // Arrange
//        var command = new CreateIncendioCommand
//        {
//            IdTerritorio = 1,
//            IdPais = 1,
//            IdProvincia = 1,
//            IdMunicipio = 1,
//            Denominacion = new string('a', 300), // Length greater than 255
//            FechaInicio = DateTime.UtcNow,
//            IdTipoSuceso = 1,
//            IdClaseSuceso = 1,
//            IdEstadoSuceso = 1,
//            Comentarios = "Comentarios de prueba",
//            GeoPosicion = new Point(1, 1) { SRID = 4326 }
//        };

//        // Act
//        var result = _validator.TestValidate(command);

//        // Assert
//        result.ShouldHaveValidationErrorFor(c => c.Denominacion)
//            .WithErrorMessage("Denominacion no puede exceder los 255 caracteres");
//    }

//    [Fact]
//    public void Validator_WithInvalidGeoPosicion_ShouldHaveValidationError()
//    {
//        // Arrange
//        var command = new CreateIncendioCommand
//        {
//            IdTerritorio = 1,
//            IdPais = 1,
//            IdProvincia = 1,
//            IdMunicipio = 1,
//            Denominacion = "Incendio de prueba",
//            FechaInicio = DateTime.UtcNow,
//            IdTipoSuceso = 1,
//            IdClaseSuceso = 1,
//            IdEstadoSuceso = 1,
//            Comentarios = "Comentarios de prueba",
//            GeoPosicion = new Point(3330, 1) { SRID = 3857 } // Wrong SRID (not WGS84)
//        };

//        // Act
//        var result = _validator.TestValidate(command);

//        // Assert
//        result.ShouldHaveValidationErrorFor(c => c.GeoPosicion)
//            .WithErrorMessage("La geometría no es válida, sistema de referencia no es Wgs84");
//    }

//    [Fact]
//    public void Validator_WithEmptyDenominacion_ShouldHaveValidationError()
//    {
//        // Arrange
//        var command = new CreateIncendioCommand
//        {
//            IdTerritorio = 1,
//            IdPais = 1,
//            IdProvincia = 1,
//            IdMunicipio = 1,
//            Denominacion = "",
//            FechaInicio = DateTime.UtcNow,
//            IdTipoSuceso = 1,
//            IdClaseSuceso = 1,
//            IdEstadoSuceso = 1,
//            Comentarios = "Comentarios de prueba",
//            GeoPosicion = new Point(1, 1) { SRID = 4326 }
//        };

//        // Act
//        var result = _validator.TestValidate(command);

//        // Assert
//        result.ShouldHaveValidationErrorFor(c => c.Denominacion)
//            .WithErrorMessage("Denominacion no puede estar en blanco");
//    }


//}
