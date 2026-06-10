namespace TicketSystem.Application.Common.Exceptions;

/// <summary>
/// Se lanza cuando una operación viola una regla de negocio
/// (evento cancelado, capacidad insuficiente, etc.).
/// </summary>
public class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message) { }
}
