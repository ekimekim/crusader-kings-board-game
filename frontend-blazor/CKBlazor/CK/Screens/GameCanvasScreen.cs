using BlazeCanvas.Components;
using BlazeCanvas;
using System.Xml;
using Microsoft.JSInterop;

namespace CKBlazor.CK.Screens
{
    public class GameCanvasScreen : IScreen
    {
        private IJSRuntime _js;

        public ScreenType ScreenType => ScreenType.GameCanvas;
        public string UniqueId { get; set; }

        public GameCanvasScreen(IJSRuntime js)
        {
            _js = js;
        }
        public async Task OnCanvasCreated() => await LoadDemoScene();

        public async Task LoadDemoScene()
        {
            Console.WriteLine("Load Start");
            await Task.Delay(2000);

            var session = await Session.CreateSession(_js, UniqueId, $"{UniqueId}-canvas");

            await Task.Delay(1000);

            await session.Start();

            Console.WriteLine("Load Finish");

            var cameraEntity = await Entity.CreateEntity(session);
            var camera = await Component.CreateComponent<CameraComponent>(cameraEntity, CameraComponent.GetArgs());
            await cameraEntity.Translate(0, 10, 15);
            await cameraEntity.SetLocalEulerAngles(-45, 0, 0);
            var cameraControls = await Component.CreateScriptComponent<TopDownCameraControls>(cameraEntity, new { });

            var lightEntity = await Entity.CreateEntity(session);
            var light = await Component.CreateComponent<LightComponent>(lightEntity, LightComponent.GetLightArgs());

            var boxEntity = await Entity.CreateEntity(session);
            await boxEntity.SetLocalEulerScale(0.4f, 0.4f, 0.4f);
            var boxModel = await Component.CreateComponent<ModelComponent>(boxEntity, ModelComponent.GetBoxArgs());
            var boxPin = await Component.CreateScriptComponent<DivPinComponent>(boxEntity, DivPinComponent.GetArgs());
            await boxPin.SetPinId("pin-01");

            var mapAsset = await Asset.LoadAsset(session, Asset.AssetType.container, "assets/board.glb");

            var mapEntity = await Entity.CreateEntity(session);
            var mapModel = await Component.CreateComponent<ModelComponent>(mapEntity, ModelComponent.GetAssetArgs(mapAsset));
        }
    }
}
