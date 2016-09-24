using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MainSite
{
	class Settings
	{
		private static Settings instance;
		private Settings() { }
		public static Settings Instance
		{
			get
			{				
				return instance ?? (instance = new Settings());
			}
		}

		public List<string> AvailableTimes
		{
			get { return ConfigurationManager.AppSettings["availableTimes"].Split(',').ToList(); }
			set { ConfigurationManager.AppSettings["availableTimes"] = string.Join(",",value); }
		}
	}
}
