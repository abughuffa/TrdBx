using System.Globalization;
using System.Resources;


namespace CleanArchitecture.Blazor.Infrastructure.Constants;

public static partial class ConstantString
{



    //public static string CreateAnItem => Localize("Create {0}");
    //public static string EditTheItem => Localize("Edit {0}");

    
    public static string ListView => Localize("List View");
    public static string DeleteAllData => Localize("Delete All Data");

    //==========================================================//
    //for button text

    public static string Execute => Localize("Execute");
    public static string ExecuteAllTasks => Localize("Execute All Tasks");

    //============================================================================//
    // for toast message
    public static string ExecuteSuccess => Localize("Executed successfully");
    public static string ExecuteFail => Localize("Execute fail");


    //========================================================

    public static string ExecuteTheItem => Localize("Execute the task {0}");
    public static string ExecuteItems => Localize("Execute selected tasks: {0}");
    public static string ExecuteConfirmation => Localize("Are you sure you want to {0}: {1}?");
    public static string ExecuteConfirmationWithId =>
    Localize("Are you sure you want to execute this task with Id: {0}?");
    public static string ExecuteConfirmWithSelected =>
    Localize("Are you sure you want to execute the selected tasks: {0}?");
    public static string ExecuteConfirmationTitle => Localize("Execute Confirmation");








}