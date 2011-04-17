(function () {

    var stitchIt = this.stitchIt = {},
        sameDir = '.',
        parentDir = '..',
        moduleCache = {};

    var modules = {
        **MODULES**
    };

    function isRelativeComponent(component) {
        return component === sameDir || component == parentDir;
    }

    function normalizeIdentifier(identifier, context) {
        var newIdentifier = [],
            component, i, j;

        var identifierComponents = identifier.split('/'),
            contextComponents = context ? context.split('/') : [];

        for (i = 0; i < identifierComponents.length; i++) {
            component = identifierComponents[i];

            switch (component) {
                case sameDir:
                    if (i === 0) {
                        // Insert context at front
                        Array.prototype.unshift.apply(newIdentifier, contextComponents);
                    }
                    break;
                
                case parentDir:
                    newIdentifier.pop();
                    break;

                default:
                    newIdentifier.push(component);
                    break;
            }
        }

        return newIdentifier.join('/');
    }

    function extractContext(identifier) {
        return identifier.split('/').slice(0, -1).join('/');
    }

    function loadModule(identifier, context) {
        var moduleFn = modules[identifier];
   
        if (!moduleFn) {
            throw new Error('Module "' + identifier + '" is not defined.');
        }
        
        var moduleContext = extractContext(identifier),
            moduleRequire = createRequire(moduleContext),
            module = {
                id: identifier,
                exports: {}
            };

        moduleFn(moduleRequire, module.exports, module);

        return module.exports;
    }

    function createRequire(context) {
        return function require(identifier) {

            var id = normalizeIdentifier(identifier, context);
            
            return id in moduleCache ? moduleCache[id] : (moduleCache[id] = loadModule(id, context));
        };
    }

    stitchIt.require = createRequire();

}).call(this);