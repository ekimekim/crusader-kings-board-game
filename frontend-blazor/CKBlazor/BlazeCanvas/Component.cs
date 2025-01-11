using Microsoft.JSInterop;

namespace BlazeCanvas
{
    public abstract class BasicComponent : Component
    {
    }

    public abstract class ScriptComponent : Component
    {
    }

    public abstract class Component
    {
        public Session Session { get; protected set; }
        public Entity Entity { get; protected set; }
        public string Id { get; protected set; }
        public abstract string JsClassName();

        protected Component()
        {
            // Please use CreateSession()
        }

        async Task InvokeJSAsync(string method)
        {
            await Session.Js.InvokeVoidAsync("window.BC.invokeComponentMethod", this.Id, method);
        }

        async Task InvokeJSAsync1(string method, object arg0)
        {
            await Session.Js.InvokeVoidAsync("window.BC.invokeComponentMethod1", this.Id, method, arg0);
        }

        async Task InvokeJSAsync2(string method, object arg0, object arg1)
        {
            await Session.Js.InvokeVoidAsync("window.BC.invokeComponentMethod2", this.Id, method, arg0, arg1);
        }

        async Task InvokeJSAsync3(string method, object arg0, object arg1, object arg2)
        {
            await Session.Js.InvokeVoidAsync("window.BC.invokeComponentMethod3", this.Id, method, arg0, arg1, arg2);
        }

        public static async Task<TComponent> CreateComponent<TComponent>(Entity entity, object args = null) where TComponent : BasicComponent
        {
            var component = Activator.CreateInstance<TComponent>();
            component.Id = entity.Session.IdGenerator.NextId();
            component.Entity = entity;
            component.Session = entity.Session;

            await entity.Session.Js.InvokeVoidAsync("window.BC.createComponent", entity.Id, component.Id, component.JsClassName(), args);

            return component;
        }

        public static async Task<TComponent> CreateScriptComponent<TComponent>(Entity entity, object args = null) where TComponent : ScriptComponent
        {
            var component = Activator.CreateInstance<TComponent>();
            component.Id = entity.Session.IdGenerator.NextId();
            component.Entity = entity;
            component.Session = entity.Session;

            await entity.Session.Js.InvokeVoidAsync("window.BC.createScriptComponent", entity.Id, component.Id, component.JsClassName(), args);

            return component;
        }
    }
}
