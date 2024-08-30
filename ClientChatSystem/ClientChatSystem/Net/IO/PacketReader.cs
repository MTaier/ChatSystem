using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientChatSystem.Net.IO;

class PacketReader : BinaryReader
{
    public NetworkStream _ns { get; set; }

    public PacketReader(NetworkStream ns) : base(ns)
    {
        _ns = ns;
    }

    public string LerMensagem()
    {
        byte[] lengthBytes = new byte[sizeof(int)];
        _ns.Read(lengthBytes, 0, sizeof(int)); // Lê os 4 bytes do comprimento da mensagem
        int length = BitConverter.ToInt32(lengthBytes, 0); // Converte os 4 bytes em um inteiro representando o comprimento da mensagem

        byte[] msgBuffer = new byte[length];
        _ns.Read(msgBuffer, 0, length); // Lê os bytes da mensagem
        string msg = Encoding.UTF8.GetString(msgBuffer); // Converte os bytes da mensagem de volta para uma string UTF-8

        return msg;
    }
}
