interface JQuery {
    slidesjs(parameters: any): any;
}

$(() => {
    $('.slider').slidesjs({
        width: 500,
        height: 284,
        navigation: false,
        play: {
            auto: true
        }
    });
});
