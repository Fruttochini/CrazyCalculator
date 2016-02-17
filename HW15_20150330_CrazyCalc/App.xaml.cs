using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using HW15_20150330_CrazyCalc.View;
using HW15_20150330_CrazyCalc.ViewModel;

namespace HW15_20150330_CrazyCalc
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public App()
		{
			var mw = new MainWindow() { DataContext = new CrazyCalcViewmodel() };
			mw.Show();
		}
	}
}
