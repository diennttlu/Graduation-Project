$(function () {
    ko.applyBindings(new ExchangeClientViewModel(clientUserNames, allScripts));
    var l = abp.localization.getResource('ToolManager');
    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/exchange-hub")
        .withAutomaticReconnect([0, 5000, 10000, 30000])
        .build();

    connection.on("ReceiveFromClient", (result) => {
        var replacePoint = result.client.replace(/\./g, "_").toLowerCase();

        $.each(result.message, function (index, value) {
            $(`#result_${replacePoint}`)
                .append(`<div class="col-md-3" style="padding-right: 0px;">${moment.utc().local().format('YYYY/MM/DD HH:mm:ss')}</div>`)
                .append(`<div class="col-md-9" style="padding-left: 0px;">${value}</div>`);
        });
        $(".btn-play").prop("disabled", false);
    });

    connection.start().then(function () {
        console.log("SignalR Started.");
    }).catch(function (err) {
        return console.error(err.toString());
    });


    var content = document.getElementById("content");
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
    $("#result").attr("disabled", "disabled");

    $(".btn-run-script").click(function (e) {
        e.preventDefault();
        var elmParent = $(this).parent();
        var input = elmParent.find("input");
        var clientValues = getCheckboxValues("client");
        if (clientValues.length > 0) {
            $(".btn-play").prop("disabled", true);
            var message = {
                ScriptId: input.val(),
                Clients: clientValues
            }
            connection.invoke("SendClient", JSON.stringify(message)).then(function () {
            }).catch(function (err) {
                return console.error(err.toString());
            });
        } else {
            abp.notify.warn(l('RequiredSelectClient'));
        }
    });

    $("#btn-send").click(function (e) {
        e.preventDefault();
        var clientValues = getCheckboxValues("client");
        var scriptValues = getCheckboxValues("script");
       
        if (clientValues.length > 0) {
            var content = editor.getValue();
            if (content) {
                $(".btn-play").prop("disabled", true);
                var message = {
                    Clients: clientValues,
                    Dependencies: scriptValues,
                    Content: content
                };
                connection.invoke("SendClient", JSON.stringify(message)).then(function () {
                }).catch(function (err) {
                    return console.error(err.toString());
                });
            } else {
                abp.notify.warn(l('RequiredInputContent'));
            }       
        } else {
            abp.notify.warn(l('RequiredSelectClient'));
        }
    });

    $("#btn-clear").click(function (e) {
        e.preventDefault();
        editor.setValue("");
    })

    $("#btn-clear-console").click(function (e) {
        e.preventDefault();
        //alert(1);
        $("#tab-content .active .console-content").empty();
    })

});