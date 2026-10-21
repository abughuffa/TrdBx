// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Invoices.DTOs;

namespace CleanArchitecture.Blazor.Application.Common.Interfaces;

public partial interface IPDFService
{
    Task<byte[]> ExportInvoiceAsync(InvoiceDto invoice);
}