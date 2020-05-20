$(document).ready(function () {
    $('#editar').click(function () {
        var urlb = '/Perfiles/Editar';
        var dataItem = $('#idUsuario').val();
        window.location.href = urlb + '/?id=' + dataItem;
    });
});
//# sourceMappingURL=Index.js.map