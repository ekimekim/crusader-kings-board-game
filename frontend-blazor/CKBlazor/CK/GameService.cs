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
        public Action<GameService>? OnScreenStackChange;
        public GameState? LastGameState { get; }

        HttpClient _httpClient;
        IJSRuntime _js;
        NetworkingLayer _networkingLayer;

        public GameService(IJSRuntime js, HttpClient httpClient)
        {
            Console.WriteLine("GameService()");
            _httpClient = httpClient;
            _js = js;

            Assets = new AssetsLayer(httpClient);
            ScreenStack = new List<IScreen>();
            _networkingLayer = new NetworkingLayer(httpClient);

            StartupAsync();
        }

        async void StartupAsync()
        {
            Console.WriteLine("StartupAsync");
            PushScreens(new DebugMessageScreen("Loading assets"));

            Console.WriteLine("await Assets.InitAssets");
            await Assets.InitAssets();
            await WaitNSeconds(5);

            ClearAllScreens();
            PushScreens(new DebugMessageScreen("Loaded assets"));
            Console.WriteLine("WaitNSeconds");
            await WaitNSeconds(5);

            ClearAllScreens();

            PushScreens(new GameCanvasScreen(this, _js), new GameHUDScreen());
        }

        public void PushScreens(params IScreen[] screens)
        {
            foreach(var screen in screens)
            {
                ScreenStack.Add(screen);
            }
            OnScreenStackChange?.Invoke(this);
        }

        public void PopScreens(params IScreen[] screens)
        {
            foreach (var screen in screens)
            {
                ScreenStack.Remove(screen);
                screen.Dispose();
            }
            OnScreenStackChange?.Invoke(this);
        }

        public void ClearAllScreens()
        {
            var removeOrder = ScreenStack.Reverse<IScreen>().ToArray();
            foreach (var screen in removeOrder)
            {
                ScreenStack.Remove(screen);
                screen.Dispose();
            }
            OnScreenStackChange?.Invoke(this);
        }

        Task WaitNSeconds(int seconds)
        {
            var t = new Task(() => Thread.Sleep(seconds * 1000));
            t.Start();
            return t;
        }
    }
}
