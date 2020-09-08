let burger = document.querySelector('.header__burger');
burger.addEventListener('click', () => {
    document.querySelector('.header__body').classList.toggle('active');
});


let searchButton = document.querySelector('.header__show-search-body');
searchButton.addEventListener('click', () => {
    document.querySelector('.header__search-body').classList.toggle('active');
})


let links = document.querySelectorAll('header li a');
let urls = Array.from(links).map(item => new URL(item));
let currentUrl = window.location;
for (let i = 0; i < urls.length; i++) {
    if (currentUrl.pathname.includes(urls[i].pathname))
        links[i].parentNode.classList.add('selected');
}