const paidBox = document.querySelector(".radio-input-box");
const notPaid = document.querySelector(".not-paid")
const paid = document.querySelector(".paid")

paidBox.addEventListener("click", function (e) {
    const target = e.target;
   
    if (!target.classList.contains("status-paid")) return;

    const radio = target.nextElementSibling;
    radio.checked = true;

    if (target.classList.contains("not-paid")) {
        notPaid.classList.remove("not-paid-not-selected");
        paid.classList.add("paid-not-selected");
    }

    if (target.classList.contains("paid")) {
        paid.classList.remove("paid-not-selected");
        notPaid.classList.add("not-paid-not-selected");
    }
})