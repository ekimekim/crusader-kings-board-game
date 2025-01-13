BC.scriptInitializers.push(() => {
    var DivPinComponent = pc.createScript('divPinComponent');

    DivPinComponent.prototype.initialize = function () {
        this.pinId = null;
        this.div = null;
    }

    DivPinComponent.prototype.setPinId = function (pinId) {
        this.pinId = pinId;
    }
    
    DivPinComponent.prototype.update = function (dt) {
        if (this.pinId == null) {
            return;
        }

        if (this.div == null) {
            this.div = window.document.getElementById(this.pinId);
        }
        if (this.div == null) {
            console.error("divPinComponent div is null", this.pinId);
        }

        const worldPos = this.entity.getPosition();
        const screenPos = new pc.Vec3();

        this._session.cameras.active.worldToScreen(worldPos, screenPos);

        const style = `position:absolute; left:${screenPos.x}px; top:${screenPos.y}px;`;
        this.div.style = style;
    };
});
