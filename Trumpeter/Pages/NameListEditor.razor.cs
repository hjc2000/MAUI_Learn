namespace Trumpeter.Pages
{
	public partial class NameListEditor : IDisposable
	{
		#region 构造、析构、释放资源
		public NameListEditor()
		{
			_textEditor = new TextEditor("name-list.txt");
		}
		~NameListEditor()
		{
			Dispose();
		}
		public void Dispose()
		{
			_textEditor.Dispose();
		}
		#endregion

		#region 类私有字段
		private TextEditor _textEditor;
		#endregion

		#region 生命周期事件
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				_textAreaValue = await _textEditor.ReadToEndAsync();
				StateHasChanged();
			}
		}
		#endregion


		#region 绑定到DOM中的变量
		private string _textAreaValue = string.Empty;
		#endregion

		#region DOM事件
		private async void OnClick()
		{
			await _textEditor.WriteAsync(_textAreaValue);
		}
		#endregion
	}
}
