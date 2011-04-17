var a = require('submodule/a');
var b = require('submodule/b');

exports.sameFunction = a.foo === b.foo;