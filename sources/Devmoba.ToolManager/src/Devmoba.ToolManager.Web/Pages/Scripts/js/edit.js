$(function () {
    var viewModel = new EditScriptViewModel(script, allScripts, HostTypes);
    ko.applyBindings(viewModel);
    var l = abp.localization.getResource('ToolManager');
    var editor = CodeMirror.fromTextArea(content, {
        lineNumbers: true,
        styleActiveLine: true,
        matchBrackets: true,
        theme: "monokai",
        extraKeys: {
            "Ctrl-Space": "autocomplete"
        },
        mode: {
            name: "javascript",
            globalVars: true
        }
    });
    editor.setValue(script.content);

    $("#btn-edit-submit").click(function (e) {
        e.preventDefault();
        var name = $("input[name=Name]").val();
        var content = editor.getValue();

        devmoba.toolManager
            .controllers
            .script
            .update(id, { name: name, content: content, comment: null, dependencyIds: null, dependencyChosens: viewModel.dependencyChosens })
            .done((result) => {
            console.log(result);
            if (result.id) {
                window.open("/Scripts/Index", "_self");
            }
        });
    })

    $("#btn-clear-content").click(function (e) {
        e.preventDefault();
        editor.setValue("");
    })
});