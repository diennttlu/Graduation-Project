const TypeBody = {
    Function: "FunctionDeclaration",
    Statement: "ExpressionStatement"
}

function FunctionDeclaration(name, columnStart, lineStart, columnEnd, lineEnd) {
    this.Name = name;
    this.ColumnStart = columnStart;
    this.LineStart = lineStart;
    this.ColumnEnd = columnEnd;
    this.LineEnd = lineEnd;
}

function getBodyProgram(script) {
    try {
        var program = esprima.parseScript(script, { loc: true });
        var body = program.body;
        return body;
    } catch (e) {
        return [];
    }
}

function getFunctionInfo(script) {
    var body = getBodyProgram(script);
    var func = [];
    body.forEach((item, index) => {
        if (item.type == TypeBody.Function) {
            func.push(new FunctionDeclaration(
                item.id.name,
                item.loc.start.column,
                item.loc.start.line,
                item.loc.end.column,
                item.loc.end.line,
            ));
        }
    });

    return JSON.stringify(func);
}

function execute(value) {
    return value;
}