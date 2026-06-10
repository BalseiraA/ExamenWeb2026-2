namespace TicketSystem.Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }

    public NotFoundException(string entity, object key)
        : base($"No se encontró {entity} con identificador '{key}'.") { }
}
