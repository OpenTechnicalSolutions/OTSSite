$("#login-form").submit(function (e) {
    var formElement = document.getElementById("#login-form");
    var jsonform = JSON.stringify(formElement);
    $.ajax({
        url: "/api/Accounts/Login",
        method: "GET",
        contentType: "application/json",
        dataType: "json",
        data: jsonform
    }).done(function (msg) {
    });
});

$("#registerForm").submit(function (e) {
    e.preventDefault();
    $.post({
        url: "/api/Accounts/Register",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify({
            UserName: (<HTMLInputElement>document.getElementById("userName")).value,
            Email: (<HTMLInputElement>document.getElementById("email")).value,
            Password1: (<HTMLInputElement>document.getElementById("password1")).value,
            Password2: (<HTMLInputElement>document.getElementById("password2")).value
        })
    }).done(function (msg) {
        console.log("SUCCESS/nSUCCESS/nSUCCESS/n" + msg);
    });
    /*$.ajax({
        url: "/api/Accounts/Register",
        method: "POST",
        contentType: "application/json",
        dataType: "json",
        data: jsonform
    }).done(function (msg) {
        console.log("SUCCESS/nSUCCESS/nSUCCESS/n" + msg);
    });*/
});