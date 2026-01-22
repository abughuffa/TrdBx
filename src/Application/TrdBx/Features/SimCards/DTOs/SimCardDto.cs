using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.SimCards.DTOs;

[Description("SimCards")]
public class SimCardDto
{
    [Description("Id")]
    public int Id { get; set; }
    [Description("SimCardNo")]
    public string SimCardNo { get; set; } = string.Empty;
    [Description("ICCID")]
    public string? ICCID { get; set; }
    [Description("SPackageId")]
    public int SPackageId { get; set; }
    [Description("SStatus")]
    public SStatus SStatus { get; set; }
    [Description("ExDate")]
    public DateOnly? ExDate { get; set; }
    [Description("OldId")]
    public int? OldId { get; set; } = null;

    [Description("IsOwen")]
    public bool IsOwen { get; set; } = true;

    
    [Description("SPackage")] public string? SPackage { get; set; }

//    i have automapper code like this:

//    private class Mapping : Profile
//    {
//        public Mapping()
//        {
//            CreateMap<SimCard, SimCardDto>(MemberList.None)
//                .ForMember(dest => dest.SPackage,
//                      opt => opt.MapFrom(src => (src.SPackage == null ? null : src.SPackage.Name)));
//            CreateMap<SimCardDto, SimCard>(MemberList.None);
//        }
//    }

//    i would like to rebuild it using Riok.Mapperly
    
//[Mapper]
//public static partial class Mapper
//{
//    public static partial SimCardDto ToDto(SimCard source);

//    [MapperIgnoreSource(nameof(SimCardDto.SPackage))]
//    public static partial SimCard FromDto(SimCardDto dto);
//    public static partial SimCard FromEditCommand(AddEditSimCardCommand command);
//    public static partial SimCard FromCreateCommand(CreateSimCardCommand command);
//    public static partial UpdateSimCardCommand ToUpdateCommand(SimCardDto dto);
//    public static partial AddEditSimCardCommand CloneFromDto(SimCardDto dto);
//    public static partial void ApplyChangesFrom(UpdateSimCardCommand source, SimCard target);
//    public static partial void ApplyChangesFrom(AddEditSimCardCommand source, SimCard target);
//    public static partial IQueryable<SimCardDto> ProjectTo(this IQueryable<SimCard> q);
//}

}

