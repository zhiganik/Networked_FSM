using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace __Project__.Scripts.Network
{
    public class InternetConnection : IInitializable
    {
        private const string EndPoint = "https://www.google.com/";
        
        public bool IsConnectionAvailable { get; private set; }

        public async void Initialize()
        {
            await HasInternet();
        }

        public async Task<bool> HasInternet()
        {
            HttpClient client = new HttpClient();

            try
            {
                var result = await client.GetAsync(EndPoint);
                IsConnectionAvailable = result.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                IsConnectionAvailable = false;
            }
            finally
            {
                client.Dispose();
            }

            return IsConnectionAvailable;
        }
    }
}