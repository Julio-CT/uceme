var dataLoader = new DataLoader('/Fotos/Index/');
var mediaManager = new MediaManager('Fotos');
$(document).ready(function () {
    enableNavBar(7);
    $('#subirFoto').click(function () {
        $('#dialogo').slideToggle(1800);
    });
});
$(window).scroll(function () {
    if ($(window).scrollTop() === $(document).height() - $(window).height()) {
        dataLoader.loadData();
    }
});
//# sourceMappingURL=Index.js.map