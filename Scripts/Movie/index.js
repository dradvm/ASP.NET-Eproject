$(document).ready(() => {
    $('#movies').DataTable({
        order: [[4, 'desc']]
    });
});

function deleteMovie(id) {
    if (confirm('Are you sure to delete the selected movie along with its showtimes?')) {
        window.location = `/movie/delete?id=${id}`;
    }
}