BC.scriptInitializers.push(() => {
    var TopDownCameraControlsComponent = pc.createScript('topDownCameraControlsComponent');

    TopDownCameraControlsComponent.prototype.initialize = function () {
        console.log("topDownCameraControlsComponent.initialize")

        var mouse = this.app.mouse;
        mouse.on(pc.EVENT_MOUSEWHEEL, this.onMouseWheel, this)

        this.direction = new pc.Vec3(0, 1, 1);
        this.centerPos = new pc.Vec3(0, 0, 0);
        this.zoom = 10;
    }
    TopDownCameraControlsComponent.prototype.onMouseWheel = function (event) {
        this.zoom -= event.wheel;

        console.log(this.zoom);

        if (this.zoom > 40)
            this.zoom = 40;
        if (this.zoom < 2)
            this.zoom = 2;
    }

    TopDownCameraControlsComponent.prototype.update = function (dt) {

        let moveSpeed = 10;
        let inputDir = new pc.Vec3();
        let keyboard = this.app.keyboard;

        if (keyboard.isPressed(pc.KEY_W))
            inputDir.z -= 1;
        if (keyboard.isPressed(pc.KEY_S))
            inputDir.z += 1;
        if (keyboard.isPressed(pc.KEY_A))
            inputDir.x -= 1;
        if (keyboard.isPressed(pc.KEY_D))
            inputDir.x += 1;

        this.centerPos.add(inputDir.scale(moveSpeed * dt))

        const mapMaxSize = 10;
        if (this.centerPos.x > mapMaxSize)
            this.centerPos.x = mapMaxSize;
        if (this.centerPos.x < -1* mapMaxSize)
            this.centerPos.x = -1 * mapMaxSize;
        if (this.centerPos.z > mapMaxSize)
            this.centerPos.z = mapMaxSize;
        if (this.centerPos.z < -1 * mapMaxSize)
            this.centerPos.z = -1 * mapMaxSize;

        var dist = this.direction.clone().scale(this.zoom);
        var newPos = this.centerPos.clone().add(dist);

        this.entity.setPosition(newPos);
    };
});
