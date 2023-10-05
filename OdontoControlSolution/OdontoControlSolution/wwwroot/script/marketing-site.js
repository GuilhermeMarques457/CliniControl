"use strict";

// Set current year
const currentYearEl = document.querySelector(".currenty-year");
const currentYear = new Date().getFullYear();
currentYearEl.textContent = currentYear;

//////////////////////////////
////// Mobile Navigation ////
//////////////////////////////
let btnNav = document.querySelector(".btn-mobile-nav");
let btnClose = document.querySelector(".btn-close");
let btnOpen = document.querySelector(".btn-open");
let navEl = document.querySelector(".main-nav");

btnNav.addEventListener("click", function () {
  btnClose.classList.toggle("hidden");
  btnOpen.classList.toggle("hidden");

  navEl.classList.toggle("opacity-transition");
});

document.addEventListener("keydown", function (e) {
  if (e.key == "Escape" && navEl.classList.contains("opacity-transition")) {
    navEl.classList.remove("opacity-transition");
    btnOpen.classList.remove("hidden");
    btnClose.classList.add("hidden");
  }
});

//////////////////////////////////////
////// Smoth scrolling animation ////
//////////////////////////////////////

const allLinks = document.querySelectorAll("a:link");

allLinks.forEach(function (link) {
  link.addEventListener("click", function (e) {
    // to change the default behavior
    e.preventDefault();

    const href = link.getAttribute("href");

    // Scroll back to top

    if (href === "#")
      window.scrollTo({
        top: 0,
        behavior: "smooth",
      });

    // Scroll back to other links
    if (href !== "#" && href.startsWith("#")) {
      const sectionEl = document.querySelector(href);
      sectionEl.scrollIntoView({
        behavior: "smooth",
      });
    }

    // To remove the navigation when it's clicked
    if (link.classList.contains("main-nav-link")) {
      btnClose.classList.toggle("hidden");
      btnOpen.classList.toggle("hidden");
      navEl.classList.toggle("opacity-transition");
    }
  });
});

//////////////////////////////////////
////// Sticky navigation bar ////
//////////////////////////////////////

const headerEl = document.querySelector(".header");
const heroEl = document.querySelector(".hero-section");

const observer = new IntersectionObserver(
  function (entries) {
    const ent = entries[0];
    if (ent.isIntersecting === false) {
      headerEl.classList.add("sticky");
    }
    if (ent.isIntersecting === true) {
      headerEl.classList.remove("sticky");
    }
  },
  {
    // in the viewport
    root: null,
    // we have an envent as son as 0% of the section is on the view port
    threshold: 0,
    rootMargin: "-70px",
  }
);
observer.observe(heroEl);

///////////////////////////////////////////////////////////
// Fixing flexbox gap property missing in some Safari versions
///////////////////////////////////////////////////////////

function checkFlexGap() {
  var flex = document.createElement("div");
  flex.style.display = "flex";
  flex.style.flexDirection = "column";
  flex.style.rowGap = "1px";

  flex.appendChild(document.createElement("div"));
  flex.appendChild(document.createElement("div"));

  document.body.appendChild(flex);
  var isSupported = flex.scrollHeight === 1;
  flex.parentNode.removeChild(flex);
  console.log(isSupported);

  if (!isSupported) document.body.classList.add("no-flexbox-gap");
}
checkFlexGap();

// https://unpkg.com/smoothscroll-polyfill@0.4.4/dist/smoothscroll.min.js just to safari

/*

*/
