const changePhoto = document.querySelector(".change-photo-input-file");
const imgInput = document.querySelector(".send-img-input");

if (changePhoto && imgInput) {
    console.log(changePhoto.src)
    console.log(imgInput.value)

    imgInput.addEventListener("change", function () {
        if (imgInput.files && imgInput.files[0]) {
            const selectedFile = imgInput.files[0];
            const objectURL = URL.createObjectURL(selectedFile);

            changePhoto.src = objectURL;
        }
    })
}




