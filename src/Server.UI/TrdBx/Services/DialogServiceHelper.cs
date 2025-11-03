using CleanArchitecture.Blazor.Server.UI.TrdBx.Components.Dialogs;
using MediatR;

namespace CleanArchitecture.Blazor.Server.UI.Services;

/// <summary>
/// Helper class for dialog service operations.
/// </summary>
public partial class DialogServiceHelper
{
    /// <summary>
    /// Shows a execute confirmation dialog.
    /// </summary>
    /// <param name="command">The command to execute on confirmation.</param>
    /// <param name="title">The title of the dialog.</param>
    /// <param name="contentText">The content text of the dialog.</param>
    /// <param name="onConfirm">The action to perform on confirmation.</param>
    /// <param name="onCancel">The action to perform on cancellation (optional).</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ShowExecuteConfirmationDialogAsync(IRequest<Result> command, string title, string contentText, Func<Task> onConfirm, Func<Task>? onCancel = null)
    {
        var parameters = new DialogParameters
            {
                { nameof(ExecuteConfirmation.ContentText), contentText },
                { nameof(ExecuteConfirmation.Command), command }
            };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall, FullWidth = true };
        var dialog = await _dialogService.ShowAsync<ExecuteConfirmation>(title, parameters, options).ConfigureAwait(false);
        var result = await dialog.Result.ConfigureAwait(false);
        if (result is not null && !result.Canceled)
        {
            await onConfirm().ConfigureAwait(false);
        }
        else if (onCancel != null)
        {
            await onCancel().ConfigureAwait(false);
        }
    }


    /// <summary>
    /// Shows a form dialog with the specified model and handles actions on completion.
    /// </summary>
    /// <typeparam name="TDialog">The type of dialog component to show.</typeparam>
    /// <typeparam name="TModel">The type of model to pass to the dialog.</typeparam>
    /// <param name="title">The title of the dialog.</param>
    /// <param name="contentText">The content text of the dialog.</param>
    /// <param name="model">The model to pass to the dialog.</param>
    /// <param name="onDialogResult">Action to perform when dialog returns a non-cancelled result.</param>
    /// <param name="parameterName">The name of the parameter in the dialog component (default is "_model").</param>
    /// <param name="options">Dialog options (optional).</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ShowFormDialogWithContentTextAsync<TDialog, TModel>(
        string title,
        string contentText,
        TModel model,
        Func<Task> onDialogResult,
        string MparameterName = "_model",
        string CparameterName = "_contentText",
        DialogOptions? options = null) where TDialog : ComponentBase
    {
        var parameters = new DialogParameters
        {
            { MparameterName, model },
            { CparameterName, contentText }
        };

        var dialogOptions = options ?? new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        var dialog = await _dialogService.ShowAsync<TDialog>(title, parameters, dialogOptions);
        var result = await dialog.Result;

        if (result != null && !result.Canceled)
        {
            await onDialogResult();
        }
    }
}
