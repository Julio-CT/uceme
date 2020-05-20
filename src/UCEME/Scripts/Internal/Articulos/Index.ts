
var manager = new ItemManager('Articulos');
var dataLoader = new DataLoader('/Articulos/Index/');

$(window).scroll(() => {
    if ($(window).scrollTop() === $(document).height() - $(window).height()) {
        dataLoader.loadData();
    }
});

$(document).ready(() => {
    enableNavBar(4);
});
