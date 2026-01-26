using CleanArchitecture.Blazor.Application.Features.Invoices.DTOs;
using CleanArchitecture.Blazor.Application.TrdBx.Features.Invoices.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Invoices.Helper;


public class InvoiceLogic 
{


    public static async Task ProcessServiceLogsForInvoiceAsync(InvoiceDto invoice, List<ServiceLog> serviceLogs)
    {
        int groupSerialIndex = 1; // Start serial index at 1 for first group

        foreach (var serviceLog in serviceLogs)
        {
            // Convert service log to invoice item group with proper serial index
            var itemGroup = await CreateInvoiceItemGroupAsync(serviceLog, groupSerialIndex++);
            invoice.InvoiceItemGroups.Add(itemGroup); // Add group to invoice

            // Mark service log as billed since it's now included in invoice
            serviceLog.IsBilled = true;
        }

        // Calculate total invoice Total by summing all group subtotals
        invoice.Total = invoice.InvoiceItemGroups.Sum(ig => ig.SubTotal);
    }

    public static async Task<InvoiceItemGroupDto> CreateInvoiceItemGroupAsync(ServiceLog serviceLog, int serialIndex)
    {
        // Convert service log amount from Double to decimal for consistency
        //decimal serviceLogAmount = (decimal)serviceLog.Amount;
        decimal subsTotal = 0.0m; // Initialize subtotal for Subscriptions
        int itemSubSerialIndex = 1; // Start sub-serial index at 1

        // Create item group with basic information from service log
        var itemGroup = new InvoiceItemGroupDto
        {
            SerialIndex = serialIndex, // Position in invoice
            ServiceLogId = serviceLog.Id, // Link to original service log 
            Description = serviceLog.Desc, // Copy description
            Amount = serviceLog.Amount,
            SubTotal = 0.0m,
            InvoiceItems = new()
        };



        // Process each Sub in the service log
        foreach (var subscription in serviceLog.Subscriptions.OrderBy(s => s.Id))
        {
            // Convert Sub to invoice item
            var invoiceItem = await CreateInvoiceItemAsync(subscription, itemSubSerialIndex++);
            itemGroup.InvoiceItems.Add(invoiceItem); // Add item to group
            subsTotal += (decimal)invoiceItem.Amount; // Accumulate Sub amounts
        }

        // Calculate group subtotal: servicelog amount + sum of all Sub amounts
        itemGroup.SubTotal = serviceLog.Amount + subsTotal;

        return itemGroup;
    }

    public static async Task<InvoiceItemDto> CreateInvoiceItemAsync(Subscription subscription, int subSerialIndex)
    {
        // Create invoice item with all required information
        return new InvoiceItemDto
        {
            SubSerialIndex = subSerialIndex, // Position in item group
            SubscriptionId = subscription.Id, // Link to original Sub
            Description = subscription.Desc, // Copy description
            Amount = (decimal)subscription.Amount, // Convert amount to decimal
        };
    }

}

