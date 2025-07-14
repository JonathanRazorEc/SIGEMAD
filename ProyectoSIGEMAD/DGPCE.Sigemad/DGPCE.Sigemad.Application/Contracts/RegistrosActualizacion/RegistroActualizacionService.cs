using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
using DGPCE.Sigemad.Domain.Common;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
public class RegistroActualizacionService : IRegistroActualizacionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegistroActualizacionService(
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task ValidarSuceso(int idSuceso)
    {
        var suceso = await _unitOfWork.Repository<Suceso>().GetByIdAsync(idSuceso);
        if (suceso is null || suceso.Borrado)
            throw new NotFoundException(nameof(Suceso), idSuceso);
    }

    public async Task<RegistroActualizacion> GetOrCreateRegistroActualizacion<T>(int? idRegistroActualizacion, int idSuceso, TipoRegistroActualizacionEnum tipoRegistro) where T : BaseDomainModel<int>
    {
        if (idRegistroActualizacion.HasValue && idRegistroActualizacion.Value > 0)
        {
            var spec = new RegistroActualizacionSpecification(new RegistroActualizacionSpecificationParams
            {
                Id = idRegistroActualizacion.Value,
                IdTipoRegistroActualizacion = (int)tipoRegistro,
            });
            var registroActualizacion = await _unitOfWork.Repository<RegistroActualizacion>().GetByIdWithSpec(spec);

            if (registroActualizacion is null)
                throw new NotFoundException(nameof(RegistroActualizacion), idRegistroActualizacion);
            return registroActualizacion;
        }

        return new RegistroActualizacion
        {
            IdTipoRegistroActualizacion = (int)tipoRegistro,
            IdSuceso = idSuceso,
            TipoEntidad = typeof(T).Name
        };
    }

    public async Task ReflectNewRegistrosInFuture<T>(
        RegistroActualizacion registroActualizacion,
        ApartadoRegistroEnum apartadoRegistro,
        List<int> nuevasReferenciasIds)
        where T : BaseDomainModel<int>
    {
        if (!nuevasReferenciasIds.Any()) return;

        var spec = new RegistroActualizacionSpecification(new RegistroActualizacionSpecificationParams
        {
            IdMinimo = registroActualizacion.Id,
            IdSuceso = registroActualizacion.IdSuceso,
            IdTipoRegistroActualizacion = registroActualizacion.IdTipoRegistroActualizacion
        });
        var registrosPosteriores = await _unitOfWork.Repository<RegistroActualizacion>().GetAllWithSpec(spec);

        foreach (var registroPosterior in registrosPosteriores)
        {
            bool seActualizoRegistroPosterior = false;
            var detallesPrevios = registroPosterior.DetallesRegistro;

            foreach (var idReferencia in nuevasReferenciasIds)
            {
                if (detallesPrevios.Any(d => d.IdReferencia == idReferencia)) continue;

                var nuevoDetalle = new DetalleRegistroActualizacion
                {
                    IdRegistroActualizacion = registroPosterior.Id,
                    IdApartadoRegistro = (int)apartadoRegistro,
                    IdReferencia = idReferencia,
                    IdEstadoRegistro = EstadoRegistroEnum.CreadoEnRegistroAnterior
                };

                registroPosterior.DetallesRegistro.Add(nuevoDetalle);
                seActualizoRegistroPosterior = true;
            }

            if (seActualizoRegistroPosterior)
                _unitOfWork.Repository<RegistroActualizacion>().UpdateEntity(registroPosterior);
        }
    }

    public async Task SaveRegistroActualizacion<T, E, G>(
        RegistroActualizacion registroActualizacion,
        T entidad,
        ApartadoRegistroEnum apartadoRegistro,
        List<int> referenciasParaEliminar,
        Dictionary<int, G> entidadesOriginales)
        where T : BaseDomainModel<int>
        where E : BaseDomainModel<int>
        where G : class
    {

        registroActualizacion.IdReferencia = entidad.Id;
        var nuevasReferenciasIds = new List<int>();

        // Obtener lista
        var propiedadLista = entidad.GetType().GetProperties().FirstOrDefault(p => p.PropertyType == typeof(List<E>));

        if (propiedadLista != null)
        {
            var lista = propiedadLista.GetValue(entidad) as System.Collections.IEnumerable;
            if (lista != null)
            {
                foreach (E item in lista)
                {
                    var estado = GetEstadoRegistro<E, G>(item, apartadoRegistro, registroActualizacion.DetallesRegistro, entidadesOriginales, referenciasParaEliminar);

                    var detalleExistente = registroActualizacion.DetallesRegistro
                        .FirstOrDefault(d => d.IdReferencia == item.Id && d.IdApartadoRegistro == (int)apartadoRegistro);

                    if (detalleExistente != null)
                    {
                        if (estado == EstadoRegistroEnum.Permanente) continue;

                        if(estado == EstadoRegistroEnum.Eliminado)
                        {
                            estado = (detalleExistente.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado ||
                            detalleExistente.IdEstadoRegistro == EstadoRegistroEnum.Creado) ? EstadoRegistroEnum.CreadoYEliminado : EstadoRegistroEnum.Eliminado;
                        }

                        detalleExistente.IdEstadoRegistro = estado;
                    }
                    else
                    {

                        (string ambito, string descripcion) =  GetAmbitoYDescripcion<E>(item);

                        registroActualizacion.DetallesRegistro.Add(new DetalleRegistroActualizacion
                        {
                            IdApartadoRegistro = (int)apartadoRegistro,
                            IdReferencia = item.Id,
                            IdEstadoRegistro = estado,
                            Ambito = ambito,
                            Descripcion = descripcion
                        });

                        if (estado == EstadoRegistroEnum.Creado)
                        {
                            nuevasReferenciasIds.Add(item.Id);
                        }
                    }
                }
            }
        }

        if (registroActualizacion.Id > 0)
        {
            await ReflectNewRegistrosInFuture<T>(registroActualizacion, apartadoRegistro, nuevasReferenciasIds);
            _unitOfWork.Repository<RegistroActualizacion>().UpdateEntity(registroActualizacion);
        }
        else
        {
            _unitOfWork.Repository<RegistroActualizacion>().AddEntity(registroActualizacion);
        }

        var afectadas = await _unitOfWork.Complete();
        if (afectadas <= 0)
            throw new Exception("No se pudo insertar/actualizar registros de actualizaciones");

    }


    private EstadoRegistroEnum GetEstadoRegistro<E, G>(
    E entidad,
    ApartadoRegistroEnum apartadoRegistro,
    IEnumerable<DetalleRegistroActualizacion> detallesPrevios,
    Dictionary<int, G> entidadesOriginales,
    List<int> referenciasParaEliminar)
        where E : BaseDomainModel<int>
        where G : class
    {
        if (referenciasParaEliminar.Contains(entidad.Id))
        {
            return EstadoRegistroEnum.Eliminado;
        }

        if (!entidadesOriginales.ContainsKey(entidad.Id))
        {
            return EstadoRegistroEnum.Creado;
        }

        var detallePrevio = detallesPrevios
            .FirstOrDefault(d => d.IdReferencia == entidad.Id && d.IdApartadoRegistro == (int)apartadoRegistro);

        var copiaOriginal = entidadesOriginales[entidad.Id];
        var copiaNueva = _mapper.Map<G>(entidad);

        if (detallePrevio != null)
        {
            if (!copiaOriginal.Equals(copiaNueva))
            {
                if (detallePrevio.IdEstadoRegistro == EstadoRegistroEnum.Creado ||
                    detallePrevio.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado)
                    return EstadoRegistroEnum.CreadoYModificado;
                return EstadoRegistroEnum.Modificado;
            }
            return EstadoRegistroEnum.Permanente;
        }

        if (copiaOriginal.Equals(copiaNueva))
        {
            return EstadoRegistroEnum.Permanente;
        }

        return EstadoRegistroEnum.Modificado;
    }

    public (string? ambito, string? descripcion) GetAmbitoYDescripcion<T>(T entidad)
    {
        string? ambito = null;
        string? descripcion = null;

        if(entidad == null)
        {
            return (ambito, descripcion);
        }

        switch(entidad)
        {
            //case Parametro parametro:
            //    ambito = "Parám. Estado - Situacion Equivalente - Superficie Afectada";
            //    descripcion = $"{parametro.EstadoIncendio.Descripcion} - {parametro.SituacionEquivalente.Descripcion} - {parametro.SuperficieAfectadaHectarea}";
            //    break;

            case IntervencionMedio intervencionMedio:
                ambito = "Intervención de medios";
                descripcion = $"{intervencionMedio.CaracterMedio.Descripcion} [{intervencionMedio.FechaHoraFin} - {intervencionMedio.FechaHoraFin}]";
                break;

            case ImpactoEvolucion impactoEvolucion:
                ambito = "Tipo impacto";
                descripcion = $"{impactoEvolucion.ImpactoClasificado.Descripcion} [{impactoEvolucion.Numero}]";
                break;

            case AreaAfectada areaAfectada:
                ambito = "Área afectada";
                descripcion = $"{areaAfectada.Municipio.Descripcion} ({areaAfectada.Provincia.Descripcion})";
                break;

            case Direccion direccion:
                ambito = "Dirección";
                descripcion = $"{direccion.FechaInicio} - {direccion.FechaFin}";
                break;

            case CoordinacionCecopi coordinacionCecopi:
                ambito = "Coord. CECOPI";
                descripcion = $"{coordinacionCecopi.FechaInicio} - {coordinacionCecopi.FechaFin}";
                break;

            case CoordinacionPMA coordinacionPMA:
                ambito = "Coord. PMA";
                descripcion = $"{coordinacionPMA.FechaInicio}";
                break;

            // TODO: Add more cases
            case MovilizacionMedio movilizacionMedio:
                ambito = "Movilización de medios";
                descripcion = $"{movilizacionMedio.Solicitante}";
                break;

            case ConvocatoriaCECOD convocatoriaCECOD:
                ambito = "Convocatoria CECODI";
                descripcion = $"{convocatoriaCECOD.FechaInicio} - {convocatoriaCECOD.FechaFin}";
                break;

             case ActivacionPlanEmergencia activacionPlanEmergencia:
                ambito = "Activ. Plan";
                descripcion = $"{activacionPlanEmergencia.TipoPlan.Descripcion} [{activacionPlanEmergencia.FechaHoraInicio} - {activacionPlanEmergencia.FechaHoraFin}]";
                break;

             case NotificacionEmergencia notificacionEmergencia:
                ambito = "Notificación oficial";
                descripcion = $"{notificacionEmergencia.TipoNotificacion.Descripcion} [{notificacionEmergencia.FechaHoraNotificacion}]";
                break;

             case ActivacionSistema activacionSistema:
                ambito = "Activ. de sistemas";
                descripcion = $"{activacionSistema.TipoSistemaEmergencia.Descripcion} [{activacionSistema.FechaHoraActivacion}]";
                break;
             
             case DeclaracionZAGEP declaracionZAGEP:
                ambito = "Declaración ZAGEP";
                descripcion = $"{declaracionZAGEP.FechaSolicitud}";
                break;

             case EmergenciaNacional emergenciaNacional:
                ambito = "Emergencia nacional";
                descripcion = $"{emergenciaNacional.Autoridad} [{emergenciaNacional.FechaHoraSolicitud} - {emergenciaNacional.FechaHoraDeclaracion}]";
                break;

            case DetalleDocumentacion documentacion:
                ambito = "Documentación";
                descripcion = $"Procedencia [{documentacion.Archivo?.NombreOriginal ?? "Sin archivo"}]";
                break;

            case DetalleOtraInformacion detalleOtraInformacion:
                ambito = "Otra información";
                descripcion = $"Procedencia [{detalleOtraInformacion.Asunto}]";
                break;
        }

        return (ambito, descripcion);
    }

}
