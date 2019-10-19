var dataLoader = new DataLoader('/Fotos/Index/');
var mediaManager = new MediaManager('Fotos');

$(document).ready(() => {
    enableNavBar(7);
    $('#subirFoto').click(() => {
        $('#dialogo').slideToggle(1800);
    });
});

$(window).scroll(() => {
    if ($(window).scrollTop() === $(document).height() - $(window).height()) {
        dataLoader.loadData();
    }
});
