$(document).ready(function (e) {
    $('#btn_login').click(function (e) {
        var username = $('#username').val();
        var key = $('#key').val();
        if (username != "") {
            if (key != "") {
                $.post(getPath() + "Session/StartSession", { username: username, key: key }, function () { }).done(function (data) {
                    if (data == "OK")
                        window.location.replace(getPath() + "/Cliente/Dashboard");
                    else
                        show_alert("fa-solid fa-circle-exclamation fa-lg", "Ocurrió un error al iniciar sesión. Intenta de nuevo más tarde", "danger", "top", "right");
                })
                
            } else
            show_alert("fa-solid fa-shield-halved", "Introduce tu clave de acceso", "danger", "top", "right");
        } else 
            show_alert("fa-solid fa-user-shield", "Ingresa tu número de usuario o cuenta antes de continuar ", "danger", "top", "right");
    });

    $('#key').keypress(function (E) {
        if (E.keyCode == 13)
            $('#btn_login').trigger("click");
    })
});