var primarycolor = getComputedStyle(document.body).getPropertyValue('--primary');
var secondarycolor = getComputedStyle(document.body).getPropertyValue('--secondary');
var successcolor = getComputedStyle(document.body).getPropertyValue('--success');
var warningcolor = getComputedStyle(document.body).getPropertyValue('--warning');
var dangercolor = getComputedStyle(document.body).getPropertyValue('--danger');
var infocolor = getComputedStyle(document.body).getPropertyValue('--info');
var darkcolor = getComputedStyle(document.body).getPropertyValue('--dark');
var lightcolor = getComputedStyle(document.body).getPropertyValue('--light');


(function ($) {
    'use strict';
    $(function () {
        var body = $('body');
        var sidebar = $('.sidebar');

        // ?óng các submenu khác khi m? m?t submenu m?i
        sidebar.on('show.bs.collapse', '.collapse', function () {
            sidebar.find('.collapse.show').collapse('hide');
        });

        // Thay ??i chi?u cao c?a sidebar và content-wrapper
        applyStyles();

        function applyStyles() {
            // Áp d?ng Perfect Scrollbar n?u c?n
            if (!body.hasClass("rtl")) {
                if (body.hasClass("sidebar-fixed")) {
                    new PerfectScrollbar('#sidebar .nav');
                }
            }
        }

        // B?t/t?t sidebar khi nh?n nút minimize
        $('[data-toggle="minimize"]').on("click", function () {
            if (body.hasClass('sidebar-toggle-display') || body.hasClass('sidebar-absolute')) {
                body.toggleClass('sidebar-hidden');
            } else {
                body.toggleClass('sidebar-icon-only');
            }
        });

        // Thêm bi?u t??ng check cho checkbox và radios
        $(".form-check label, .form-radio label").append('<i class="input-helper"></i>');

        // Qu?n lý ch? ?? toàn màn hình
        $("#fullscreen-button").on("click", function () {
            if (!document.fullscreenElement && !document.mozFullScreenElement &&
                !document.webkitFullscreenElement && !document.msFullscreenElement) {
                // M? ch? ?? toàn màn hình
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
                // Thoát ch? ?? toàn màn hình
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

