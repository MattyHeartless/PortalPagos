var table_Pagos
var name = "Pagos";

$(document).ready(function () {
    oTable = $('.tblPagos').DataTable();

    $.post(getPath() + "Pagos/getPagos", {
    }, function () { }).done(function (data) {
        console.log(data)

        if (table_Pagos)
            $('#tblPagos').DataTable().clear().destroy();
        table_Pagos = $('#tblPagos').DataTable({

            "bPaginate": true,
            "bLengthChange": false,
            "bFilter": false,
            "pageLength": 10,
            "bInfo": false,
            "searching": false,
            "responsive": true,
            "order": [[0, "desc"]],
            "processing":true,
            "ordering": false,
            "aaData": data.reverse(),
            "columnDefs": [
                { "className": "dt-center", "targets": "_all" }
            ],
            "columns": [
                { "data": "number" },
                { "data": "type" },
                {
                    "data": null,
                    render: function (data, type, row) {
                        return row.label.substring(0, row.label.indexOf(" "));
                    }
                },
                {
                    "data": null,
                    render: function (data, type, row) {
                        return row.label.substring(row.label.indexOf(" ") + 1, row.label.length);
                    }
                },
                {
                    "data": null,
                    render: function (data, type, row) {
                        return Intl.NumberFormat('es-US', { style: 'currency', currency: 'USD' }).format(row.amountToPay);
                    }
                },
                {
                    "data": null,
                    render: function (data, type, row) {
                        return Intl.NumberFormat('es-US', { style: 'currency', currency: 'USD' }).format(row.total);
                    }
                },
                {
                    "data": null,
                    "render": function (data, type, row) {

                        return "<button class='btn btn-primary pay_invoice' data-clientid='"+row.clientId+"' data-invoiceid='"+row.id+"' data-amounttopay='"+row.amountToPay+"'><i class='fas fa-dollar-sign''></i> Pagar</button>"
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
                processing: '<i class="fa-solid fa-circle-notch fa-spin fa-spin-reverse"></i>',
                paginate: {
                    first: "Primero",
                    previous: "Anterior",
                    next: "Siguiente",
                    last: "Ultimo"
                },
                scrollY: 400
            },
            "createdRow": function (row, data, dataIndex) {

                if (data.status != '1') {
                    $(row).find('td:eq(6)').hide();

                } else {
                    $(row).addClass('bg-danger pendiente');
                }
            }

        })

    })



    $(document).on("click", ".pay_invoice", function (e) {
        var price = $(this).data("amounttopay");
        var service = (parseFloat(price) * 0.0457);
        var invoiceId = $(this).data("invoiceid");
        $('#lblprecio').text("$" + price);
        $('#lblsubtotal').text("$" + price);
        $('#lblcargoserv').text("$" + service.toFixed(2) + 3);
        $('#lbltotal').val(parseFloat(price) + service + 3);
        $('#invoiceId').val(invoiceId);
        $('#id_precio').val(parseFloat(price));
        $('#pago-modal').modal("toggle");
    });

})