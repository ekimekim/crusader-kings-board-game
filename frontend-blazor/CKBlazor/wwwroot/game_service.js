let game_service = {};
window.game_service = game_service;
game_service._ref = null;

game_service.test = function () {
    console.log("game_service test");
};

game_service.init = function (ref) {
    game_service._ref = ref;
    console.log("game_service _ref set", ref);
}

game_service.start = function () {
    game_service.animate();
};

game_service.animate = function () {
    requestAnimationFrame(game_service.animate);

    game_service._ref.invokeMethod("Update");
};


console.log("game_service loaded");
