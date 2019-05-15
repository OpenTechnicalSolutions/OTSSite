$('#formLogout').submit(function (e) {
    e.preventDefault();
    $.post({
        url: 'api/Accounts/Logout'
    }).done(function () {
        window.location.reload();
    });
});
//# sourceMappingURL=SiteUiScripts.js.map