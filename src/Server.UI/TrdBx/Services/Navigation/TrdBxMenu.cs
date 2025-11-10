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
                            Title = "Services's Prices",
                            Href = "/pages/TrdBx/ServicePrices",
                            PageStatus = PageStatus.Completed
                        },
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
                        }
                    }
                },
                new()
                {
                    Title = "Tests",
                    Icon = Icons.Material.Filled.Api,
                    PageStatus = PageStatus.Completed,
                    IsParent = true,
                    MenuItems = new List<MenuSectionSubItemModel>
                    {
                        new()
                    {
                        Title = "Wialon Units",
                        Href = "/pages/TrdBx/WialonUnits",
                        PageStatus = PageStatus.Completed
                    },
                    new()
                    {
                        Title = "Libyana Sim Cards",
                        Href = "/pages/TrdBx/LibyanaSimCards",
                        PageStatus = PageStatus.Completed
                    },



                        new()
                        {
                           Title = "Syncronize Data",
                            Href = "/pages/TrdBx/DbForceSyncs",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Charts",
                            Href = "/pages/TrdBx/Charts",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                           Title = "Diagnostics",
                            Href =  "/pages/TrdBx/Diagnostics",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                           Title = "Data Matching",
                            Href =  "/pages/TrdBx/DbMatchings",
                            PageStatus = PageStatus.Completed
                        }
                    }
                },
                new()
                {
                    Title = "Test Cases",
                    Icon = Icons.Material.Filled.Analytics,
                    PageStatus = PageStatus.Completed,
                    IsParent = true,
                    MenuItems = new List<MenuSectionSubItemModel>
                    {
                        new()
                        {
                            Title = "Fake Data",
                            Href = "/pages/TrdBx/FakeData",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Backup & Restore",
                            Href = "/pages/TrdBx/BackupRestore",
                            PageStatus = PageStatus.Completed
                        },

                        
                        //new()
                        //{
                        //   Title = "Activate Cases",
                        //    Href = "/pages/TrdBx/ActivateTestCases",
                        //    PageStatus = PageStatus.Completed
                        //},
                        //new()
                        //{
                        //   Title = "ActivateGprs Cases",
                        //    Href = "/pages/TrdBx/ActivateGprsTestCases",
                        //    PageStatus = PageStatus.Completed
                        //},
                        //new()
                        //{
                        //   Title = "ActivateHosting Cases",
                        //    Href = "/pages/TrdBx/ActivateHostingTestCases",
                        //    PageStatus = PageStatus.Completed
                        //},
                        //new()
                        //{
                        //   Title = "Deactivate Cases",
                        //    Href = "/pages/TrdBx/DeactivateTestCases",
                        //    PageStatus = PageStatus.Completed
                        //}
                    }
                }
            }
    };

}