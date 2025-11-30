using CleanArchitecture.Blazor.Server.UI.Models.NavigationMenu;
namespace CleanArchitecture.Blazor.Server.UI.Services.Navigation;

internal static class ShipmentDeliveryMenu
{

    public readonly static MenuSectionModel ShipmentDeliveryMenuSection =
    new MenuSectionModel
    {
        Title = "Shipment Delivery",
        SectionItems = new List<MenuSectionItemModel>
            {
                new ()
                {
                    Title = "System Objects",
                    Icon = Icons.Material.Filled.ShoppingCart,
                    PageStatus = PageStatus.Completed,
                    IsParent = true,
                    MenuItems = new List<MenuSectionSubItemModel>
                    {
                         new()
                        {
                            Title = "Warehouses",
                            Href = "/pages/Warehouses",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Shipments",
                            Href = "/pages/Shipments",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Vehicles",
                            Href = "/pages/Vehicles",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Drivers",
                            Href = "/pages/Drivers",
                            PageStatus = PageStatus.Completed
                        }
                    }
                }
            }
    };

}