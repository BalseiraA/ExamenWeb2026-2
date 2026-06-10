using MediatR;
using TicketSystem.Application.Common.Exceptions;
using TicketSystem.Application.Common.Interfaces;
using TicketSystem.Domain.Enums;

namespace TicketSystem.Application.Features.Events.Commands.CancelEvent;

public class CancelEventCommandHandler : IRequestHandler<CancelEventCommand>
{
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelEventCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CancelEventCommand request, CancellationToken cancellationToken)
    {
        var existing = await _eventRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("evento", request.Id);

        if (existing.Status == EventStatus.Cancelled)
            throw new BusinessRuleException("El evento ya se encuentra cancelado.");

        existing.Status = EventStatus.Cancelled;
        _eventRepository.Update(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
