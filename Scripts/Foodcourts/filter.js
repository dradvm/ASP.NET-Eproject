// Hàm để xử lý lọc
function filterShops(selectedName) {
    // Ẩn tất cả các card
    $(".about__card--model").hide();

    // Hiển thị các card có data-name khớp với selectedName
    if (selectedName === "all") {
        // Hiển thị tất cả nếu chọn "all"
        $(".about__card--model").fadeIn();
    } else {
        $(".about__card--model").each(function () {
            if ($(this).data("name") === selectedName) {
                $(this).fadeIn(); // Hiển thị với hiệu ứng fade
            }
        });
    }
}

// Khi tài liệu sẵn sàng
$(document).ready(function () {
    // Lắng nghe sự kiện click vào mục filter
    $(".filter-item").click(function (e) {
        e.preventDefault(); // Ngăn chặn việc chuyển hướng link

        // Lấy tên của shop từ data-name
        var selectedName = $(this).data("name");

        // Gọi hàm filterShops để thực hiện lọc
        filterShops(selectedName);
    });
});
