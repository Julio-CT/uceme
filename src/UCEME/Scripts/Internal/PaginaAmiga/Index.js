var PaginaAmiga = /** @class */ (function () {
    function PaginaAmiga() {
    }
    PaginaAmiga.prototype.anadir = function () {
        var urlb = '/PaginaAmiga/Anadir';
        $('#dialogoCrear').load(urlb);
        $('#dialogoCrear').dialog({
            dialogClass: 'noclose',
            modal: true,
            width: 350,
            height: 450,
            autoOpen: false,
            show: 'slow'
        });
        $('#dialogoCrear').load();
        $('#dialogoCrear').dialog('open');
    };
    PaginaAmiga.prototype.editarItem = function (id) {
        var urlb = '/PaginaAmiga/Editar';
        urlb += '/?id=' + id;
        $('#dialogoEditar').load(urlb);
        $('#dialogoEditar').dialog({
            dialogClass: 'noclose',
            modal: true,
            width: 350,
            height: 550,
            autoOpen: false,
            show: 'slow'
        });
        $('#dialogoEditar').load();
        $('#dialogoEditar').dialog('open');
    };
    PaginaAmiga.prototype.cancelarEdicion = function () {
        $('#dialogoEditar').dialog('close');
    };
    PaginaAmiga.prototype.cancelarCreacion = function () {
        $('#dialogoCrear').dialog('close');
    };
    PaginaAmiga.prototype.borrarItem = function (id) {
        var statusConfirm = confirm('¿Realmente desea eliminarlo?');
        if (statusConfirm) {
            var urlb = '/PaginaAmiga/Eliminar';
            var idIt = id;
            $.ajax({
                url: urlb,
                data: { id: idIt },
                dataType: 'json',
                traditional: true,
                success: function () {
                    alert('Elemento eliminado');
                    location.reload();
                }
            });
        }
    };
    return PaginaAmiga;
}());
var paginaAmiga = new PaginaAmiga();
//# sourceMappingURL=Index.js.map