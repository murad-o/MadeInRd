// Валидация на загрузку изображения
let fileInput = document.getElementById('file-input');
fileInput?.addEventListener('change', () => {
    const validExtensions = ['.jpg', '.png', '.jpeg'];
    let fileName = fileInput.files[0].name;
    let fileExtension = fileName.substr(fileName.lastIndexOf('.'));

    if (!validExtensions.includes(fileExtension)) {
        fileInput.value = "";
        alert("Принимаются только следующие типы файлов: " + validExtensions.join(', '));
    }
});


// Удаление изображения
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

    if (result.ok) {
        document.getElementById('current-image-row').style.display = 'none';
        document.querySelector('.upload-img-block').style.display = "block";
    } else {
        alert('Неизвестная ошибка: ' + result.statusText);
    }
});