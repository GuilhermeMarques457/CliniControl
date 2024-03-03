document.addEventListener("DOMContentLoaded", function () {
    const forms = document.querySelectorAll(".form-change-status");

    if (forms.length > 0) {
        forms.forEach(form => form.submit());
    }

});




   


