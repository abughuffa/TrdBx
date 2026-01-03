
using CleanArchitecture.Blazor.Application.Features.SimCards.Commands.AddEdit;
using CleanArchitecture.Blazor.Application.Features.SimCards.Commands.Create;
using CleanArchitecture.Blazor.Application.Features.SimCards.Commands.Update;
using CleanArchitecture.Blazor.Application.Features.SimCards.DTOs;
using CleanArchitecture.Blazor.Application.Features.WialonTasks.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.SimCards.Mappers;

#pragma warning disable RMG020
#pragma warning disable RMG012
[Mapper]
public static partial class Mapper
{

    [MapProperty(nameof(SimCard.SPackage.Name), nameof(SimCardDto.SPackage))]
    public static partial SimCardDto ToDto(SimCard source);
    [MapperIgnoreSource(nameof(SimCardDto.SPackage))]
    public static partial SimCard FromDto(SimCardDto dto);
    public static partial SimCard FromEditCommand(AddEditSimCardCommand command);
    public static partial SimCard FromCreateCommand(CreateSimCardCommand command);
    public static partial UpdateSimCardCommand ToUpdateCommand(SimCardDto dto);
    public static partial AddEditSimCardCommand CloneFromDto(SimCardDto dto);
    public static partial void ApplyChangesFrom(UpdateSimCardCommand source, SimCard target);
    public static partial void ApplyChangesFrom(AddEditSimCardCommand source, SimCard target);
    public static partial IQueryable<SimCardDto> ProjectTo(this IQueryable<SimCard> q);

    //public static partial IQueryable<SimCardDto> ProjectTo(this IQueryable<SimCard> q)
    //{
    //    return q.Select(simCard => new SimCardDto
    //    {
    //        Id = simCard.Id,
    //        SPackageId = simCard.SPackageId,
    //        SimCardNo = simCard.SimCardNo,
    //        ICCID = simCard.ICCID,  
    //        SStatus= simCard.SStatus,   
    //        ExDate = simCard.ExDate,    
    //        OldId = simCard.OldId,  
    //        SPackage = simCard.SPackage != null ? simCard.SPackage.Name : string.Empty
    //    });
    //}

}




//I have this entity:

//public class SimCard
//{
//    public int Id { get; set; }
//    public int SPackageId { get; set; }
//    public string SimNo { get; set; }
//    public SPackage SPackage { get; set; }
//}

//which has navigation probrty to the entity:

//public class SPackage
//{
//    public int Id { get; set; }
//    public string Name { get; set; }
//    public Collecation<SimCard> SimCards { get; set; }
//}

//And the Dto:

//public class SimCardDto
//{
//    public int Id { get; set; }
//    public int SPackageId { get; set; }
//    public string SimNo { get; set; }
//    public string SPackage { get; set; }
//}

//when i retrive data from database includining SPackage, i would like to map SimCard to SimCardDto using Riok.Mapperly, can you help me to create SimCardMapper class
