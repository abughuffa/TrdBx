
using CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataDiagnosises.DTOs;



namespace CleanArchitecture.Blazor.Application.TrdBx.Features.MyData.Online.DataDiagnosises.Mappers;
#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class Mapper
{
    public static partial DataDiagnosisDto ToDto(DataDiagnosis source);
    public static partial DataDiagnosis FromDto(DataDiagnosisDto source);
    public static partial IQueryable<DataDiagnosisDto> ProjectTo(this IQueryable<DataDiagnosis> q);
}

