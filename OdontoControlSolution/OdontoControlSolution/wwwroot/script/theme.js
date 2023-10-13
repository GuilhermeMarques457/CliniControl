let currentTheme;
const themeToggle = document.getElementById("theme-toggle");

document.addEventListener("DOMContentLoaded", function () {
    if (localStorage.getItem("theme")) {
        currentTheme = localStorage.getItem("theme");
    }

    currentTheme === "dark" ? ativarModoNoturno() : ativarModoClaro();
});

themeToggle.addEventListener("click", function () {
    const body = document.body;
    body.classList.contains("dark-mode") ? ativarModoClaro() : ativarModoNoturno()
});

function ativarModoNoturno() {
    localStorage.setItem("theme", "dark");
    themeToggle.checked = true;

    document.body.classList.add("dark-mode");
    document.documentElement.style.setProperty("--dark-color", "#f9fbfc");
    document.documentElement.style.setProperty("--white-color", "#0a0f12");

    document.documentElement.style.setProperty("--dark-color-opacity", "rgba(239, 245, 248, 0.5)");
    document.documentElement.style.setProperty("--white-color-opacity", "rgba(10, 15, 18, 0.5)");

    document.documentElement.style.setProperty("--dark-color-lighter", "#e0e2e3");
    document.documentElement.style.setProperty("--white-color-darker", "#6c6f71");

    document.documentElement.style.setProperty("--pure-dark-color", "#fff");
    document.documentElement.style.setProperty("--pure-white-color", "#000");
}

function ativarModoClaro() {
    localStorage.removeItem("theme")
    document.body.classList.remove("dark-mode");
    document.documentElement.style.setProperty("--dark-color", "#0a0f12");
    document.documentElement.style.setProperty("--white-color", "#f9fbfc");

    document.documentElement.style.setProperty("--white-color-opacity", "rgba(239, 245, 248, 0.5)");
    document.documentElement.style.setProperty("--color-color-opacity", "rgba(10, 15, 18, 0.5)");

    document.documentElement.style.setProperty("--dark-color-lighter", "#6c6f71");
    document.documentElement.style.setProperty("--white-color-darker", "#e0e2e3");

    document.documentElement.style.setProperty("--pure-dark-color", "#000");
    document.documentElement.style.setProperty("--pure-white-color", "#fff");
   
}