$(document).ready(function () {
    $("#btnSubmit").click(function (e) {
        e.preventDefault(); // Ngăn form tự động gửi đi (submit mặc định)

        // Hiển thị thông báo thành công
        alert("Gửi thông tin thành công! Cảm ơn bạn đã liên hệ với chúng tôi.");

        // Xóa dữ liệu trong các trường input và textarea
        $(".form")[0].reset();
    });

    $("#btnRest").click(function () {
        // Xóa dữ liệu trong các trường input và textarea khi nhấn nút "Start Over"
        $(".form")[0].reset();
    });
});
