var tblVouchers;
var name = "Vouchers";
$(document).ready(function (e) {
    $.post(getPath() + "/Cliente/GetVouchers", function () { }).done(function (data) {
        console.log(data);

        if (tblVouchers)
            $('#tblVouchers').DataTable().clear().destroy();

        tblVouchers = $('#tblVouchers').DataTable({

            "bPaginate": false,
            "bLengthChange": true,
            "bFilter": false,
            "pageLength": 150,
            "bInfo": false,
            "searching": false,
            "responsive": true,
            'processing': true,
            'language': {
                'loadingRecords': '&nbsp;',
                'processing': '<i class="fa-solid fa-circle-notch fa-spin fa-spin-reverse"></i>'
            },
            "order": [[0, "desc"]],
            "ordering": false,
            "aaData": data.reverse(),
            "columnDefs": [
                { "className": "dt-center", "targets": "_all" }
            ],
            "columns": [
                {
                    "data": null,
                    render: function (data, type, row) {
                        console.log(row.Metadata.invoice_id)
                        var invoiceid = row.Metadata.invoice_id ? row.Metadata.invoice_id : 0;
                        return invoiceid;
                    }
                },
                {
                    "data": null,
                    render: function (data, type, row) {
                        return "OXXO";
                    }
                },
                {
                    "data": null,
                    render: function (data, type, row) {
                        
                        return formateOnlyDate1(row.Created);
                    }
                },
                {
                    "data": null,
                    render: function (data, type, row) {
                        var cantidad = row.Amount / 100;
                        console.log("lala " + row.Amount);
                        return "$ "+cantidad;
                    }
                },
                {
                    "data": null,
                    render: function (data, type, row) {
                     
                        return row.Status;
                    }
                },
                {
                    "data": null,
                    "render": function (data, type, row) {
                        if (row.NextAction != null)
                            console.log(row.NextAction.OxxoDisplayDetails.HostedVoucherUrl);
                        if (row.NextAction != null)
                            return "<a class='btn btn-primary' href='" + row.NextAction.OxxoDisplayDetails.HostedVoucherUrl + "' target='_blank'><i class='fas fa-link'></i> Ver voucher</a>"
                        else
                            return "";
                        
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

                if (data.NextAction == null) {
                

                } else {
                    $(row).addClass('bg-danger pendiente');
                }
            }

        })
    });
})