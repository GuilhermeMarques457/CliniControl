const statusMessageBox = document.querySelector(".status-message-box");

if (statusMessageBox) {
    statusMessageBox.addEventListener("click", function (e) {
        if (!e.target.classList.contains("closable-item")) return;

        statusMessageBox.remove();
    })
}