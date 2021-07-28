$(function () {
    ko.applyBindings(new CreateScriptViewModel(allScripts, HostTypes));

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

    editor.setValue(`function main() {
	// code
}`);

    $("#btn-create-submit").click(function (e) {
        e.preventDefault();
        var name = $("input[name=Name]").val();
        var content = editor.getValue();
        var dependencies = getCheckboxValues("script");
        devmoba.toolManager.controllers.script.create({ name: name, content: content, dependencyIds: dependencies }).done((result) => {
            console.log(result);
            window.location.replace("/Scripts");
        });
    })

    $("#btn-clear-content").click(function (e) {
        e.preventDefault();
        editor.setValue("");
    })
});