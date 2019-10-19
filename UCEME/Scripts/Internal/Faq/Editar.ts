var manager = new ItemManager('Faq');

$(document).ready(() => {
    $(() => {
        $('#back').click(() => {
            history.go(-1);
        });
    });

    enableNavBar(6);
  
    $(() => {
        $('#addElem').click(e => {
            e.preventDefault();
            var itemIndex = $('#contenedor #elemento').length;

            var adendum = '<div id=\'elemento\'>';
            adendum += '<label>Pregunta nueva:</label>';
            adendum += '<div class=\'display-label\' ';
            adendum += 'style=\'width:80%;height:40px;';
            adendum += 'background-color:#e2e2e2;margin-bottom:15px;\'>';
            adendum += '&nbsp;&nbsp;&nbsp;<input class=\'text-box ';
            adendum += 'single-line\' type=\'text\' ';
            adendum += 'id=\'TituloNuevo\' name=\'[';
            adendum += itemIndex + '].titulo\' /></div>';
            adendum += '<label>Respuesta nueva:</label>';
            adendum += '<div class=\'display-field\' ';
            adendum += 'style=\'width:80%;height:120px;';
            adendum += 'background-color:wheat;\'>&nbsp;';
            adendum += '&nbsp;&nbsp;<input  style=\'width:';
            adendum += '80%;height:100px;\' type=\'text\' ';
            adendum += 'id=\'TextoNuevo\' name=\'[';
            adendum += itemIndex + '].texto\' /></div>';
            adendum += '<div class=\'botones\' ';
            adendum += 'style=\'width:80%;margin-bottom';
            adendum += ':30px;background-color:wheat\'>';
            adendum += '<input id=\'cancelar\' class=\'cancelar\' ';
            adendum += 'onclick=\'manager.quitarNuevo()\' style=\'height:';
            adendum += '32px;margin:0px;padding:0px;\' /><br /></div> ';
            adendum += '</div>';
            $('#newitem').append(adendum);
        });
    });
});
