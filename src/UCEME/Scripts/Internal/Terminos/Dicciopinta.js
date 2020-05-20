$(document).ready(function () {
    var divIniciales = document.getElementById('hiddenLetters');
    var iniciales = divIniciales.getAttribute("data-iniciales");
    for (var i = 0; i < iniciales.length; i++) {
        var ini = iniciales[i];
        var capa = '#' + ini;
        var addendum = '<a href=\'' + capa + '\'><strong>' + ini + '</strong></a>';
        $(capa).html(addendum);
    }
});
function verDetalle(e) {
    var urlb = '/Terminos/Detalles';
    urlb += '/?id=' + e;
    var divname = '#contenedor' + e;
    $(divname).load(urlb);
    var dialogo = $.ui.dialog;
    dialogo.modal = true;
    dialogo.width = 400;
    dialogo.position = 'center';
    dialogo.autoOpen = false;
    dialogo.show = 'slow';
    $(divname).dialog(dialogo);
    $(divname).load();
    $(divname).dialog('open');
}
function editarTermino(e) {
    var urlb = '/Terminos/Editar';
    location.href = urlb + '/?id=' + e;
}
function eliminarTermino(e) {
    var r = confirm('¿Desea borrar el Termino?');
    if (r) {
        var urlb = '/Terminos/Eliminar';
        location.href = urlb + '/?id=' + e;
    }
}
//# sourceMappingURL=Dicciopinta.js.map