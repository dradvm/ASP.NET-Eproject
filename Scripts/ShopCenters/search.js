// filterShops.js

// Function tìm kiếm
function filterShops(inputSelector, cardSelector) {
    $(inputSelector).on("keyup", function () {
        let inputValue = $(this).val().toLowerCase(); // Lấy giá trị và chuyển thành chữ thường

        $(cardSelector).filter(function () {
            let shopName = $(this).data("name").toLowerCase(); // Lấy tên shop từ data-name
            if (shopName.includes(inputValue)) {
                $(this).show(); // Hiện shop nếu tên chứa chuỗi nhập vào
            } else {
                $(this).hide(); // Ẩn shop nếu không khớp
            }
        });
    });
}

// Gọi function khi trang được load
$(document).ready(function () {
    filterShops("input[type='text']", ".about__filter");
});
