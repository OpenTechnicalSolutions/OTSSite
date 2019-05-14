$("#loginForm").submit(function (e) {
    e.preventDefault();
    var form = $('#loginForm');
    var username = $('#userName', form).val();
    var password = $('#password', form).val();
    $.post({
        url: "/api/Accounts/Login",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify({
            UserName: username,
            Password: password
        })
    }).done(function (msg) {
        console.log("SUCCESS!/n" + msg);
    });
});

$("#registerForm").submit(function (e) {
    e.preventDefault();
    var form = $('#registerForm');
    $.post({
        url: "/api/Accounts/Register",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify({
            UserName: $('#userName', form).val(),
            Email: $('#email', form).val(),
            Password1: $('#password1', form).val(),
            Password2: $('#password2', form).val()
        })
    }).done(function (msg) {
        console.log("SUCCESS/nSUCCESS/nSUCCESS/n" + msg);
    });
});