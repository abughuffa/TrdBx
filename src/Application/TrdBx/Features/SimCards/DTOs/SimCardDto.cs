using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Blazor.Domain.Enums;

namespace CleanArchitecture.Blazor.Application.Features.SimCards.DTOs;

[Description("SimCards")]
public class SimCardDto
{
    [Display(Name = "Id")]
    public int Id { get; set; }
    [Display(Name = "SimCardNo")]
    public string SimCardNo { get; set; } = string.Empty;
    [Display(Name = "ICCID")]
    public string? ICCID { get; set; }
    [Display(Name = "SPackageId")]
    public int SPackageId { get; set; }
    [Display(Name = "SStatus")]
    public SStatus SStatus { get; set; }
    [Display(Name = "ExDate")]
    public DateOnly? ExDate { get; set; }
    [Display(Name = "OldId")]
    public int? OldId { get; set; } = null;

    [Display(Name = "IsOwned")]
    public bool IsOwned { get; set; } = true;

    
    [Display(Name = "SPackage")] public string? SPackage { get; set; }

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

