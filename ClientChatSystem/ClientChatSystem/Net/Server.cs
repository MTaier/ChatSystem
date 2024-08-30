using ClientChatSystem.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientChatSystem.Net;

class Server
{
    TcpClient _client;
    public PacketReader PacketReader;
    public event Action connectedEvent;
    public event Action msgReceivedEvent;
    public event Action userDisconnectEvent;

    public Server()
    {
        _client = new TcpClient();
    }

    public void ConectarAoServidor(string username)
    {
        if (!_client.Connected)
        {
            _client.Connect("127.0.0.1", 4444);
            PacketReader = new PacketReader(_client.GetStream());

            if (!string.IsNullOrEmpty(username))
            {
                var connectPacket = new PacketBuilder();
                connectPacket.EscreverOpCode(0);
                connectPacket.EscreverMensagem(username);
                _client.Client.Send(connectPacket.GetPacketBytes());
            }
            LerPacotes();
        }
    }

    public void DesconectarDoServidor()
    {
        if (_client.Connected)
        {
            // Aqui você pode adicionar lógica para enviar um pacote de desconexão, se necessário.
            var disconnectPacket = new PacketBuilder();
            disconnectPacket.EscreverOpCode(15); // Supondo que 1 seja o código de operação para desconexão.
            _client.Client.Send(disconnectPacket.GetPacketBytes());

            _client.Close();
            PacketReader.Close(); // Certifique-se de fechar o PacketReader também.
        }
    }

    private void LerPacotes()
    {
        Task.Run(() =>
        {
            while (true)
            {
                var opcode = PacketReader.ReadByte();
                switch (opcode)
                {
                    case 1:
                        connectedEvent?.Invoke();
                        break;
                    case 5:
                        msgReceivedEvent?.Invoke();
                        break;
                    case 10:
                        userDisconnectEvent?.Invoke();
                        break;
                    default:
                        Console.WriteLine("ah yes..");
                        break;
                }
            }
        });
    }

    public void MandarMensagemAoServidor(string mensagem)
    {
        var messagePacket = new PacketBuilder();
        messagePacket.EscreverOpCode(5);
        messagePacket.EscreverMensagem(mensagem);
        _client.Client.Send(messagePacket.GetPacketBytes());

    }

    public bool EstaConectado()
    {
        return _client.Connected;
    }
}
