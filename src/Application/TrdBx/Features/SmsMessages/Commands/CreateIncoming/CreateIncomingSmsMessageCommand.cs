using CleanArchitecture.Blazor.Application.Common.Interfaces;
using CleanArchitecture.Blazor.Application.Features.SmsMessages.Caching;
using CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;
using CleanArchitecture.Blazor.Domain.Enums;


namespace CleanArchitecture.Blazor.Application.Features.SmsMessages.Commands.CreateIncoming;

public class CreateIncomingSmsMessageCommand : ICacheInvalidatorRequest<Result<int>>
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public SmsProvider SmsProvider { get; set; }
    public string? ProviderMessageId { get; set; }
    public DateTime SMSDate { get; set; } = DateTime.UtcNow;
    public int PartsCount { get; set; } = 1;
    public string? Encoding { get; set; }

    public string CacheKey => SmsMessageCacheKey.GetAllCacheKey;
    public IEnumerable<string> Tags => SmsMessageCacheKey.Tags;
}

public class CreateIncomingSmsMessageCommandHandler
    : IRequestHandler<CreateIncomingSmsMessageCommand, Result<int>>
{
    private readonly IObjectMapper _objectMapper;
    private readonly IApplicationDbContextFactory _dbContextFactory;

    public CreateIncomingSmsMessageCommandHandler(
        IObjectMapper objectMapper,
        IApplicationDbContextFactory dbContextFactory)
    {
        _objectMapper = objectMapper;
        _dbContextFactory = dbContextFactory;
    }

    public async ValueTask<Result<int>> Handle(
        CreateIncomingSmsMessageCommand request,
        CancellationToken cancellationToken)
    {
        var itemDto = new SmsMessageDto
        {
            PhoneNumber = request.PhoneNumber,
            Message = request.Message,
            Direction = SmsDirection.Incoming,
            SmsProvider = request.SmsProvider,
            SmsStatus = SmsStatus.Delivered,
            ProviderMessageId = request.ProviderMessageId,
            PartsCount = request.PartsCount,
            Encoding = request.Encoding,
            SMSDate = request.SMSDate,
            DeliveredAt = request.SMSDate
        };

        await using var context = await _dbContextFactory.CreateAsync(cancellationToken);
        var item = _objectMapper.Map<SmsMessage>(itemDto);
        item.AddDomainEvent(new SmsMessageCreatedEvent(item));
        context.SmsMessages.Add(item);
        await context.SaveChangesAsync(cancellationToken);
        return await Result<int>.SuccessAsync(item.Id);
    }
}