using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Data;
using System.Text.RegularExpressions;

namespace Kalkulacka
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public string Calculate()
        {
            string result = "";

            try
            {
                result = Convert.ToDouble(new DataTable().Compute(ResultLabel.Text, null)).ToString("0.000000");
            }
            catch
            {
                result = "error";
            }

            return result;
        }


        /// <summary>Event funkce, ktera je spustena po kliknuti na tlacitka.</summary>
        /// <param name="sender">Objekt, na ktery se kliklo.</param>
        /// <param name="args">Parametry eventu.</param>
        void OnButtonClicked(object sender, EventArgs args)
        {

            Button button = (Button)sender;
            string buttonValue = button.Text; // text buttonu, ktery predstavuje jeho hodnotu
            string resultText = ResultLabel.Text;

            switch (buttonValue)
            {
                case "=":
                    ResultLabel.Text = Calculate();
                    break;

                default:
                    if (resultText == "error" || resultText == "0")
                    {
                        ResultLabel.Text = buttonValue;
                    }
                    else
                    {
                        ResultLabel.Text += buttonValue; 
                    }

                    break;
            }
        }
    }
}
