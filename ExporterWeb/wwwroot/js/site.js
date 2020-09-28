let searchButton = document.querySelector('.upper-header-search')
searchButton.addEventListener('click', () => {
    document.querySelector('.large-search').classList.toggle('active');
})


let companyName = document.querySelector('.header-account__name');
companyName?.addEventListener('click', () => {
    document.querySelector('.account-pop-up').classList.toggle('active');
});


let links = document.getElementsByClassName('large-menu__link');
let urls = Array.from(links).map(item => new URL(item));
let currentUrl = window.location;
urls.forEach((item, i) => {
    if (currentUrl.pathname.includes(urls[i].pathname))
        links[i].parentNode.classList.add('selected');
});


let burger = document.querySelector('.upper-header__burger');
let overlay = document.querySelector('.overlay');
burger.addEventListener('click', toggleMobileMenu);
overlay.addEventListener('click', toggleMobileMenu);

function toggleMobileMenu() {
    document.querySelector('body').classList.toggle('lock');
    burger.classList.toggle('active');
    document.querySelector('.small-body').classList.toggle('active');
    overlay.classList.toggle('active');
}