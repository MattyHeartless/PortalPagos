$(document).ready(function (e) {
    $('#btn_login').click(function (e) {
        var username = $('#username').val();
        var key = $('#key').val();
        if (username != "") {
            if (key != "") {
                $('#btn_login').html('<i class="fa-solid fa-circle-notch fa-spin"></i>')
                $.post(getPath() + "Session/StartSession", { username: username, key: key }, function () { }).done(function (data) {
                    if (data == "OK") {
                        $('#btn_login').html('<i class="fa-solid fa-circle-check fa-shake"></i>').removeClass("default-btn").addClass("succeed_action")
                        
                        window.location.replace(getPath() + "/Cliente/Dashboard");
                    }
                      
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