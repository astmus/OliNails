using OliMaster;
using OliMaster.Droid.OliNailsService;
using System.Threading.Tasks;

[System.Web.Services.WebServiceBindingAttribute(Name = "BasicHttpBinding_IOliService", Namespace = "http://tempuri.org/")]
public class OliServiceDelegate : OliService, IOliServiceDelegate
{	
	public OliServiceDelegate()
	{
		
	}

	public new Task<string> GetHelloWorldAsync()
	{
		return Task.Run<string>(() => { return GetHelloWorld(); });
	}
}

