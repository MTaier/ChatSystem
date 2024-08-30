using ServerChatSystem.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerChatSystem;

class Client
{
    public string Username { get; set; }
    public Guid UID { get; set; }
    public TcpClient ClientSocket { get; set; }
    PacketReader _packetReader;

    public Client(TcpClient cliente)
    {
        ClientSocket = cliente;
        UID = Guid.NewGuid();
        _packetReader = new PacketReader(ClientSocket.GetStream());

        var opcode = _packetReader.ReadByte();
        Username = _packetReader.LerMensagem();

        Console.WriteLine($"[{DateTime.Now}]: Client {Username} conectou-se");

        Task.Run(() => Process());
    }

    void Process()
    {
        while ( true )
        {
            try
            {
                var opcode = _packetReader.ReadByte();
                switch ( opcode )
                {
                    case 5:
                        var msg = _packetReader.LerMensagem();
                        Console.WriteLine($"[{DateTime.Now}]: Mensagem recebida! {msg}");
                        Program.BroadcastMensagem($"[{DateTime.Now}]: [{Username}]: {msg}");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception) 
            {
                Console.WriteLine($"[{UID.ToString()}]: Desconectado!");
                Program.BroadcastDesconectado(UID.ToString());
                ClientSocket.Close();
                break;
            }
        }
    }
}
