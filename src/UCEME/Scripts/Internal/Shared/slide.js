$(document).ready(function () {
    var visi = 0;
    // Expand Panel
    $("#open").click(function () {
        $("div#menul").toggle("slide");
        $("div.tab").toggle("slide");
        visi = 1;
        setTimeout(function () {
            if (visi === 1) {
                $("div#menul").toggle("slide");
                $("div.tab").toggle("slide");
                $("#toggle a").toggle();
            }
        }, 7000);
        return false;
    });
    // Collapse Panel
    $("#close").click(function () {
        $("div#menul").toggle("slide");
        visi = 0;
        return false;
    });
    // Switch buttons from "Log In | Register" to "Close Panel" on click
    $("#toggle a").click(function () {
        $("#toggle a").toggle();
    });
    var $sidebar = $("#menulateral"), $window = $(window), offset = $sidebar.offset(), topPadding = 0;
    $window.scroll(function () {
        if ($window.scrollTop() > offset.top) {
            $sidebar.stop().animate({
                marginTop: $window.scrollTop() - offset.top + topPadding
            });
        }
        else {
            $sidebar.stop().animate({
                marginTop: 0
            });
        }
    });
});
//# sourceMappingURL=slide.js.map