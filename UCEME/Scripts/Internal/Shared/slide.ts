$(document).ready(() => {
    var visi = 0;

    // Expand Panel
    $("#open").click(() => {
        $("div#menul").toggle("slide");
        $("div.tab").toggle("slide");
        visi = 1;
        setTimeout(() => {
            if (visi === 1) {
                $("div#menul").toggle("slide");
                $("div.tab").toggle("slide");
                $("#toggle a").toggle();
            }
        }, 7000);
        return false;
    });

    // Collapse Panel
    $("#close").click(() => {
        $("div#menul").toggle("slide");
        visi = 0;
        return false;
    });

    // Switch buttons from "Log In | Register" to "Close Panel" on click
    $("#toggle a").click(() => {
        $("#toggle a").toggle();
    });

    var $sidebar = $("#menulateral"),
        $window = $(window),
        offset = $sidebar.offset(),
        topPadding = 0;

    $window.scroll(() => {
        if ($window.scrollTop() > offset.top) {
            $sidebar.stop().animate({
                marginTop: $window.scrollTop() - offset.top + topPadding
            });
        } else {
            $sidebar.stop().animate({
                marginTop: 0
            });
        }
    });
});