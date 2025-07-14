//using AutoMapper;
//using DGPCE.Sigemad.Application.Contracts.Persistence;
//using DGPCE.Sigemad.Application.Exceptions;
//using DGPCE.Sigemad.Application.Features.Incendios.Commands.CreateIncendios;
//using DGPCE.Sigemad.Application.Mappings;
//using DGPCE.Sigemad.Domain.Constracts;
//using DGPCE.Sigemad.Domain.Modelos;
//using FluentAssertions;
//using Microsoft.Extensions.Logging;
//using Moq;
//using NetTopologySuite.Geometries;

//namespace DGPCE.Sigemad.Application.Tests.Features.Incendios.Commands;
//public class CreateIncendioCommandHandlerTests
//{
//    private readonly Mock<ILogger<CreateIncendioCommandHandler>> _loggerMock;
//    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
//    private readonly IMapper _mapper;
//    private readonly Mock<IGeometryValidator> _geometryValidatorMock;
//    private readonly Mock<ICoordinateTransformationService> _coordinateTransformationServiceMock;

//    public CreateIncendioCommandHandlerTests()
//    {
//        _loggerMock = new Mock<ILogger<CreateIncendioCommandHandler>>();
//        _unitOfWorkMock = new Mock<IUnitOfWork>();
//        _geometryValidatorMock = new Mock<IGeometryValidator>();
//        _coordinateTransformationServiceMock = new Mock<ICoordinateTransformationService>();

//        var mapperConfig = new MapperConfiguration(cfg =>
//        {
//            cfg.AddProfile<MappingProfile>();
//        });
//        _mapper = mapperConfig.CreateMapper();
//    }

//    [Fact]
//    public async Task Handle_GivenValidCommand_ShouldReturnSuccess()
//    {
//        // Arrange

//        var command = new CreateIncendioCommand
//        {
//            IdTerritorio = 1,
//            IdPais = 1,
//            IdProvincia = 1,
//            IdMunicipio = 1,
//            Denominacion = "Test Incendio",
//            FechaInicio = DateTime.UtcNow,
//            IdTipoSuceso = 1,
//            IdClaseSuceso = 1,
//            IdEstadoSuceso = 1,
//            Contenido = "Contenido de prueba",
//            Comentarios = "Comentarios de prueba",
//            RutaMapaRiesgo = "Ruta/Mapa/Riesgo",
//            GeoPosicion = new Point(-2, 42) { SRID = 4326 }
//        };

//        var territorio = new Territorio { Id = 1 };
//        var pais = new Pais { Id = 1 };
//        var provincia = new Provincia { Id = 1 };
//        var municipio = new Municipio { Id = 1 };
//        var tipoSuceso = new TipoSuceso { Id = 1 };
//        var claseSuceso = new ClaseSuceso { Id = 1 };
//        var estadoSuceso = new EstadoSuceso { Id = 1 };

//        _unitOfWorkMock.Setup(u => u.Repository<Territorio>().GetByIdAsync(command.IdTerritorio))
//            .ReturnsAsync(territorio);
//        _unitOfWorkMock.Setup(u => u.Repository<Pais>().GetByIdAsync(command.IdPais))
//            .ReturnsAsync(pais);
//        _unitOfWorkMock.Setup(u => u.Repository<Provincia>().GetByIdAsync(command.IdProvincia))
//            .ReturnsAsync(provincia);
//        _unitOfWorkMock.Setup(u => u.Repository<Municipio>().GetByIdAsync(command.IdMunicipio))
//            .ReturnsAsync(municipio);
//        _unitOfWorkMock.Setup(u => u.Repository<TipoSuceso>().GetByIdAsync(command.IdTipoSuceso))
//            .ReturnsAsync(tipoSuceso);
//        _unitOfWorkMock.Setup(u => u.Repository<ClaseSuceso>().GetByIdAsync(command.IdClaseSuceso))
//            .ReturnsAsync(claseSuceso);
//        _unitOfWorkMock.Setup(u => u.Repository<EstadoSuceso>().GetByIdAsync(command.IdEstadoSuceso))
//            .ReturnsAsync(estadoSuceso);

//        _geometryValidatorMock.Setup(g => g.IsGeometryValidAndInEPSG4326(It.IsAny<Geometry>()))
//            .Returns(true);

//        _coordinateTransformationServiceMock.Setup(u => u.ConvertToUTM(It.IsAny<Geometry>()))
//            .Returns((Geometry geometry) => (500000.0, 4649776.22482, 30));

//        _unitOfWorkMock.Setup(u => u.Repository<Incendio>().AddEntity(It.IsAny<Incendio>()))
//            .Callback<Incendio>(incendio =>
//            {
//                incendio.Id = 1;
//            });

//        _unitOfWorkMock.Setup(u => u.Complete())
//            .ReturnsAsync(1);

//        var handler = new CreateIncendioCommandHandler(
//            _loggerMock.Object,
//            _unitOfWorkMock.Object,
//            _geometryValidatorMock.Object,
//            _coordinateTransformationServiceMock.Object
//            );

//        // Act
//        var result = await handler.Handle(command, CancellationToken.None);

//        // Assert
//        result.Should().NotBeNull();
//        result.Should().BeOfType<CreateIncendioResponse>();
//        result.Id.Should().Be(1);
//    }

//    [Fact]
//    public async Task Handle_GivenInvalidTerritorioId_ShouldThrowNotFoundException()
//    {
//        // Arrange

//        var command = new CreateIncendioCommand
//        {
//            IdTerritorio = 1,
//            IdPais = 1,
//            IdProvincia = 1,
//            IdMunicipio = 1,
//            Denominacion = "Test Incendio",
//            FechaInicio = DateTime.UtcNow,
//            IdTipoSuceso = 1,
//            IdClaseSuceso = 1,
//            IdEstadoSuceso = 1,
//            Contenido = "Contenido de prueba",
//            Comentarios = "Comentarios de prueba",
//            RutaMapaRiesgo = "Ruta/Mapa/Riesgo",
//            GeoPosicion = new Point(-2, 42) { SRID = 4326 }
//        };

//        _unitOfWorkMock.Setup(u => u.Repository<Territorio>().GetByIdAsync(command.IdTerritorio))
//            .ReturnsAsync((Territorio)null);

//        var handler = new CreateIncendioCommandHandler(
//            _loggerMock.Object,
//            _unitOfWorkMock.Object,
//            _geometryValidatorMock.Object,
//            _coordinateTransformationServiceMock.Object
//            );

//        // Act
//        Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

//        // Assert
//        await action.Should().ThrowAsync<NotFoundException>()
//            .WithMessage($"*{nameof(Territorio)}*");
//    }

//    [Fact]
//    public async Task Handle_GivenInvalidPaisId_ShouldThrowNotFoundException()
//    {
//        // Arrange

//        var command = new CreateIncendioCommand
//        {
//            IdTerritorio = 1,
//            IdPais = 1,
//            IdProvincia = 1,
//            IdMunicipio = 1,
//            Denominacion = "Test Incendio",
//            FechaInicio = DateTime.UtcNow,
//            IdTipoSuceso = 1,
//            IdClaseSuceso = 1,
//            IdEstadoSuceso = 1,
//            Contenido = "Contenido de prueba",
//            Comentarios = "Comentarios de prueba",
//            RutaMapaRiesgo = "Ruta/Mapa/Riesgo",
//            GeoPosicion = new Point(-2, 42) { SRID = 4326 }
//        };

//        var territorio = new Territorio { Id = 1 };

//        _unitOfWorkMock.Setup(u => u.Repository<Territorio>().GetByIdAsync(command.IdTerritorio))
//            .ReturnsAsync(territorio);

//        _unitOfWorkMock.Setup(u => u.Repository<Pais>().GetByIdAsync(command.IdPais))
//            .ReturnsAsync((Pais)null);

//        var handler = new CreateIncendioCommandHandler(
//            _loggerMock.Object,
//            _unitOfWorkMock.Object,
//            _geometryValidatorMock.Object,
//            _coordinateTransformationServiceMock.Object
//            );

//        // Act
//        Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

//        // Assert
//        await action.Should().ThrowAsync<NotFoundException>()
//            .WithMessage($"*{nameof(Pais)}*");
//    }

//    [Fact]
//    public async Task Handle_GivenInvalidProvinciaId_ShouldThrowNotFoundException()
//    {
//        // Arrange

//        var command = new CreateIncendioCommand
//        {
//            IdTerritorio = 1,
//            IdPais = 1,
//            IdProvincia = 1,
//            IdMunicipio = 1,
//            Denominacion = "Test Incendio",
//            FechaInicio = DateTime.UtcNow,
//            IdTipoSuceso = 1,
//            IdClaseSuceso = 1,
//            IdEstadoSuceso = 1,
//            Contenido = "Contenido de prueba",
//            Comentarios = "Comentarios de prueba",
//            RutaMapaRiesgo = "Ruta/Mapa/Riesgo",
//            GeoPosicion = new Point(-2, 42) { SRID = 4326 }
//        };

//        var territorio = new Territorio { Id = 1 };
//        var pais = new Pais { Id = 1 };

//        _unitOfWorkMock.Setup(u => u.Repository<Territorio>().GetByIdAsync(command.IdTerritorio))
//            .ReturnsAsync(territorio);
//        _unitOfWorkMock.Setup(u => u.Repository<Pais>().GetByIdAsync(command.IdPais))
//            .ReturnsAsync(pais);
//        _unitOfWorkMock.Setup(u => u.Repository<Provincia>().GetByIdAsync(command.IdProvincia))
//            .ReturnsAsync((Provincia)null);

//        var handler = new CreateIncendioCommandHandler(
//            _loggerMock.Object,
//            _unitOfWorkMock.Object,
//            _geometryValidatorMock.Object,
//            _coordinateTransformationServiceMock.Object
//            );

//        // Act
//        Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

//        // Assert
//        await action.Should().ThrowAsync<NotFoundException>()
//            .WithMessage($"*{nameof(Provincia)}*");
//    }

//    [Fact]
//    public async Task Handle_GivenInvalidMunicipioId_ShouldThrowNotFoundException()
//    {
//        // Arrange

//        var command = new CreateIncendioCommand
//        {
//            IdTerritorio = 1,
//            IdPais = 1,
//            IdProvincia = 1,
//            IdMunicipio = 1,
//            Denominacion = "Test Incendio",
//            FechaInicio = DateTime.UtcNow,
//            IdTipoSuceso = 1,
//            IdClaseSuceso = 1,
//            IdEstadoSuceso = 1,
//            Contenido = "Contenido de prueba",
//            Comentarios = "Comentarios de prueba",
//            RutaMapaRiesgo = "Ruta/Mapa/Riesgo",
//            GeoPosicion = new Point(-2, 42) { SRID = 4326 }
//        };

//        var territorio = new Territorio { Id = 1 };
//        var pais = new Pais { Id = 1 };
//        var provincia = new Provincia { Id = 1 };

//        _unitOfWorkMock.Setup(u => u.Repository<Territorio>().GetByIdAsync(command.IdTerritorio))
//            .ReturnsAsync(territorio);
//        _unitOfWorkMock.Setup(u => u.Repository<Pais>().GetByIdAsync(command.IdPais))
//            .ReturnsAsync(pais);
//        _unitOfWorkMock.Setup(u => u.Repository<Provincia>().GetByIdAsync(command.IdProvincia))
//            .ReturnsAsync(provincia);
//        _unitOfWorkMock.Setup(u => u.Repository<Municipio>().GetByIdAsync(command.IdMunicipio))
//            .ReturnsAsync((Municipio)null);

//        var handler = new CreateIncendioCommandHandler(
//            _loggerMock.Object,
//            _unitOfWorkMock.Object,
//            _geometryValidatorMock.Object,
//            _coordinateTransformationServiceMock.Object
//            );

//        // Act
//        Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

//        // Assert
//        await action.Should().ThrowAsync<NotFoundException>()
//            .WithMessage($"*{nameof(Municipio)}*");
//    }

//    [Fact]
//    public async Task Handle_GivenInvalidTipoSucesoId_ShouldThrowNotFoundException()
//    {
//        // Arrange

//        var command = new CreateIncendioCommand
//        {
//            IdTerritorio = 1,
//            IdPais = 1,
//            IdProvincia = 1,
//            IdMunicipio = 1,
//            Denominacion = "Test Incendio",
//            FechaInicio = DateTime.UtcNow,
//            IdTipoSuceso = 1,
//            IdClaseSuceso = 1,
//            IdEstadoSuceso = 1,
//            Contenido = "Contenido de prueba",
//            Comentarios = "Comentarios de prueba",
//            RutaMapaRiesgo = "Ruta/Mapa/Riesgo",
//            GeoPosicion = new Point(-2, 42) { SRID = 4326 }
//        };

//        var territorio = new Territorio { Id = 1 };
//        var pais = new Pais { Id = 1 };
//        var provincia = new Provincia { Id = 1 };
//        var municipio = new Municipio { Id = 1 };

//        _unitOfWorkMock.Setup(u => u.Repository<Territorio>().GetByIdAsync(command.IdTerritorio))
//            .ReturnsAsync(territorio);
//        _unitOfWorkMock.Setup(u => u.Repository<Pais>().GetByIdAsync(command.IdPais))
//            .ReturnsAsync(pais);
//        _unitOfWorkMock.Setup(u => u.Repository<Provincia>().GetByIdAsync(command.IdProvincia))
//            .ReturnsAsync(provincia);
//        _unitOfWorkMock.Setup(u => u.Repository<Municipio>().GetByIdAsync(command.IdMunicipio))
//            .ReturnsAsync(municipio);
//        _unitOfWorkMock.Setup(u => u.Repository<TipoSuceso>().GetByIdAsync(command.IdTipoSuceso))
//            .ReturnsAsync((TipoSuceso)null);

//        var handler = new CreateIncendioCommandHandler(
//            _loggerMock.Object,
//            _unitOfWorkMock.Object,
//            _geometryValidatorMock.Object,
//            _coordinateTransformationServiceMock.Object
//            );

//        // Act
//        Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

//        // Assert
//        await action.Should().ThrowAsync<NotFoundException>()
//            .WithMessage($"*{nameof(TipoSuceso)}*");
//    }

//    [Fact]
//    public async Task Handle_GivenInvalidClaseSucesoId_ShouldThrowNotFoundException()
//    {
//        // Arrange

//        var command = new CreateIncendioCommand
//        {
//            IdTerritorio = 1,
//            IdPais = 1,
//            IdProvincia = 1,
//            IdMunicipio = 1,
//            Denominacion = "Test Incendio",
//            FechaInicio = DateTime.UtcNow,
//            IdTipoSuceso = 1,
//            IdClaseSuceso = 1,
//            IdEstadoSuceso = 1,
//            Contenido = "Contenido de prueba",
//            Comentarios = "Comentarios de prueba",
//            RutaMapaRiesgo = "Ruta/Mapa/Riesgo",
//            GeoPosicion = new Point(-2, 42) { SRID = 4326 }
//        };

//        var territorio = new Territorio { Id = 1 };
//        var pais = new Pais { Id = 1 };
//        var provincia = new Provincia { Id = 1 };
//        var municipio = new Municipio { Id = 1 };
//        var tipoSuceso = new TipoSuceso { Id = 1 };

//        _unitOfWorkMock.Setup(u => u.Repository<Territorio>().GetByIdAsync(command.IdTerritorio))
//            .ReturnsAsync(territorio);
//        _unitOfWorkMock.Setup(u => u.Repository<Pais>().GetByIdAsync(command.IdPais))
//            .ReturnsAsync(pais);
//        _unitOfWorkMock.Setup(u => u.Repository<Provincia>().GetByIdAsync(command.IdProvincia))
//            .ReturnsAsync(provincia);
//        _unitOfWorkMock.Setup(u => u.Repository<Municipio>().GetByIdAsync(command.IdMunicipio))
//            .ReturnsAsync(municipio);
//        _unitOfWorkMock.Setup(u => u.Repository<TipoSuceso>().GetByIdAsync(command.IdTipoSuceso))
//            .ReturnsAsync(tipoSuceso);
//        _unitOfWorkMock.Setup(u => u.Repository<ClaseSuceso>().GetByIdAsync(command.IdClaseSuceso))
//            .ReturnsAsync((ClaseSuceso)null);

//        var handler = new CreateIncendioCommandHandler(
//            _loggerMock.Object,
//            _unitOfWorkMock.Object,
//            _geometryValidatorMock.Object,
//            _coordinateTransformationServiceMock.Object
//            );

//        // Act
//        Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

//        // Assert
//        await action.Should().ThrowAsync<NotFoundException>()
//            .WithMessage($"*{nameof(ClaseSuceso)}*");
//    }

//    [Fact]
//    public async Task Handle_GivenInvalidEstadoSucesoId_ShouldThrowNotFoundException()
//    {
//        // Arrange

//        var command = new CreateIncendioCommand
//        {
//            IdTerritorio = 1,
//            IdPais = 1,
//            IdProvincia = 1,
//            IdMunicipio = 1,
//            Denominacion = "Test Incendio",
//            FechaInicio = DateTime.UtcNow,
//            IdTipoSuceso = 1,
//            IdClaseSuceso = 1,
//            IdEstadoSuceso = 1,
//            Contenido = "Contenido de prueba",
//            Comentarios = "Comentarios de prueba",
//            RutaMapaRiesgo = "Ruta/Mapa/Riesgo",
//            GeoPosicion = new Point(-2, 42) { SRID = 4326 }
//        };

//        var territorio = new Territorio { Id = 1 };
//        var pais = new Pais { Id = 1 };
//        var provincia = new Provincia { Id = 1 };
//        var municipio = new Municipio { Id = 1 };
//        var tipoSuceso = new TipoSuceso { Id = 1 };
//        var claseSuceso = new ClaseSuceso { Id = 1 };

//        _unitOfWorkMock.Setup(u => u.Repository<Territorio>().GetByIdAsync(command.IdTerritorio))
//            .ReturnsAsync(territorio);
//        _unitOfWorkMock.Setup(u => u.Repository<Pais>().GetByIdAsync(command.IdPais))
//            .ReturnsAsync(pais);
//        _unitOfWorkMock.Setup(u => u.Repository<Provincia>().GetByIdAsync(command.IdProvincia))
//            .ReturnsAsync(provincia);
//        _unitOfWorkMock.Setup(u => u.Repository<Municipio>().GetByIdAsync(command.IdMunicipio))
//            .ReturnsAsync(municipio);
//        _unitOfWorkMock.Setup(u => u.Repository<TipoSuceso>().GetByIdAsync(command.IdTipoSuceso))
//            .ReturnsAsync(tipoSuceso);
//        _unitOfWorkMock.Setup(u => u.Repository<ClaseSuceso>().GetByIdAsync(command.IdClaseSuceso))
//            .ReturnsAsync(claseSuceso);
//        _unitOfWorkMock.Setup(u => u.Repository<EstadoSuceso>().GetByIdAsync(command.IdEstadoSuceso))
//            .ReturnsAsync((EstadoSuceso)null);

//        var handler = new CreateIncendioCommandHandler(
//            _loggerMock.Object,
//            _unitOfWorkMock.Object,
//            _geometryValidatorMock.Object,
//            _coordinateTransformationServiceMock.Object
//            );

//        // Act
//        Func<Task> action = async () => await handler.Handle(command, CancellationToken.None);

//        // Assert
//        await action.Should().ThrowAsync<NotFoundException>()
//            .WithMessage($"*{nameof(EstadoSuceso)}*");
//    }
//}
