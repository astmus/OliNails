using System;
using System.IO;
using System.Web;

namespace MainSite
{
	public class Logger
	{
		private string pathToCurrentLogFile;
		private Logger()
		{
			pathToCurrentLogFile = HttpContext.Current.Server.MapPath("~/Logs/log.txt");
			CreateLogFileIfNotExists();
		}

		private static Logger _instance = null;

		public static Logger Instance
		{
			get { return _instance ?? (_instance = new Logger()); }
		}

		private void CreateLogFileIfNotExists()
		{
			if (System.IO.File.Exists(pathToCurrentLogFile)) return;
			//string userFilename = Path.Combine(folderPath, DateTime.Now.ToString("mm-ss") + ".txt");

			using (FileStream stream = System.IO.File.Create(pathToCurrentLogFile))
			{
				StreamWriter strwr = new StreamWriter(stream);				
				strwr.Close();
				stream.Close();
			}
		}

		public void LogError(string message)
		{ 		
			using (StreamWriter writer = new StreamWriter(pathToCurrentLogFile, true))
			{
				writer.WriteLine("Error -> "+message);
				writer.Close();
			}
		}

		public void LogDebug(string message)
		{
			using (StreamWriter writer = new StreamWriter(pathToCurrentLogFile, true))
			{
				writer.WriteLine("Debug -> "+message);
				writer.Close();
			}
		}
	}
}