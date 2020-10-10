using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace Kalkulacka
{
    public partial class App : Application
    {
        /// <summary>promenna pro fixni sirku a vysku tlacitek</summary>
        public static double buttonSize { get; set; }
        /// <summary>hodnota, jake ma odsazeni tlacitek</summary>
        public static double buttonSpacing { get; set; }

        public App()
        {
            InitializeComponent();

            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            int columnCount = 4;
            App.buttonSpacing = 15;

            // (sirka displeje) - (velikost mezer * celkovy pocet vertikalnich mezer) / (pocet sloupcu)
            App.buttonSize = ((mainDisplayInfo.Width / mainDisplayInfo.Density) - (App.buttonSpacing * (columnCount + 1))) / columnCount;

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
