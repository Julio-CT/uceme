var manager = new ItemManager('Blog');
var dataLoader = new DataLoader('/Blog/Index/');
$(window).scroll(function () {
    if ($(window).scrollTop() === $(document).height() - $(window).height()) {
        dataLoader.loadData();
    }
});
$(document).ready(function () {
    enableNavBar(3);
});
//# sourceMappingURL=Index.js.map