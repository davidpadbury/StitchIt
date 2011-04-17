(function () {

    var stitchIt = this.stitchIt = {};

    var modules = {
        **MODULES**
    };

    function require(identifier) {
        var module = {
            id: identifier,
            exports: {}
        };

        var moduleFn = modules[identifier];

        moduleFn(require, module.exports, module);

        return module.exports;
    }

    stitchIt.require = require;

}).call(this);