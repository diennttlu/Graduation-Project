$(function () {
    var l = abp.localization.getResource('ToolManager');

    devmoba.datatables.enableIndividualColumnSearch("#scriptTable", [
        { name: "id" },
        { name: "name" },
        { searchDisabled: true },
        { searchDisabled: true }
    ]);

    var datatableConfig = abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        lengthMenu: [15, 25, 50, 100],
        searching: true,
        autoWidth: false,
        scrollCollapse: true,
        orderCellsTop: true,
        order: [[0, "asc"]],
        
        ajax: abp.libs.datatables.createAjax(devmoba.toolManager.controllers.script.getList, () => {
            return devmoba.datatables.searchHelper.getSearchConditions();
        }),
        columnDefs: [
            {
                targets: [0],
            },
            {
                targets: [1],
            },
            {
                orderable: false,
                targets: [2],
                render: function (data, type, row, meta) {
                    if (row.dependencies) {
                        var result = ``;
                        row.dependencies.forEach(function (value, index) {
                            if (value.scriptDependency) {
                                result += `${value.scriptDependency.id} - ${value.scriptDependency.name}; `;
                            }
                        });
                        return result;
                    }
                    return "";
                }
            },
            {
                targets: [3],
                rowAction: {
                    items:
                        [
                            {
                                text: l(`Edit`),
                                visible: function (data) {
                                    return abp.auth.isGranted('ScriptGroup.Edit');
                                },
                                action: function (data) {
                                    newTab(`/Scripts/Edit?id=${data.record.id}`, "_self");
                                }
                            },
                            {
                                text: l('Delete'),
                                visible: function (data) {
                                    return abp.auth.isGranted('ScriptGroup.Delete');
                                },
                                confirmMessage: data => l('DeleteConfirm'),
                                action: function (data) {
                                    devmoba.toolManager.controllers.script.delete(data.record.id).then(() => {
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
            { data: "name", width: "300px", class: "content-cell" },
            { data: "dependencies", width: "900px", class: "content-cell" },
            { data: null, width: "100px" },
        ]
    });

    var dataTable = $('#scriptTable').DataTable(devmoba.datatables.fixDomConfiguration(datatableConfig));

    function newTab(url, target) {
        window.open(url, target);
    }
});