window.BC = (function () {

    const BC = {};

    BC.app = {};
    BC.sessions = {};
    BC.entities = {};
    BC.components = {};
    BC.scriptInitializers = [];

    // Called by BlazeCanvasComponent
    BC.createSession = (sessionId, csRef) => {
        console.log("createSession", sessionId);

        const session = new BC.Session(sessionId, csRef);
        BC.sessions[sessionId] = session;
    };

    // Called by BlazeCanvasComponent
    BC.createEntity = (sessionId, entityId, parentId) => {
        console.log("createEntity", sessionId, entityId, parentId);

        const session = window.BC.sessions[sessionId];
        const entity = new pc.Entity({ name: entityId });

        entity._session = session;

        if (parentId) {
            const parent = BC.entities[parentId];
            parent.addChild(entity);
        }
        else {
            session.app.root.addChild(entity);
        }

        BC.entities[entityId] = entity;
    };

    // Called by BlazeCanvasComponent
    BC.createComponent = (entityId, componentId, componentType, args) => {
        console.log("createComponent", entityId, componentId, componentType, args);

        const entity = window.BC.entities[entityId];

        args = BC.instanceArgsObjectsRecursive(entity._session, args);

        const component = entity.addComponent(componentType, args)
        BC.components[componentId] = component;

        if (component == null) {
            console.error("createComponent failed:", scriptComponentType);
        }

        component._entity = entity;
        component._session = entity._session;

        if (componentType == "camera") {
            entity._session.cameras.active = component;
        }
    };

    // Called by BlazeCanvasComponent
    BC.createScriptComponent = (entityId, componentId, scriptComponentType, args) => {
        console.log("createScriptComponent", entityId, componentId, scriptComponentType, args);

        const entity = window.BC.entities[entityId];

        args = BC.instanceArgsObjectsRecursive(entity._session, args);

        if (!entity._scriptIsInit) {
            entity.addComponent('script');
            entity._scriptIsInit = true;
        }
        entity.script.create(scriptComponentType);

        const component = entity.script[scriptComponentType]
        BC.components[componentId] = component;

        if (component == null) {
            console.error("createScriptComponent failed:", scriptComponentType);
        }

        Object.keys(args).forEach((key) => {
            let val = args[key];
            component[key] = val;
        });

        component._entity = entity;
        component._session = entity._session;

        component.initialize(); // This isn't called automatically?
    };

    // Called by BlazeCanvasComponent
    BC.loadAssetAsync = async (sessionId, assetId, assetType, file) => {
        console.log("loadAssetAsync", sessionId, assetId, assetType, file);

        const session = window.BC.sessions[sessionId];

        file = BC.instanceArgsObjectsRecursive(session, file);

        let errorResult = null;
        let loadedAsset = null;
        try {
            let loadPromise = new Promise((resolve, reject) => {
                var asset = new pc.Asset(assetId, assetType, file);
                session.app.assets.add(asset);
                asset.ready((a) => {
                    resolve(asset);
                });
                session.app.assets.load(asset);
            });
            loadedAsset = await loadPromise;

            console.log("loadAssetAsync success:", loadedAsset, sessionId, assetId, assetType, file);
        }
        catch (error) {
            errorResult = JSON.stringify(error);
            console.log("loadAssetAsync error:", error, loadedAsset, sessionId, assetId, assetType, file);
        }

        return errorResult;
    };

    // Called by BlazeCanvasComponent
    BC.invokeSessionMethod = (sessionId, method) => window.BC.sessions[sessionId][method]();
    BC.invokeSessionMethod1 = (sessionId, method, arg0) => window.BC.sessions[sessionId][method](arg0);
    BC.invokeSessionMethod2 = (sessionId, method, arg0, arg1) => window.BC.sessions[sessionId][method](arg0, arg1);
    BC.invokeSessionMethod3 = (sessionId, method, arg0, arg1, arg2) => window.BC.sessions[sessionId][method](arg0, arg1, arg2);

    // Called by BlazeCanvasComponent
    BC.invokeEntityMethod = (entityId, method) => window.BC.entities[entityId][method]();
    BC.invokeEntityMethod1 = (entityId, method, arg0) => window.BC.entities[entityId][method](arg0);
    BC.invokeEntityMethod2 = (entityId, method, arg0, arg1) => window.BC.entities[entityId][method](arg0, arg1);
    BC.invokeEntityMethod3 = (entityId, method, arg0, arg1, arg2) => window.BC.entities[entityId][method](arg0, arg1, arg2);

    // Called by BlazeCanvasComponent
    BC.invokeComponentMethod = (entityId, method) => window.BC.components[entityId][method]();
    BC.invokeComponentMethod1 = (entityId, method, arg0) => window.BC.components[entityId][method](arg0);
    BC.invokeComponentMethod2 = (entityId, method, arg0, arg1) => window.BC.components[entityId][method](arg0, arg1);
    BC.invokeComponentMethod3 = (entityId, method, arg0, arg1, arg2) => window.BC.components[entityId][method](arg0, arg1, arg2);

    BC.allowedMethods = [
        "app.assets.find"
    ];

    BC.allowedConstructors = [
        "pc.Color"
    ];

    BC.allowedMethodsConstructorsCheck = false;

    BC.instanceArgsObjectsRecursive = function (session, args, key) {

        const argsIsObj = (typeof args === 'object' && args !== null && !Array.isArray(args));

        if (!argsIsObj) {
            return args;
        }

        if (args["_asset"]) {
            var assetName = args["_asset"];
            var asset = session.app.assets.find(assetName);

            if (asset.resource && asset.resource.model) {
                asset = asset.resource.model;
            }

            return asset;
        }

        // Call _method args
        if (args["_method"]) {
            if (!BC.allowedMethodsConstructorsCheck || BC.allowedMethods.includes(args["_method"])) {

                // parse types
                let method = BC.navigateToBulletSeperatedChildObject(session, args["_method"]);

                // construct using args
                var typeArgs = args["_args"];
                if (typeArgs) {
                    const instance = method(...typeArgs);
                    return instance;
                } else {
                    const instance = method();
                    return instance;
                }
            } else {
                console.error("_method not allowed:", args["_method"]);
            }
        }

        // Instantiate _type args
        if (args["_type"]) {
            if (!BC.allowedMethodsConstructorsCheck || BC.allowedTypes.includes(args["_type"])) {
                // parse types
                let typeConstructor = BC.navigateToBulletSeperatedChildObject(session, args["_type"]);

                // construct using args
                var typeArgs = args["_args"];
                if (typeArgs) {
                    const instance = new typeConstructor(...typeArgs);
                    return instance;
                } else {
                    const instance = new typeConstructor();
                    return instance;
                }
            } else {
                console.error("_type not allowed:", args["_type"]);
            }
        }

        // Check children
        const argsFixed = {};
        Object.keys(args).forEach((key) => {
            let val = args[key];
            const valIsObj = (typeof val === 'object' && val !== null && !Array.isArray(val));

            if (valIsObj) {
                val = BC.instanceArgsObjectsRecursive(session, val, key);
            }

            argsFixed[key] = val;
        });

        return argsFixed;
    }

    BC.Session = class {
        constructor(sessionId, csRef) {
            this.sessionId = sessionId;
            this.csRef = csRef;
            this.canvas = null;
            this.app = null;
            this.cameras = { active: null };
        }

        helloworld() {
            alert("hello world");
        }

        start(canvasId) {
            const canvas = document.getElementById(canvasId);
            const app = new pc.Application(canvas, {
                mouse: new pc.Mouse(canvas),
                touch: new pc.TouchDevice(canvas),
                keyboard: new pc.Keyboard(window),
            });
            this.canvas = canvas;
            this.app = app;

            app.setCanvasFillMode(pc.FILLMODE_FILL_WINDOW);
            app.setCanvasResolution(pc.RESOLUTION_AUTO);
            app.start();

            app.scene.ambientLight = new pc.Color(0.5, 0.5, 0.5);

            app.setCanvasFillMode(pc.FILLMODE_FILL_WINDOW);
            app.setCanvasResolution(pc.RESOLUTION_AUTO);
            app.start();

            window.addEventListener("resize", this.onResize.bind(this));

            for (const scriptInitializer of BC.scriptInitializers) {
                scriptInitializer();
            }
        }

        onResize() {
            this.app.resizeCanvas();
        }

        startRotating(entityId) {
            const cube = BC.entities[entityId];

            cube._session.app.on('update', function (dt) {
                cube.rotate(10 * dt, 20 * dt, 30 * dt);
            });
        }

        invokeCSMethod(methodName) {
            this.csRef.invokeMethodAsync(methodName);
        }
    }

    BC.navigateToBulletSeperatedChildObject = function (session, fullPath) {
        var parts = fullPath.split(".");
        let childObj = null;
        for (const part of parts) {
            if (childObj == null) {
                // Root object
                if (part == "app") {
                    childObj = session.app;
                }
                else if (part == "pc") {
                    childObj = window.pc;
                } else {
                    console.error("navigateToBulletSeperatedChildObject root obj not found:", fullPath);
                }
            }
            else {
                // Navigate down
                childObj = childObj[part];
            }
        }
        if (childObj == null) {
            console.error("childObj not found:", fullPath);
        }

        return childObj;
    }

    return BC;

})(); 