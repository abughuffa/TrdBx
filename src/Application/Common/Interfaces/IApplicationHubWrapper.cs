using CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;

namespace CleanArchitecture.Blazor.Application.Common.Interfaces;


public interface IApplicationHubWrapper
{
    Task JobStarted(int id,string message);
    Task JobCompleted(int id,string message);


    Task SmsReceived(SmsReceivedPayload payload);
    Task SmsStatusChanged(SmsStatusChangedPayload payload);
}
