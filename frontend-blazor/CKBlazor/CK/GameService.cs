using Microsoft.JSInterop;

namespace CKBlazor.CK
{
    public class GameService
    {
        public int FrameId { get; private set; }
        public float Elapsed { get; private set; }

        bool _hasInit;
        bool _hasStarted;
        DateTime _startAt;
        IJSRuntime _jsRuntime;

        public GameService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public void TryInit()
        {
            if (_hasInit)
                return;
            _hasInit = true;
            Init();
        }

        async void Init()
        {
            await _jsRuntime.InvokeVoidAsync("window.game_service.init", DotNetObjectReference.Create(this));
         
            _hasStarted = true;
            _startAt = DateTime.Now;
            //Console.WriteLine($"GameService.try invoke start");
            //await _jsRuntime.InvokeVoidAsync("window.game_service.start");
        }


        [JSInvokable]
        public void Update()
        {
            FrameId++;
            Elapsed = (float)(DateTime.Now - _startAt).TotalSeconds;
            Console.WriteLine($"GameService.Update {FrameId} {Elapsed}");
        }

        //public async Task<string> InvokeJsFunction()
        //{
        //    // Example of calling a JavaScript function from C#
        //    return await _jsRuntime.InvokeAsync<string>("exampleJsFunction");
        //}

        //public async Task UpdateDomFromJs()
        //{
        //    // Example of manipulating the DOM from C#
        //    await _jsRuntime.InvokeVoidAsync("window.game_service.test");
        //}
    }
}
