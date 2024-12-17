// Function lọc theo tìm kiếm
function filterShopsBySearch(inputSelector, cardSelector) {
    $(inputSelector).on("keyup", function () {
        let inputValue = $(this).val().toLowerCase();

        $(cardSelector).each(function () {
            let shopName = $(this).find(".card-title").text().toLowerCase(); // Lấy tên cửa hàng từ class card-title
            if (shopName.includes(inputValue)) {
                $(this).parent().show(); // Hiển thị card nếu khớp
            } else {
                $(this).parent().hide(); // Ẩn card nếu không khớp
            }
        });
    });
}

$(document).ready(function () {
    // Chỉ giữ chức năng tìm kiếm
    filterShopsBySearch("input[type='text']", ".card");
});
