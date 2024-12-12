const image = document.getElementById('image');
const preview = document.getElementById('preview');

function loadImage() {
    let file = image.files[0];
    if (file) {
        let reader = new FileReader();
        reader.onload = function (e) {
            preview.src = e.target.result;
        }
        reader.readAsDataURL(file);
    }
    else {
        preview.src = '/Assets/Images/empty.jpg';
    }
}