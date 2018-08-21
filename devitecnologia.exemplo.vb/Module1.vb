Imports devitecnologia.api.client

Module Module1

    Sub Main()
        Dim baseAddress As String = ""
        Dim username As String = ""
        Dim passMd5 As String = ""
        Dim organizacaoId As Guid = Guid.Parse("")
        Dim empresaId As Guid = Guid.Parse("")

        Dim ws As WS = New WS(baseAddress, username, passMd5, organizacaoId, empresaId)

        Dim token = ws.GetToken()

        Console.WriteLine($"token: {token}")

        ws.OnReceiveXML = (Function(xml)
                               Console.WriteLine($"OnReceiveXML: {DateTime.Now}")
                               Console.WriteLine(xml)
                           End Function)

        ws.Iniciar()

        Console.WriteLine($"EmitirCupomParaPrevenda: {DateTime.Now}")

        Dim prevendaCabecalhoId As Guid = Guid.Parse("")

        ws.EmitirCupomParaPrevenda(prevendaCabecalhoId)

        Console.ReadKey()
    End Sub

End Module