var dataLoader = new DataLoader('/Tecnicas/Index/');
var manager = new ItemManager('Tecnicas');

$(window).scroll(() => {
    if ($(window).scrollTop() === $(document).height() - $(window).height()) {
        dataLoader.loadData();
    }
});

$(document).ready(() => {
    enableNavBar(3);
});