namespace DGPCE.Sigemad.Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, object key) : base($"Entity '{name}' con clave ({key})  no fue encontrado")
        {
        }
    }
}
