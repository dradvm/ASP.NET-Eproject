//xử lý phân trang

$(document).ready(() => {
    $('#menuFoodcourts').DataTable({
        order: [[4, 'asc']], // Sắp xếp mặc định theo cột đầu tiên (Shop Type)
    });
});




//xử lý delete Shopping center
function deleteMenuEntry(id) {
    if (confirm("Are you sure you want to delete this menu Food Court?")) {
        var token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            url: '/menu/delete',
            type: 'POST',
            data:
            {
                __RequestVerificationToken: token,
                id: id
            },
            success: function (response) {
                if (response.success) {
                    alert(response.message);  // Hiển thị thông báo thành công
                    location.reload(); // Reload lại trang
                } else {
                    alert("Error: " + response.message); // Hiển thị thông báo lỗi
                }
            },
            error: function () {
                alert("An error occurred while deleting the Food Court.");
            }
        });
    }
}



