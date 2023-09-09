function show_alert(icon, message, type, from, align) {

    $.notify({
        icon: icon,
        message: message

    }, {
        type: type,
        z_index: 2000,
        timer: 2500,
        placement: {
            from: from,
            align: align
        }
    });
}

function formateOnlyDate1(fd) {
    if (fd != "" && fd != "null" && fd != null) {
        f = new Date(parseInt(fd.substr(6)));
        fecha = ("0" + (f.getDate())).slice(-2) + "/" + ("0" + (f.getMonth() + 1)).slice(-2) + "/" + f.getFullYear();
    } else fecha = "";
    return fecha;
}


function getPath() {
    var origin = window.location.origin;
    var $Patch;
    if (origin == "http://localhost:61277") {
        $Patch = origin + "/";
    }else if (origin == "http://192.168.100.38:61277") {
        $Patch = origin + "/";
    }
    else if (origin == "http://172.20.10.2:61277") {
        $Patch = origin + "/";
    }else {
        $Patch = origin + "/REDZ";
    }
    return $Patch;
}