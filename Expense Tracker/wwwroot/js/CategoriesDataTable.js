$(document).ready(function () {
    $('#Categories').dataTable({
        "processing":true,
        "serverside": true,
        "filter": true,
        "pageLength":5,
        "ajax": {
            "url": "/Category/Data",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [0],
            "visible": false,
            "searchable": false
        }],
        "columns": [
            { "data": "id", "name": "Id", "autowidth": true },
            { "data": "fullCategory", "name": "fullCategory", "autowidth": true },
            { "data": "type", "name": "Type", "autowidth": true },
            {
                "render": function (data, type, row) {
                    return '<a class="btn btn-lg" href="/Category/AddOrEdit/' + row.id + '">    <i class="fa-solid fa-pen" ></i></a> &nbsp; <a class="btn btn-lg" href="/Category/Delete/' + row.id + '" ><i class="fa-solid fa-trash"></i></a>'
                },
                "orderable": false
            }
        ]
    });
});