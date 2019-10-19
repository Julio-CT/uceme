var dataLoader = new DataLoader('/Videos/Index/');
var mediaManager = new MediaManager('Videos');
$(document).ready(function () {
    enableNavBar(7);
    $('#subirVideo').click(function () {
        $('#dialogo').slideToggle(1800);
    });
});
$(window).scroll(function () {
    if ($(window).scrollTop() === $(document).height() - $(window).height()) {
        dataLoader.loadData();
    }
});
//# sourceMappingURL=Index.js.map