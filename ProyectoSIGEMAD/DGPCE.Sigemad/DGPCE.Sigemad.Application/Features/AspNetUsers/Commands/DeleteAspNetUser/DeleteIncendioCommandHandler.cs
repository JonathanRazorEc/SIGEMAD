using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.AspNetUsers.Commands.DeleteAspNetUser;
using DGPCE.Sigemad.Application.Specifications.AspNetUsers;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.AspNetUsers.Commands.DeleteAspNetUser;

public class DeleteAspNetUserCommandHandler : IRequestHandler<DeleteAspNetUserCommand>
{
    private readonly ILogger<DeleteAspNetUserCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteAspNetUserCommandHandler(
        ILogger<DeleteAspNetUserCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(DeleteAspNetUserCommand request, CancellationToken cancellationToken)
    {
        var spec = new AspNetUserByIdSpec(request.Id);
        var userToDelete = await _unitOfWork.Repository<AspNetUser>().GetByIdWithSpec(spec);

        if (userToDelete is null)
        {
            _logger.LogWarning($"El AspNetUser con id:{request.Id}, no existe en la base de datos");
            throw new NotFoundException(nameof(Incendio), request.Id);
        }

        _unitOfWork.Repository<AspNetUser>().DeleteEntity(userToDelete);

        await _unitOfWork.Complete();
        _logger.LogInformation($"El AspNetUser con id: {request.Id}, se actualizo estado de borrado con éxito");

        return Unit.Value;
    }
}
