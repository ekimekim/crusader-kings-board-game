using Microsoft.JSInterop;

namespace BlazeCanvas
{
    public class Session : IDisposable
    {
        public string Id { get; private set; }
        public string CanvasId { get; private set; }
        public DotNetObjectReference<Session> SelfAsInvokableRef { get; private set; }
        public IJSRuntime Js { get; private set; }
        public UniqueIdGenerator IdGenerator { get; private set; }

        private Session()
        {
        }

        public static async Task<Session> CreateSession(IJSRuntime js, string id, string canvasId)
        {
            var session = new Session();
            session.Id = id;
            session.CanvasId = canvasId;
            session.SelfAsInvokableRef = DotNetObjectReference.Create(session);
            session.Js = js;
            session.IdGenerator = new UniqueIdGenerator(session.Id);

            await js.InvokeVoidAsync("window.BC.createSession", session.Id, session.SelfAsInvokableRef);

            return session;
        }

        public async Task Start()
        {
            await InvokeJSAsync1("start", CanvasId);
        }

        async Task InvokeJSAsync(string method)
        {
            await Js.InvokeVoidAsync("window.BC.invokeSessionMethod", this.Id, method);
        }

        async Task InvokeJSAsync1(string method, object arg0)
        {
            await Js.InvokeVoidAsync("window.BC.invokeSessionMethod1", this.Id, method, arg0);
        }

        async Task InvokeJSAsync2(string method, object arg0, object arg1)
        {
            await Js.InvokeVoidAsync("window.BC.invokeSessionMethod2", this.Id, method, arg0, arg1);
        }

        async Task InvokeJSAsync3(string method, object arg0, object arg1, object arg2)
        {
            await Js.InvokeVoidAsync("window.BC.invokeSessionMethod3", this.Id, method, arg0, arg1, arg2);
        }

        public async Task StartRotating(Entity entity)
        {
            await InvokeJSAsync1("startRotating", entity.Id);
        }

        [JSInvokable]
        public void OnFoo()
        {
            Console.WriteLine(">>>>>>>>>>>>>>> OnFoo triggered!");
        }

        public void Dispose()
        {
            // Cleanup
            SelfAsInvokableRef?.Dispose();
        }
    }
}
