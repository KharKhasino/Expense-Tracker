$(document).ready(function () {
    $('#Transactions').dataTable({
        "processing": true,
        "serverside": true,
        "filter": true,
        "pageLength": 5,
        "ajax": {
            "url": "/Transactions/Data",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [0],
            "visible": false,
            "searchable": false,
        }],
        "columns": [
            { "data": "id", "name": "Id", "autowidth": true },
            { "data": "categoryInfo", "name": "CategoryInfo", "autowidth": true },
            { "data": "formattedDate", "name": "FormattedDate", "autowidth": true },
            { "data": "formattedAmount", "name": "FormattedAmount", "autowidth": true },
            {
                "render": function (data, type, row) {
                    return '<a class="btn btn-lg" href="/Transactions/AddOrEdit/' + row.id + '">    <i class="fa-solid fa-pen" ></i></a>  &nbsp; <a class="btn btn-lg" href="/Transactions/Details/' + row.id + '" ><i class="fa-solid fa-book"></i></a> &nbsp; <a class="btn btn-lg" href="/Transactions/Delete/' + row.id + '" ><i class="fa-solid fa-trash"></i></a>'
                },
                "orderable": false
            }
        ]
    });
});