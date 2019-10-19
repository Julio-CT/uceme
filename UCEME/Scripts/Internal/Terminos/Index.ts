function anadirTermino() {
  $('#dialogoCrearT').dialog(
        {
          dialogClass: 'noclose',
          modal: true,
          position: ['middle', 225],
          width: 600,
          autoOpen: false,
          show: 'slow'
        });
  $('#dialogoCrearT').load();
  $('#dialogoCrearT').dialog('open');
}

function cerrarDialogoCrearT() {
  $('#dialogoCrearT').dialog('close');
}

function formBuscar() {
  var url = '/Terminos/Dicciopinta';
  url += '/?busqueda=' + $('#cajaBuscar').val();

  $.get(url, data => {
      $('#diccionario').html(data);
  });
}
