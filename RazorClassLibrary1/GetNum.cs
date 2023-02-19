using Microsoft.JSInterop;

namespace RazorClassLibrary1
{
	public class GetNum
	{
		private IJSObjectReference? _module = null;
		private readonly ValueTask<IJSObjectReference> _moduleTask;//异步任务，用于加载模块

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
