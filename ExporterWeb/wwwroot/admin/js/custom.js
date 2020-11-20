let exportersLink = document.querySelectorAll('.exporters-link');
exportersLink.forEach(item => {
    if (item.pathname === location.pathname) {
       item.classList.add('active');
    }
});