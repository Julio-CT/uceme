$(document).ready(() => {
    enableNavBar(6);
    $('#editar').click(() => {
        var urlb = '/Faq/Editar';
        window.location.href = urlb;
    });
});
