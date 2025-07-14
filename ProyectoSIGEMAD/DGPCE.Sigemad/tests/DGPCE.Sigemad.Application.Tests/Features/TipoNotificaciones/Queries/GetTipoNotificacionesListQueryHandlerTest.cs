using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.TipoNotificaciones.Queries.GetTipoNotificacionesList;
using DGPCE.Sigemad.Domain.Modelos;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Tests.Features.TipoNotificaciones.Queries;
public class GetTipoNotificacionesListQueryHandlerTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetTipoNotificacionesListQueryHandler _handler;

    public GetTipoNotificacionesListQueryHandlerTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new GetTipoNotificacionesListQueryHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ValidQuery_ShouldReturnListTiposRegistros()
    {
        // Arrange
        var request = new GetTipoNotificacionesListQuery();

        var tipoNotificaciones = new List<TipoNotificacion>
        {
            new TipoNotificacion { Id = 1, Descripcion = "Notificacion 1" },
            new TipoNotificacion { Id = 2, Descripcion = "Notificacion 2" }
        };

        _unitOfWorkMock.Setup(m => m.Repository<TipoNotificacion>().GetAllAsync()).ReturnsAsync(tipoNotificaciones);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        result[0].Id.Should().Be(1);
        result[0].Descripcion.Should().Be("Notificacion 1");
        result[1].Id.Should().Be(2);
        result[1].Descripcion.Should().Be("Notificacion 2");
    }

    [Fact]
    public async Task Handle_WithNoTiposRegistros_ShouldReturnEmptyList()
    {
        // Arrange
        var query = new GetTipoNotificacionesListQuery();
        var tipoNotificacionesList = new List<TipoNotificacion>();

        _unitOfWorkMock.Setup(m => m.Repository<TipoNotificacion>().GetAllAsync()).ReturnsAsync(tipoNotificacionesList);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_WithNullTiposRegistros_ShouldReturnEmptyList()
    {
        // Arrange
        var request = new GetTipoNotificacionesListQuery();

        _unitOfWorkMock.Setup(m => m.Repository<TipoNotificacion>().GetAllAsync())
            .ReturnsAsync((IReadOnlyList<TipoNotificacion>)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}
