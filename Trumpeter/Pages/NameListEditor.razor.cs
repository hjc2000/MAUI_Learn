using Microsoft.JSInterop;
using RazorClassLibrary1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trumpeter.Pages
{
	public partial class NameListEditor
	{
		protected override void OnAfterRender(bool firstRender)
		{
			if(firstRender)
			{
				_getNum = new(JS);
			}
		}

		GetNum _getNum;
	}
}
