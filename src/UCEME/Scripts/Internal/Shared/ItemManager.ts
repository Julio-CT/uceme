interface IItemManager {
    anadir: () => void;
    editarItem: (id: number) => void;
    borrarItem: (id: number) => void;
}

class ItemManager implements IItemManager
{
    defaultPath: string;

    anadir(): void { }

    editarItem(id: number): void { }

    borrarItem(id: number): void { }

    quitarNuevo(): void { }

    constructor(path: string) {
        this.defaultPath = path;

        this.anadir = () => {
            var urlb = '/' + this.defaultPath + '/Anadir';
            location.href = urlb;
        }

        this.editarItem = (id: number) => {
            var urlb = '/' + this.defaultPath + '/Editar';
            location.href = urlb + '/?id=' + id;
        }

        this.borrarItem =  (id: number) => {
            var statusConfirm = confirm('¿Realmente desea eliminar el elemento?');
            if (statusConfirm) {
                var urlb = '/' + this.defaultPath + '/Eliminar';
                var idIt = id;
                $.ajax({
                    url: urlb,
                    data: { id: idIt },
                    dataType: 'json',
                    traditional: true,
                    success: () => {
                        alert('Elemento eliminado');
                        location.reload();
                    }
                });
            }
        }

        this.quitarNuevo = () => {
            $('#newitem div').each(function () { $(this).remove(); });
            location.reload();
        }
    }
}