﻿(function ($) {
    'use strict';
    $(function () {
        var body = $('body');
        var sidebar = $('.sidebar');

        // Add active class to nav-link based on url dynamically
        // Active class can be hard coded directly in html file also as required

        function addActiveClass(element) {
            if (current === "" && element.attr('href') === "/") {
                // for root url
                element.parents('.nav-item').last().addClass('active');
                if (element.parents('.sub-menu').length) {
                    element.closest('.collapse').addClass('show');
                    element.addClass('active');
                }
            } else {
                // for other url
                if (element.attr('href').indexOf(current) !== -1) {
                    element.parents('.nav-item').last().addClass('active');
                    if (element.parents('.sub-menu').length) {
                        element.closest('.collapse').addClass('show');
                        element.addClass('active');
                    }
                    if (element.parents('.submenu-item').length) {
                        element.addClass('active');
                    }
                }
            }
        }

        // Get the part of the URL after the domain, split by '/' and get the part after the first '/'
        var current = location.pathname.split("/")[1];

        $('.nav li a', sidebar).each(function () {
            var $this = $(this);
            addActiveClass($this);
        });

        $('.horizontal-menu .nav li a').each(function () {
            var $this = $(this);
            addActiveClass($this);
        });

        // Close other submenu in sidebar on opening any
        sidebar.on('show.bs.collapse', '.collapse', function () {
            sidebar.find('.collapse.show').collapse('hide');
        });

        // Change sidebar and content-wrapper height
        applyStyles();

        function applyStyles() {
            // Applying perfect scrollbar
            if (!body.hasClass("rtl")) {
                if (body.hasClass("sidebar-fixed")) {
                    var fixedSidebarScroll = new PerfectScrollbar('#sidebar .nav');
                }
            }
        }

        $('[data-toggle="minimize"]').on("click", function () {
            if ((body.hasClass('sidebar-toggle-display')) || (body.hasClass('sidebar-absolute'))) {
                body.toggleClass('sidebar-hidden');
            } else {
                body.toggleClass('sidebar-icon-only');
            }
        });

        // checkbox and radios
        $(".form-check label,.form-radio label").append('<i class="input-helper"></i>');

        // fullscreen
        $("#fullscreen-button").on("click", function toggleFullScreen() {
            if ((document.fullScreenElement !== undefined && document.fullScreenElement === null) ||
                (document.msFullscreenElement !== undefined && document.msFullscreenElement === null) ||
                (document.mozFullScreen !== undefined && !document.mozFullScreen) ||
                (document.webkitIsFullScreen !== undefined && !document.webkitIsFullScreen)) {
                if (document.documentElement.requestFullScreen) {
                    document.documentElement.requestFullScreen();
                } else if (document.documentElement.mozRequestFullScreen) {
                    document.documentElement.mozRequestFullScreen();
                } else if (document.documentElement.webkitRequestFullScreen) {
                    document.documentElement.webkitRequestFullScreen(Element.ALLOW_KEYBOARD_INPUT);
                } else if (document.documentElement.msRequestFullscreen) {
                    document.documentElement.msRequestFullscreen();
                }
            } else {
                if (document.cancelFullScreen) {
                    document.cancelFullScreen();
                } else if (document.mozCancelFullScreen) {
                    document.mozCancelFullScreen();
                } else if (document.webkitCancelFullScreen) {
                    document.webkitCancelFullScreen();
                } else if (document.msExitFullscreen) {
                    document.msExitFullscreen();
                }
            }
        })
    });
})(jQuery);
