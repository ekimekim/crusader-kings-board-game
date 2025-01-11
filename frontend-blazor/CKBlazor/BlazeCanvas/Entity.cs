using Microsoft.JSInterop;

namespace BlazeCanvas
{
    public class Entity
    {
        public Session Session { get; private set; }
        public string Id { get; private set; }

        private Entity()
        {
            // Please use CreateSession()
        }

        public static async Task<Entity> CreateEntity(Session session)
        {
            var entity = new Entity();
            entity.Id = session.IdGenerator.NextId();
            entity.Session = session;

            await session.Js.InvokeVoidAsync("window.BC.createEntity", session.Id, entity.Id);

            return entity;
        }

        public async Task Translate(float x, float y, float z)
        {
            await InvokeJSAsync3("translate", x, y, z);
        }

        public async Task SetPosition(float x, float y, float z)
        {
            await InvokeJSAsync3("setPosition", x, y, z);
        }

        public async Task SetLocalEulerAngles(float x, float y, float z)
        {
            await InvokeJSAsync3("setLocalEulerAngles", x, y, z);
        }

        async Task InvokeJSAsync(string method)
        {
            await Session.Js.InvokeVoidAsync("window.BC.invokeEntityMethod", this.Id, method);
        }

        async Task InvokeJSAsync1(string method, object arg0)
        {
            await Session.Js.InvokeVoidAsync("window.BC.invokeEntityMethod1", this.Id, method, arg0);
        }

        async Task InvokeJSAsync2(string method, object arg0, object arg1)
        {
            await Session.Js.InvokeVoidAsync("window.BC.invokeEntityMethod2", this.Id, method, arg0, arg1);
        }

        async Task InvokeJSAsync3(string method, object arg0, object arg1, object arg2)
        {
            await Session.Js.InvokeVoidAsync("window.BC.invokeEntityMethod3", this.Id, method, arg0, arg1, arg2);
        }
    }
}
