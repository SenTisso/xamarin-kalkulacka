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

        /// <summary>Metoda na vypocitani matematickeho vyrazu ze stringu.</summary>
        /// <param name="expression">Matematicky vyraz.</param>
        /// <returns>Vysledek nebo "error" jako string.</returns>
        public string Calculate(string expression)
        {
            string result = "";

            try
            {
                result = Convert.ToDouble(new DataTable().Compute(expression, null)) // dostane vypocet z textu
                    .ToString("0.000000") // prevede double na string
                    .TrimEnd(new Char[] { '0' }); // odstrani prebytecne nuly na konci

                // pokud je na konci tecka, tak ji odstrani
                if (result.Last().ToString() == ".") result = result.Replace(".", "");
            }
            catch
            {
                result = "error";
            }

            return result;
        }

        /// <summary>Zkontroluje zadavany znak s textem a usoudi, zda je matematicky legalni ho zadat (pouze za ucelem lepsiho UX).</summary>
        /// <param name="symbol">Znak, ktery uzivatel zadava.</param>
        /// <returns>Zda se ma zadana hodnota tlacitka pridat do vyrazu.</returns>
        private bool ShouldAddSymbol(string buttonSymbol)
        {
            bool result = false;

            string lastResultCharacter = "";
            if (ResultLabel.Text.Length > 0) lastResultCharacter = ResultLabel.Text.Last().ToString();

            bool isButtonSymbolANumber = new Regex(@"^[0-9]$").IsMatch(buttonSymbol); // zda je hodnota tlacitka cislo
            bool isLastCharANumber = new Regex(@"^[0-9]$").IsMatch(lastResultCharacter); // zda je posledni character vyrazu cislo

            if (buttonSymbol == ")") // pokud je hodnota tlacitka ")"...
            {
                if (lastResultCharacter == ")" || isLastCharANumber) result = true; // ...tak se muze vypsat pouze po ")" nebo po cislu
            }
            else if (buttonSymbol == "(") // pokud je hodnota tlacitka "("...
            {
                if (lastResultCharacter != ")" && !isLastCharANumber) result = true; // ...tak se muze vypsat pouze po nejakem vyrazu (ne po cislech a ")")
            }
            else if (!isButtonSymbolANumber) // pokud je hodnota tlacitka nejaky dalsi vyraz (ne cislo)...
            {
                if (isLastCharANumber || lastResultCharacter == ")") result = true; // ...tak se muze vypsat pouze po cislu nebo po ")"
            }
            else // pokud je hodnota tlacitka nejake cislo...
            {
                if (lastResultCharacter != ")") result = true; // ...tak se muze vypsat vzdy, krome po ")"
            }

            if (ResultLabel.Text == "error" || ResultLabel.Text == "0") // pokud ve vyrazu je error nebo pouze 0...
            {
                if (buttonSymbol == "(" || isButtonSymbolANumber) result = true; // tak se muze vypsat pouze "(" nebo cislo
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
                    // pokud tam je error nebo pouze zacatecni nula nebo pokud se maze posledni symbol
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
                    ResultLabel.Text = Calculate(ResultLabel.Text);
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
