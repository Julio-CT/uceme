class MediaManager extends ItemManager {
    defaultPath: string;

    cambiarEstrella(id: string): void { }

    aceptar(id: string): void { }

    cancelar(id: string): void { }

    cancelarDialogo(): void { }

    editarFoto(id: string): void { }

    eliminarFoto(id: number): void { }

    constructor(path: string) {
        super(path);

        this.cambiarEstrella = (id: string) => {
            var url = '/' + this.defaultPath + '/cambiarDestacada';
            url += '/?id=' + id;

            $.get(url, data => {
                var src = (data) ? '../Images/destacadaSi.png' : '../Images/destacadaNo.png';
                $('#' + id).attr('src', src);
            });
        }

        this.aceptar = (id: string) => {
            var urlb = '/' + this.defaultPath + '/guardarCambios';
            var idIt = id;
            var posIt = $('#p' + id).val();
            var textoIt = $('#t' + id).val();
            var descripcion = "";
            var descDiv = $('#desc' + id);
            if (descDiv) {
                descripcion = descDiv.val();
            }
            $.ajax({
                url: urlb,
                data: { id: idIt, pos: posIt, titulo: textoIt, descripcion: descripcion },
                dataType: 'json',
                traditional: true,
                success() {
                    alert('Elemento editado correctamente');
                    location.reload();
                }
            });
        }

        this.eliminarFoto = (id: number) => {
            var statusConfirm = confirm('¿Realmente desea eliminar la foto?');
            if (statusConfirm) {
                var urlb = '/' + this.defaultPath + '/Eliminar';
                var idIt = id;
                $.ajax({
                    url: urlb,
                    data: { id: idIt },
                    dataType: 'json',
                    traditional: true,
                    success: () => {
                        alert('Foto eliminada');
                        location.reload();
                    }
                });
            }
        }

        this.cancelar = (id: string) => {
            $('#divA' + id).show();
            $('#divB' + id).hide();
            $('#divM' + id).show();
            $('#divE' + id).hide();
        }

        this.cancelarDialogo = () => {
            $('#dialogo').slideToggle(1800);
        }

        this.editarFoto = (id: string) => {
            $('#divA' + id).hide();
            $('#divB' + id).show();
            $('#divM' + id).hide();
            $('#divE' + id).show();
        }
    }
}