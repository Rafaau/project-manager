// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function CloseModal(modalId) {
    $(modalId).modal('hide');
}

function CloseDropdown(dropdownId) {
    $(dropdownId).dropdown('hide');
}

function ScrollToAnchor() {
    document.getElementById("anchor").scrollIntoView({ behavior: "smooth" });
}

function AnimateConversations() {
    const btns = document.querySelectorAll(".single-conversations");

    btns.forEach((btns, number) => {
        btns.style.animationDelay = (number / 10 + "s");
    })
}

function AnimateNotifications() {
    const btns = document.querySelectorAll(".single-notification");

    btns.forEach((btns, number) => {
        btns.style.animationDelay = (number / 10 + "s");
    })
}

function CopyToClipboard(id) {
    var copyText = $('[id$="fieldToCopy"]').val();

    navigator.clipboard.writeText(copyText);
}



