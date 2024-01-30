using Microsoft.JSInterop;

namespace CKBlazor.CK
{
    public class GameService
    {
        public int FrameId;
        bool _hasStarted;
        private IJSRuntime _jsRuntime;

        public GameService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            Start();
        }


        void Start()
        {
            //if (_hasStarted)
            //    return;
            Console.WriteLine("started");

            //_jsRuntime.InvokeVoidAsync("alert", "test");

            UpdateDomFromJs();

        }

        public async Task<string> InvokeJsFunction()
        {
            // Example of calling a JavaScript function from C#
            return await _jsRuntime.InvokeAsync<string>("exampleJsFunction");
        }

        public async Task UpdateDomFromJs()
        {
            // Example of manipulating the DOM from C#
            await _jsRuntime.InvokeVoidAsync("window.game_service.test");
        }
    }
}
