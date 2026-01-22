//using System;
//using System.Collections.Generic;
//using System.Text;
//using Microsoft.AspNetCore.SignalR;

//namespace CleanArchitecture.Blazor.Infrastructure.TrdBx.Services;


//public interface IXInvoiceService
//{
//    Task<XInvoice> GenerateXInvoiceAsync(int customerId, List<int> serviceLogIds);
//    Task<XInvoice> GenerateXInvoiceForAllUnbilledAsync(int customerId);
//    Task<bool> DeleteXInvoiceAsync(int invoiceId);
//    Task<bool> DeleteItemFromXInvoiceAsync(int invoiceItemId);
//    Task<bool> DeleteItemGroupFromXInvoiceAsync(int invoiceItemGroupId);
//    Task<XInvoice> UpdateXInvoiceDetailsAsync(int invoiceId, UpdateXInvoiceRequest request);
//    Task<XInvoiceItemGroup> UpdateItemGroupAsync(int itemGroupId, UpdateItemGroupRequest request);
//    Task<XInvoiceItem> UpdateXInvoiceItemAsync(int invoiceItemId, UpdateXInvoiceItemRequest request);
//    Task<XInvoice> AddItemGroupToXInvoiceAsync(int invoiceId, int serviceLogId);
//    Task<XInvoiceItem> AddItemToItemGroupAsync(int itemGroupId, int subId);
//    Task<XInvoice> GetXInvoiceWithDetailsAsync(int invoiceId);
//    Task<List<XInvoice>> GetCustomerXInvoicesAsync(int customerId);
//    Task<XInvoice> RecalculateXInvoiceTotalsAsync(int invoiceId);
//}

//public class XInvoiceService : IXInvoiceService
//{
//    private readonly ApplicationDbContext _context;
//    private readonly ILogger<XInvoiceService> _logger;

//    public XInvoiceService(ApplicationDbContext context, ILogger<XInvoiceService> logger)
//    {
//        _context = context; // Database context for EF Core operations
//        _logger = logger;   // Logger for tracking operations
//    }

//    //#region Generation Methods

//    //public async Task<XInvoice> GenerateXInvoiceAsync(int customerId, List<int> serviceLogIds)
//    //{
//    //    // Start database transaction to ensure data consistency
//    //    using var transaction = await _context.Database.BeginTransactionAsync();

//    //    try
//    //    {
//    //        // Validate customer exists in database
//    //        var customer = await _context.Customers
//    //            .FirstOrDefaultAsync(c => c.Id == customerId);

//    //        if (customer == null)
//    //            throw new ArgumentException($"Customer with ID {customerId} not found.");

//    //        // Fetch unbilled service logs for specified customer and IDs
//    //        // Include related Subscriptions and their TrackingUnits for complete data
//    //        var serviceLogs = await _context.ServiceLogs
//    //            .Include(sl => sl.Subscriptions) // Eager load Subscriptions collection
//    //            .ThenInclude(sub => sub.TrackingUnit) // Eager load TrackingUnit for each Sub
//    //            .Where(sl => sl.CustomerId == customerId // Filter by customer
//    //                      && !sl.IsBilled // Only unbilled service logs
//    //                      && serviceLogIds.Contains(sl.Id)) // Only specified IDs
//    //            .OrderBy(sl => sl.Id) // Order by ID for consistent ordering
//    //            .ToListAsync();

//    //        if (!serviceLogs.Any())
//    //            throw new InvalidOperationException("No unbilled service logs found for the specified criteria.");

//    //        // Create new invoice instance with basic information
//    //        var invoice = new XInvoice
//    //        {
//    //            CustomerId = customerId,
//    //            XInvoiceDate = DateTime.UtcNow, // Use UTC time for consistency
//    //            XInvoiceNumber = await GenerateXInvoiceNumberAsync(), // Generate unique invoice number
//    //            Status = "Draft" // Default status for new invoices
//    //        };

//    //        // Process service logs and convert them to invoice item groups
//    //        await ProcessServiceLogsForXInvoiceAsync(invoice, serviceLogs);

//    //        // Save invoice and all related entities to database
//    //        _context.XInvoices.Add(invoice);
//    //        await _context.SaveChangesAsync(); // Save all changes
//    //        await transaction.CommitAsync(); // Commit transaction

//    //        _logger.LogInformation($"XInvoice {invoice.XInvoiceNumber} created successfully for customer {customerId}");
//    //        return invoice;
//    //    }
//    //    catch (Exception ex)
//    //    {
//    //        await transaction.RollbackAsync(); // Rollback on error to maintain data consistency
//    //        _logger.LogError(ex, $"Error generating invoice for customer {customerId}");
//    //        throw; // Re-throw exception for caller to handle
//    //    }
//    //}

//    //public async Task<XInvoice> GenerateXInvoiceForAllUnbilledAsync(int customerId)
//    //{
//    //    // Get all unbilled service log IDs for the specified customer
//    //    var serviceLogIds = await _context.ServiceLogs
//    //        .Where(sl => sl.CustomerId == customerId && !sl.IsBilled)
//    //        .Select(sl => sl.Id) // Project to only get IDs
//    //        .ToListAsync();

//    //    // Call main generation method with all found IDs
//    //    return await GenerateXInvoiceAsync(customerId, serviceLogIds);
//    //}

//    //private async Task ProcessServiceLogsForXInvoiceAsync(XInvoice invoice, List<ServiceLog> serviceLogs)
//    //{
//    //    int groupSerialIndex = 1; // Start serial index at 1 for first group

//    //    foreach (var serviceLog in serviceLogs)
//    //    {
//    //        // Convert service log to invoice item group with proper serial index
//    //        var itemGroup = await CreateXInvoiceItemGroupAsync(serviceLog, groupSerialIndex++);
//    //        invoice.ItemGroups.Add(itemGroup); // Add group to invoice

//    //        // Mark service log as billed since it's now included in invoice
//    //        serviceLog.IsBilled = true;
//    //    }

//    //    // Calculate total invoice amount by summing all group subtotals
//    //    invoice.TotalAmount = invoice.ItemGroups.Sum(ig => ig.SubTotal);
//    //}

//    //private async Task<XInvoiceItemGroup> CreateXInvoiceItemGroupAsync(ServiceLog serviceLog, int serialIndex)
//    //{
//    //    // Create item group with basic information from service log
//    //    var itemGroup = new XInvoiceItemGroup
//    //    {
//    //        ServiceLogId = serviceLog.Id, // Link to original service log
//    //        SerialIndex = serialIndex, // Position in invoice
//    //        ServiceNumber = serviceLog.ServiceNo, // Copy service number
//    //        Description = serviceLog.Desc // Copy description
//    //    };

//    //    // Convert service log amount from Double to decimal for consistency
//    //    decimal serviceLogAmount = (decimal)serviceLog.Amount;
//    //    decimal subsTotalAmount = 0; // Initialize subtotal for Subscriptions
//    //    int itemSubSerialIndex = 1; // Start sub-serial index at 1

//    //    // Process each Sub in the service log
//    //    foreach (var sub in serviceLog.Subscriptions.OrderBy(s => s.Id))
//    //    {
//    //        // Convert Sub to invoice item
//    //        var invoiceItem = await CreateXInvoiceItemAsync(sub, itemSubSerialIndex++);
//    //        itemGroup.Items.Add(invoiceItem); // Add item to group
//    //        subsTotalAmount += (decimal)sub.Amount; // Accumulate Sub amounts
//    //    }

//    //    // Calculate group subtotal: service log amount + sum of all Sub amounts
//    //    itemGroup.SubTotal = serviceLogAmount + subsTotalAmount;

//    //    return itemGroup;
//    //}

//    //private async Task<XInvoiceItem> CreateXInvoiceItemAsync(Sub sub, int subSerialIndex)
//    //{
//    //    // Ensure TrackingUnit is loaded for the Sub (in case of lazy loading)
//    //    if (sub.TrackingUnit == null)
//    //    {
//    //        sub.TrackingUnit = await _context.TrackingUnits.FindAsync(sub.TrackingUnitId);
//    //    }

//    //    // Create invoice item with all required information
//    //    return new XInvoiceItem
//    //    {
//    //        SubId = sub.Id, // Link to original Sub
//    //        SubSerialIndex = subSerialIndex, // Position in item group
//    //        Description = sub.Desc, // Copy description
//    //        StartDate = sub.StartDate, // Copy start date
//    //        EndDate = sub.EndDate, // Copy end date
//    //        Amount = (decimal)sub.Amount, // Convert amount to decimal
//    //        TrackingUnitSerialNumber = sub.TrackingUnit?.TrackingUnitSNo // Get unit serial number
//    //    };
//    //}

//    //private async Task<string> GenerateXInvoiceNumberAsync()
//    //{
//    //    var now = DateTime.UtcNow;
//    //    var yearMonth = now.ToString("yyyyMM"); // Format: YYYYMM

//    //    // Count existing invoices for the current month to create sequential number
//    //    var count = await _context.XInvoices
//    //        .CountAsync(i => i.XInvoiceNumber.StartsWith($"INV-{yearMonth}"));

//    //    // Format: INV-YYYYMM-0001, INV-YYYYMM-0002, etc.
//    //    return $"INV-{yearMonth}-{count + 1:0000}";
//    //}

//    //#endregion

//    #region Delete Methods







//    #endregion

//    #region Update/Edit Methods

//    public async Task<XInvoice> UpdateXInvoiceDetailsAsync(int invoiceId, UpdateXInvoiceRequest request)
//    {
//        using var transaction = await _context.Database.BeginTransactionAsync();

//        try
//        {
//            // Find invoice by ID
//            var invoice = await _context.XInvoices
//                .FirstOrDefaultAsync(i => i.Id == invoiceId);

//            if (invoice == null)
//                throw new ArgumentException($"XInvoice with ID {invoiceId} not found.");

//            // Update invoice properties if provided in request
//            if (!string.IsNullOrEmpty(request.XInvoiceNumber))
//                invoice.XInvoiceNumber = request.XInvoiceNumber; // Update invoice number

//            if (request.XInvoiceDate.HasValue)
//                invoice.XInvoiceDate = request.XInvoiceDate.Value; // Update invoice date

//            if (!string.IsNullOrEmpty(request.Status))
//                invoice.Status = request.Status; // Update status (Draft, Sent, Paid, etc.)

//            // Recalculate totals if requested (for manual amount changes)
//            if (request.RecalculateTotals)
//            {
//                await RecalculateXInvoiceTotalsAsync(invoiceId);
//            }
//            else
//            {
//                await _context.SaveChangesAsync(); // Save basic updates
//            }

//            await transaction.CommitAsync();
//            _logger.LogInformation($"XInvoice {invoiceId} updated successfully");

//            // Return updated invoice with all details loaded
//            return await GetXInvoiceWithDetailsAsync(invoiceId);
//        }
//        catch (Exception ex)
//        {
//            await transaction.RollbackAsync();
//            _logger.LogError(ex, $"Error updating invoice {invoiceId}");
//            throw;
//        }
//    }

//    public async Task<XInvoiceItemGroup> UpdateItemGroupAsync(int itemGroupId, UpdateItemGroupRequest request)
//    {
//        using var transaction = await _context.Database.BeginTransactionAsync();

//        try
//        {
//            // Load item group with related data
//            var itemGroup = await _context.XInvoiceItemGroups
//                .Include(ig => ig.XInvoice) // Load parent invoice for total recalculation
//                .Include(ig => ig.Items) // Load items for subtotal calculation
//                .FirstOrDefaultAsync(ig => ig.Id == itemGroupId);

//            if (itemGroup == null)
//                throw new ArgumentException($"Item group with ID {itemGroupId} not found.");

//            // Update item group properties if provided
//            if (!string.IsNullOrEmpty(request.Description))
//                itemGroup.Description = request.Description; // Update description

//            if (!string.IsNullOrEmpty(request.ServiceNumber))
//                itemGroup.ServiceNumber = request.ServiceNumber; // Update service number

//            // Update serial index if changed (reordering groups)
//            if (request.SerialIndex.HasValue && request.SerialIndex.Value != itemGroup.SerialIndex)
//            {
//                await UpdateGroupSerialIndexAsync(itemGroup.XInvoiceId, itemGroupId, request.SerialIndex.Value);
//            }

//            // Recalculate subtotal if amounts were modified
//            if (request.ServiceLogAmount.HasValue || request.Items != null)
//            {
//                await RecalculateItemGroupSubtotalAsync(itemGroup, request);
//            }

//            await _context.SaveChangesAsync();
//            await transaction.CommitAsync();

//            _logger.LogInformation($"Item group {itemGroupId} updated successfully");
//            return itemGroup;
//        }
//        catch (Exception ex)
//        {
//            await transaction.RollbackAsync();
//            _logger.LogError(ex, $"Error updating item group {itemGroupId}");
//            throw;
//        }
//    }

//    public async Task<XInvoiceItem> UpdateXInvoiceItemAsync(int invoiceItemId, UpdateXInvoiceItemRequest request)
//    {
//        using var transaction = await _context.Database.BeginTransactionAsync();

//        try
//        {
//            // Load invoice item with parent relationships
//            var invoiceItem = await _context.XInvoiceItems
//                .Include(ii => ii.XInvoiceItemGroup) // Load parent item group
//                    .ThenInclude(ig => ig.XInvoice) // Load grandparent invoice
//                .FirstOrDefaultAsync(ii => ii.Id == invoiceItemId);

//            if (invoiceItem == null)
//                throw new ArgumentException($"XInvoice item with ID {invoiceItemId} not found.");

//            // Update item properties if provided
//            if (!string.IsNullOrEmpty(request.Description))
//                invoiceItem.Description = request.Description; // Update description

//            if (request.Amount.HasValue)
//                invoiceItem.Amount = request.Amount.Value; // Update amount

//            if (request.StartDate.HasValue)
//                invoiceItem.StartDate = request.StartDate.Value; // Update start date

//            if (request.EndDate.HasValue)
//                invoiceItem.EndDate = request.EndDate.Value; // Update end date

//            // Update sub-serial index if changed (reordering within group)
//            if (request.SubSerialIndex.HasValue && request.SubSerialIndex.Value != invoiceItem.SubSerialIndex)
//            {
//                await UpdateItemSubSerialIndexAsync(invoiceItem.XInvoiceItemGroupId, invoiceItemId, request.SubSerialIndex.Value);
//            }

//            // Recalculate group subtotal after item modifications
//            var itemGroup = invoiceItem.XInvoiceItemGroup;
//            // Sum of all items in group + service log amount
//            itemGroup.SubTotal = await _context.XInvoiceItems
//                .Where(ii => ii.XInvoiceItemGroupId == itemGroup.Id)
//                .SumAsync(ii => ii.Amount) +
//                (itemGroup.ServiceLog != null ? (decimal)itemGroup.ServiceLog.Amount : 0);

//            // Recalculate total invoice amount
//            var invoice = itemGroup.XInvoice;
//            invoice.TotalAmount = await _context.XInvoiceItemGroups
//                .Where(ig => ig.XInvoiceId == invoice.Id)
//                .SumAsync(ig => ig.SubTotal);

//            await _context.SaveChangesAsync();
//            await transaction.CommitAsync();

//            _logger.LogInformation($"XInvoice item {invoiceItemId} updated successfully");
//            return invoiceItem;
//        }
//        catch (Exception ex)
//        {
//            await transaction.RollbackAsync();
//            _logger.LogError(ex, $"Error updating invoice item {invoiceItemId}");
//            throw;
//        }
//    }

//    #endregion

//    #region Add Methods

//    public async Task<XInvoice> AddItemGroupToXInvoiceAsync(int invoiceId, int serviceLogId)
//    {
//        using var transaction = await _context.Database.BeginTransactionAsync();

//        try
//        {
//            // Load existing invoice
//            var invoice = await _context.XInvoices
//                .Include(i => i.ItemGroups) // Load existing groups for serial index calculation
//                .FirstOrDefaultAsync(i => i.Id == invoiceId);

//            if (invoice == null)
//                throw new ArgumentException($"XInvoice with ID {invoiceId} not found.");

//            // Load service log with its Subscriptions
//            var serviceLog = await _context.ServiceLogs
//                .Include(sl => sl.Subscriptions) // Load all Subscriptions
//                .ThenInclude(sub => sub.TrackingUnit) // Load TrackingUnit for each Sub
//                .FirstOrDefaultAsync(sl => sl.Id == serviceLogId);

//            if (serviceLog == null)
//                throw new ArgumentException($"Service log with ID {serviceLogId} not found.");

//            // Validate service log is not already billed
//            if (serviceLog.IsBilled)
//                throw new InvalidOperationException($"Service log {serviceLogId} is already billed.");

//            // Calculate next serial index for the new group
//            // If invoice has groups, use max index + 1, otherwise start at 1
//            int nextSerialIndex = invoice.ItemGroups.Any() ?
//                invoice.ItemGroups.Max(ig => ig.SerialIndex) + 1 : 1;

//            // Convert service log to invoice item group
//            var itemGroup = await CreateXInvoiceItemGroupAsync(serviceLog, nextSerialIndex);
//            invoice.ItemGroups.Add(itemGroup); // Add to invoice

//            // Mark service log as billed
//            serviceLog.IsBilled = true;

//            // Recalculate total invoice amount with new group
//            invoice.TotalAmount = invoice.ItemGroups.Sum(ig => ig.SubTotal);

//            await _context.SaveChangesAsync();
//            await transaction.CommitAsync();

//            _logger.LogInformation($"Item group added to invoice {invoiceId} from service log {serviceLogId}");
//            return await GetXInvoiceWithDetailsAsync(invoiceId);
//        }
//        catch (Exception ex)
//        {
//            await transaction.RollbackAsync();
//            _logger.LogError(ex, $"Error adding item group to invoice {invoiceId}");
//            throw;
//        }
//    }

//    public async Task<XInvoiceItem> AddItemToItemGroupAsync(int itemGroupId, int subId)
//    {
//        using var transaction = await _context.Database.BeginTransactionAsync();

//        try
//        {
//            // Load item group with related data
//            var itemGroup = await _context.XInvoiceItemGroups
//                .Include(ig => ig.Items) // Load existing items for sub-serial index
//                .Include(ig => ig.XInvoice) // Load parent invoice for total recalculation
//                .FirstOrDefaultAsync(ig => ig.Id == itemGroupId);

//            if (itemGroup == null)
//                throw new ArgumentException($"Item group with ID {itemGroupId} not found.");

//            // Load Sub with its TrackingUnit
//            var sub = await _context.Subscriptions
//                .Include(s => s.TrackingUnit) // Load TrackingUnit information
//                .FirstOrDefaultAsync(s => s.Id == subId);

//            if (sub == null)
//                throw new ArgumentException($"Sub with ID {subId} not found.");

//            // Check if Sub is already included in any invoice item
//            var existingItem = await _context.XInvoiceItems
//                .FirstOrDefaultAsync(ii => ii.SubId == subId);

//            if (existingItem != null)
//                throw new InvalidOperationException($"Sub {subId} is already included in an invoice.");

//            // Calculate next sub-serial index for the new item
//            // If group has items, use max index + 1, otherwise start at 1
//            int nextSubSerialIndex = itemGroup.Items.Any() ?
//                itemGroup.Items.Max(ii => ii.SubSerialIndex) + 1 : 1;

//            // Convert Sub to invoice item
//            var invoiceItem = await CreateXInvoiceItemAsync(sub, nextSubSerialIndex);
//            invoiceItem.XInvoiceItemGroupId = itemGroupId; // Set parent relationship

//            _context.XInvoiceItems.Add(invoiceItem); // Add to database

//            // Update group subtotal with new item amount
//            itemGroup.SubTotal += (decimal)sub.Amount;

//            // Recalculate total invoice amount
//            var invoice = itemGroup.XInvoice;
//            invoice.TotalAmount = await _context.XInvoiceItemGroups
//                .Where(ig => ig.XInvoiceId == invoice.Id)
//                .SumAsync(ig => ig.SubTotal);

//            await _context.SaveChangesAsync();
//            await transaction.CommitAsync();

//            _logger.LogInformation($"Item added to item group {itemGroupId}");
//            return invoiceItem;
//        }
//        catch (Exception ex)
//        {
//            await transaction.RollbackAsync();
//            _logger.LogError(ex, $"Error adding item to item group {itemGroupId}");
//            throw;
//        }
//    }

//    #endregion

//    #region Helper Methods

//    private async Task UpdateGroupSerialIndexAsync(int invoiceId, int itemGroupId, int newSerialIndex)
//    {
//        // Load all groups for the invoice ordered by current serial index
//        var groups = await _context.XInvoiceItemGroups
//            .Where(ig => ig.XInvoiceId == invoiceId)
//            .OrderBy(ig => ig.SerialIndex)
//            .ToListAsync();

//        // Find the group being moved
//        var targetGroup = groups.FirstOrDefault(ig => ig.Id == itemGroupId);
//        if (targetGroup == null) return;

//        // Remove group from current position
//        groups.Remove(targetGroup);

//        // Clamp new index to valid range (1 to count+1)
//        newSerialIndex = Math.Clamp(newSerialIndex, 1, groups.Count + 1);

//        // Insert at new position (adjust for 0-based index)
//        groups.Insert(newSerialIndex - 1, targetGroup);

//        // Update serial indexes for all groups
//        for (int i = 0; i < groups.Count; i++)
//        {
//            groups[i].SerialIndex = i + 1; // Set sequential indexes
//        }
//    }

//    private async Task UpdateItemSubSerialIndexAsync(int itemGroupId, int invoiceItemId, int newSubSerialIndex)
//    {
//        // Load all items in the group ordered by current sub-serial index
//        var items = await _context.XInvoiceItems
//            .Where(ii => ii.XInvoiceItemGroupId == itemGroupId)
//            .OrderBy(ii => ii.SubSerialIndex)
//            .ToListAsync();

//        // Find the item being moved
//        var targetItem = items.FirstOrDefault(ii => ii.Id == invoiceItemId);
//        if (targetItem == null) return;

//        // Remove item from current position
//        items.Remove(targetItem);

//        // Clamp new index to valid range
//        newSubSerialIndex = Math.Clamp(newSubSerialIndex, 1, items.Count + 1);

//        // Insert at new position
//        items.Insert(newSubSerialIndex - 1, targetItem);

//        // Update sub-serial indexes for all items
//        for (int i = 0; i < items.Count; i++)
//        {
//            items[i].SubSerialIndex = i + 1; // Set sequential indexes
//        }
//    }

//    private async Task RecalculateItemGroupSubtotalAsync(XInvoiceItemGroup itemGroup, UpdateItemGroupRequest request)
//    {
//        // Determine service log amount: use provided value or existing value
//        decimal serviceLogAmount = request.ServiceLogAmount.HasValue ?
//            request.ServiceLogAmount.Value :
//            (itemGroup.ServiceLog != null ? (decimal)itemGroup.ServiceLog.Amount : 0);

//        // Update item amounts if provided in request
//        if (request.Items != null)
//        {
//            foreach (var itemUpdate in request.Items)
//            {
//                var item = itemGroup.Items.FirstOrDefault(ii => ii.Id == itemUpdate.ItemId);
//                if (item != null && itemUpdate.Amount.HasValue)
//                {
//                    item.Amount = itemUpdate.Amount.Value; // Update item amount
//                }
//            }
//        }

//        // Recalculate group subtotal: service log amount + sum of all item amounts
//        itemGroup.SubTotal = serviceLogAmount + itemGroup.Items.Sum(ii => ii.Amount);
//    }

//    public async Task<XInvoice> RecalculateXInvoiceTotalsAsync(int invoiceId)
//    {
//        // Load invoice with all related data for recalculation
//        var invoice = await _context.XInvoices
//            .Include(i => i.ItemGroups) // Load item groups
//                .ThenInclude(ig => ig.Items) // Load items in each group
//            .Include(i => i.ItemGroups)
//                .ThenInclude(ig => ig.ServiceLog) // Load service log for each group
//            .FirstOrDefaultAsync(i => i.Id == invoiceId);

//        if (invoice == null)
//            throw new ArgumentException($"XInvoice with ID {invoiceId} not found.");

//        // Recalculate subtotal for each item group
//        foreach (var itemGroup in invoice.ItemGroups)
//        {
//            // Get service log amount or 0 if not available
//            decimal serviceLogAmount = itemGroup.ServiceLog != null ?
//                (decimal)itemGroup.ServiceLog.Amount : 0;

//            // Recalculate group subtotal
//            itemGroup.SubTotal = serviceLogAmount + itemGroup.Items.Sum(ii => ii.Amount);
//        }

//        // Recalculate total invoice amount
//        invoice.TotalAmount = invoice.ItemGroups.Sum(ig => ig.SubTotal);

//        await _context.SaveChangesAsync(); // Save recalculated amounts
//        return invoice;
//    }

//    #endregion

//    #region Get Methods

//    public async Task<XInvoice> GetXInvoiceWithDetailsAsync(int invoiceId)
//    {
//        // Load invoice with all related data for display
//        return await _context.XInvoices
//            .Include(i => i.Customer) // Load customer details
//            .Include(i => i.ItemGroups) // Load item groups
//                .ThenInclude(ig => ig.ServiceLog) // Load service log for context
//            .Include(i => i.ItemGroups)
//                .ThenInclude(ig => ig.Items) // Load items in each group
//                    .ThenInclude(item => item.Sub) // Load original Sub for reference
//            .AsNoTracking() // Read-only for better performance
//            .FirstOrDefaultAsync(i => i.Id == invoiceId);
//    }

//    public async Task<List<XInvoice>> GetCustomerXInvoicesAsync(int customerId)
//    {
//        // Get all invoices for a specific customer
//        return await _context.XInvoices
//            .Where(i => i.CustomerId == customerId) // Filter by customer
//            .OrderByDescending(i => i.XInvoiceDate) // Most recent first
//            .Include(i => i.ItemGroups) // Include groups for summary display
//            .AsNoTracking() // Read-only for better performance
//            .ToListAsync();
//    }

//    #endregion
//}
