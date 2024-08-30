using ServerChatSystem.Net.IO;
using System;
using System.Net;
using System.Net.Sockets;
using System.Xml;

namespace ServerChatSystem;

class Program
{
    static TcpListener _listener;
    static List<Client> _users;

    static void Main(string[] args)
    {
        _users = new List<Client>();
        _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 4444);
        _listener.Start();

        while (true)
        {
            var client = new Client(_listener.AcceptTcpClient());
            _users.Add(client);

            BroadcastConexao();
        }
    }

    static void BroadcastConexao()
    {
        foreach (var user in _users)
        {
            foreach (var usr in _users)
            {
                var broadcastPacket = new PacketBuilder();
                broadcastPacket.EscreverOpCode(1);
                broadcastPacket.EscreverMensagem(usr.Username);
                broadcastPacket.EscreverMensagem(usr.UID.ToString());
                user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
            }
        }
    }

    public static void BroadcastMensagem(string mensagem)
    {
        foreach (var user in _users)
        {
            var msgPacket = new PacketBuilder();
            msgPacket.EscreverOpCode(5);
            msgPacket.EscreverMensagem(mensagem);
            user.ClientSocket.Client.Send(msgPacket.GetPacketBytes());
        }
    }

    public static void BroadcastDesconectado(string uid)
    {
        var disconnectedUser = _users.Where(x => x.UID.ToString() == uid).FirstOrDefault();
        _users.Remove(disconnectedUser);
        foreach (var user in _users)
        {
            var broadcastPacket = new PacketBuilder();
            broadcastPacket.EscreverOpCode(10);
            broadcastPacket.EscreverMensagem(uid);
            user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
        }

        BroadcastMensagem($"[{disconnectedUser.Username}] Desconectado!");
    }
}