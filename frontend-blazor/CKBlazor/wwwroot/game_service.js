import * as THREE from 'three';
import { OrbitControls } from 'three/addons/controls/OrbitControls.js';
import { FBXLoader } from 'three/addons/loaders/FBXLoader.js';
import { GLTFLoader } from 'three/addons/loaders/GLTFLoader.js';
import { RGBELoader } from 'three/addons/loaders/RGBELoader.js';


let game_service = {};
window.game_service = game_service;
game_service._ref = null;
game_service._renderer = null;
game_service._camera = null;
game_service._scene = null;

game_service._cube = null;

game_service._pendingRendererId;

game_service.test = function ()
{
    console.log("game_service test");
};

game_service.init = function (ref)
{
    try {
        game_service.init_internal(ref);
    }
    catch (error)
    {
        console.log("game_service.init error", error);
    }
};

game_service.init_internal = function (ref)
{
    console.log("game_service init started");
    game_service._ref = ref;

    game_service._camera = new THREE.PerspectiveCamera(60, window.innerWidth / window.innerHeight, 1, 20000);
    game_service._camera.position.set(0, 0, 5);
    game_service._camera.lookAt(new THREE.Vector3(0, 0, 0));

    game_service._scene = new THREE.Scene();
    game_service._scene.background = new THREE.Color(0x000000);
    game_service._scene.fog = new THREE.Fog(0xa0a0a0, 4, 40);

    //const grid = new THREE.GridHelper(2000, 20, 0x000000, 0x000000);
    //grid.material.opacity = 0.2;
    //grid.material.transparent = true;
    //grid.position.set(0, -100, 0);
    //game_service._scene.add(grid);

    // Create a cube
    const geometry = new THREE.BoxGeometry();
    const material = new THREE.MeshBasicMaterial({ color: 0x00ff00 });
    game_service._cube = new THREE.Mesh(geometry, material);

    // Add the cube to the scene
    game_service._scene.add(game_service._cube);


    window.addEventListener('resize', game_service.resize_renderer);

    console.log("game_service _ref set", ref);

    if (game_service._pendingRendererId != null)
    {
        console.log("found _pendingRendererId, will init_new_renderer");
        game_service.init_new_renderer(game_service._pendingRendererId);
        game_service._pendingRendererId = null;
    }

    console.log("game_service init finished");

    requestAnimationFrame(game_service.animate);
};

game_service.animate = function ()
{
    console.log("game_service animate");
    requestAnimationFrame(game_service.animate);

    // Rotate the cube
    game_service._cube.rotation.x += 0.01;
    game_service._cube.rotation.y += 0.01;

    //game_service._ref.invokeMethod("Update");
    game_service.render();
};

game_service.render = function ()
{
    if (game_service._renderer != null) {
        game_service._renderer.render(game_service._scene, game_service._camera);
    }
}



game_service.init_new_renderer = function (containerId)
{
    console.log("game_service initRenderer called");
    if (game_service._ref == null)
    {
        console.log("game_service initRenderer will wait, setting _pendingRendererId", containerId);
        game_service._pendingRendererId = containerId;
        return;
    }


    const container = document.getElementById(containerId);

    let renderer = new THREE.WebGLRenderer({ antialias: true });
    renderer.setPixelRatio(window.devicePixelRatio);
    renderer.setSize(window.innerWidth, window.innerHeight);
    renderer.shadowMap.enabled = true;
    container.appendChild(renderer.domElement);

    game_service._renderer = renderer;
    game_service.resize_renderer();
}


game_service.resize_renderer = function ()
{

    if (game_service._renderer == null)
        return;

    //game_service._camera.aspect = window.innerWidth / window.innerHeight;
    //game_service._camera.updateProjectionMatrix();
    //game_service._renderer.setSize(window.innerWidth, window.innerHeight);

    let width = 500;
    let height = 500;

    game_service._camera.aspect = width / height;
    game_service._camera.updateProjectionMatrix();
    game_service._renderer.setSize(width, height);

}

console.log("game_service constructed");


