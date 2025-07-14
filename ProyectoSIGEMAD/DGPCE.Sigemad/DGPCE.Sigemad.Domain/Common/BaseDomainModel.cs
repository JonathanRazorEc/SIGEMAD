namespace DGPCE.Sigemad.Domain.Common
{
    public abstract class BaseDomainModel<T>: BaseEntity
    {
        public T Id { get; set; }
    }
}
