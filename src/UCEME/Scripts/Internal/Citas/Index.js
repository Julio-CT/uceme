var Cita = /** @class */ (function () {
    function Cita() {
        var _this = this;
        var self = this;
        this.idHospital = '';
        this.diasValidos = [];
        var checkDataFields = function () {
            if ($('#Nombre').val() !== undefined && $('#Nombre').val() !== '' && $('#Telefono').val() !== undefined && $('#Telefono').val() !== '' && $('#Email').val() !== undefined && $('#Email').val() !== '' && isEmail($('#Email').val())) {
                $('#divEnviar').show('slow');
            }
            else {
                $('#divEnviar').hide('slow');
            }
        };
        var initializeCita = function (cita) {
            $('#Telefono').change(function () {
                checkDataFields();
            });
            $('#Email').change(function () {
                checkDataFields();
            });
            $('#Nombre').change(function () {
                checkDataFields();
            });
            $('#dropdownHospital a').click(function (e) {
                e.preventDefault();
                $('#selectedHospital').text($(this).text());
                cita.idHospital = $(this).data('value');
                cita.getDiasDelServidor();
                // if hospital has changed then we need to clear
            });
            $('#botonEnviar').click(function () { cita.comprobarYEnviar(); });
        };
        this.getDiasDelServidor = function () {
            if (_this.idHospital !== '0') {
                var url = '/Citas/ListaDias';
                url += '/?id=' + _this.idHospital;
                document.getElementById('datepick').value = "";
                $.get(url, function (data) {
                    _this.diasValidos = [];
                    for (var i = 0; i < data.length; i++) {
                        _this.diasValidos[i] = data[i];
                    }
                });
                $('#divdias').show('slow');
                $('#divhoras').hide('slow');
                $('#divdatos').hide('slow');
            }
        };
        var getHorasServidorBs = function () {
            var fecha = $('#datepick').val();
            var ano = parseInt(fecha.substring(6));
            var mes = parseInt(fecha.substring(3, 5)) - 1;
            var dia = parseInt(fecha.substring(0, 2));
            var date = new Date(ano, mes, dia);
            var diasemana = date.getDay();
            if (diasemana === 0) {
                diasemana = 7;
            }
            var url = '/Citas/ListaHoras' + '/?diasem=' + diasemana + '&hospi=' + _this.idHospital +
                '&dia=' + dia + '&mes=' + mes + '&ano=' + ano;
            $.get(url, function (data) {
                var adendum = '';
                for (var i = 0; i < data.length; i++) {
                    adendum += '<li><a href="#" data- value=\'' + data[i] + '\' >' + data[i] + '</a></li>';
                }
                document.getElementById('comboHorasList').innerHTML = adendum;
                $('#dropdownHoras a').click(function (e) {
                    e.preventDefault();
                    $('#selectedHora').text($(this).text());
                    self.horaSeleccionada = $(this).text();
                    $('#divdatos').show('slow');
                });
            });
            $('#divhoras').show('slow');
        };
        var isEmail = function (search) {
            var serchfind;
            var regexp = new RegExp(/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/);
            serchfind = regexp.test(search);
            console.log(serchfind);
            return serchfind;
        };
        this.filtraDias = function (dt) {
            for (var i = 0; i < _this.diasValidos.length; i++) {
                if (dt.getDay() === _this.diasValidos[i] && dt >= Date.now()) {
                    return [true, ''];
                }
            }
            return [false, ''];
        };
        this.datePickerConfig = function () {
            $('#datepick').datepicker({
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                firstDay: 1,
                beforeShowDay: _this.filtraDias,
                dateFormat: 'dd-mm-yy'
            });
            $('#datepick').change(function () {
                getHorasServidorBs();
                $('#selectedHora').text('Seleccione');
                self.horaSeleccionada = '';
            });
        };
        this.comprobarYEnviar = function () {
            if (document.getElementById('termsAndCondCheck').checked === true) {
                if (_this.idHospital !== '0') {
                    var urlb = '/citas/nuevaCita';
                    var fecha = $('#datepick').val();
                    if (fecha !== '') {
                        var ano = fecha.substring(6);
                        var mes = fecha.substring(3, 5);
                        var dia = fecha.substring(0, 2);
                        var fechaCompleta = ano + mes + dia;
                        ano = parseInt(fecha.substring(6));
                        mes = parseInt(fecha.substring(3, 5)) - 1;
                        dia = parseInt(fecha.substring(0, 2));
                        var date = new Date(ano, mes, dia);
                        var diasemana = date.getDay();
                        if (diasemana === 0) {
                            diasemana = 7;
                        }
                        var hora = _this.horaSeleccionada;
                        if (hora !== '') {
                            var nombre = $('#Nombre').val();
                            if (nombre !== undefined && nombre !== '') {
                                var telefono = $('#Telefono').val();
                                if (telefono !== undefined && telefono !== '') {
                                    var email = $('#Email').val();
                                    if (email !== undefined && email !== '' && isEmail(email)) {
                                        var observaciones = $('#observaciones').val();
                                        $.ajax({
                                            url: urlb,
                                            data: {
                                                hospi: _this.idHospital,
                                                dia: fechaCompleta,
                                                diasemana: diasemana,
                                                hora: hora,
                                                nombre: nombre,
                                                telefono: telefono,
                                                email: email,
                                                observaciones: observaciones
                                            },
                                            dataType: 'json',
                                            traditional: true,
                                            success: function () {
                                                alert('                                 Cita previa reservada.' +
                                                    '\n\n' +
                                                    'Si relleno el campo de email recibira uno de confirmacion.' +
                                                    '\n\n' +
                                                    '                                     Muchas gracias.');
                                                var url = '/Home';
                                                window.location.href = url;
                                            }
                                        });
                                    }
                                    else {
                                        alert('El email es obligatorio');
                                    }
                                }
                                else {
                                    alert('El telefono es obligatorio');
                                }
                            }
                            else {
                                alert('El nombre es obligatorio');
                            }
                        }
                        else {
                            alert('Seleccione una hora correcta');
                        }
                    }
                    else {
                        alert('Seleccione una fecha correcta');
                    }
                }
                else {
                    alert('Seleccione un Hospital');
                }
            }
        };
        initializeCita(this);
    }
    Cita.prototype.datePickerConfig = function () { };
    Cita.prototype.getDiasDelServidor = function () { };
    Cita.prototype.filtraDias = function (dt) { return []; };
    Cita.prototype.comprobarYEnviar = function () { };
    return Cita;
}());
$(document).ready(function () {
    enableNavBar(8);
    var cita = new Cita();
    cita.datePickerConfig();
});
//# sourceMappingURL=Index.js.map