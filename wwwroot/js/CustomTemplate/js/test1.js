(function ($) {
    'use strict';
    $(function () {
        var body = $('body');
        var sidebar = $('.sidebar');

        // Kiểm tra trạng thái sidebar từ localStorage khi tải trang
        var sidebarState = localStorage.getItem('sidebarState');
        if (sidebarState === 'closed') {
            body.addClass('sidebar-icon-only'); // hoặc 'sidebar-hidden' tùy thuộc vào trạng thái đóng
        } else {
            body.removeClass('sidebar-icon-only sidebar-hidden');
        }

        // Đóng các submenu khác khi mở một submenu mới
        sidebar.on('show.bs.collapse', '.collapse', function () {
            sidebar.find('.collapse.show').collapse('hide');
        });

        // Thay đổi chiều cao của sidebar và content-wrapper
        applyStyles();

        function applyStyles() {
            // Áp dụng Perfect Scrollbar nếu cần
            if (!body.hasClass("rtl")) {
                if (body.hasClass("sidebar-fixed")) {
                    new PerfectScrollbar('#sidebar .nav');
                }
            }
        }

        // Bật/tắt sidebar khi nhấn nút minimize
        $('[data-toggle="minimize"]').on("click", function () {
            if (body.hasClass('sidebar-toggle-display') || body.hasClass('sidebar-absolute')) {
                body.toggleClass('sidebar-hidden');
                localStorage.setItem('sidebarState', body.hasClass('sidebar-hidden') ? 'closed' : 'open');
            } else {
                body.toggleClass('sidebar-icon-only');
                localStorage.setItem('sidebarState', body.hasClass('sidebar-icon-only') ? 'closed' : 'open');
            }
        });

        // Thêm biểu tượng check cho checkbox và radios
        $(".form-check label, .form-radio label").append('<i class="input-helper"></i>');

        // Quản lý chế độ toàn màn hình
        $("#fullscreen-button").on("click", function () {
            if (!document.fullscreenElement && !document.mozFullScreenElement &&
                !document.webkitFullscreenElement && !document.msFullscreenElement) {
                // Mở chế độ toàn màn hình
                if (document.documentElement.requestFullscreen) {
                    document.documentElement.requestFullscreen();
                } else if (document.documentElement.mozRequestFullScreen) { // Firefox
                    document.documentElement.mozRequestFullScreen();
                } else if (document.documentElement.webkitRequestFullscreen) { // Chrome, Safari and Opera
                    document.documentElement.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
                } else if (document.documentElement.msRequestFullscreen) { // IE/Edge
                    document.documentElement.msRequestFullscreen();
                }
            } else {
                // Thoát chế độ toàn màn hình
                if (document.exitFullscreen) {
                    document.exitFullscreen();
                } else if (document.mozCancelFullScreen) { // Firefox
                    document.mozCancelFullScreen();
                } else if (document.webkitExitFullscreen) { // Chrome, Safari and Opera
                    document.webkitExitFullscreen();
                } else if (document.msExitFullscreen) { // IE/Edge
                    document.msExitFullscreen();
                }
            }
        });
    });
})(jQuery);
