using CleanArchitecture.Blazor.Application.Features.SmsMessages.Caching;
using CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;
using CleanArchitecture.Blazor.Domain.Enums;
using System.ComponentModel.DataAnnotations;
namespace CleanArchitecture.Blazor.Application.Features.SmsMessages.Commands.CreateOutgoing;

public class CreateOutgoingSmsMessageCommand : ICacheInvalidatorRequest<Result<int>>
{

    [Display(Name = "PhoneNumber")]
    public string PhoneNumber { get; set; } = string.Empty;
    [Display(Name = "Message")]
    public string Message { get; set; } = string.Empty;

    public string CacheKey => SmsMessageCacheKey.GetAllCacheKey;
     public IEnumerable<string> Tags => SmsMessageCacheKey.Tags;
}

public class CreateOutgoingSmsMessageCommandHandler : IRequestHandler<CreateOutgoingSmsMessageCommand, Result<int>>
{
        private readonly IObjectMapper _objectMapper;
        private readonly ISmsSender _smsSender;
        private readonly IApplicationDbContextFactory _dbContextFactory;
        public CreateOutgoingSmsMessageCommandHandler(
            IObjectMapper objectMapper,
            ISmsSender smsSender,
            IApplicationDbContextFactory dbContextFactory)
        {
            _objectMapper = objectMapper;
            _smsSender = smsSender;
            _dbContextFactory = dbContextFactory;
        }

    public async ValueTask<Result<int>> Handle(CreateOutgoingSmsMessageCommand request, CancellationToken cancellationToken)
    {

        var itemDto = new SmsMessageDto
                {
                    Direction = SmsDirection.Outgoing,
                    SmsProvider = _smsSender.SmsProvider,
                    PhoneNumber = request.PhoneNumber,
                    Message = request.Message,
                    SmsStatus = SmsStatus.Sent,
                    SMSDate = DateTime.UtcNow
                };

        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);

        try
        {
            var result = await _smsSender.SendAsync(request.PhoneNumber, request.Message, cancellationToken);

            if (result.Success)
            {
                itemDto.SmsStatus = SmsStatus.Sent;
            }
            else
            {
                itemDto.SmsStatus = SmsStatus.Failed;
                itemDto.ErrorMessage = result.ErrorMessage;
            }
        }
        catch (Exception ex)
        {
           // _logger.LogError("Failed to send SMS to {Phone}", request.PhoneNumber);
            itemDto.SmsStatus = SmsStatus.Failed;
            itemDto.ErrorMessage = ex.Message;
        }
        // raise a create domain event
        var item = _objectMapper.Map<SmsMessage>(itemDto);
        item.AddDomainEvent(new SmsMessageCreatedEvent(item));
        context.SmsMessages.Add(item);
        await context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);
    }
}


