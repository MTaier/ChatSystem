using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System;
using System.Windows.Threading;
using System.Globalization;
using ClientChatSystem.Net;
using ClientChatSystem.MVVM.ViewModel;
using ClientChatSystem.MVVM.Model;

namespace ClientChatSystem;

public partial class MainWindow : Window
{
    UserModel _userModel;

    public MainWindow()
    {
        _userModel = new UserModel();
        InitializeComponent();

        // Inicia um DispatcherTimer para atualizar o TextBlock a cada segundo
        DispatcherTimer timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += Timer_Tick;
        timer.Start();
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        // Atualiza o TextBlock com a hora atual
        txtHora.Text = DateTime.Now.ToString("HH:mm");
    }

    private void Border_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();
        }
    }

    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.MainWindow.WindowState = WindowState.Minimized;
    }

    private void WindowStateButton_Click(object sender, RoutedEventArgs e)
    {
        if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
        {
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }
        else
        {
            Application.Current.MainWindow.WindowState = WindowState.Normal;
        }
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void btnConectar_Click(object sender, RoutedEventArgs e)
    {
        if (_userModel.Username != null && _userModel.Username.Length > 0)
        {
            txtInicial.Text = _userModel.Username.Substring(0, 1);
        }
    }
}