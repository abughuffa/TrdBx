using CleanArchitecture.Blazor.Server.UI.Models.NavigationMenu;
namespace CleanArchitecture.Blazor.Server.UI.Services.Navigation;

internal static class TrdBxMenu
{

    public readonly static MenuSectionModel TrdBxMenuSection =
    new MenuSectionModel
    {
        Title = "TrdBx",
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
                       
                        new ()
                        {
                            Title = "Customers",
                            Href = "/pages/TrdBx/Customers",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Sim Cards",
                            Href = "/pages/TrdBx/SimCards",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Tracked Assets",
                            Href = "/pages/TrdBx/TrackedAssets",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Tracking Units",
                            Href = "/pages/TrdBx/TrackingUnits",
                            PageStatus = PageStatus.Completed
                        }
                    }
                },
                new()
                {
                    Title = "Services & Invoices",
                    Icon = Icons.Material.Filled.Analytics,
                    PageStatus = PageStatus.Completed,
                    IsParent = true,
                    MenuItems = new List<MenuSectionSubItemModel>
                    {
                        new()
                        {
                            Title = "Tickets",
                            Href = "/pages/TrdBx/Tickets",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Services",
                            Href = $"/pages/TrdBx/TrackingUnits/0/ServiceLogs?returnUrl={Uri.EscapeDataString("/")}",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                           Title = "Invoices",
                            Href = "/pages/TrdBx/Invoices",
                            PageStatus = PageStatus.Completed
                        },
                         new ()
                        {
                            Title = "Services's Prices",
                            Href = "/pages/TrdBx/ServicePrices",
                            PageStatus = PageStatus.Completed
                        }
                    }
                },
                new()
                {
                    Title = "Local Data",
                    Icon = Icons.Material.Filled.Dataset,
                    PageStatus = PageStatus.Completed,
                    IsParent = true,
                    MenuItems = new List<MenuSectionSubItemModel>
                    {
                        new()
                        {
                            Title = "My Data",
                            Href = "/pages/TrdBx/MyData/Local",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Charts",
                            Href = "/pages/TrdBx/MyData/Local/Charts",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Backup & Restore",
                            Href = "/pages/TrdBx/MyData/Local/BackupRestore",
                            PageStatus = PageStatus.Completed
                        }
                    }
                },
                new()
                {
                    Title = "Online Data",
                    Icon = Icons.Material.Filled.Api,
                    PageStatus = PageStatus.Completed,
                    IsParent = true,
                    MenuItems = new List<MenuSectionSubItemModel>
                    {
                        new()
                        {
                            Title = "Wialon Units",
                            Href = "/pages/TrdBx/MyData/Online/WialonUnits",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Libyana Sim Cards",
                            Href = "/pages/TrdBx/MyData/Online/LibyanaSimCards",
                            PageStatus = PageStatus.Completed
                        }
                        ,
                        new()
                        {
                           Title = "Data Matches",
                            Href =  "/pages/TrdBx/MyData/Online/DataMatches",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                           Title = "Data Diagnosises",
                            Href =  "/pages/TrdBx/MyData/Online/DataDiagnosises",
                            PageStatus = PageStatus.Completed
                        }

                    }
                }
            }
    };

}