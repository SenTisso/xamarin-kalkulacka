using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
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

        private bool ShouldAddSymbol(string symbol)
        {
            bool result = true;
            string lastResultCharacter = "";
            if (ResultLabel.Text.Length > 0) lastResultCharacter = ResultLabel.Text.Last().ToString();

            // pokud symbol neni cislo
            if (!new Regex(@"^[0-9]$").IsMatch(symbol))
            {
                if (ResultLabel.Text == "error" && symbol != "(") // pokud tam je error, tak se nic krome ( a cisel nemuze vypsat
                {
                    result = false;
                }
                else if (!new Regex(@"[0-9]").IsMatch(lastResultCharacter)) // pokud posledni charakter expression neni cislo
                {
                    if (symbol == ")") // pokud je symbol ) a posledni je )
                    {
                        if (lastResultCharacter != ")") result = false;
                    }
                    else if (symbol == "(") // pokud je symbol ) a posledni je )
                    {
                        if (lastResultCharacter == ")") result = false;
                    }
                    else if (lastResultCharacter != ")") result = false;
                }
                else if (symbol == "(") result = false;

            }
            else // pokud symbol je cislo
            {
                if (lastResultCharacter == ")") result = false; // po ) nejde vypisovat cisla
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
                case "DEL":
                    // pokud tam je error nebo pouze zacatecni nula
                    if (resultText == "error" || resultText == "0" || resultText.Length <= 1)
                    {
                        ResultLabel.Text = "0";
                    }
                    else 
                    {
                        ResultLabel.Text = resultText.Substring(0, resultText.Length - 1);
                    }
                    break;

                case "C":
                    ResultLabel.Text = "0";
                    break;

                case "=":
                    ResultLabel.Text = Calculate();
                    break;

                default:

                    // muze se pridat symbol?
                    if (ShouldAddSymbol(buttonValue))
                    {
                        if (resultText == "error" || resultText == "0")
                        {
                            ResultLabel.Text = "";
                        }

                        ResultLabel.Text += buttonValue;

                    }

                    break;
            }
        }
    }
}
