$(document).ready(() => {
    $('#gallery').DataTable({
        ordering: false
    });
});

function deleteGalleryEntry(id)
{
    if (confirm('Are you sure to delete the selected gallery entry?')) {
        $(document).ready(() => {
            token = $('input[name="__RequestVerificationToken"]').val();
            $.ajax({
                type: 'post',
                url: `/gallery/delete`,
                data: {
                    __RequestVerificationToken: token,
                    id: id
                },
                success: function (response) {
                    if (response == 'OK') {
                        location.href = '/gallery/index';
                    }
                }
            })
        })
    }
}