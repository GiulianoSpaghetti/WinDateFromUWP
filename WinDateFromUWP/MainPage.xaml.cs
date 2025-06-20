﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.System.Profile.SystemManufacturers;
using Windows.UI.Popups;
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
        private Uri uri = new Uri("https://github.com/GiulianoSpaghetti/WinDateFromUWP");
        private string ricorrenza;
        public MainPage()
        {
            MessageDialog d;
            string s;
            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US";
            this.InitializeComponent();
            try
            {
                container = localSettings.CreateContainer("WinDateFrom", Windows.Storage.ApplicationDataCreateDisposition.Existing);
                s = localSettings.Containers["WinDateFrom"].Values["Data"] as string;
            }
            catch (Exception ex)
            {
                container = localSettings.CreateContainer("WinDateFrom", Windows.Storage.ApplicationDataCreateDisposition.Always);
                s= null;
            }
            if (s == null)
                data.Date= DateTime.Now;
            else
                data.Date = DateTime.Parse(s);
            s = localSettings.Containers["WinDateFrom"].Values["Nome"] as string;
            if (s != null)
                nome.Text = s;
            if (!SystemSupportInfo.LocalDeviceInfo.SystemProductName.Contains("Xbox") && !SystemSupportInfo.LocalDeviceInfo.SystemProductName.Contains("Surface"))
            {
                d = new MessageDialog("Unsupported platform");
                d.Commands.Add(new UICommand("Exit", new UICommandInvokedHandler(exit)));
                IAsyncOperation<IUICommand> asyncOperation = d.ShowAsync();
            }
        }

        private void exit(IUICommand command)
        {
            Application.Current.Exit();
        }

        private void calcola_Click(object sender, RoutedEventArgs e)
        {
            anniversario.Text = "";
            risultato.Text = "";
            nome.Text = nome.Text.Trim();
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
                {
                    anniversario.Text = "Is your anniversary";
                    ricorrenza = "anniversary";
                }
                else
                {
                    anniversario.Text = "Is your mesiversary";
                    ricorrenza = "mesiversary";
                }

            }
            if (nome.Text != null && nome.Text != "")
                risultato.Text = $"You meet {nome.Text} about {differenza.Days} days ago";
            else
              risultato.Text = $"{differenza.Days} days are passed";
            if (ricorrenza!="" && nome.Text!="" && differenza.Days>0)
            {
                    augura.IsEnabled = true;
            }
            localSettings.Containers["WinDateFrom"].Values["Data"] = data.Date.ToString();
            localSettings.Containers["WinDateFrom"].Values["Nome"]=nome.Text;
            localSettings.Containers["WinDateFrom"].Dispose();
            calcola.IsEnabled = false;
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
            await Launcher.LaunchUriAsync(uri);
        }

        public async void augura_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri($"https://twitter.com/intent/tweet?text=Happy%20{ricorrenza}%20my%20love."));
            augura.IsEnabled = false;
        }

    }
}
