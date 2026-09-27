using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.SmsMessages.DTOs;

[Description("SmsMessages")]

public class SmsMessageDto
{
    [Display(Name = "Id")]public int      Id           { get; set; }
    public string  To          { get; set; } = null!;
    public string? From        { get; set; }
    public string  Body        { get; set; } = null!;
    public int     SimSlot     { get; set; }
    // ── used by both directions ─────────────────────────
    public string? Reference   { get; set; }   // client-supplied id (outbound)
    public string? GatewayId   { get; set; }   // id returned by /inbox or webhook  ← inbound dedupe key
    public SmsDirection Direction { get; set; } = SmsDirection.Outbound;
    public SmsStatus    Status    { get; set; } = SmsStatus.Queued;
    public string? Error       { get; set; }
    public DateTime? SentAt      { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public DateTime? ReceivedAt  { get; set; }   // ← set on inbound
    //public string? TenantId       { get; set; }
}

public record SmsInboundListItem(
    int Id, string From, string Body,
    long GatewayId, DateTime ReceivedAt);

public record InboundSmsDto(
    long   GatewayId,
    string From,
    string Body,
    long   Ts,
    int?   SimSlot = null);


// ── NEW: strongly-typed payloads (client + server share these) ──────
public sealed record SmsReceivedPayload(
    int Id,
    long GatewayId,
    string From,
    string Body,
    long Ts,
    DateTime ReceivedAt);

public sealed record SmsStatusChangedPayload(
    int Id,
    string Reference,
    string Status,          // "Queued" | "Sent" | "Delivered" | "Failed" | "Unknown"
    string? Error,
    DateTime? SentAt,
    DateTime? DeliveredAt);

/// <summary>
/// Response of GET /status on the Android SMS Gateway.
/// Mirrors: { "mode": "WIFI", "uptimeSec": 123, "simReady": true,
///            "sentCount": 4, "receivedCount": 9 }
/// </summary>
public record SmsGatewayStatusDto(
    string Mode,
    long   UptimeSec,
    bool   SimReady,
    int    SentCount,
    int    ReceivedCount);

/// <summary>
/// Response of POST /send on the Android SMS Gateway.
/// Mirrors: { "id": "...", "status": "queued",
///            "reference": "my-ref", "error": null }
/// status is one of "queued" | "sent" | "failed".
/// </summary>

public record SendSmsResult(
    string  Id,
    string  Status,
    string? Reference = null,
    string? Error     = null);
/// <summary>
/// One row from GET /inbox?since=&amp;limit=.
/// Mirrors: { "id": 42, "from": "+123", "body": "hi", "ts": 1699999999999 }
/// </summary>
public record SmsInboxItemDto(
    long   Id,
    string From,
    string Body,
    long   Ts);