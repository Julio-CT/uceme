class PaginaAmiga {
    anadir(): void {
        var urlb = '/PaginaAmiga/Anadir';
        $('#dialogoCrear').load(urlb);
        $('#dialogoCrear').dialog(
            {
                dialogClass: 'noclose',
                modal: true,
                width: 350,
                height: 450,
                autoOpen: false,
                show: 'slow'
            });

        $('#dialogoCrear').load();

        $('#dialogoCrear').dialog('open');
    }

    editarItem(id: any): void {
        var urlb = '/PaginaAmiga/Editar';
        urlb += '/?id=' + id;
        $('#dialogoEditar').load(urlb);
        $('#dialogoEditar').dialog(
            {
                dialogClass: 'noclose',
                modal: true,
                width: 350,
                height: 550,
                autoOpen: false,
                show: 'slow'
            });

        $('#dialogoEditar').load();

        $('#dialogoEditar').dialog('open');
    }

    cancelarEdicion() {
        $('#dialogoEditar').dialog('close');
    }

    cancelarCreacion() {
        $('#dialogoCrear').dialog('close');
    }

    borrarItem(id: any): void {
        var statusConfirm = confirm('¿Realmente desea eliminarlo?');
        if (statusConfirm) {
            var urlb = '/PaginaAmiga/Eliminar';
            var idIt = id;
            $.ajax({
                url: urlb,
                data: { id: idIt },
                dataType: 'json',
                traditional: true,
                success() {
                    alert('Elemento eliminado');
                    location.reload();
                }
            });
        }
    }
}

var paginaAmiga = new PaginaAmiga();