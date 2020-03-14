$(document).ready(() => {
    $('#Fecha').datepicker({
        dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
        firstDay: 1,
        dateFormat: 'dd-mm-yy'
    });

    enableNavBar(4);
});