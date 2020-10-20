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

    $(".phone-mask").inputmask('+7 (999) 999-99-99');
    
    /*
    $('form.for-validate .required-input').on('change', function () {
        let canSubmit = true;
        $(this).attr('data-changed', true);
        $(this).parents('form.for-validate').find('.required-input').each(function () {
            if ($(this).hasClass('phone-mask') || $(this).hasClass('email-mask')) {
                if (!$(this).inputmask("isComplete")) {
                    canSubmit = false;
                    if ($(this).attr('data-changed')) {
                        $(this).parents('.form-group').find('.input-group.required').addClass('alert-required');
                    }
                } else {
                    $(this).parents('.form-group').find('.input-group.required').removeClass('alert-required')
                }
            } else {
                if ($(this).hasClass('custom-control-input') && $(this).prop('checked') !== true) {
                    canSubmit = false;
                } else {
                    if (!$(this).hasClass('custom-control-input')) {
                        if (!$(this).val()) {
                            canSubmit = false;
                            if ($(this).attr('data-changed')) {
                                $(this).parents('.form-group').find('.input-group.required').addClass('alert-required');
                                if ($(this).is('select') || $(this).hasClass('dp')) {
                                    $(this).next().find('.select2-selection.select2-selection--single').addClass('alert-border');
                                    $(this).parents('.form-group').css({
                                        'z-index': 1,
                                        'position': 'relative',
                                    })
                                }
                                if ($(this).hasClass('dp')) {
                                    $(this).addClass('alert-border');
                                }
                            }
                        } else {
                            $(this).parents('.form-group').find('.input-group.required').removeClass('alert-required')
                        }
                    }
                }
            }
        });

        if (canSubmit) {
            $(this).parents('form.for-validate').find('a.form-submit').removeClass('disabled').parent()
                .removeAttr('data-toggle')
                .removeAttr('data-placement')
                .removeAttr('data-title')
                .tooltip('disable');
        } else {
            $(this).parents('form.for-validate').find('a.form-submit').addClass('disabled').parent()
                .attr('data-toggle', 'tooltip')
                .attr('data-placement', 'top')
                .attr('data-title', 'Пожалуйста, заполните все обязательные поля')
                .tooltip('enable');
        }
    });
     */

});
