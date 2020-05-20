var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Perfil = /** @class */ (function (_super) {
    __extends(Perfil, _super);
    function Perfil() {
        var _this = _super.call(this, 'Perfiles') || this;
        _this.anadirNuevo = function () {
            var urlb = '/Perfiles/Agregar';
            var idCur = $('#idCurriculum').val();
            var tit = $('#TituloNuevo').val();
            var fec = $('#FechasNuevo').val();
            var txt = $('#TextoNuevo').val();
            var idUsu = $('#idUsuario').val();
            $.ajax({
                url: urlb,
                data: {
                    idCurriculum: idCur,
                    Titulo: tit,
                    Fechas: fec,
                    Texto: txt,
                    IdUsuario: idUsu
                },
                dataType: 'json',
                traditional: true,
                success: function () {
                    alert('Perfil A�adido.');
                    location.reload();
                }
            });
        };
        return _this;
    }
    Perfil.prototype.anadirNuevo = function () { };
    ;
    return Perfil;
}(ItemManager));
var perfilManager = new Perfil();
$(document).ready(function () {
    $(function () {
        $('#back').click(function () {
            history.go(-1);
        });
    });
    $(function () {
        $('#addElem').click(function (e) {
            e.preventDefault();
            var adendum = '<div style=\'border:double;';
            adendum += '<border-color:azure;height:65px;\'>';
            adendum += '<div class=\'display-field\' style=\'float:left;\'>';
            adendum += '<input id=\'TituloNuevo\' name=\'TituloNuevo\' ';
            adendum += 'style=\'width:325px\' type=\'text\' /></div>&nbsp;';
            adendum += '<div class=\'display-field\' style=\'float:left;';
            adendum += 'margin-left:15px;\'><input id=\'FechasNuevo\' ';
            adendum += 'name=\'FechasNuevo\' style=\'width:190px\' ';
            adendum += 'type=\'text\' /></div>';
            adendum += '<div class=\'display-field\' style=\'float:left;';
            adendum += 'margin-left:15px;\'><input id=\'TextoNuevo\' ';
            adendum += 'name=\'TextoNuevo\' style=\'width:325px\' type=\'text\' />';
            adendum += '</div>';
            adendum += '<div class=\'iconos\' style=\'float:left;width:32px;';
            adendum += 'height:32px;padding:0;margin:0;margin-left:10px;\'>';
            adendum += '<input class=\'aceptarico\' onclick=\'perfilManager.anadirNuevo()\' ';
            adendum += 'style=\'float:left;margin-left:1px;width:32px';
            adendum += ';height:32px;padding:0;margin:0;\' /></div>';
            adendum += '<div class=\'iconos\' style=\'float:left;;width:32px;';
            adendum += 'height:32px;padding:0;margin:0;margin-left:10px\'>';
            adendum += '<input  class=\'cancelarico\' onclick=\'perfilManager.quitarNuevo()\' ';
            adendum += 'style=\'float:left;margin-left:1px;width:32px;';
            adendum += 'height:32px;padding:0px;margin:0px;\' /></div>';
            adendum += '<div class=\'clearboth\' /></div>';
            $('#newitem').append(adendum);
        });
    });
});
//# sourceMappingURL=Editar.js.map