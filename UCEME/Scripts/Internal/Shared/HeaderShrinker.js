function headerShrinker() {
    window.addEventListener('scroll', function () {
        var distanceY = window.pageYOffset || document.documentElement.scrollTop, shrinkOn = 50;
        if (distanceY > shrinkOn) {
            $('.bigLogo').addClass('smallLogo');
            $('.site-title-div').addClass('site-title-div-small');
            //$('#headerNavBar').addClass('float-left');
        }
        else {
            $('.smallLogo').removeClass('smallLogo');
            $('.site-title-div-small').removeClass('site-title-div-small');
            //$('#headerNavBar').removeClass('float-left');
        }
    });
}
$(document).ready(function () { return headerShrinker(); });
//# sourceMappingURL=HeaderShrinker.js.map