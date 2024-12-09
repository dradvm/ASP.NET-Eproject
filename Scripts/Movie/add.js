const image = document.getElementById('image');
const preview = document.getElementById('preview');

function loadImage() {
    let file = image.files[0];
    if (file) {
        let reader = new FileReader();
        reader.onload = function (e) {
            preview.src = e.target.result;
            preview.style.height = '200px';
            preview.style.width = 'auto';
        }
        reader.readAsDataURL(file);
    }
}