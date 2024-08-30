using ClientChatSystem.MVVM.Core;
using ClientChatSystem.MVVM.Model;
using ClientChatSystem.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClientChatSystem.MVVM.ViewModel;

class MainViewModel
{
    public ObservableCollection<UserModel> Users { get; set; }
    public ObservableCollection<string> Messages { get; set; }
    public RelayCommand ConnectToServerCommand { get; set; }
    public RelayCommand SendMessageCommand { get; set; }
    public string Username { get; set; }
    public string Message { get; set; }
    private Server _server;

    public MainViewModel()
    {
        Users = new ObservableCollection<UserModel>();
        Messages = new ObservableCollection<string>();
        _server = new Server();
        _server.connectedEvent += UsuarioConectado;
        _server.msgReceivedEvent += MensagemRecebida;
        _server.userDisconnectEvent += RemoverUsuario;
        ConnectToServerCommand = new RelayCommand(o => _server.ConectarAoServidor(Username), o => !string.IsNullOrEmpty(Username));
        SendMessageCommand = new RelayCommand(o => _server.MandarMensagemAoServidor(Message), o => !string.IsNullOrEmpty(Message));
    }

    private void RemoverUsuario()
    {
        var uid = _server.PacketReader.LerMensagem();
        var user = Users.Where(x => x.UID == uid).FirstOrDefault();
        Application.Current.Dispatcher.Invoke(() => Users.Remove(user));
    }

    private void MensagemRecebida()
    {
        var msg = _server.PacketReader.LerMensagem();
        Application.Current.Dispatcher.Invoke(() => Messages.Add(msg));
    }

    private void UsuarioConectado()
    {
        var user = new UserModel
        {
            Username = _server.PacketReader.LerMensagem(),
            UID = _server.PacketReader.LerMensagem()
        };

        if (!Users.Any(x => x.UID == user.UID))
        {
            Application.Current.Dispatcher.Invoke(() => Users.Add(user));
        }
    }

}
