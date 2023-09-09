var tbl_facturas;
var name = "Facturas";
var btn_action = "save";
$(document).ready(function (e) {
    $('#slct_estadoUSA').niceSelect("destroy").hide();
    $('#slct_estadoCAN').niceSelect("destroy").hide();

    if ($('#section_listado').css("display") == "block") {
        btn_action = "edit";
        GetPagosSinTimbrar()
    }

    $('#slct_pais').change(function (e) {
        $('.selects_pais').niceSelect("destroy").hide();

        var pais = $('#slct_pais').val();
        var element = "#slct_estado" + pais;

        $(element).fadeIn();
        $(element).niceSelect();
    })

    $('form').submit(function (e) {
        if (isFormCorrect()) {
            $('#btn_save').html("<i class='fa-solid fa-circle-notch fa-spin'></i>")
            if ($('#usocfdi').val() != "") {
                if ($('#regimen').val() != "") {
                    var form = JSON.stringify($("form").serializeArray());
                    $.post(getPath() + "/Facturacion/SaveDatosFiscales", { form: form, action:btn_action }, function () { }).done(function (data) {
                        if (data == "OK") {
                            $('#btn_save').html("<i class='fa-solid fa-check fa-flip'></i> Guardar datos")
                            location.reload();
                        }
                    });
                    console.log($("form").serializeArray());
                } else {
                    e.preventDefault();
                    $('#btn_save').html("<i class='fa-solid fa-check'></i> Guardar datos")
                    show_alert("fa-solid fa-circle-exclamation fa-lg", "Selecciona un régimen fiscal válido antes de continuar", "danger", "top", "center");
                }
                
            } else {
                e.preventDefault();
                $('#btn_save').html("<i class='fa-solid fa-check'></i> Guardar datos")
                show_alert("fa-solid fa-circle-exclamation fa-lg", "Selecciona un UsoCFDI válido antes de continuar", "danger", "top", "center");
            }
           
        } else {
            e.preventDefault();
            $('#btn_save').html("<i class='fa-solid fa-check'></i> Guardar datos")
            show_alert("fa-solid fa-circle-exclamation fa-lg", "Completa todos los campos antes de continuar", "danger", "top", "center");
        }

    });

    $('#btn_modificar').click(function (e) {
        $('#section_listado').hide();
        $('#section_form').fadeIn();
        $('.cancel_action').fadeIn();
        $('.selects_pais').niceSelect("destroy").hide();
     
        $.post(getPath() + "/Facturacion/GetDatosFiscales", function () { }).done(function (data) {
            var pais = data.pais;
            var element = "#slct_estado" + pais;

            $(element).fadeIn();
            $(element).niceSelect();
            console.log(data);
            $('#nombre').val(data.nombre);
            $("#rfc").val(data.rfc);
            $('#usocfdi').val(data.uso_cfdi).niceSelect("update");
            $('#regimen').val(data.regimen_fiscal).niceSelect("update");
            $('#direccion').val(data.direccion);
            $('#slct_pais').val(data.pais).niceSelect("update");
            $('#colonia').val(data.colonia);
            $('#codigopostal').val(data.codigo_postal)
            $(element).val(data.estado).niceSelect("update");
        })
    });

    $('#btn_cancel').click(function (e) {
        e.preventDefault();
        $('#section_listado').fadeIn();
        $('#section_form').hide();
        $('.cancel_action').hide();
    });

    $(document).on("click", ".solicitar_timbrado", function (e) {
        var id_invoice = $(this).data("id");
        var invoice = $(this).data("invoice");
        $.post(getPath() + "/Facturacion/SetNuevoArchivoTimbre", { invoice: invoice, id_invoice: id_invoice }, function () { }).done(function (data) {
            if (data == "OK") {
                //Se creó el archivo
            } else if (data == "NOK") {
                //El archivo ya existe
            } else {
                //Error de creación de archivo
                //Falta agregar la bandera para que si quiere su factura pague el 16% de IVA
            }
        });
    })

})

function isFormCorrect() {
    var flag = true;
    $("input[type=text]").each(function (i, e) {
        if ($(e).val() == "") {
            $(e).css("border", "#af1d1d solid")
            flag = false;
        } else {
            $(e).css("border", "")
        }
    });
    return flag;
}

function GetPagosSinTimbrar() {
    $.post(getPath() + "/Facturacion/GetFacturasSinTimbrar", function () { }).done(function (data) {
        console.log(data);
        if (tbl_facturas)
            $('#tbl_facturas').DataTable().clear().destroy();
        tbl_facturas = $('#tbl_facturas').DataTable({

            "bPaginate": false,
            "bLengthChange": true,
            "bFilter": false,
            "pageLength": 150,
            "bInfo": false,
            "searching": true,
            "responsive": true,
            "order": [[0, "desc"]],
            "ordering": false,
            "aaData": data.reverse(),
            "columnDefs": [
                { "className": "dt-center", "targets": "_all" }
            ],
            "columns": [
                { "data": "invoice" },
                { "data": "fecha" },
                { "data": "fecha_timbrado" },
                { "data": "tipo" },
                {
                    "data": null,
                    render: function (data, type, row) {
                        if (row.fecha_timbrado == "Sin timbrar") {
                            return '<button class="btn btn-outline-dark solicitar_timbrado"  data-id="' + row.id_invoice +'" data-invoice="'+row.invoice+'"><i class="fa-solid fa-file-signature"></i><span> Solicitar</span></button>'
                        } else

                            '<button class="btn btn-outline-danger descargar_archivos" data-id="' + row.id_invoice +'"><i class="fa-solid fa-file-arrow-down" ></i> Descargar</span></button>'
                    }
                }

            ],
            order: [[0, 'desc']],
            language: {
                search: "Buscar&nbsp;:",
                lengthMenu: "Mostrar _MENU_ " + name,
                info: "De _START_ al _END_ de _TOTAL_ " + name,
                infoEmpty: "Sin " + name,
                infoFiltered: "(Filtrado de _MAX_ en total)",
                infoPostFix: "",
                loadingRecords: "Cargando " + name + "...",
                zeroRecords: "No hay registro",
                emptyTable: "No hay " + name + " registrados",
                processing:'<i class="fa-solid fa-circle-notch fa-spin fa-spin-reverse"></i>',
                paginate: {
                    first: "Primero",
                    previous: "Anterior",
                    next: "Siguiente",
                    last: "Ultimo"
                },
                scrollY: 400
            }

        })
    })
}