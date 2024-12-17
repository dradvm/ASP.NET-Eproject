const swiper = new Swiper('.swiper', {
    // Cấu hình Swiper
    direction: 'horizontal', // Trượt ngang
    slidesPerView: 4,        // Hiển thị 4 phần tử
    spaceBetween: 20,        // Khoảng cách giữa các phần tử
    loop: true,              // Cho phép lặp vô hạn

    autoplay: {
        delay: 3000, // Thời gian chờ giữa các slide (đơn vị: ms)
        disableOnInteraction: false, // Không dừng autoplay khi người dùng tương tác
    },
});