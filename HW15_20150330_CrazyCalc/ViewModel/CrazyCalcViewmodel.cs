using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW15_20150330_CrazyCalc.Model;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows;

namespace HW15_20150330_CrazyCalc.ViewModel
{
    /// <summary>
    /// ViewModel for Crazy Calculator MVVM realization
    /// Realizes INotifyPropertyChanged for UpdateTrigger in View
    /// </summary>
    class CrazyCalcViewmodel : INotifyPropertyChanged
    {
        #region Variables
        /// <summary>
        /// Model of Crazy Calculator
        /// </summary>
        CrazyCalcModel _calc;
        /// <summary>
        /// Displayed Equatation in TextBlock of Calculator View
        /// </summary>
        StringBuilder equat;

        /// <summary>
        /// Contains 16 keys (for each button except "Del") and "coordinates" for each button in Calculator View
        /// </summary>
        public Dictionary<int, Point> ButCoord
        { get; set; }

        /// <summary>
        /// ICommand for any of 16 buttons
        /// </summary>
        private ICommand _buttonPress;
        /// <summary>
        /// ICommand for Del button
        /// </summary>
        private ICommand _DelPress;

        /// <summary>
        /// realization for INotifyPropertyChanged interface
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
        /// <summary>
        ///Property to Bind ICommand to Buttons
        /// </summary>
        public ICommand ButtonPress
        {
            get { return _buttonPress; }
        }
        /// <summary>
        /// Property to Bind ICommand to Del Button
        /// </summary>
        public ICommand DelPress
        {
            get { return _DelPress; }
        }

        /// <summary>
        /// Property for easy usage of Calculator model;
        /// </summary>
        public CrazyCalcModel Calculator
        {
            get { return _calc; }
        }

        /// <summary>
        /// Getter for Equatation
        /// used for Binding Calculator equatation to Calculator Textblock content
        /// Contains digits, operation symbols
        /// </summary>
        public string Equatation
        {
            get
            {
                return equat.ToString();
            }
        }

        #endregion

        #region Constructors
        public CrazyCalcViewmodel()
        {
            equat = new StringBuilder();
            equat.Append("0");
            _calc = new CrazyCalcModel();
            ButCoord = new Dictionary<int, Point>();
            ///adding "coordinates" to each button
            ButCoord.Add(1, new Point(0, 0));
            ButCoord.Add(2, new Point(0, 1));
            ButCoord.Add(3, new Point(0, 2));
            ButCoord.Add(4, new Point(0, 3));
            ButCoord.Add(5, new Point(1, 0));
            ButCoord.Add(6, new Point(1, 1));
            ButCoord.Add(7, new Point(1, 2));
            ButCoord.Add(8, new Point(1, 3));
            ButCoord.Add(9, new Point(2, 0));
            ButCoord.Add(10, new Point(2, 1));
            ButCoord.Add(11, new Point(2, 2));
            ButCoord.Add(12, new Point(2, 3));
            ButCoord.Add(13, new Point(3, 0));
            ButCoord.Add(14, new Point(3, 1));
            ButCoord.Add(15, new Point(3, 2));
            ButCoord.Add(16, new Point(3, 3));

            _buttonPress = new Command<string>(DoOper);
            _DelPress = new Command<object>(ClearEntries);
        }

        #endregion

        #region Methods
        /// <summary>
        /// Method for ICommand _buttonPress for each of 16 buttons except Del
        /// </summary>
        /// <param name="oper"></param>
        private void DoOper(string oper)
        {
            string[] tmp;
            tmp = Equatation.Split(' ');

            ///Oper contains "Command Parameter" property from each button of View
            ///Analyzing which button was pressed
            switch (oper)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case "0":
                    ///adding digit to Equatation string (TextBlock)
                    if (Equatation == "0")
                        equat = new StringBuilder(oper);
                    else
                        equat.Append(oper);

                    break;

                case ".":
                    ///adding point (coma) to Equatation string
                    if (tmp.Length == 1 && !tmp[0].Contains(","))
                        equat.Append(",");
                    else
                        if (tmp.Length == 3 && !tmp[2].Contains(","))
                        equat.Append(",");
                    break;
                case "+":
                case "-":
                case "*":
                case "/":
                    ///Because of I add "space" between first argument and operation,
                    ///and between operation and second argument, I can check via Equatation.Split
                    ///what already can be passed to Calculator Model:
                    ///first argument or operation sign or second argument
                    if (tmp.Length < 2)
                    {
                        ///passing first argument to Model
                        Calculator.FirstNum = Convert.ToDouble(Equatation);
                        ///passing operation sign to Model
                        Calculator.Operation = oper;
                        //adding operation sign to Equatation string
                        equat.Append(" " + oper + " ");
                    }
                    else
                    {
                        ///if there is first argument and operation sign in Equatation string,
                        ///and second argument is not null and operation button is pressed
                        ///calculate result...
                        if (tmp.Length == 3
                            && !string.IsNullOrEmpty(tmp[2]))
                        {
                            Calculator.SecondNum = Convert.ToDouble(tmp[2]);
                            equat.Clear();
                            ///...put it as first argument and add next operation sign
                            equat.Append(Calculator.Result + " " + oper + " ");
                            Calculator.FirstNum = Calculator.Result;
                            Calculator.Operation = oper;
                        }
                        else
                        /// if second argument is absent (null) replace operation sign
                        /// in Equatation string and in Calculator Model
                        {
                            string fn = tmp[0];
                            equat.Clear();
                            equat.Append(fn + " " + oper + " ");
                            Calculator.Operation = oper;
                        }
                    }
                    break;
                case "=":
                    if (tmp.Length == 3 &&
                        !string.IsNullOrEmpty(tmp[2]))
                    {
                        Calculator.SecondNum = Convert.ToDouble(tmp[2]);
                        equat.Clear();
                        equat.Append(Calculator.Result);
                        Calculator.FirstNum = Calculator.Result;
                    }
                    break;
                default:
                    break;
            }
            ///Tell View that Equatation property has been changed
            CallPropertyChanged("Equatation");

            ///Shuffle Buttons (see below) :)
            ShuffleButtons();
        }

        /// <summary>
        /// Shuffles coordinates of buttons
        /// </summary>
        private void ShuffleButtons()
        {
            Random rand = new Random();
            for (int i = 0; i < ButCoord.Count + rand.Next(1, 100); i++)
            {
                ///receiving r1 and r2 as keys for Button Coordinates Dictionary
                int r1 = rand.Next(1, ButCoord.Count + 1);
                int r2 = rand.Next(1, ButCoord.Count + 1);
                ///using r1 and r2 changing button coordinates for 2 buttons
                Point tmp = ButCoord[r1];
                ButCoord[r1] = ButCoord[r2];
                ButCoord[r2] = tmp;
            }
            ///Tell View that Button Coordinates Dictionary property has changed
            CallPropertyChanged("ButCoord");
        }

        /// <summary>
        /// Used for ICommand DelPress
        /// Clears Equatation string and all arguments in Calculator Model
        /// </summary>
        /// <param name="par"></param>
        private void ClearEntries(object par)
        {
            Calculator.FirstNum = 0;
            Calculator.SecondNum = 0;
            Calculator.Operation = null;
            equat.Clear();
            equat.Append("0");
            CallPropertyChanged("Equatation");
        }

        /// <summary>
        /// Method to raise PropertyChanged event
        /// </summary>
        /// <param name="Property"></param>
        private void CallPropertyChanged(string Property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Property));
        }

        #endregion
    }
}
