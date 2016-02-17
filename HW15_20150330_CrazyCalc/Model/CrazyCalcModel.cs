using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW15_20150330_CrazyCalc.Model
{
	/// <summary>
	/// Calculator Model for Crazy Calculator MVVM Realization
	/// </summary>
	class CrazyCalcModel
	{
		/// <summary>
		/// first number
		/// </summary>
		double _firstNum;
		/// <summary>
		/// second number
		/// </summary>
		double _secondNum;
		/// <summary>
		/// operation sign
		/// </summary>
		string _operation;

		#region Properties
		/// <summary>
		/// Return Result of Operation with FirstNum and SecondNum
		/// </summary>
		public double Result
		{
			get 
			{
				switch(_operation)
				{
					case "+":
						return _firstNum + _secondNum;
					case "-":
						return _firstNum - _secondNum;
					case "*":
						return _firstNum * _secondNum;
					case "/":
						return _firstNum / _secondNum;
					default:
						return 0;
				}
			}
		}

		/// <summary>
		/// Getter and Setter for _operation
		/// </summary>
		public string Operation
		{
			get { return _operation; }
			set { _operation = value; }
		}
		
		/// <summary>
		/// Getter and Setter for _firstNum
		/// </summary>
		public double FirstNum
		{
			get { return _firstNum; }
			set { _firstNum = value; }
		}
		/// <summary>
		/// Getter and Setter for _secondNum
		/// </summary>
		public double SecondNum
		{
			get { return _secondNum; }
			set { _secondNum = value; }
		}
#endregion

		/// <summary>
		/// Default Constructor
		/// </summary>
		public CrazyCalcModel()
		{
			Operation = null;
			FirstNum = 0;
			SecondNum = 0;
		}
	}
}
