(function ($) {
    "use strict"; var $wrapper = $('.main-wrapper');
    var $pageWrapper = $('.page-wrapper');
    var $slimScrolls = $('.slimscroll');
    var Sidemenu = function () {
        this.$menuItem = $('#sidebar-menu a');
    };
    function init() {
        $('#sidebar-menu ul li.submenu a.active').parents('li:last').children('a:first').addClass('active').trigger('click');
    }
    init();
    $('body').append('<div class="sidebar-overlay"></div>');
    $(document).on('click', '#mobile_btn', function () {
        $wrapper.toggleClass('slide-nav');
        $('.sidebar-overlay').toggleClass('opened');
        $('html').addClass('menu-opened');
        return false;
    });
    $(".sidebar-overlay").on("click", function () {
        $wrapper.removeClass('slide-nav');
        $(".sidebar-overlay").removeClass("opened");
        $('html').removeClass('menu-opened');
    });
    if ($('.page-wrapper').length > 0) { var height = $(window).height(); $(".page-wrapper").css("min-height", height); }
$(window).resize(function(){if($('.page-wrapper').length>0){var height=$(window).height();$(".page-wrapper").css("min-height",height);}});if($('.select').length>0){$('.select').select2({minimumResultsForSearch:-1,width:'100%'});}
    $(document).on('click', '#toggle_btn', function () {
        if ($('body').hasClass('mini-sidebar')) { $('body').removeClass('mini-sidebar'); $('.subdrop + ul').slideDown(); }
        else { $('body').addClass('mini-sidebar'); $('.subdrop + ul').slideUp(); }
        return false;
    });

})(jQuery);