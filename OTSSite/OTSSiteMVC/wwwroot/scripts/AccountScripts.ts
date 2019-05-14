//Login script
$("#loginForm").submit(function (e) {
    e.preventDefault();                             //prevents default submit
    var form = $('#loginForm');                     //Gets the form    
    $.post({                                        //Ajax POST action
        url: "/api/Accounts/Login",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify({
            UserName: $('#userName', form).val(),   //get username value
            Password: $('#password', form).val()    //get password value
        })
    }).done(function (msg) {                        //What to do when completed.
        console.log("SUCCESS!/n" + msg);
        window.location.replace("/Home/Index");
    });
});
//Register script
$("#registerForm").submit(function (e) {
    e.preventDefault();                             //Disable default submit
    var form = $('#registerForm');                  //Form data
    $.post({                                        //ajax post
        url: "/api/Accounts/Register",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify({
            UserName: $('#userName', form).val(),   //get username
            Email: $('#email', form).val(),         //get email
            Password1: $('#password1', form).val(), //get password
            Password2: $('#password2', form).val()  //get password2
        })
    }).done(function (msg: Response) {

        window.location.replace("/Account/Login")
    });
});