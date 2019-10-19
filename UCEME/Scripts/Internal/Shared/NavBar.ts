var enableNavBar = (index: number) => {
    $('ul.nav li').removeClass('active'); //Anula todas las selecciones
    $('ul.nav li:nth-child(' + index + ')').addClass('active'); //Asigna la clase Active al TAB Seleccionado
};

function mostrarDiv(id: string) {
    $('#' + id).css('display', 'block');
}

function ocultarDiv(id: string) {
    $('#' + id).css('display', 'none');
}