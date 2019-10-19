var enableNavBar = function (index) {
    $('ul.nav li').removeClass('active'); //Anula todas las selecciones
    $('ul.nav li:nth-child(' + index + ')').addClass('active'); //Asigna la clase Active al TAB Seleccionado
};
function mostrarDiv(id) {
    $('#' + id).css('display', 'block');
}
function ocultarDiv(id) {
    $('#' + id).css('display', 'none');
}
//# sourceMappingURL=NavBar.js.map