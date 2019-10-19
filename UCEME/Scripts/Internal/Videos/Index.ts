var dataLoader = new DataLoader('/Videos/Index/');
var mediaManager = new MediaManager('Videos');

$(document).ready(() => {
    enableNavBar(7);
    $('#subirVideo').click(() => {
        $('#dialogo').slideToggle(1800);
    });
});

$(window).scroll(() => {
    if ($(window).scrollTop() === $(document).height() - $(window).height()) {
        dataLoader.loadData();
    }
});
