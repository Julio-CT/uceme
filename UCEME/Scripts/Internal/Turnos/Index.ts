class Turno {
    muestraNuevo() {
        $('#nuevo').show('slow');
        return false;
    }

    cancelNuevo() {
        $('#nuevo').hide('slow');
        return false;
    }

    editarItem(id: any): boolean {
        var view = '#view' + id;
        var edit = '#edit' + id;
        $(view).hide('slow', () => { $(edit).show('slow'); });
        return false;
    }

    cancel(id: any): boolean {
        var view = '#view' + id;
        var edit = '#edit' + id;
        $(edit).hide('slow', () => { $(view).show('slow'); });
        return false;
    }

    crearHospitalPop() {
        $('#nuevo').dialog(
            {
                dialogClass: 'noclose',
                modal: true,
                position: ['middle', 225],
                width: 600,
                autoOpen: false,
                show: 'slow'
            });
        $('#nuevo').load();
        $('#nuevo').dialog('open');
    }

    editadoItem(id: any): void {
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
            success() {
                location.reload();
            }
        });
    }

    borrarItem(id: any): void {
        var r = confirm('¿Desea eliminar el turno y todos las citas asociadas?');
        if (r) {
            var urlb = '/Turnos/deleteItem';
            var idIt = id;
            $.ajax({
                url: urlb,
                data: { idTurno: idIt },
                dataType: 'json',
                traditional: true,
                success() {
                    alert('Turno eliminado.\n' +
                        'Recibira un correo con las citas que debe reagendar.');
                    location.reload();
                }
            });
        }
    }
}

$(document).ready(() => {
    enableNavBar(8);
});

var turno = new Turno();