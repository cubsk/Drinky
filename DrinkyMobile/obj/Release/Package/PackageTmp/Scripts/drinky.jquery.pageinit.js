
(function ($) {

    $.fn.pageinit = function (onload) {
        if (this.attr("page_initialized"))
            return;
        this.attr("page_initialized", "true")
        onload(this);
    };

})(jQuery);