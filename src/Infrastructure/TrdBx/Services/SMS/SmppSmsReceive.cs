using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using CleanArchitecture.Blazor.Application.Common.Interfaces;
using CleanArchitecture.Blazor.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Blazor.Infrastructure.Services;

public class SmppSmsReceiver : ISmsReceiver //, IAsyncDisposable
{
    private readonly ILogger<SmppSmsReceiver> _logger;
    //private SerialPort? _port;
    private readonly Channel<SmsReceiveResult> _channel =
        Channel.CreateUnbounded<SmsReceiveResult>();

    public SmsProvider SmsProvider => SmsProvider.GsmModem;

    public SmppSmsReceiver(IConfiguration configuration, ILogger<SmppSmsReceiver> logger)
    {
        //_portName = configuration["SmsSettings:GsmModem:PortName"] ?? "COM3";
        //_baudRate = configuration.GetValue<int>("SmsSettings:GsmModem:BaudRate", 9600);
        _logger = logger;
    }

    public async IAsyncEnumerable<SmsReceiveResult> ListenAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {

         await Task.Yield(); // Silences CS1998 warning
    throw new NotImplementedException();
    yield break;

        // throw new NotImplementedException("Please implement based on the selected SMPP library");
        //   yield break; 


    }
}