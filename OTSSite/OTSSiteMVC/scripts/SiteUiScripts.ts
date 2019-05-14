function fillPartial(url: string) {
   $.get({
        url: url,

   }).done(function (msg: HTMLElement) {
       if (msg != null) {
            var siteOverLay = <HTMLDivElement>document.getElementById('siteOverlay');
            siteOverLay.innerHTML += msg;
            siteOverLay.hidden = false;
        }
    })
}

$('#partialLogin').click( function (e) {
    e.preventDefault();
    fillPartial('/Account/Login');
});

$(document).on('click','#partialRegister', function (e) {
    e.preventDefault();
    fillPartial('/Account/Register');
});