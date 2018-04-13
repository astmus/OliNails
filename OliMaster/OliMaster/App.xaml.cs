using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace OliMaster
{
	public partial class App : Application
	{
		public static IOliServiceDelegate OliService { get; set; }
		public App ()
		{
			InitializeComponent();

			MainPage = new OliMaster.MainPage();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
