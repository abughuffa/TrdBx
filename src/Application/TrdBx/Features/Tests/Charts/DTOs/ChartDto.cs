// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.Charts.Dto;

[Description("Chart")]
public class ChartDto
{
    [Description("Date")]
    public DateOnly Date { get; set; }

    [Description("Count")]
    public double Count { get; set; }

    [Description("Objects")]
    public List<string>? Objects { get; set; }


}


