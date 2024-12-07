$(document).ready(() => {
    $('#gallery').DataTable({
        ordering: false
    });
});

function deleteGalleryEntry(id)
{
    console.log(1);
    if (confirm('Are you sure to delete the selected gallery entry?')) {
        window.location = `/gallery/delete?id=${id}`; 
    }
}