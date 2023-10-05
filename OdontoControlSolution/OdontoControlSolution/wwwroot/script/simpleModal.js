const simpleModal = document.querySelector('.simple-modal');
const btnCloseSimpleModal = document.querySelector('.close-simple-modal');
const simpleModalOpenBox = document.querySelector('.appointment-update-exams-box');
const overlaySimple = document.querySelector('.overlay-simple');
const hiddenSimple = document.querySelector('.hidden-simple');
const inputElementSrc = document.createElement('input');


const closeModalSimple = function () {
    const modalContent = document.querySelector(".simple-modal-content");
    modalContent.remove();
    inputElementSrc.remove();

    simpleModal.classList.add("hidden");
    overlaySimple.classList.add("element-hidden");
};

const openModalSimple = function (btnTarget) {
    const modalBox = document.querySelector('.simple-modal-box');

    const modalContentToBeAdded = btnTarget.cloneNode(true);
    modalContentToBeAdded.classList.add("simple-modal-content");

    const formDelete = document.querySelector(".form-delete-image");

    if (formDelete) {
        // This is to take just the relative path of the img
        const urlCompleto = modalContentToBeAdded.src;
        const url = new URL(urlCompleto);

        inputElementSrc.value = `~${url.pathname.split("~")}`;
        inputElementSrc.name = "urlPathImg"
        inputElementSrc.classList.add("hidden");
    }


    if (modalBox) {
        modalBox.appendChild(modalContentToBeAdded);

        if (inputElementSrc) {
            formDelete.appendChild(inputElementSrc);
        }
    }

    simpleModal.classList.remove("hidden");
    overlaySimple.classList.remove("element-hidden");
};

simpleModalOpenBox.addEventListener("click", function (e) {
    const btnTarget = e.target.closest('.open-simple-modal');
    if (!btnTarget) return;

    openModalSimple(btnTarget);
})

btnCloseSimpleModal.addEventListener('click', closeModalSimple);
overlaySimple.addEventListener('click', closeModalSimple);
