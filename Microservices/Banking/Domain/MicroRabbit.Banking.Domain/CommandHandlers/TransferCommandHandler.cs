using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MicroBank.Domain.Core.Bus;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Events;

namespace MicroRabbit.Banking.Domain.CommandHandlers
{
    public class TransferCommandHandler : IRequestHandler<CreateTransferCommand, bool>
    {
        public readonly IEventBus _bus;
        public TransferCommandHandler(IEventBus bus)
        {
            _bus = bus;
        }
        public Task<bool> Handle(CreateTransferCommand request, CancellationToken cancellationToken)
        {
           _bus.Publish(new TransferCreatedEvent(request.From, request.To, request.Amount));
            return Task.FromResult(true);
            
        }
    }
}