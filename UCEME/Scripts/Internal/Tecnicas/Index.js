var dataLoader = new DataLoader('/Tecnicas/Index/');
var manager = new ItemManager('Tecnicas');
$(window).scroll(function () {
    if ($(window).scrollTop() === $(document).height() - $(window).height()) {
        dataLoader.loadData();
    }
});
$(document).ready(function () {
    enableNavBar(4);
});
//# sourceMappingURL=Index.js.map