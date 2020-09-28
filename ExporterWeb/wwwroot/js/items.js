let items = [
    document.querySelector('.industry-filters__btn'),
    document.querySelector('.items-overlay'),
    document.querySelector('.industries-mobile-header__close-btn'),
];

items.forEach(item => {
    item.addEventListener('click', toggleMobileFilters);
});

function toggleMobileFilters() {
    document.querySelector('.industries').classList.toggle('active');
    document.querySelector('.items-overlay').classList.toggle('active');
    document.querySelector('body').classList.toggle('lock');
}