const EnableClient = 5;

function DisableUncheckedCkb(name) {
    $.each($(`input[name='${name}']:not(:checked)`), function () {
        $(this).attr("disabled", true);
    });
}

function EnableUncheckedCkb(name) {
    $.each($(`input[name='${name}']:not(:checked)`), function () {
        $(this).removeAttr("disabled");
    });
}

function TabClient(username) {
    this.username = username;
    this.replacePoint = username.replace(/\./g, "_").toLowerCase();
}

function ExchangeClientViewModel(clients, scripts) {
    var self = this;
    self.clients = ko.observableArray(clients);
    self.scriptSelections = ko.observableArray(scripts);
    self.tabClients = ko.observableArray([]);
    self.isActiveTab = ko.observable();

    self.checkedClientEvent = function (client) {
        var checkboxs = getCheckboxValues("client");
        if (checkboxs.length >= EnableClient) {
            DisableUncheckedCkb("client");
        } else {
            EnableUncheckedCkb("client");
        }
        if (client.hasChecked) {
            self.tabClients.push(new TabClient(client.username.toString()));
        } else {
            self.tabClients.remove(function (item) {
                return item.username == client.username;
            })
        }
    }
}
