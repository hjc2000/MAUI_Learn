using System.IO;

namespace Trumpeter
{
	internal class TextEditor : IDisposable
	{
		#region 类构造、析构
		public TextEditor(string fileName)
		{
			//获取应用数据储存路径
			_storageDir = FileSystem.Current.AppDataDirectory;
			_storageDir = _storageDir.Replace('\\', '/');

			//初始化文件流和读写器
			string filePath = _storageDir + "/" + fileName;
			_fileStream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
			_writer = new StreamWriter(_fileStream);
			_reader = new StreamReader(_fileStream);
		}
		~TextEditor()
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
		private string _storageDir;  //应用数据储存路径
		private Stream _fileStream;  //文件流
		private StreamWriter _writer;    //文件流读取器
		private StreamReader _reader;   //文件流写入器
		#endregion

		#region 公共方法
#nullable enable
		/// <summary>
		/// 写入流。
		/// 写入前会先将文件大小设置为 0
		/// 写入后会自动调用 FlushAsync 以将缓冲区内容刷新到流中
		/// </summary>
		/// <param name="text">想要写入的字符串</param>
		/// <returns></returns>
		public async Task WriteAsync(string? text)
		{
			/*将流的大小设置为0，则将文件大小设置为0.不这么做的话如果原本文件较大，我们这次
			写入较短的内容后，打开文件就会发现文件的开头是我们写的内容，后面跟着一些原本的内容
			因为写入文件流是从当前的位置指针开始往后覆盖内容。如果写入的内容长度超过文件原本的
			长度，则文件会变大*/
			_fileStream.SetLength(0);
			await _writer.WriteAsync(text);
			await _writer.FlushAsync();
		}

		/// <summary>
		/// 读取流，直到末尾。然后重置流的位置指针
		/// </summary>
		/// <returns></returns>
		public async Task<string> ReadToEndAsync()
		{
			string reStr = await _reader.ReadToEndAsync();
			_fileStream.Position = 0;
			return reStr;
		}
		#endregion

	}
}
