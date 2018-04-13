using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OliMaster
{
	public partial class MainPage : ContentPage
	{
		
		public MainPage()
		{
			InitializeComponent();
		}

		private async void Button_Clicked(object sender, EventArgs e)
		{
			five.Text = await App.OliService.GetHelloWorldAsync();
		}
	}
}
