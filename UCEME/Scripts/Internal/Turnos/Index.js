var Turno = /** @class */ (function () {
    function Turno() {
    }
    Turno.prototype.muestraNuevo = function () {
        $('#nuevo').show('slow');
        return false;
    };
    Turno.prototype.cancelNuevo = function () {
        $('#nuevo').hide('slow');
        return false;
    };
    Turno.prototype.editarItem = function (id) {
        var view = '#view' + id;
        var edit = '#edit' + id;
        $(view).hide('slow', function () { $(edit).show('slow'); });
        return false;
    };
    Turno.prototype.cancel = function (id) {
        var view = '#view' + id;
        var edit = '#edit' + id;
        $(edit).hide('slow', function () { $(view).show('slow'); });
        return false;
    };
    Turno.prototype.crearHospitalPop = function () {
        $('#nuevo').dialog({
            dialogClass: 'noclose',
            modal: true,
            position: ['middle', 225],
            width: 600,
            autoOpen: false,
            show: 'slow'
        });
        $('#nuevo').load();
        $('#nuevo').dialog('open');
    };
    Turno.prototype.editadoItem = function (id) {
        var urlb = '/Turnos/editItem';
        var idIt = id;
        var edit = '#edit' + id;
        var edia = edit + ' #item_Dia';
        var dia = $(edia).val();
        var einicio = edit + ' #item_Strinicio';
        var inicio = $(einicio).val();
        var efin = edit + ' #item_Strfin';
        var fin = $(efin).val();
        var epara = edit + ' #item_Paralelas';
        var para = $(epara).val();
        var eporh = edit + ' #item_Porhora';
        var porh = $(eporh).val();
        $.ajax({
            url: urlb,
            data: {
                idTurno: idIt,
                dia: dia,
                paralelas: para,
                porhora: porh,
                strinicio: inicio,
                strfin: fin
            },
            dataType: 'json',
            traditional: true,
            success: function () {
                location.reload();
            }
        });
    };
    Turno.prototype.borrarItem = function (id) {
        var r = confirm('¿Desea eliminar el turno y todos las citas asociadas?');
        if (r) {
            var urlb = '/Turnos/deleteItem';
            var idIt = id;
            $.ajax({
                url: urlb,
                data: { idTurno: idIt },
                dataType: 'json',
                traditional: true,
                success: function () {
                    alert('Turno eliminado.\n' +
                        'Recibira un correo con las citas que debe reagendar.');
                    location.reload();
                }
            });
        }
    };
    return Turno;
}());
$(document).ready(function () {
    enableNavBar(8);
});
var turno = new Turno();
//# sourceMappingURL=Index.js.map