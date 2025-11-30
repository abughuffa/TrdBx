
//using CleanArchitecture.Blazor.Application.Features.Diagnostics.Commands.AddEdit;
//using CleanArchitecture.Blazor.Application.Features.Diagnostics.Commands.Create;
//using CleanArchitecture.Blazor.Application.Features.Diagnostics.Commands.Update;
using CleanArchitecture.Blazor.Application.Features.Diagnostics.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Diagnostics.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class Mapper
{
    public static partial DiagnosticDto ToDto(Diagnostic source);
    public static partial Diagnostic FromDto(DiagnosticDto dto);
    //public static partial Diagnostic FromEditCommand(AddEditDiagnosticCommand command);
    ////public static partial Diagnostic FromCreateCommand(CreateDiagnosticCommand command);
    ////public static partial UpdateDiagnosticCommand ToUpdateCommand(DiagnosticDto dto);
    //////public static partial AddEditDiagnosticCommand CloneFromDto(DiagnosticDto dto);
    ////public static partial void ApplyChangesFrom(UpdateDiagnosticCommand source, Diagnostic target);
    //public static partial void ApplyChangesFrom(AddEditDiagnosticCommand source, Diagnostic target);
    public static partial IQueryable<DiagnosticDto> ProjectTo(this IQueryable<Diagnostic> q);
}

