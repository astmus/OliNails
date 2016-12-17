using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

namespace MainSite.Styles
{
	public static class StyleColors
	{
		public static Color Active = Color.FromArgb(1, 76, 175, 80);
		public static Color TableHeaderBackground = Color.FromArgb(1, 80, 80, 80);
		public static Color EvenLines = Color.FromArgb(1, 230, 230, 245);
		public static Color NotEvenLines = Color.FromArgb(1, 245, 240, 230);
		public static Color Reserved = Color.FromArgb(1, 228, 83, 131);
		private static Lazy<Color> _lightActive = new Lazy<Color>(()=> 
		{
			return Color.FromArgb(1, Active.R + 40, Active.G + 40, Active.B + 40);
		});
		public static Color LightActive = _lightActive.Value;
	}
}