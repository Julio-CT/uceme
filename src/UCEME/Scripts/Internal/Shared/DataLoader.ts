class DataLoader {
    
    private pagina: number;
    private inCallback: boolean;
    private path: string;
    loadData(): void { }

    constructor(url: string) {
        this.pagina = 0;
        this.inCallback = false;
        this.path = url;
        this.loadData = () => {
            if (this.pagina > -1 && !this.inCallback) {
                this.inCallback = true;
                this.pagina++;
                $('div#loading').html('<p><img src="/Images/loader.gif"></p>');
                $.get(this.path + this.pagina, data => {
                    if (data !== '') {
                        $('#contenedor').append(data);
                    } else {
                        this.pagina = -1;
                    }

                    this.inCallback = false;
                    $('div#loading').empty();
                });
            }
        }
    }
}