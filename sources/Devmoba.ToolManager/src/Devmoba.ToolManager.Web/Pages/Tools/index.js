const ToolStatus = {
    Active: 1,
    Inactive: 0
}

const ProcessState = {
    NA: 0,
    Killed: 1,
    Started: 2
}

const SwitchTool = {
    TurnOn: 1,
    TurnOff: 0
}

const StartedState = "<span class='online-status'><i class='fa fa-circle' aria-hidden='true'></i> Started</span>";
const KilledStatus = "<span class='offline-status'><i class='fa fa-circle-thin' aria-hidden='true'></i> Killed</span>";
const NAStatus = "<span><i class='fa fa-circle-thin' aria-hidden='true'></i> N/A</span>";

const ActiveStatus = "<span class='online-status'><i class='fa fa-circle' aria-hidden='true'></i> Active</span>";
const InactiveStatus = "<span class='offline-status'><i class='fa fa-circle-thin' aria-hidden='true'></i> Inactive</span>";

$(function () {
    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/exchange-hub")
        .withAutomaticReconnect([0, 5000, 10000, 30000])
        .build();

    connection.on("ReceiveFromClient", (result) => {
        setTimeout(function () {
            if (result.errorMessage) {
                ShowErrorMessage(result.errorMessage);
                return;
            }

            switch (result.sw) {
                case SwitchTool.TurnOn:
                    var rowSelector = $(`#id_${result.toolId}`).parent().parent();
                    var data = dataTable.row(rowSelector).data();
                    data.toolStatus = ToolStatus.Active;
                    data.processState = ProcessState.Started;
                    dataTable.row(rowSelector).data(data);

                    abp.notify.info(l('TurnOnToolSuccess'));
                    return;
                case SwitchTool.TurnOff:
                    var rowSelector = $(`#id_${result.toolId}`).parent().parent();
                    var data = dataTable.row(rowSelector).data();
                    data.toolStatus = ToolStatus.Inactive;
                    data.processState = ProcessState.Killed;
                    dataTable.row(rowSelector).data(data);

                    abp.notify.info(l('TurnOffToolSuccess'));
                    return;
                default:
                    return;
            }
        }, 1000);
    });

    connection.on("ReceiveFromTool", (tool) => {
        var rowSelector = $(`#id_${tool.id}`).parent().parent();
        var data = dataTable.row(rowSelector).data();
        data.name = tool.name;
        data.appId = tool.appId;
        data.version = tool.version;
        data.toolStatus = tool.toolStatus;
        data.lastUpdate = tool.lastUpdate;
        data.processState = tool.processState;
        data.processId = tool.processId;
        data.exeFilePath = tool.exeFilePath;
        dataTable.row(rowSelector).data(data);
        console.log(tool);
    });

    connection.on("ReloadToolTable", () => {
        setTimeout(function () {
            dataTable.ajax.reload();
        }, 1000);
    });

    connection.start().then(function () {
        console.log("SignalR Started.");
    }).catch(function (err) {
        return console.error(err.toString());
    });

    var l = abp.localization.getResource('ToolManager');

    devmoba.datatables.enableIndividualColumnSearch("#toolTable", [
        { name: "id" },
        { name: "name" },
        { name: "appId" },
        { name: "version" },
        { name: "clientId", options: allClientMachines },
        { name: "toolStatus", options: allToolStatus },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true },
        { searchDisabled: true }
    ]);

    var datatableConfig = abp.libs.datatables.normalizeConfiguration({
        processing: false,
        serverSide: true,
        paging: true,
        lengthMenu: [15, 25, 50, 100],
        searching: true,
        autoWidth: false,
        scrollCollapse: true,
        orderCellsTop: true,
        order: [[0, "asc"]],
        initComplete: () => {
            $('select.search_c_4').chosen({ disable_search_threshold: 5, search_contains: true });
            $('select.search_c_5').chosen({ disable_search_threshold: 5, search_contains: true });
        },
        ajax: abp.libs.datatables.createAjax(devmoba.toolManager.controllers.tool.getList, () => {
            return devmoba.datatables.searchHelper.getSearchConditions();
        }),
        columnDefs: [
            {
                targets: [0],
                render: function (data, type, row, meta) {
                    return `<span id='id_${row.id}'>${data}</span>`;
                }
            },
            {
                targets: [1],
                render: function (data, type, row, meta) {
                    return data;
                }
            },
            {
                targets: [2],
                render: function (data, type, row, meta) {
                    return data;
                }
            },
            {
                targets: [3],
                render: function (data, type, row, meta) {
                    return data;
                }
            },
            {
                orderable: false,
                targets: [4],
                render: function (data, type, row, meta) {
                    return data;
                }
            },
            {
                orderable: false,
                targets: [5],
                render: function (data, type, row, meta) {
                    if (data == ToolStatus.Active)
                        return ActiveStatus;
                    if (data == ToolStatus.Inactive)
                        return InactiveStatus;
                }
            },
            {
                targets: [6],
                render: function (data, type, row, meta) {
                    if (data && type === 'display') {
                        let m = moment.utc(data);
                        data = `<span title="${m.local().format('YYYY/MM/DD HH:mm:ss')}">${m.fromNow()}</span>`;
                    }
                    return data;
                }
            },
            {
                orderable: false,
                targets: [7],
                render: function (data, type, row, meta) {
                    switch (data) {
                        case ProcessState.Started:
                            return StartedState;
                        case ProcessState.Killed:
                            return KilledStatus;
                        default:
                            return NAStatus;
                    }
                }
            },
            //{
            //    orderable: false,
            //    targets: [7],
            //    render: function (data, type, row, meta) {
            //        return data;
            //    }
            //},
            {
                orderable: false,
                targets: [8],
                render: function (data, type, row, meta) {
                    return data;
                }
            },
            {
                targets: [9],
                render: function (data, type, row, meta) {
                    var html = ``;
                    if (row.processId > 0 && row.toolStatus == ToolStatus.Inactive && abp.auth.isGranted('ToolGroup.TurnOn')) {
                        html += `<button class='btnTurnOn btn-sm btn-outline-success' data-id='${row.id}' data-username='${row.username}'><i class='fas fa-power-off'></i> On</button>`;
                    }
                    if (row.exeFilePath && row.toolStatus == ToolStatus.Active && abp.auth.isGranted('ToolGroup.TurnOff')) {
                        html += `<button class='btnTurnOff btn-sm btn-outline-danger'  data-id='${row.id}' data-username='${row.username}'><i class='fas fa-power-off'></i> Off</button>`;
                    }
                    html += `<button class='btnDelete btn-sm btn-outline-dark'  data-id='${row.id}'><i class='fas fa-trash'></i> Delete</button>`;
                    return html;
                }
            },
        ],
        columns: [
            { data: "id", width: "60px", class: "content-cell" },
            { data: "name", width: "150px", class: "content-cell" },
            { data: "appId", width: "270px", class: "content-cell" },
            { data: "version", width: "80px", class: "content-cell" },
            { data: "username", width: "130px", class: "content-cell" },
            { data: "toolStatus", width: "100px", class: "content-cell" },
            { data: "lastUpdate", width: "120px", class: "content-cell" },
            { data: "processState", width: "120px", class: "content-cell" },
            //{ data: "processId", width: "80px", class: "content-cell" },
            { data: "exeFilePath", width: "400px", class: "scroll-cell" },
            { data: null, width: "130px", class: "btn-command" }
        ]
    });

    var dataTable = $('#toolTable').DataTable(devmoba.datatables.fixDomConfiguration(datatableConfig));

    $(document).on('click', '.btnTurnOn', function () {
        var id = $(this).data('id');
        var username = $(this).data('username');
        connection.invoke("TurnOnTool", id, username)
            .then(function () { }).catch(function (err) {
                abp.notify.warn(l('ClientOffline'));
                return console.error(err.toString());
            });
    });


    $(document).on('click', '.btnTurnOff', function () {
        var id = $(this).data('id');
        var username = $(this).data('username');
        connection.invoke("TurnOffTool", id, username)
            .then(function () {
            }).catch(function (err) {
                abp.notify.warn(l('ClientOffline'));
                return console.error(err.toString());
            });
    });

    $(document).on('click', '.btnDelete', function () {
        abp.message.confirm(l('DeleteConfirm')).then((confirmed) => {
            if (confirmed) {
                var id = $(this).data('id');
                devmoba.toolManager.controllers.tool.delete(id).then(() => {
                    abp.notify.info(l('SuccessfullyDeleted'));
                    dataTable.ajax.reload();
                });
            }
        });
    });

    function ShowErrorMessage(message) {
        var html = `<div class="alert alert-warning alert - dismissible">`
            + `<a href="#"class="close" data-dismiss="alert" aria - label="close">&times;</a>`
            + `<strong>${message}!</strong>`
            + `</div >`;
        $("#message-error").html(html);
    };
});