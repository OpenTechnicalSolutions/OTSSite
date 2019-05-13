﻿$("#loginForm").submit(function (e) {
    e.preventDefault();
    $.get({
        url: "/api/Accounts/Login",
        method: "GET",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify({
            UserName: (<HTMLInputElement>document.getElementById("userName")).value,
            Password: (<HTMLInputElement>document.getElementById("password")).value
        })
    }).done(function (msg) {
        console.log("SUCCESS!/n" + msg);
    });
});

$("#registerForm").submit(function (e) {
    e.preventDefault();
    var form = $('#registerForm');
    var token = $('input[name="__RequestVerificationToken"]', form).val();
    console.log(token);
    $.post({
        url: "/api/Accounts/Register",
        //headers: { '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]', form).val()},
        contentType: "application/json",
        dataType: "json",
        data: {
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]', form).val(),
            createUserDto: JSON.stringify({
                UserName: $('#userName', form).val(),
                Email: $('#email', form).val(),
                Password1: $('#password1', form).val(),
                Password2: $('#password2', form).val()
            })
        }
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