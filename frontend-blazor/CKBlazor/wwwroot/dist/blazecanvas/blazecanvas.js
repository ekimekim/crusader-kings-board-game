window.BC = (function () {

    const BC = {};

    BC.app = {};
    BC.sessions = {};
    BC.entities = {};
    BC.components = {};

    // Called by BlazeCanvasComponent
    BC.createSession = (sessionId, csRef) => {
        const session = new BC.Session(sessionId, csRef);
        BC.sessions[sessionId] = session;

        console.log("started createSession", sessionId);
    };

    // Called by BlazeCanvasComponent
    BC.createEntity = (sessionId, entityId, parentId) => {
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


        console.log("started createEntity", entityId);
    };

    // Called by BlazeCanvasComponent
    BC.createComponent = (entityId, componentId, componentType, args) => {
        args = BC.instanceArgsObjectsRecursive(args);

        const entity = window.BC.entities[entityId];

        const component = entity.addComponent(componentType, args)
        BC.components[componentId] = component;

        component._entity = entity;
        component._session = entity._session;

        console.log("started createComponent", componentId);
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

    BC.instanceArgsObjectsArray = function (argsArray) {
        for (var i = 0; i < args.length; i++) {
            argsArray[i] = BC.instanceArgsObjectsRecursive(argsArray[i]);
        }
        return argsArray;
    }

    BC.instanceArgsObjectsRecursive = function (args) {

        const argsIsObj = (typeof args === 'object' && args !== null && !Array.isArray(args));

        if (!argsIsObj) {
            return args;
        }

        // Instance special type
        if (args["_type"]) {

            // parse types
            var typeParts = args["_type"].split(".");
            let typeConstructor = window;
            for (const part of typeParts) {
                typeConstructor = typeConstructor[part];
            }

            // construct using args
            var typeArgs = args["_args"];
            if (typeArgs) {
                const instance = new typeConstructor(...typeArgs);
                return instance;
            } else {
                const instance = new typeConstructor();
                return instance;
            }
        }

        // Check children
        const argsFixed = {};
        Object.keys(args).forEach((key) => {
            let val = args[key];
            const valIsObj = (typeof val === 'object' && val !== null && !Array.isArray(val));

            if (valIsObj) {
                val = BC.instanceArgsObjectsRecursive(val);
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
        }

        helloworld() {
            alert("hello world");
        }

        start(canvasId) {
            const canvas = document.getElementById(canvasId);
            const app = new pc.Application(canvas, {
                mouse: new pc.Mouse(canvas),
                touch: new pc.TouchDevice(canvas),
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

            const camera = new pc.Entity();
            camera.addComponent('camera', {
                clearColor: new pc.Color(0.1, 0.1, 0.1)
            });
            camera.translate(0, 0, 5);
            app.root.addChild(camera);
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

    return BC;

})(); 