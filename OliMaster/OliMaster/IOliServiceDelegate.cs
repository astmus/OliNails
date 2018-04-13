using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OliMaster
{
    public interface IOliServiceDelegate
    {
		string GetData(int value);
		string GetHelloWorld();
		Task<string> GetHelloWorldAsync();
	}
}
