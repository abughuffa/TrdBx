using CleanArchitecture.Blazor.Application.Common.Constants;
using CleanArchitecture.Blazor.Server.UI.Models.NavigationMenu;

namespace CleanArchitecture.Blazor.Server.UI.Services.Navigation;

public class MenuService : IMenuService
{
    private readonly List<MenuSectionModel> _features = new()
    {


        new MenuSectionModel
        {
            Title = "Application",
            SectionItems = new List<MenuSectionItemModel>
            {
                new() { Title = "Home", Icon = Icons.Material.Filled.Home, Href = "/" },
                new()
                {
                    Title = "Chatbot",
                    Roles = new[] { Roles.Admin, Roles.Users },
                    Icon = Icons.Material.Filled.ChatBubble,
                    Href ="/ai/chatbot",
                    PageStatus = PageStatus.Completed
                },
                new ()
                {
                    Title = "Basic Objects",
                    Icon = Icons.Material.Filled.Dashboard,
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
                            Title = "Services Log",
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
                            Roles = new[] { Roles.Admin },
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
                            Roles = new[] { Roles.Admin },
                            Href = "/pages/TrdBx/MyData/Local/TrdBxData",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Impulse Charts",
                            Href = "/pages/TrdBx/MyData/Local/ImpulseCharts",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Backup & Restore",
                            Roles = new[] { Roles.Admin },
                            Href = "/pages/TrdBx/MyData/Local/RestoreBackup",
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
                            Title = "Wialon Server Test",
                            Href = "/wialon/session-manager",
                            PageStatus = PageStatus.Completed
                        },
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
                        },
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
                },
                new()
                {
                    Title = "Online Tasks",
                    Icon = Icons.Material.Filled.Web,
                    PageStatus = PageStatus.Completed,
                    IsParent = true,
                    MenuItems = new List<MenuSectionSubItemModel>
                    {
                        new()
                        {
                            Title = "Wialon Tasks",
                            Href = "/pages/TrdBx/WialonTasks/0/0",
                            PageStatus = PageStatus.Completed
                        }

                    }
                }
            }
               
        },
        new MenuSectionModel
        {
            Title = "MANAGEMENT",
            Roles = new[] { Roles.Admin },
            SectionItems = new List<MenuSectionItemModel>
            {
                new()
                {
                    IsParent = true,
                    Title = "Authorization",
                    Icon = Icons.Material.Filled.ManageAccounts,
                    MenuItems = new List<MenuSectionSubItemModel>
                    {
                        new()
                        {
                            Title = "Multi-Tenant",
                            Href = "/system/tenants",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Users",
                            Href = "/identity/users",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Roles",
                            Href = "/identity/roles",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Profile",
                            Href = "/user/profile",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Login History",
                            Href = "/pages/identity/loginaudits",
                            PageStatus = PageStatus.Completed
                        },
                    }
                },
                new()
                {
                    IsParent = true,
                    Title = "System",
                    Icon = Icons.Material.Filled.Devices,
                    MenuItems = new List<MenuSectionSubItemModel>
                    {
                        new()
                        {
                            Title = "Picklist",
                            Href = "/system/picklistset",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Audit Trails",
                            Href = "/system/audittrails",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Email Templates",
                            Href = "/pages/system/email-templates",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Logs",
                            Href = "/system/logs",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Jobs",
                            Href = "/jobs",
                            PageStatus = PageStatus.Completed,
                            Target = "_blank"
                        }
                    }
                }
            }
        }
    };

    public IEnumerable<MenuSectionModel> Features => _features;
}
