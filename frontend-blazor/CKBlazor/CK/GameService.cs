using System.Net.Http.Json;
using CKBlazor.CK.Assets;
using CKBlazor.CK.Data;
using CKBlazor.CK.Networking;
using CKBlazor.CK.Screens;
using Microsoft.JSInterop;

namespace CKBlazor.CK
{
    public class GameService
    {
        public AssetsLayer Assets { get; }
        public List<IScreen> ScreenStack { get; }
        public Action<GameService> OnScreenStackChange;
        public GameState? LastGameState { get; }

        HttpClient _httpClient;
        IJSRuntime _js;
        NetworkingLayer _networkingLayer;

        public GameService(IJSRuntime js, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _js = js;

            Assets = new AssetsLayer(httpClient);
            ScreenStack = new List<IScreen>();
            _networkingLayer = new NetworkingLayer(httpClient);

            StartupAsync();
        }

        async void StartupAsync()
        {
            ScreenStack.Add(new DebugMessageScreen("Loading assets"));
            if(OnScreenStackChange != null)
                OnScreenStackChange(this);

            await Assets.InitAssets();

            ScreenStack.Add(new DebugMessageScreen("Loaded assets"));
            if (OnScreenStackChange != null)
                OnScreenStackChange(this);


            ScreenStack.Clear();

            ScreenStack.Add(new GameCanvasScreen(this, _js));
            ScreenStack.Add(new GameHUDScreen());

            if (OnScreenStackChange != null)
                OnScreenStackChange(this);
        }
    }
}
