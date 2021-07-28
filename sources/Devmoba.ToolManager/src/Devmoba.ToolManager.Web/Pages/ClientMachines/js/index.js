const ClientStatus = {
    Offline: 0,
    Online: 1
}

const OnlineStatus = "<span class='online-status'><i class='fa fa-circle' aria-hidden='true'></i> Online</span>";
const OfflineStatus = "<span class='offline-status'><i class='fa fa-circle-thin' aria-hidden='true'></i> Offline</span>";

$(function () {
    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/exchange-hub")
        .withAutomaticReconnect([0, 5000, 10000, 30000])
        .build();

    connection.on("ReceiveClientInfo", (result) => {
        if (result.connectionStatus) {
            let m = moment.utc(result.lastUpdate);
            $(`#ipLan_${result.clientId}`).text(`${result.ipLan}`);
            $(`#ipPublic_${result.clientId}`).text(`${result.ipPublic}`);
            $(`#cpu_${result.clientId}`).text(`${result.cpuPercentage} %`);
            $(`#memory_${result.clientId}`).text(`${result.memoryPercentage} %`);
            $(`#clientStatus_${result.clientId}`).html(OnlineStatus);
            $(`#lastUpdate_${result.clientId}`).html(`<span title="${m.local().format('YYYY/MM/DD HH:mm:ss')}">${m.fromNow()}</a></span>`);
        } else {
            $(`#clientStatus_${result.clientId}`).html(OfflineStatus);
            $(`#cpu_${result.clientId}`).text(`0 %`);
            $(`#memory_${result.clientId}`).text(`0 %`);
        }
        
    });

    connection.on("ReloadClientTable", () => {
        dataTable.ajax.reload();
    });

    connection.on("ReceiveError", (result) => {
        var html = `<div class="alert alert-warning alert - dismissible">`
            + `<a href="#"class="close" data-dismiss="alert" aria - label="close">&times;</a>`
            + `<strong>${result.message}!</strong>`
            + `</div >`;
        $("#message-error").html(html);
    });

    connection.start().then(function () {
        console.log("SignalR Started.");
    }).catch(function (err) {
        return console.error(err.toString());
    });

    var l = abp.localization.getResource('ToolManager');
    devmoba.datatables.enableIndividualColumnSearch("#clientMachineTable", [
        { name: "id" },
        { name: "username" },
        { name: "ipLan" },
        { name: "ipPublic" },
        { name: "clientStatus", options: allClientStatus },
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
        },
        ajax: abp.libs.datatables.createAjax(devmoba.toolManager.controllers.clientMachine.getList, () => {
            return devmoba.datatables.searchHelper.getSearchConditions();
        }),
        columnDefs: [
            {
                targets: [0],
            },
            {
                orderable: false,
                targets: [1],
            },
            {
                targets: [2],
                render: function (data, type, row, meta) {
                    return `<span id='ipLan_${row.id}'>${data}</span>`;
                }
            },
            {
                targets: [3],
                render: function (data, type, row, meta) {
                    return `<span id='ipPublic_${row.id}'>${data}</span>`;
                }
            },
            {
                orderable: false,
                targets: [4],
                render: function (data, type, row, meta) {
                    if (data == ClientStatus.Online)
                        return `<span id='clientStatus_${row.id}'>${OnlineStatus}</span>`;
                    if (data == ClientStatus.Offline)
                        return `<span id='clientStatus_${row.id}'>${OfflineStatus}</span>`;
                }
            },
            {
                targets: [5],
                render: function (data, type, row, meta) {
                    if (data && type === 'display') {
                        let m = moment.utc(data);
                        data = `<span id="lastUpdate_${row.id}"><span title="${m.local().format('YYYY/MM/DD HH:mm:ss')}">${m.fromNow()}</span></span>`;
                    }
                    return data;
                }
            },
            {
                orderable: false,
                targets: [6],
                render: function (data, type, row, meta) {
                    return `<strong id="cpu_${row.id}">0 %</strong>`;
                }
            },
            {
                orderable: false,
                targets: [7],
                render: function (data, type, row, meta) {
                    return `<strong id="memory_${row.id}">0 %</strong>`;
                }
            },
            {
                targets: [8],
                rowAction: {
                    items:
                        [
                            {
                                text: l('Delete'),
                                visible: function (data) {
                                    return abp.auth.isGranted('ClientMachineGroup.Delete');
                                },
                                confirmMessage: data => l('DeleteConfirm'),
                                action: function (data) {
                                    devmoba.toolManager.controllers.clientMachine.delete(data.record.id).then(() => {
                                        abp.notify.info(l('SuccessfullyDeleted'));
                                        dataTable.ajax.reload();
                                    });
                                }
                            }
                        ]
                }
            },
        ],
        columns: [
            { data: "id", width: "100px", class: "content-cell" },
            { data: "username", width: "200px", class: "content-cell" },
            { data: "ipLan", width: "300px", class: "content-cell" },
            { data: "ipPublic", width: "300px", class: "content-cell" },
            { data: "clientStatus", width: "150px", class: "content-cell" },
            { data: "lastUpdate", width: "200px", class: "content-cell" },
            { data: "cpuPercentage", width: "100px", class: "content-cell" },
            { data: "memoryPercentage", width: "100px", class: "content-cell" },
            { data: null, width: "100px" },
        ]
    });

    var dataTable = $('#clientMachineTable').DataTable(devmoba.datatables.fixDomConfiguration(datatableConfig));
});

