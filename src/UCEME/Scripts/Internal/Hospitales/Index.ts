
class Negocio {
    private type: string;
    private deleteUrl: string;
    private editUrl: string;
    private articulo: string;
    private dialogoCrear: string;
    private dialogoEditar: string;

    constructor(type: string, deleteUrl: string, editUrl: string, articulo: string, dialogoCrear: string, dialogoEditar: string) {
        this.type = type;
        this.deleteUrl = deleteUrl;
        this.editUrl = editUrl;
        this.articulo = articulo;
        this.dialogoCrear = dialogoCrear;
        this.dialogoEditar = dialogoEditar;
    }

    public eliminar(e: string): void {
        var r = confirm('¿Desea borrar ' + this.articulo + ' ' + this.type + '?');
        if (r) {
            var urlb = '/' + this.deleteUrl;
            location.href = urlb + '/?id=' + e;
        }
    }

    public add(): void {
        $('#' + this.dialogoCrear).dialog(
            {
                dialogClass: 'noclose',
                modal: true,
                width: 400,
                height: 500,
                autoOpen: false,
                show: 'slow'
            });

        $('#' + this.dialogoCrear).load();

        $('#' + this.dialogoCrear).dialog('open');
    }

    public cerrarDialogoCrear(): void {
        $('#' + this.dialogoCrear).dialog('close');
    }

    public editar(e: string): void {
        var urlb = '/' + this.editUrl;
        urlb += '/?id=' + e;
        $('#' + this.dialogoEditar).load(urlb);
        $('#' + this.dialogoEditar).dialog(
            {
                dialogClass: 'noclose',
                modal: true,
                width: 350,
                height: 650,
                autoOpen: false,
                show: 'slow'
            });
        $('#' + this.dialogoEditar).load();

        $('#' + this.dialogoEditar).dialog('open');
    }
}

class Hospital extends Negocio {
    constructor() {
        super('Hospital', 'Hospitales/DeleteHospital', 'Hospitales/EditarHospital', 'el', 'dialogoCrearH', 'dialogoEditar');
    }
}

class Compania extends Negocio {
    constructor() {
        super('Compañia de TODOS LOS HOSPITALES', 'Hospitales/DeleteComp', 'Hospitales/EditarCompania', 'la', 'dialogoCrearC', 'dialogoEditar');
    }
}

$(document).ready(() => {
    enableNavBar(3);

    var hospital = new Hospital();
    var compania = new Compania();

    $('#addHospital').click(() => { hospital.add() });

    $("[name='addCompany']").click(() => { compania.add() });

    $('#cancelCrearHospital').click(() => { hospital.cerrarDialogoCrear() });

    $('#cancelCrearCompania').click(() => { compania.cerrarDialogoCrear() });

    $("[name='editarHospital']").click((e) => {
        var id = $(e.target).closest('.cont').attr('id');
        hospital.editar(id);
    });

    $("[name='editarCompania']").click((e) => {
        var id = $(e.target).closest('.compania').attr('id');
        compania.editar(id);
    });

    $("[name='eliminarHospital']").click((e) => {
        var id = $(e.target).closest('.cont').attr('id');
        hospital.eliminar(id);
    });
});

