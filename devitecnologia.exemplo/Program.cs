using devitecnologia.api.client;
using System;

namespace devitecnologia.exemplo
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "";
            string username = "";
            string passMd5 = "";
            Guid organizacaoId = Guid.Parse("");
            Guid empresaId = Guid.Parse("");

            WS ws = new WS(baseAddress, username, passMd5, organizacaoId, empresaId);

            var token = ws.GetToken();

            Console.WriteLine($"token: {token}");

            ws.OnReceiveXML = (xml =>
            {
                Console.WriteLine($"OnReceiveXML: {DateTime.Now}");
                Console.WriteLine(xml);
            });

            ws.Iniciar();

            Console.WriteLine($"EmitirCupomParaPrevenda: {DateTime.Now}");

            Guid prevendaCabecalhoId = Guid.Parse("");

            ws.EmitirCupomParaPrevenda(prevendaCabecalhoId);

            Console.ReadKey();
        }
    }
}