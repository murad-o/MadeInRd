'use strict';

let mainSearchBtn = document.querySelector('.big-main-search-toggler');
let bigMainSearchBlock = document.querySelector('.big-main-search');
let bigMainSearchInput = document.querySelector('.big-main-search__input');
mainSearchBtn.addEventListener('click', () => {
    mainSearchBtn.classList.add('passive');
    bigMainSearchBlock.classList.add('active');
});

document.addEventListener('click', () => {
    if (event.target.className != mainSearchBtn.className && event.target.className != bigMainSearchInput.className) {
        bigMainSearchBlock.classList.remove('active');
        mainSearchBtn.classList.remove('passive');
    }
});


let currentLanguage = document.getElementById('current-language').textContent;
document.querySelectorAll('.language-switcher__possible-item').forEach(item => {
    item.addEventListener('click', () => {
        let selectedLanguage = item.firstElementChild.value; // get selected language from input
        if (currentLanguage.length === 0) {
            location.pathname = "/" + selectedLanguage + location.pathname;
        } else {
            location.pathname = location.pathname.replace(currentLanguage, selectedLanguage);
        }
    });
});


let burger = document.querySelector('.upper-header__burger');
let overlay = document.querySelector('.overlay');
burger.addEventListener('click', toggleMobileMenu);
overlay.addEventListener('click', toggleMobileMenu);

function toggleMobileMenu() {
    document.querySelector('body').classList.toggle('lock');
    burger.classList.toggle('active');
    document.querySelector('.mobile-content').classList.toggle('active');
    overlay.classList.toggle('active');
}


let menuPopupToggler = document.querySelector('.big-menu__popup-toggler');
let bigPopupMenu = document.querySelector('.big-popup-menu');
menuPopupToggler.addEventListener('click', () => {
    bigPopupMenu.classList.toggle('active');
});

document.addEventListener('click', () => {
    if (event.target.className != menuPopupToggler.className) {
        bigPopupMenu.classList.remove('active');
    }
});


let links = document.querySelectorAll('.main-menu-link');   
let urls = Array.from(links).map(link => new URL(link));
let currentUrl = window.location;
urls.forEach((url, index) => {
    if (currentUrl.pathname.includes(url.pathname)) {
        links[index].classList.add('selected');
    }
});

let popupLinks = document.querySelectorAll('.big-popup-menu__link');
let popupUrls = Array.from(popupLinks).map(link => new URL(link));
popupUrls.forEach((url, index) => {
    if (currentUrl.pathname.includes(url.pathname)) {
        popupLinks[index].classList.add('selected');
        document.querySelector('.big-menu__popup-toggler').classList.add('selected');
    }
})


$('[data-fancybox=""]').fancybox({
    touch: false
});


// Показать/скрыть пароль
function show_hide_password(event, id) {
    let input = document.getElementById(id);

    if (input.getAttribute('type') == 'password') {
        event.currentTarget.classList.add('view');
        input.setAttribute('type', 'text');
    } else {
        event.currentTarget.classList.remove('view');
        input.setAttribute('type', 'password');
    }
}


$(document).ready(function () {
    
});
