let searchButton = document.querySelector('.upper-header-search')
searchButton.addEventListener('click', () => {
    document.querySelector('.large-search').classList.toggle('active');
})


let companyName = document.getElementById('company-name');
if (companyName != null) {
    companyName.addEventListener('click', () => {
        document.querySelector('.company-pop-up').classList.toggle('active');
    });
}


let links = document.getElementsByClassName('large-menu__link');
let urls = Array.from(links).map(item => new URL(item));
let currentUrl = window.location;
urls.forEach((item, i) => {
    if (currentUrl.pathname.includes(urls[i].pathname))
        links[i].parentNode.classList.add('selected');
});


let burger = document.querySelector('.upper-header__burger');
let overlay = document.querySelector('.overlay');
burger.addEventListener('click', showMobileMenu);
overlay.addEventListener('click', showMobileMenu);

function showMobileMenu() {
    document.querySelector('body').classList.toggle('lock');
    document.querySelector('.header').classList.toggle('active');
    burger.classList.toggle('active');
    document.querySelector('.small-body').classList.toggle('active');
    document.querySelector('.overlay').classList.toggle('active');
}