var manager = new ItemManager('Articulos');
var dataLoader = new DataLoader('/Articulos/Index/');
$(window).scroll(function () {
    if ($(window).scrollTop() === $(document).height() - $(window).height()) {
        dataLoader.loadData();
    }
});
$(document).ready(function () {
    enableNavBar(5);
});
//# sourceMappingURL=Index.js.map