var manager = new ItemManager('Blog');
var dataLoader = new DataLoader('/Blog/Index/');

$(window).scroll(() => {
    if ($(window).scrollTop() === $(document).height() - $(window).height()) {
        dataLoader.loadData();
    }
});

$(document).ready(() => {
    enableNavBar(4);
});