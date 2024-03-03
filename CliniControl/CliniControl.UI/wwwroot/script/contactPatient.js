
const requestedPatientContainer = document.querySelector(".requested-patient-container");

requestedPatientContainer.addEventListener("click", function (e) {
    const currentTarget = e.target;

    if (currentTarget.classList.contains("btn-send-message")) {
        currentTarget.style.display = 'none';

        var formElement = currentTarget.nextElementSibling;
        formElement.style.display = 'block';
    }
    else if (currentTarget.classList.contains("btn-did-not-contacted")) {
        currentTarget.closest('.form-contact').style.display = 'none';

        var btnMessage = currentTarget.closest(".requested-patient-box").querySelector(".btn-go-whatssap");
        console.log(btnMessage)
        btnMessage.style.display = 'flex';
    }

    return;
})






