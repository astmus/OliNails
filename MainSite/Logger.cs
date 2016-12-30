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
			pathToCurrentLogFile = MakePathForFileName(DateTime.Now.Date.ToString("yyyy-MM_") + "log.txt");
			CreateFileIfNotExists(pathToCurrentLogFile);
		}

		private static Logger _instance = null;

		public static Logger Instance
		{
			get { return _instance ?? (_instance = new Logger()); }
		}

		#region private methods

		private void CreateFileIfNotExists(string fileName)
		{
			if (System.IO.File.Exists(fileName)) return;

			using (FileStream stream = System.IO.File.Create(fileName))
			{
				stream.Close();
			}
		}

		private string MakePathForFileName(string name)
		{
			return HttpContext.Current.Server.MapPath("~/Logs/" + name);
		}

		private string FormattedLogString(string message, string type)
		{
			string address = HttpContext.Current.Request.UserHostAddress.ToString();
			return String.Format("{0} [{1}] {2} -> {3};", DateTimeHelper.currentLocalDateTime().ToString("dd.MM hh:mm:ss"), address, type, message);
		}

		#endregion

		#region public methods

		public void LogInfo(string message)
		{
			using (StreamWriter writer = new StreamWriter(pathToCurrentLogFile, true))
			{
				writer.WriteLine(FormattedLogString(message, "Info"));
				writer.Close();
			}
		}

		public void LogError(string message)
		{
			using (StreamWriter writer = new StreamWriter(pathToCurrentLogFile, true))
			{
				writer.WriteLine(FormattedLogString(message, "Error"));
				writer.Close();
			}
		}

		public void LogDebug(string message)
		{
			using (StreamWriter writer = new StreamWriter(pathToCurrentLogFile, true))
			{
				writer.WriteLine(FormattedLogString(message, "Debug"));
				writer.Close();
			}
		}

		public void LogUserCount()
		{
			string fileName = MakePathForFileName(DateTime.Now.Date.ToString("v_yyyy-MM") + ".txt");
			CreateFileIfNotExists(fileName);
			using (StreamReader reader = new StreamReader(fileName))
			{
				uint count = 0;
				uint.TryParse(reader.ReadLine(), out count);
				count++;
				reader.Close();
				using (StreamWriter writer = new StreamWriter(fileName))
				{
					writer.WriteLine(count.ToString());
					writer.Close();
				}
			}
		}

		public uint GetUserCountForCurrentMonth()
		{
			string fileName = MakePathForFileName(DateTime.Now.Date.ToString("v_yyyy-MM") + ".txt");
			if (!System.IO.File.Exists(fileName)) return 0;
			uint count = 0;
			using (StreamReader reader = new StreamReader(fileName))
			{				
				uint.TryParse(reader.ReadLine(), out count);				
				reader.Close();				
			}
			return count;
		}

		#endregion
	}
}