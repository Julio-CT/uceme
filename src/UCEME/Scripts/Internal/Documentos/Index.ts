
class Documento {
    public cancelar(): void {
        $('#dialogo').slideToggle(1800);
    }

    public borrarItem(id: string): void {
        var statusConfirm = confirm('¿Realmente desea eliminar el documento?');
        if (statusConfirm) {
            var urlb = '/Documentos/eliminarDocumento';
            var idIt = id;
            $.ajax({
                url: urlb,
                data: { idItem: idIt },
                dataType: 'json',
                traditional: true,
                success() {
                    location.reload();
                }
            });
        }
    }
}

$(document).ready(() => {
    enableNavBar(7);
    var documento = new Documento();

    $('#cancelDocumentUpload').click(() => {
        documento.cancelar();
    });
    $('#subirDocu').click(() => {
        documento.cancelar();
    });
});
