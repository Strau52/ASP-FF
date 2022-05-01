// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function confirmDelete(uniqId, isDeleteClicked) {
    var deleteSpan = 'deleteSpan_' + uniqId;
    var confirmDerleteSpan = 'confirmDeleteSpan_' + uniqId;

    if (isDeleteClicked) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDerleteSpan).show();
    } else {
        $('#' + deleteSpan).show();
        $('#' + confirmDerleteSpan).hide();
    }
}