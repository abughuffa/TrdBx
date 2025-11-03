namespace CleanArchitecture.Blazor.Application.Features.WialonApis.Models;
public class XWialonException : Exception
{
    public int ErrorCode { get; }
    public XWialonException(int errorCode) : base($"Wialon error: {errorCode}")
        => ErrorCode = errorCode;

    public XWialonException(int errorCode, string ex) : base($"Wialon error: - {ex}") => ErrorCode = errorCode;
}
