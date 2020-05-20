var DataLoader = /** @class */ (function () {
    function DataLoader(url) {
        var _this = this;
        this.pagina = 0;
        this.inCallback = false;
        this.path = url;
        this.loadData = function () {
            if (_this.pagina > -1 && !_this.inCallback) {
                _this.inCallback = true;
                _this.pagina++;
                $('div#loading').html('<p><img src="/Images/loader.gif"></p>');
                $.get(_this.path + _this.pagina, function (data) {
                    if (data !== '') {
                        $('#contenedor').append(data);
                    }
                    else {
                        _this.pagina = -1;
                    }
                    _this.inCallback = false;
                    $('div#loading').empty();
                });
            }
        };
    }
    DataLoader.prototype.loadData = function () { };
    return DataLoader;
}());
//# sourceMappingURL=DataLoader.js.map