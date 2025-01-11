BC.scriptInitializers.push(() => {
    var TopDownCameraControlsComponent = pc.createScript('topDownCameraControlsComponent');

    TopDownCameraControlsComponent.prototype.initialize = function () {
        console.log("topDownCameraControlsComponent.initialize")
        this.direction = new pc.Vec3(0, -1, -1);
    }

    TopDownCameraControlsComponent.prototype.update = function (dt) {
        var movement = this.direction.clone().scale(0.2 * dt);
        this.entity.translate(movement);
    };
});
