using System.Text.RegularExpressions;

namespace Trumpeter.Pages
{
	public partial class Index : IDisposable
	{
		#region 构造、析构
		public Index()
		{
			_textEditor = new TextEditor("name-list.txt");
			Task.Run(async () =>
			{
				string readResult = await _textEditor.ReadToEndAsync();
				_names = readResult.Split(new char[] { '\n', '\r' },
					StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
			});
		}
		~Index()
		{
			Dispose();
		}
		public void Dispose()
		{
			_textEditor.Dispose();
		}
		#endregion

		#region 生命周期事件
		protected override void OnInitialized()
		{
			if (TrumpeterStorage.FirstInit)
			{
				TrumpeterStorage.FirstInit = false;
				Input1 = "1";
				Input2 = string.Empty;
			}

			// 判断按钮是否可用
			ChangeButtonState();
		}
		#endregion

		#region 私有字段、属性
		TextEditor _textEditor;
		string[] _names;

		/// <summary>
		/// 互斥锁，不允许两个线程同时操作摇号结果
		/// </summary>
		private Mutex _mutex = new Mutex();

		/// <summary>
		/// 停止摇号的信号
		/// </summary>
		private bool _stop = true;

		/// <summary>
		/// 摇号范围的下限。这个参数不允许直接设置，设置 Input1 属性的时候会自动设置
		/// TrumpeterStorage.Min 从而间接设置本属性
		/// </summary>
		private int Min
		{
			get
			{
				return TrumpeterStorage.Min;
			}
		}

		/// <summary>
		/// 摇号范围的上限。这个参数不允许直接设置，设置 Input2 属性的时候会自动设置
		/// TrumpeterStorage.Max 从而间接设置本属性
		/// </summary>
		private int Max
		{
			get
			{
				return TrumpeterStorage.Max;
			}
		}

		/// <summary>
		/// 正则规则，匹配所有非数字的字符串
		/// </summary>
		private Regex _regex = new Regex(@"\D");

		/// <summary>
		/// 产生随机数的对象
		/// </summary>
		private Random _random = new Random((int)DateTime.Now.Ticks);
		#endregion

		#region 绑定到DOM的属性、字段
		private string ButtonText = "开始摇号";
		private bool ButtonDisabled = true;
		private bool _checked = false;

		/// <summary>
		/// 摇号结果的数字
		/// </summary>
		private int DisplayNum
		{
			get
			{
				return TrumpeterStorage.DisplayNum;
			}
			set
			{
				TrumpeterStorage.DisplayNum = value;
			}
		}

		/// <summary>
		/// 显示到界面中的名字
		/// </summary>
		private string DisplayName
		{
			get
			{
				return TrumpeterStorage.DisplayName;
			}
			set
			{
				TrumpeterStorage.DisplayName = value;
			}
		}

		/// <summary>
		/// 输入框1
		/// </summary>
		private string Input1
		{
			get
			{
				return TrumpeterStorage.Input1;
			}
			set
			{
				TrumpeterStorage.Input1 = value;
				TrumpeterStorage.Input1 = _regex.Replace(TrumpeterStorage.Input1, string.Empty);
				try
				{
					TrumpeterStorage.Min = int.Parse(TrumpeterStorage.Input1);
				}
				catch
				{
					/*删除非法字符后变成空字符串，就会引发异常。正则表达式匹配非数字的
					 字符串，然后我们删除它。但是，int.Parse 失败除了因为有非数字字符
					外，还可能是因为输入了空字符串*/
					TrumpeterStorage.Min = 0;
				}

				ChangeButtonState();
			}
		}

		/// <summary>
		/// 输入框2
		/// </summary>
		private string Input2
		{
			get
			{
				return TrumpeterStorage.Input2;
			}
			set
			{
				TrumpeterStorage.Input2 = value;
				TrumpeterStorage.Input2 = _regex.Replace(TrumpeterStorage.Input2, string.Empty);

				try
				{
					TrumpeterStorage.Max = int.Parse(TrumpeterStorage.Input2);
				}
				catch
				{
					/*删除非法字符后变成空字符串，就会引发异常。正则表达式匹配非数字的
					 字符串，然后我们删除它。但是，int.Parse 失败除了因为有非数字字符
					外，还可能是因为输入了空字符串*/
					TrumpeterStorage.Max = 0;
				}

				ChangeButtonState();
			}
		}
		#endregion

		#region 私有方法
		/// <summary>
		/// 判断按钮是否可用并更新状态
		/// </summary>
		private void ChangeButtonState()
		{
			if (_checked)
			{
				if (_names.Length > 0)
				{
					ButtonDisabled = false;
				}
				else
				{
					ButtonDisabled = true;
				}
			}
			else
			{
				if (Min < Max)
				{
					ButtonDisabled = false;
				}
				else
				{
					ButtonDisabled = true;
				}
			}
		}
		#endregion

		#region DOM事件
		private async Task OnButtonClick()
		{
			_stop = !_stop;
			if (_stop)
			{
				ButtonText = "开始摇号";
			}
			else
			{
				ButtonText = "停止摇号";
			}

			// 临界区,禁止多个线程同时操作号码
			_mutex.WaitOne();
			while (!_stop)
			{
				if (_checked)
				{
					//读取名单，从名单中选出名字
					int index = _random.Next(0, _names.Length);
					DisplayName = _names[index];
				}
				else
				{
					DisplayNum = _random.Next(Min, Max + 1);
				}
				StateHasChanged();
				await Task.Delay(25);
			}
			_mutex.ReleaseMutex();
		}
		void OnCheckChange()
		{
			ChangeButtonState();
		}
		#endregion
	}

	public class TrumpeterStorage
	{
		public int Max { get; set; } = 0;
		public int Min { get; set; } = 0;
		public string Input1 { get; set; } = string.Empty;
		public string Input2 { get; set; } = string.Empty;
		public bool FirstInit = true;
		public int DisplayNum { get; set; } = 0;
		public string DisplayName { get; set; } = string.Empty;
	}
}
