﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url": "/Admin/User/GetAll"
        },
        "columns": [
            { "data": "name", "width": "25%" },
            { "data": "email", "width": "25%" },
            { "data": "role", "width": "25%" },
            {
                "data": {
                    id: "id",
                    lockoutEnd: "lockoutEnd"
                },
                "render": function(data) {
                    const today = new Date().getTime();
                    const lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {
                        //user is locked out.
                        return `
                            <div class="text-center">
                                <a onclick=LockUnlock('${data.id
                            }') class="btn btn-danger text-white" style = "cursor: pointer; width: 100px" >
                                    <i class="fas fa-lock-open"></i> Unlock
                                </a>
                            </div>
                            `;
                    } else {
                        return `
                            <div class="text-center">
                                <a onclick=LockUnlock('${data.id
                            }') class="btn btn-success text-white" style = "cursor: pointer; width:100px" >
                                    <i class="fas fa-lock"></i> Lock
                                </a>
                            </div>
                            `;
                    }

                },
                "width": "25%"
            }
        ],
        "initComplete": function () {
            this.api().columns([2]).every(function () {
                var column = this;
                var select = $('<select><option value="">Filter Results</option></select>')
                    //.appendTo($(column.footer()).empty())
                    .appendTo('#dropdown')
                    .on('change',
                        function () {
                            var val = $.fn.dataTable.util.escapeRegex(
                                $(this).val()
                            );

                            column
                                .search(val ? '^' + val + '$' : '', true, false)
                                .draw();
                        });

                column.data().unique().sort().each(function (d, j) {
                    select.append('<option value="' + d + '">' + d + '</option>')
                });
            });
        }
    });
/*  Setup for underneath search function per column... Don't care for these. 
 $("#tblData tfoot th").each(function () {
        var title = $("#tblData thead th").eq($(this).index()).text();
        $(this).html('<input type="text" placeholder="Search ' + title + ' " />');
    });

    dataTable.columns().every(function() {
        var datatableColumn = this;

        $(this.footer()).find('input').on('keyup change', function () {
            datatableColumn.search(this.value).draw();
        });
    });*/

    //var indexCol = 2;
    //$("#tblData tfoot th").each(function (i) {
    //    if (i === indexCol) {
    //        this.innerHTML = fnCreateSelect(tblData.fnGetColumnData(i));
    //        $('select', this).change(function() {
    //            tblData.fnFilter($(this).val(), i);//            
    //        });
    //    }
    //});
}



function LockUnlock(id) {

    $.ajax({
        type: "POST",
        url: "/Admin/User/LockUnlock",
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function(data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            } else {
                toastr.error(data.message);
            }
        }
    });
}