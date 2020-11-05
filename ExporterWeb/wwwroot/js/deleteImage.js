let deleteImageForm = document.getElementById('delete-image-form');
deleteImageForm?.addEventListener('submit', async function(e) {
    e.preventDefault();

    if (!confirm('Вы действительно хотите удалить изображение?'))
        return;

    const handlerParam = 'handler=deleteimage';
    const url = location.search ? `${location.search}&${handlerParam}` : `?${handlerParam}`;
    const result = await fetch(url, {
        method: 'post',
        body: new FormData(this)
    })

    if (result.ok)
        document.getElementById('edit-img').style.display = 'none';
    else
        alert('Неизвестная ошибка: ' + result.statusText);
});