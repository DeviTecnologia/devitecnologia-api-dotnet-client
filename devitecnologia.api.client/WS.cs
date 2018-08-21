using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace devitecnologia.api.client
{
    public class WS
    {
        private static string _token = "";
        private static string _baseAddress;
        private HubConnection _connection;
        string _username;
        string _passMd5;
        string _organizacaoId;
        string _empresaId;

        private WS() { }

        public WS(string baseAddress, string username, string passMd5, Guid organizacaoId, Guid empresaId)
        {
            _baseAddress = baseAddress;
            _username = username;
            _passMd5 = passMd5;
            _organizacaoId = organizacaoId.ToString();
            _empresaId = empresaId.ToString();
        }

        public string GetToken()
        {
            var data = "grant_type=password&username=" + _username + "&password=" + _passMd5
                + "&organizacaoid=" + _organizacaoId + "&empresaid=" + _empresaId;

            HttpClient Client = new HttpClient();
            Client.BaseAddress = new Uri(_baseAddress);
            var response = Client.PostAsync("/api/token",
                 new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded"))
                 .GetAwaiter().GetResult();

            response.EnsureSuccessStatusCode();
            var responseString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            var JSONObj = JsonConvert.DeserializeObject<dynamic>(responseString);

            _token = JSONObj.access_token;
            return _token;
        }

        public void Iniciar()
        {
            string server = _baseAddress + $"/ws?access_token={_token}";

            _connection = new HubConnectionBuilder()
                .WithUrl(server)
                .Build();

            _connection.On<string>("ReceiveXML", (message) =>
            {
                OnReceiveXML(message);
            });

            try
            {
                _connection.StartAsync().Wait();
                Console.WriteLine("Connection started");
            }
            catch (Exception ex)
            {
                Console.WriteLine("erro: " + ex.Message);
            }
        }

        public async void EmitirCupomParaPrevenda(Guid prevendaId)
        {
            try
            {
                await _connection.InvokeAsync("EmiteCupom", prevendaId.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("erro: " + ex.Message);
            }
        }

        public Action<string> OnReceiveXML { get; set; }
    }
}