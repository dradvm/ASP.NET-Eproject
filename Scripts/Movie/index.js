$(document).ready(() => {
    $('#movies').DataTable({
        order: [[4, 'desc']]
    });
});

function disableMovie(id) {
    if (confirm('Are you sure to disable the selected movie?')) {
        $(document).ready(() => {
            token = $('input[name="__RequestVerificationToken"]').val();
            $.ajax({
                type: 'post',
                url: `/movie/disable`,
                data: {
                    __RequestVerificationToken: token,
                    id: id
                },
                success: function (response) {
                    if (response == 'OK') {
                        location.href = '/movie/index';
                    }
                }
            })
        });
    }
}

function deleteMovie(id) {
    if (confirm('Are you sure to delete the selected movie along with its showtimes?')) {
        $(document).ready(() => {
            token = $('input[name="__RequestVerificationToken"]').val();
            $.ajax({
                type: 'post',
                url: `/movie/delete`,
                data: {
                    __RequestVerificationToken: token,
                    id: id
                },
                success: function (response) {
                    if (response == 'OK') {
                        location.href = '/movie/index';
                    }
                }
            })
        });
    }
}