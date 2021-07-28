const ClientStatus = {
    Offline: 0,
    Online: 1
}

const OnlineStatus = "<span class='online-status'><i class='fa fa-circle' aria-hidden='true'></i> Online</span>";
const OfflineStatus = "<span class='offline-status'><i class='fa fa-circle-thin' aria-hidden='true'></i> Offline</span>";

$(function () {
    //abp.log.debug('Index.js initialized!');

    var l = abp.localization.getResource('ToolManager');
    devmoba.datatables.enableIndividualColumnSearch("#dashboardTable", [
        { name: "clientName" },
        { name: "ipLan" },
        { name: "ipPublic" },
        { name: "clientStatus", options: allClientStatus },
        { name: "activeNumber" },
        { name: "inactiveNumber" },
        { name: "total" }
    ]);

    var datatableConfig = abp.libs.datatables.normalizeConfiguration({
        processing: false,
        serverSide: false,
        paging: true,
        lengthMenu: [15, 25, 50, 100],
        searching: true,
        autoWidth: false,
        scrollCollapse: true,
        orderCellsTop: true,
        order: [[0, "asc"]],
        initComplete: () => {
            $('select.search_c_3').chosen({ disable_search_threshold: 5, search_contains: true });
        },
        ajax: abp.libs.datatables.createAjax(devmoba.toolManager.controllers.clientMachine.getClientMachineOverview, () => {
            return devmoba.datatables.searchHelper.getSearchConditions();
        }),
        columnDefs: [
            {
                targets: [0]
            },
            {
                orderable: false,
                targets: [1],
            },
            {
                orderable: false,
                targets: [2]
            },
            {
                orderable: false,
                targets: [3],
                render: function (data, type, row, meta) {
                    if (data == ClientStatus.Online)
                        return `<span id='clientStatus_${row.id}'>${OnlineStatus}</span>`;
                    if (data == ClientStatus.Offline)
                        return `<span id='clientStatus_${row.id}'>${OfflineStatus}</span>`;
                }
            },
            {
                targets: [4]
            },
            {
                targets: [5]
            },
            {
                targets: [6],
                render: function (data, type, row, meta) {
                    var toolReport = row.toolReport;
                    return toolReport.activeNumber + toolReport.inactiveNumber;
                }
            },
           
        ],
        columns: [
            { data: "clientName", width: "100px" },
            { data: "ipLan", width: "300px" },
            { data: "ipPublic", width: "300px" },
            { data: "clientStatus", width: "150px" },
            { data: "toolReport.activeNumber", width: "100px" },
            { data: "toolReport.inactiveNumber", width: "100px" },
            { data: null, width: "100px" }
        ]
    });

    var dataTable = $('#dashboardTable').DataTable(devmoba.datatables.fixDomConfiguration(datatableConfig));
});
