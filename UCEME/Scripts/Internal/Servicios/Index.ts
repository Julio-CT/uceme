class Servicio {
    anadirServicio(): void {
        $('#dialogoCrearServicio').dialog(
            {
                dialogClass: 'noclose',
                modal: true,
                width: 600,
                autoOpen: false,
                show: 'slow'
            });

        $('#dialogoCrearServicio').load();

        $('#dialogoCrearServicio').dialog('open');
    }

    cerrarDialogoCrearServicio(): void {
        $('#dialogoCrearServicio').dialog('close');
    }

    editarServicio(id: string): void {
        var urlb = '/Servicios/EditarServicio';
        location.href = urlb + '/?id=' + id;
    }

    eliminarServicio(id: string): void {
        var r = confirm('¿Desea borrar el Servicio?');
        if (r) {
            var urlb = '/Servicios/Eliminar';
            location.href = urlb + '/?id=' + id;
        }
    }
}

$(document).ready(() => {
    enableNavBar(2);
});

var servicio = new Servicio();