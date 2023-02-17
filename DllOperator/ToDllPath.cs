using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DllOperator
{
	public class ToDllPath
	{
		/// <summary>
		/// 获取进程可执行文件所在的路径
		/// </summary>
		/// <returns></returns>
		public static string GetProcessPath()
		{
			var moudle = Process.GetCurrentProcess().MainModule;
			string reStr = string.Empty;
			if (moudle != null)
			{
				reStr = moudle.FileName ?? string.Empty;
			}
			return reStr;
		}
	}
}
