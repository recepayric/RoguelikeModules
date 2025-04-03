using MVC.Base.Runtime.Abstract.Injectable.Service.Backend.Processor;
using MVC.Base.Runtime.Concrete.Injectable.Service.Backend;
using MVC.Base.Runtime.Concrete.Promise;

namespace MVC.Base.Runtime.Abstract.Injectable.Service.Backend
{
    public interface IBackendService
    {
        void AddProcessor(IRequestProcessor processor);

        IPromise Request(string command, string data);

        IPromise Request(string command, object data);

        IPromise Request(string command);

        void Send(string command, string data);

        void Listen(string command, ExtensionCallback callback);

        void RemoveListen(string command);

        T GetData<T>(string command) where T : new();

        string GetData(string command);
        void Send(string command, object data);
        string ServerUrl { get; }

        void AddProgressBarListener(string key, ProgressCallback callback);

        void RemoveProgressBarListener(string key);

    }
}