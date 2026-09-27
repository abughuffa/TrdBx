using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Domain.Common.Entities;
using CleanArchitecture.Blazor.Domain.Enums;
using CleanArchitecture.Blazor.Domain.Identity;

namespace CleanArchitecture.Blazor.Domain.Entities;


public class SmsCursor : BaseEntity
{
    public string Key   { get; set; } = null!;
    public long Value { get; set; } = 0L!;
}