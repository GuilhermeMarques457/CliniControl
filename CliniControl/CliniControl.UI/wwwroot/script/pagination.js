const registerPerPages = 10;
const totalRegisters = document.querySelector("#total-registers").textContent;
const totalPages = Math.ceil(totalRegisters / registerPerPages);
const totalRegistersData = [...document.querySelectorAll(".table-row-body")];
const tableBody = document.querySelector(".table-body");
const nextTab = document.querySelector(".next-tab");
const previousTab = document.querySelector(".previous-tab");
const paginationElements = document.querySelector('.pagination-numbers');
let currentPage = 1;

const showRegistersOfThePage = function () {
    const startIndex = (currentPage - 1) * registerPerPages;

    const endIndex = startIndex + registerPerPages;
    const currentRegistersData = totalRegistersData.slice(startIndex, endIndex);

    tableBody.innerHTML = '';

    currentRegistersData.forEach((row) => {
        tableBody.appendChild(row);
    });


}

const addTabWithEventClickToTab = function (value) {
    const aba = document.createElement("span")

    if (value == currentPage) {
        aba.classList.add("current-tab")
    }

    aba.textContent = value;
    aba.classList.add("tab-number")

    if (value != '...') {
        aba.addEventListener('click', () => {
            currentPage = value;
            showRegistersOfThePage();
            createTabsToPagination();
        });
    }
    

    paginationElements.appendChild(aba);
}

const createFirstDefaultTab = function () {
    addTabWithEventClickToTab(1)
    addTabWithEventClickToTab('...')
}

const createFinalDefaultTab = function () {
    addTabWithEventClickToTab('...')
    addTabWithEventClickToTab(totalPages)
}

const createTabsToPagination= function () {
    paginationElements.innerHTML = '';

    let pagesToShow = 5;

    if (totalPages > 5) {

        if (currentPage > 2) {
            pagesToShow = 3;
            createFirstDefaultTab();           
        }

        if (currentPage < totalPages - 3) {
            for (let pageValue = currentPage; pageValue < currentPage + pagesToShow; pageValue++) {
                if (pageValue > totalPages) continue;
                addTabWithEventClickToTab(pageValue)
            }

            createFinalDefaultTab();
        } else {
            for (let pageValue = totalPages - 4; pageValue <= currentPage + pagesToShow; pageValue++) {
                if (pageValue > totalPages) continue;
                addTabWithEventClickToTab(pageValue)
            }
        }

    }
    else {
        for (let pageValue = 1; pageValue <= totalPages; pageValue++) {
            addTabWithEventClickToTab(pageValue)
        }
    }
    
}

nextTab.addEventListener("click", function (e) {
    currentPage++;

    if (currentPage > totalPages) {
        currentPage = 1
    }

    showRegistersOfThePage();
    createTabsToPagination();
})

previousTab.addEventListener("click", function () {
    currentPage--;

    if (currentPage <= 0) {

        currentPage = totalPages;
    }

    showRegistersOfThePage();
    createTabsToPagination();
})

showRegistersOfThePage();
createTabsToPagination();