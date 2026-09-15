using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using CleanArchitecture.Blazor.Application.Common.Interfaces;
using CleanArchitecture.Blazor.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Blazor.Infrastructure.Services;

public class GsmModemSmsReceiver : ISmsReceiver //, IAsyncDisposable
{
    private readonly string _portName;
    private readonly int _baudRate;
    private readonly ILogger<GsmModemSmsReceiver> _logger;
    //private SerialPort? _port;
    private readonly Channel<SmsReceiveResult> _channel =
        Channel.CreateUnbounded<SmsReceiveResult>();

    public SmsProvider SmsProvider => SmsProvider.GsmModem;

    public GsmModemSmsReceiver(IConfiguration configuration, ILogger<GsmModemSmsReceiver> logger)
    {
        _portName = configuration["SmsSettings:GsmModem:PortName"] ?? "COM3";
        _baudRate = configuration.GetValue<int>("SmsSettings:GsmModem:BaudRate", 9600);
        _logger = logger;
    }

    public async IAsyncEnumerable<SmsReceiveResult> ListenAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {

 await Task.Yield(); // Silences CS1998 warning
    throw new NotImplementedException();
    yield break;
        // yield break;

        //         throw new NotImplementedException("Please implement based on the selected GSM modem library");

//   async IAsyncEnumerable<SmsReceiveResult> Iterator()
//         {
//                     yield break;

//         }
        // _port = new SerialPort(_portName, _baudRate, Parity.None, 8, StopBits.One)
        // {
        //     NewLine = "\r\n",
        //     ReadTimeout = 5000,
        //     WriteTimeout = 5000
        // };
        // _port.Open();

        // // Configure modem: text mode, new SMS indication
        // await WriteCommandAsync("AT", cancellationToken);
        // await WriteCommandAsync("AT+CMGF=1", cancellationToken);   // text mode
        // await WriteCommandAsync("AT+CNMI=2,1,0,0,0", cancellationToken); // new msg -> +CMTI

        // _port.DataReceived += OnDataReceived;

        // try
        // {
        //     await foreach (var msg in _channel.Reader.ReadAllAsync(cancellationToken))
        //         yield return msg;
        // }
        // finally
        // {
        //     _port.DataReceived -= OnDataReceived;
        //     if (_port.IsOpen) _port.Close();
        // }
    }

    // private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
    // {
    //     try
    //     {
    //         var line = _port!.ReadLine().Trim();
    //         _logger.LogDebug("Modem: {Line}", line);

    //         // Modem signals:  +CMTI: "SM",<index>
    //         if (line.StartsWith("+CMTI:"))
    //         {
    //             var parts = line.Split(',');
    //             if (parts.Length == 2 && int.TryParse(parts[1].Trim(), out var index))
    //             {
    //                 _ = Task.Run(() => ReadMessageAtAsync(index));
    //             }
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogWarning(ex, "Error reading from GSM modem");
    //     }
    // }

    // private async Task ReadMessageAtAsync(int index)
    // {
    //     try
    //     {
    //         var raw = await WriteCommandAsync($"AT+CMGR={index}", CancellationToken.None);
    //         // Typical response:
    //         //   +CMGR: "REC UNREAD","+123456789","","24/01/15,10:30:00+00"
    //         //   Hello world
    //         //   OK
    //         var lines = raw.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    //         string? header = null, body = null;
    //         foreach (var l in lines)
    //         {
    //             var t = l.Trim();
    //             if (t.StartsWith("+CMGR:")) header = t;
    //             else if (header != null && body == null && t != "OK") body = t;
    //         }
    //         if (header is null || body is null) return;

    //         var headerParts = header.Substring(header.IndexOf(':') + 1)
    //                                 .Split(',').Select(p => p.Trim('"', ' ')).ToArray();
    //         var phone = headerParts.Length > 1 ? headerParts[1] : "unknown";

    //         _channel.Writer.TryWrite(new SmsReceiveResult(
    //             PhoneNumber: phone,
    //             Message: body,
    //             SMSDate: DateTime.UtcNow,
    //             Encoding: "GSM7"));

    //         // Delete from modem so we don't re-read it
    //         await WriteCommandAsync($"AT+CMGD={index}", CancellationToken.None);
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "Failed to read SMS at index {Index}", index);
    //     }
    // }

    // private async Task<string> WriteCommandAsync(string command, CancellationToken ct)
    // {
    //     if (_port is null || !_port.IsOpen) return string.Empty;
    //     _port.WriteLine(command);
    //     await Task.Delay(500, ct);      // give modem time to respond
    //     try { return _port.ReadExisting(); }
    //     catch { return string.Empty; }
    // }

    // public async ValueTask DisposeAsync()
    // {
    //     _channel.Writer.TryComplete();
    //     if (_port is { IsOpen: true }) _port.Close();
    //     _port?.Dispose();
    //     await Task.CompletedTask;
    // }
}