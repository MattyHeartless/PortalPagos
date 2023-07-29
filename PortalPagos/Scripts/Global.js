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

function getPath() {
    var origin = window.location.origin;
    var $Patch;
    if (origin == "http://localhost:61277") {
        $Patch = origin + "/";
    } else {
        $Patch = origin + "/REDZ";
    }
    return $Patch;
}