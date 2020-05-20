var ItemManager = /** @class */ (function () {
    function ItemManager(path) {
        var _this = this;
        this.defaultPath = path;
        this.anadir = function () {
            var urlb = '/' + _this.defaultPath + '/Anadir';
            location.href = urlb;
        };
        this.editarItem = function (id) {
            var urlb = '/' + _this.defaultPath + '/Editar';
            location.href = urlb + '/?id=' + id;
        };
        this.borrarItem = function (id) {
            var statusConfirm = confirm('¿Realmente desea eliminar el elemento?');
            if (statusConfirm) {
                var urlb = '/' + _this.defaultPath + '/Eliminar';
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
        this.quitarNuevo = function () {
            $('#newitem div').each(function () { $(this).remove(); });
            location.reload();
        };
    }
    ItemManager.prototype.anadir = function () { };
    ItemManager.prototype.editarItem = function (id) { };
    ItemManager.prototype.borrarItem = function (id) { };
    ItemManager.prototype.quitarNuevo = function () { };
    return ItemManager;
}());
//# sourceMappingURL=ItemManager.js.map