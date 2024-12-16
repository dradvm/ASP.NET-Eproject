$(document).ready(() => {
    $('#shoppingCenters').DataTable({
        order: [[4, 'asc']], // Sắp xếp mặc định theo cột đầu tiên (Shop Type)
    });
});




//xử lý delete Shopping center
function deleteFoodcourtsEntry(id) {
    if (confirm("Are you sure you want to delete this shopping center?")) {
        $.ajax({
            url: '/foodcourts/delete',
            type: 'POST',
            contentType: 'application/json', // Đảm bảo gửi dữ liệu JSON
            data: JSON.stringify({ id: id }), // Gửi dữ liệu id dưới dạng JSON
            success: function (response) {
                if (response.success) {
                    alert(response.message);  // Hiển thị thông báo thành công
                    location.reload(); // Reload lại trang
                } else {
                    alert("Error: " + response.message); // Hiển thị thông báo lỗi
                }
            },
            error: function () {
                alert("An error occurred while deleting the shopping center.");
            }
        });
    }
}
