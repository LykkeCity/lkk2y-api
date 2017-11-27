using System;
using Common.Log;

namespace Lykke.Service.Lkk2Y_Api.Client
{
    public class Lkk2Y_ApiClient : ILkk2Y_ApiClient, IDisposable
    {
        private readonly ILog _log;

        public Lkk2Y_ApiClient(string serviceUrl, ILog log)
        {
            _log = log;
        }

        public void Dispose()
        {
            //if (_service == null)
            //    return;
            //_service.Dispose();
            //_service = null;
        }
    }
}
