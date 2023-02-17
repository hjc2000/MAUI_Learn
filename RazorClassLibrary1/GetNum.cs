using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorClassLibrary1
{
	public class GetNum
	{
		IJSObjectReference? _module = null;
		readonly ValueTask<IJSObjectReference> _moduleTask;//异步任务，用于加载模块

		public GetNum(IJSRuntime jSRuntime)
		{
			_moduleTask = jSRuntime.InvokeAsync<IJSObjectReference>(
		"import",
			"./_content/RazorClassLibrary1/js/index.js");
		}

		public async ValueTask<int> GetAsync()
		{
			if (_module == null)
			{
				_module = await _moduleTask;
			}
			return await _module.InvokeAsync<int>("GetNum");
		}
	}
}
