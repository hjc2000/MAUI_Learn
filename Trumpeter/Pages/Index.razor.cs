namespace Trumpeter.Pages
{
	public partial class Index
	{
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

		/// <summary>
		/// 摇号范围的下限。这个参数不允许直接设置
		/// </summary>
		private int Min
		{
			get
			{
				return TrumpeterStorage.Min;
			}
		}
		/// <summary>
		/// 摇号范围的上限。这个参数不允许直接设置
		/// </summary>
		private int Max
		{
			get
			{
				return TrumpeterStorage.Max;
			}
		}

		/// <summary>
		/// 输入框1的文本
		/// </summary>
		public string Input1
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
					// 删除非法字符后变成空字符串，就会引发异常
					TrumpeterStorage.Min = 0;
				}

				ChangeButtonState();
			}
		}
		/// <summary>
		/// 输入框2的文本
		/// </summary>
		public string Input2
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
					// 删除非法字符后变成空字符串，就会引发异常
					TrumpeterStorage.Max = 0;
				}

				ChangeButtonState();
			}
		}

		/// <summary>
		/// 摇号结果的数字
		/// </summary>
		int DisplayNum
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
		/// 判断按钮是否可用并更新状态
		/// </summary>
		void ChangeButtonState()
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

	public class TrumpeterStorage
	{
		public int Max { get; set; } = 0;
		public int Min { get; set; } = 0;
		public string Input1 { get; set; } = string.Empty;
		public string Input2 { get; set; } = string.Empty;
		public bool FirstInit = true;
		public int DisplayNum { get; set; } = 0;
	}
}
