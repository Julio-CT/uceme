var Servicio = /** @class */ (function () {
    function Servicio() {
    }
    Servicio.prototype.anadirServicio = function () {
        $('#dialogoCrearServicio').dialog({
            dialogClass: 'noclose',
            modal: true,
            width: 600,
            autoOpen: false,
            show: 'slow'
        });
        $('#dialogoCrearServicio').load();
        $('#dialogoCrearServicio').dialog('open');
    };
    Servicio.prototype.cerrarDialogoCrearServicio = function () {
        $('#dialogoCrearServicio').dialog('close');
    };
    Servicio.prototype.editarServicio = function (id) {
        var urlb = '/Servicios/EditarServicio';
        location.href = urlb + '/?id=' + id;
    };
    Servicio.prototype.eliminarServicio = function (id) {
        var r = confirm('¿Desea borrar el Servicio?');
        if (r) {
            var urlb = '/Servicios/Eliminar';
            location.href = urlb + '/?id=' + id;
        }
    };
    return Servicio;
}());
$(document).ready(function () {
    enableNavBar(2);
});
var servicio = new Servicio();
//# sourceMappingURL=Index.js.map