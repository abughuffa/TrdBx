// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Common.Interfaces;

public partial interface IPDFService
{
    Task<byte[]> ExportReportAsync<TData>(IEnumerable<TData> data, Dictionary<string, string> param, Dictionary<string, Func<TData, object?>> mappers);
}