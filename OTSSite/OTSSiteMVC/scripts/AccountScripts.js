//Login script
$("#loginForm").submit(function (e) {
    e.preventDefault(); //prevents default submit
    var form = $('#loginForm'); //Gets the form    
    $.post({
        url: "/api/Accounts/Login",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify({
            UserName: $('#userName', form).val(),
            Password: $('#password', form).val() //get password value
        })
    }).done(function (msg) {
        console.log("SUCCESS!/n" + msg);
        window.location.replace("/Home/Index");
    });
});
//Register script
$("#registerForm").submit(function (e) {
    e.preventDefault(); //Disable default submit
    var form = $('#registerForm'); //Form data
    $.post({
        url: "/api/Accounts/Register",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify({
            UserName: $('#userName', form).val(),
            Email: $('#email', form).val(),
            Password1: $('#password1', form).val(),
            Password2: $('#password2', form).val() //get password2
        })
    }).done(function (msg) {
        window.location.replace("/Account/Login");
    });
});
//# sourceMappingURL=AccountScripts.js.map