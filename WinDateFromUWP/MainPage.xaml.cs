using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x410

namespace WinDateFromUWP
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        private Windows.Storage.ApplicationDataContainer container;

        public MainPage()
        {
            string s;
            this.InitializeComponent();
            container = localSettings.CreateContainer("WinDateFrom", Windows.Storage.ApplicationDataCreateDisposition.Always);
            s = localSettings.Containers["WinDateFrom"].Values["Data"] as string;
            if (s == null)
                data.Date= DateTime.Now;
            else
                data.Date = DateTime.Parse(s);
            s = localSettings.Containers["WinDateFrom"].Values["Nome"] as string;
            if (s != null)
                nome.Text = s;
        }

        private void calcola_Click(object sender, RoutedEventArgs e)
        {
            anniversario.Text = "";
            risultato.Text = "";
            DateTime d = DateTime.Now;
            TimeSpan differenza = d - data.Date.Date;
            if (differenza.TotalDays < 0)
            {
                risultato.Text = "Invalid rvalue";
                return;
            }
            if (d.Day == data.Date.Day && differenza.TotalDays > 1)
            {
                if (d.Month == data.Date.Month)
                    anniversario.Text = $"Is your anniversary";
                else
                    anniversario.Text = $"Is your mesiversary";
            }
            if (nome.Text != null && nome.Text != "")
                risultato.Text = $"You meet {nome.Text} about {differenza.Days} days ago";
            else
                risultato.Text = $"{differenza.Days} days are passed";
            localSettings.Containers["WinDateFrom"].Values["Data"] = data.Date.ToString();
                localSettings.Containers["WinDateFrom"].Values["Nome"]=nome.Text;
        }

        private void OnInformationi_Clicked(object sender, RoutedEventArgs e)
        {
            Applicazione.Visibility = Visibility.Collapsed;
            Informazioni.Visibility = Visibility.Visible;
        }

        private void OnApp_Clicked(object sender, RoutedEventArgs e)
        {
            Informazioni.Visibility = Visibility.Collapsed;
            Applicazione.Visibility = Visibility.Visible;
        }
        private void DeleteSettings_Clicked(object sender, RoutedEventArgs e)
        {
            localSettings.Containers["WinDateFrom"].DeleteContainer("WinDateFrom");
        }

        public async void Informations_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://github.com/numerunix/WinDateFromUWP"));
        }
    }
}
