using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientChatSystem.Net.IO;

class PacketBuilder
{
    MemoryStream _ms;

    public PacketBuilder()
    {
        _ms = new MemoryStream();
    }

    public void EscreverOpCode(byte opcode)
    {
        _ms.WriteByte(opcode);
    }

    public void EscreverMensagem(string msg)
    {
        byte[] msgBytes = Encoding.UTF8.GetBytes(msg);
        int msgLength = msgBytes.Length;

        _ms.Write(BitConverter.GetBytes(msgLength), 0, sizeof(int)); // Escreve o comprimento da mensagem como um inteiro de 4 bytes
        _ms.Write(msgBytes, 0, msgBytes.Length); // Escreve os bytes da mensagem
    }

    public byte[] GetPacketBytes()
    {
        return _ms.ToArray();
    }
}
