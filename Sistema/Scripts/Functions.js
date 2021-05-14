window.DEFAULT_PAGESIZE = 10;
window.shared_function = [];
window.DEFAULT_GRID_PAGESIZE = 20;


String.prototype.toDateFromJson = function (format) {
    if (this) {
        if (/\/Date\(-?[\d]+\)\//.test(this)) {
            var extract = parseInt(/-?[\d]+/.exec(this)[0]);
            return Date.parse(new Date(extract)).toString(format ? format : null, false);
        }
        else if (/\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}/.test(this)) {
            return Date.parse(this).toString(format ? format : null);
        }
        else if (/^(\d{1,2})\/(\d{1,2})\/(\d{4})$/.test(this)) {
            return Date.parse(this).toString(format ? format : null);
        }
        return this;
    }
    return null;
};