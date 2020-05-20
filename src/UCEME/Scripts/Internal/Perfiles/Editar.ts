class Perfil extends ItemManager {

    anadirNuevo(): void { };

    constructor() {
        super('Perfiles');

        this.anadirNuevo = () => {
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
                success() {
                    alert('Perfil Añadido.');
                    location.reload();
                }
            });
        }
    }
}

var perfilManager = new Perfil();

$(document).ready(() => {
    $(() => {
        $('#back').click(() => {
            history.go(-1);
        });
    });

    $(() => {
        $('#addElem').click(e => {
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