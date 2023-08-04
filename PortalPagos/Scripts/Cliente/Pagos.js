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

            "bPaginate": false,
            "bLengthChange": true,
            "bFilter": false,
            "pageLength": 150,
            "bInfo": false,
            "searching": false,
            "responsive": true,
            "order": [[0, "desc"]],
            "ordering": false,
            "aaData": data.reverse(),
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
                        return "<button class='btn btn-primary' onclick='getUser(\"" + row.id + "\")'><i class='fas fa-dollar-sign''></i> Pagar</button>"
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

})