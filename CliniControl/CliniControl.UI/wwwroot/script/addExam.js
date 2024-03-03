
const appointmentBox = document.querySelector(".appointments-list-container");
const appointmentUpdateBox = document.querySelector(".appointment-update-status-box")
const fileInputs = document.querySelectorAll('.input-exams');

fileInputs.forEach(temp => {
    temp.addEventListener('change', function (e) {
        checkFile(e);
    });
})

const checkFile = function (event) {
    if (!event) return;

    const fileInput = event.target;
    const currentForm = fileInput.closest(".form-file");

    if (fileInput.files.length > 0) {
        currentForm.submit();
    }

}

if (appointmentBox)
    appointmentBox.addEventListener('click', function (e) {
        checkFile();
    })

if (appointmentUpdateBox)
    appointmentUpdateBox.addEventListener('click', function (e) {
        checkFile();
    })




