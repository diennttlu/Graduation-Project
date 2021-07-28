function CreateScriptViewModel(scripts, hostTypes) {
    var self = this;
    self.scriptSelections = ko.observableArray(scripts);
    self.hostTypes = ko.observableArray(hostTypes);
}