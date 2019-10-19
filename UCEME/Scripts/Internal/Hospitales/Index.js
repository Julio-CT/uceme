var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Negocio = /** @class */ (function () {
    function Negocio(type, deleteUrl, editUrl, articulo, dialogoCrear, dialogoEditar) {
        this.type = type;
        this.deleteUrl = deleteUrl;
        this.editUrl = editUrl;
        this.articulo = articulo;
        this.dialogoCrear = dialogoCrear;
        this.dialogoEditar = dialogoEditar;
    }
    Negocio.prototype.eliminar = function (e) {
        var r = confirm('¿Desea borrar ' + this.articulo + ' ' + this.type + '?');
        if (r) {
            var urlb = '/' + this.deleteUrl;
            location.href = urlb + '/?id=' + e;
        }
    };
    Negocio.prototype.add = function () {
        $('#' + this.dialogoCrear).dialog({
            dialogClass: 'noclose',
            modal: true,
            width: 400,
            height: 500,
            autoOpen: false,
            show: 'slow'
        });
        $('#' + this.dialogoCrear).load();
        $('#' + this.dialogoCrear).dialog('open');
    };
    Negocio.prototype.cerrarDialogoCrear = function () {
        $('#' + this.dialogoCrear).dialog('close');
    };
    Negocio.prototype.editar = function (e) {
        var urlb = '/' + this.editUrl;
        urlb += '/?id=' + e;
        $('#' + this.dialogoEditar).load(urlb);
        $('#' + this.dialogoEditar).dialog({
            dialogClass: 'noclose',
            modal: true,
            width: 350,
            height: 650,
            autoOpen: false,
            show: 'slow'
        });
        $('#' + this.dialogoEditar).load();
        $('#' + this.dialogoEditar).dialog('open');
    };
    return Negocio;
}());
var Hospital = /** @class */ (function (_super) {
    __extends(Hospital, _super);
    function Hospital() {
        return _super.call(this, 'Hospital', 'Hospitales/DeleteHospital', 'Hospitales/EditarHospital', 'el', 'dialogoCrearH', 'dialogoEditar') || this;
    }
    return Hospital;
}(Negocio));
var Compania = /** @class */ (function (_super) {
    __extends(Compania, _super);
    function Compania() {
        return _super.call(this, 'Compañia de TODOS LOS HOSPITALES', 'Hospitales/DeleteComp', 'Hospitales/EditarCompania', 'la', 'dialogoCrearC', 'dialogoEditar') || this;
    }
    return Compania;
}(Negocio));
$(document).ready(function () {
    enableNavBar(3);
    var hospital = new Hospital();
    var compania = new Compania();
    $('#addHospital').click(function () { hospital.add(); });
    $("[name='addCompany']").click(function () { compania.add(); });
    $('#cancelCrearHospital').click(function () { hospital.cerrarDialogoCrear(); });
    $('#cancelCrearCompania').click(function () { compania.cerrarDialogoCrear(); });
    $("[name='editarHospital']").click(function (e) {
        var id = $(e.target).closest('.cont').attr('id');
        hospital.editar(id);
    });
    $("[name='editarCompania']").click(function (e) {
        var id = $(e.target).closest('.compania').attr('id');
        compania.editar(id);
    });
    $("[name='eliminarHospital']").click(function (e) {
        var id = $(e.target).closest('.cont').attr('id');
        hospital.eliminar(id);
    });
});
//# sourceMappingURL=Index.js.map