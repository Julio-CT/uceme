
class Cita {
    public datePickerConfig(): void { }
    public idHospital: string;
    public horaSeleccionada: string;
    public getDiasDelServidor(): void { }
    private diasValidos: any[];
    private filtraDias(dt: any): any[] { return [] }
    private comprobarYEnviar(): void { }

    constructor() {
        var self = this;
        this.idHospital = '';
        this.diasValidos = [];

        var checkDataFields = () => {
            if ($('#Nombre').val() !== undefined && $('#Nombre').val() !== '' && $('#Telefono').val() !== undefined && $('#Telefono').val() !== '' && $('#Email').val() !== undefined && $('#Email').val() !== '' && isEmail($('#Email').val())) {
                $('#divEnviar').show('slow');
            } else {
                $('#divEnviar').hide('slow');
            }
        }

        var initializeCita = (cita: Cita) => {
            $('#Telefono').change(() => {
                checkDataFields();
            });

            $('#Email').change(() => {
                checkDataFields();
            });

            $('#Nombre').change(() => {
                checkDataFields();
            });

            $('#dropdownHospital a').click(function (e) {
                e.preventDefault();
                $('#selectedHospital').text($(this).text());
                cita.idHospital = $(this).data('value');
                cita.getDiasDelServidor();

                // if hospital has changed then we need to clear
            });

            $('#botonEnviar').click(() => { cita.comprobarYEnviar(); });
        }

        this.getDiasDelServidor = () => {
            if (this.idHospital !== '0') {
                var url = '/Citas/ListaDias';
                url += '/?id=' + this.idHospital;
                (<HTMLInputElement>document.getElementById('datepick')).value = "";

                $.get(url, data => {
                    this.diasValidos = [];
                    for (var i = 0; i < data.length; i++) {
                        this.diasValidos[i] = data[i];
                    }
                });

                $('#divdias').show('slow');

                $('#divhoras').hide('slow');
                $('#divdatos').hide('slow');
            }
        }

        var getHorasServidorBs = () => {
            var fecha = $('#datepick').val();
            var ano = parseInt(fecha.substring(6));
            var mes = parseInt(fecha.substring(3, 5)) - 1;
            var dia = parseInt(fecha.substring(0, 2));
            var date = new Date(ano, mes, dia);
            var diasemana = date.getDay();
            if (diasemana === 0) {
                diasemana = 7;
            }

            var url = '/Citas/ListaHoras' + '/?diasem=' + diasemana + '&hospi=' + this.idHospital +
                '&dia=' + dia + '&mes=' + mes + '&ano=' + ano;

            $.get(url, data => {
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
        }

        var isEmail = (search: string): boolean =>
        {
            var serchfind: boolean;

            var regexp = new RegExp(/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/);

            serchfind = regexp.test(search);

            console.log(serchfind)
            return serchfind
        }

        this.filtraDias = (dt: any): any[] => {
            for (var i = 0; i < this.diasValidos.length; i++) {
                if (dt.getDay() === this.diasValidos[i] && dt >= Date.now()) {
                    return [true, ''];
                }
            }

            return [false, ''];
        }

        this.datePickerConfig = () => {
            $('#datepick').datepicker({
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                firstDay: 1,
                beforeShowDay: this.filtraDias,
                dateFormat: 'dd-mm-yy'
            });

            $('#datepick').change(() => {
                getHorasServidorBs();

                $('#selectedHora').text('Seleccione');
                self.horaSeleccionada = '';
            });
        }

        this.comprobarYEnviar = () => {
            if ((<HTMLInputElement>document.getElementById('termsAndCondCheck')).checked === true) {
                if (this.idHospital !== '0') {
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

                        var hora = this.horaSeleccionada;
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
                                                hospi: this.idHospital,
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
                                            success() {
                                                alert('                                 Cita previa reservada.' +
                                                    '\n\n' +
                                                    'Si relleno el campo de email recibira uno de confirmacion.' +
                                                    '\n\n' +
                                                    '                                     Muchas gracias.');
                                                var url = '/Home';
                                                window.location.href = url;
                                            }
                                        });
                                    } else {
                                        alert('El email es obligatorio');
                                    }
                                } else {
                                    alert('El telefono es obligatorio');
                                }
                            } else {
                                alert('El nombre es obligatorio');
                            }
                        } else {
                            alert('Seleccione una hora correcta');
                        }
                    } else {
                        alert('Seleccione una fecha correcta');
                    }
                } else {
                    alert('Seleccione un Hospital');
                }
            }
        }

        initializeCita(this);
    }
}

$(document).ready(() => {
    enableNavBar(8);
    var cita = new Cita();
    cita.datePickerConfig();
});
