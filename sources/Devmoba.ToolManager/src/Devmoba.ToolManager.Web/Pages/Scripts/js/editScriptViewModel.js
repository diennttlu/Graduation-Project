function DependencyChosen(scriptId, scriptDependencyId, checked, inDb) {
    this.scriptId = scriptId;
    this.scriptDependencyId = scriptDependencyId;
    this.checked = checked;
    this.inDb = inDb;
}


function EditScriptViewModel(script, allScripts, hostTypes) {
    var self = this;
    self.script = ko.observable(script);
    self.scriptSelections = ko.observableArray(allScripts);
    self.hostTypes = ko.observableArray(hostTypes);
    self.dependencyChosens = CreateDependencyChosens(allScripts);

    self.checkedScriptEvent = function (item) {
        var index = self.dependencyChosens.findIndex(c => c.scriptDependencyId === item.id);
        if (item.checked) {
            if (index > -1) {
                self.dependencyChosens[index].checked = true;
            }
            else {
                self.dependencyChosens.push(new DependencyChosen(id, item.id, true, false))
            }
        } else {
            if (!self.dependencyChosens[index].inDb) {
                self.dependencyChosens.splice(index, 1);
            } else {
                self.dependencyChosens[index].checked = false;
            }
        }
        //if (item.checked) {
        //    console.log(item.checked);
        //    devmoba.toolManager.controllers.dependency.create({ scriptId: id, scriptDependencyId: item.id }).done((dependency) => { console.log(dependency) });
        //} else {
        //    devmoba.toolManager.controllers.dependency.deleteAsyncByScriptIdAndScriptDependencyId(id, item.id).done((dependency) => { console.log(dependency) });
        //    console.log(item.checked);
        //}
    }
}


function CreateDependencyChosens(allScripts) {
    var checkedScripts = allScripts.filter(script => script.checked == true);
    var dependencyChosens = [];
    checkedScripts.forEach(item => {
        dependencyChosens.push(new DependencyChosen(id, item.id, true, true));
    });
    return dependencyChosens;
}