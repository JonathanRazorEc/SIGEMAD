namespace DGPCE.Sigemad.Application.Dtos.Common;
public class BaseDto<T>
{
    public T Id { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
}
