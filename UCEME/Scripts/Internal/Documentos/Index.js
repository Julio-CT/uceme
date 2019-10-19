var Documento = /** @class */ (function () {
    function Documento() {
    }
    Documento.prototype.cancelar = function () {
        $('#dialogo').slideToggle(1800);
    };
    Documento.prototype.borrarItem = function (id) {
        var statusConfirm = confirm('¿Realmente desea eliminar el documento?');
        if (statusConfirm) {
            var urlb = '/Documentos/eliminarDocumento';
            var idIt = id;
            $.ajax({
                url: urlb,
                data: { idItem: idIt },
                dataType: 'json',
                traditional: true,
                success: function () {
                    location.reload();
                }
            });
        }
    };
    return Documento;
}());
$(document).ready(function () {
    enableNavBar(7);
    var documento = new Documento();
    $('#cancelDocumentUpload').click(function () {
        documento.cancelar();
    });
    $('#subirDocu').click(function () {
        documento.cancelar();
    });
});
//# sourceMappingURL=Index.js.map