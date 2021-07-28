function getCheckboxValues(name) {
    var favorite = [];
    $.each($(`input[name='${name}']:checked`), function () {
        favorite.push($(this).val());
    });
    return favorite;
}