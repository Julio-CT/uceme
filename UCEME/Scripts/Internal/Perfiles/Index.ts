$(document).ready(() => {
    $('#editar').click(() => {
        var urlb = '/Perfiles/Editar';
        var dataItem = $('#idUsuario').val();
        window.location.href = urlb + '/?id=' + dataItem;
    });
});
