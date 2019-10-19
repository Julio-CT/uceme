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
var MediaManager = /** @class */ (function (_super) {
    __extends(MediaManager, _super);
    function MediaManager(path) {
        var _this = _super.call(this, path) || this;
        _this.cambiarEstrella = function (id) {
            var url = '/' + _this.defaultPath + '/cambiarDestacada';
            url += '/?id=' + id;
            $.get(url, function (data) {
                var src = (data) ? '../Images/destacadaSi.png' : '../Images/destacadaNo.png';
                $('#' + id).attr('src', src);
            });
        };
        _this.aceptar = function (id) {
            var urlb = '/' + _this.defaultPath + '/guardarCambios';
            var idIt = id;
            var posIt = $('#p' + id).val();
            var textoIt = $('#t' + id).val();
            var descripcion = "";
            var descDiv = $('#desc' + id);
            if (descDiv) {
                descripcion = descDiv.val();
            }
            $.ajax({
                url: urlb,
                data: { id: idIt, pos: posIt, titulo: textoIt, descripcion: descripcion },
                dataType: 'json',
                traditional: true,
                success: function () {
                    alert('Elemento editado correctamente');
                    location.reload();
                }
            });
        };
        _this.cancelar = function (id) {
            $('#divA' + id).show();
            $('#divB' + id).hide();
            $('#divM' + id).show();
            $('#divE' + id).hide();
        };
        _this.cancelarDialogo = function () {
            $('#dialogo').slideToggle(1800);
        };
        _this.editarFoto = function (id) {
            $('#divA' + id).hide();
            $('#divB' + id).show();
            $('#divM' + id).hide();
            $('#divE' + id).show();
        };
        return _this;
    }
    MediaManager.prototype.cambiarEstrella = function (id) { };
    MediaManager.prototype.aceptar = function (id) { };
    MediaManager.prototype.cancelar = function (id) { };
    MediaManager.prototype.cancelarDialogo = function () { };
    MediaManager.prototype.editarFoto = function (id) { };
    return MediaManager;
}(ItemManager));
//# sourceMappingURL=MediaManager.js.map