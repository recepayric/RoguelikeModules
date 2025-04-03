using System.Collections.Generic;
using MVC.Base.Runtime.Concrete.Injectable.Service.Backend;
using MVC.Base.Runtime.Concrete.Promise;
using UnityEngine;

namespace MVC.Base.Runtime.Abstract.Injectable.Service.Backend
{
    public interface IProcessorService
    {
        List<string> ClearList { get; }

        Dictionary<string, Promise> PromiseMap { get; }

        Dictionary<string, object> ResponseMap { get; }

        string ServerUrl { get; }

        string SessionId { get; set; }
        
        Dictionary<string, ExtensionCallback> ExtensionMap { get; }

        void SetResponse(string command, object data);

        void UpdateProgressBar(string key, long progress);
    }
}