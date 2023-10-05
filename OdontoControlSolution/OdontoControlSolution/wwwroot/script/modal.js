const modal = document.querySelector('.modal');
const btnOpenModal = document.querySelectorAll('.btn-modal');
const btnCloseModal = document.querySelector('.close-modal');
const overlay = document.querySelector('.overlay');
const hidden = document.querySelector('.hidden');
const modalMessage = document.querySelector('.modal-message');

// Places where my modals are
const table = document.querySelector('.table-body');
const appointmentContainer = document.querySelector(".appointments-list-container");
const appoinementDetails = document.querySelector(".appointment-details-box"); 


const closeModal = function () {
    modal.classList.add("hidden");
    overlay.classList.add("element-hidden");
    modalMessage.querySelector("b").remove();

    const modalButton = document.querySelector('.btn-modal-item').nextElementSibling;

    modalButton.remove();
};

const openModal = function (e) {
    const btn = e.target.closest('.btn-modal');
    if (!btn) return;

    const itemNameElement = btn.parentNode.querySelector('.item-name');
    const itemIdElement = btn.parentNode.querySelector('.item-id');
    const modalBox = document.querySelector('.modal-box');
    const message = `<b>${itemNameElement.textContent}?</b>`
    const formModal = document.querySelector(".form-modal");

    if (modalBox) {
        const inputElementID = document.createElement('input');
        inputElementID.name = 'ID';
        inputElementID.value = itemIdElement.textContent;
        inputElementID.style.display = "none";

        formModal.appendChild(inputElementID);
        modalMessage.innerHTML += message;
    }

    modal.classList.remove("hidden");
    overlay.classList.remove("element-hidden");
};


if (table) {
    table.addEventListener('click', openModal)
}

if (appoinementDetails) {
    appoinementDetails.addEventListener('click', openModal)
}

if (appointmentContainer) {
    appointmentContainer.addEventListener('click', openModal)
}

btnCloseModal.addEventListener('click', closeModal);
overlay.addEventListener('click', closeModal);
