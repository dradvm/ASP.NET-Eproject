// Function lọc theo filter
function filterShopsByFilter(selectedName) {
    $(".about__card--model").hide(); // Ẩn tất cả các card

    if (selectedName === "all") {
        $(".about__card--model").fadeIn(); // Hiển thị tất cả
    } else {
        $(".about__card--model").each(function () {
            if ($(this).data("name") === selectedName) {
                $(this).fadeIn(); // Hiển thị các card khớp với selectedName
            }
        });
    }
}

// Function lọc theo tìm kiếm
function filterShopsBySearch(inputSelector, cardSelector) {
    $(inputSelector).on("keyup", function () {
        let inputValue = $(this).val().toLowerCase();

        $(cardSelector).filter(function () {
            let shopName = $(this).data("name").toLowerCase();
            if (shopName.includes(inputValue)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });
}

$(document).ready(function () {
    // Sự kiện cho Filter
    $(".filter-item").click(function (e) {
        e.preventDefault();
        let selectedName = $(this).data("name");
        filterShopsByFilter(selectedName);
    });

    // Sự kiện cho Search
    filterShopsBySearch("input[type='text']", ".about__filter");
});
