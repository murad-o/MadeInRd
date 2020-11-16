// Валидация на загрузку изображения
let fileInput = document.getElementById('file-input');
fileInput.addEventListener('change', () => {
    const validExtensions = ['.jpg', '.png', '.jpeg'];
    let fileName = fileInput.files[0].name;
    let fileExtension = fileName.substr(fileName.lastIndexOf('.'));
    if (!validExtensions.includes(fileExtension)) {
        fileInput.value = "";
        alert("Принимаются только следующие типы файлов: " + validExtensions.join(', '));
    }
});