using Microsoft.JSInterop;
using RazorClassLibrary1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trumpeter.Pages
{
	public partial class NameListEditor:IDisposable
	{
		#region 构造、析构、释放资源
		public NameListEditor()
		{
			//获取应用数据储存路径
			_storageDir = FileSystem.Current.AppDataDirectory;
			_storageDir = _storageDir.Replace('\\', '/');

			//初始化文件流和读写器
			string filePath = _storageDir + "/name-list.txt";
			_fileStream = File.Open(filePath, FileMode.OpenOrCreate);
			_writer = new StreamWriter(_fileStream);
			_reader = new StreamReader(_fileStream);
		}

		~NameListEditor()
		{
			Dispose();
		}

		public void Dispose()
		{
			_writer.Dispose();
			_reader.Dispose();
			_fileStream.Dispose();
		}
		#endregion

		#region 类私有字段
		string _storageDir;	//应用数据储存路径
		Stream _fileStream;	//文件流
		StreamWriter _writer;	//文件流读取器
		StreamReader _reader;   //文件流写入器
		#endregion

		#region 生命周期事件
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				_textAreaValue = await _reader.ReadToEndAsync();
				_fileStream.Position = 0;
				StateHasChanged();
			}
		}
		#endregion


		#region 绑定到DOM中的变量
		string _textAreaValue = string.Empty;
		#endregion

		#region DOM事件
		async void OnClick()
		{
			/*将流的大小设置为0，则将文件大小设置为0.不这么做的话如果原本文件较大，我们这次
			写入较短的内容后，打开文件就会发现文件的开头是我们写的内容，后面跟着一些原本的内容
			因为写入文件流是从当前的位置指针开始往后覆盖内容。如果写入的内容长度超过文件原本的
			长度，则文件会变大*/
			_fileStream.SetLength(0);
			await _writer.WriteAsync(_textAreaValue);
			await _writer.FlushAsync();
		}
		#endregion
	}
}
