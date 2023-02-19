using System.Runtime.InteropServices;

namespace DllOperator
{
	public class DllLoader : IDisposable
	{
		public DllLoader(string dll_file_path)
		{
			int h = 0;
			int flags = (int)LoaderOptimization.MultiDomain;
			_hDll = LoadLibrary(dll_file_path, h, flags);
		}

		private IntPtr _hDll = IntPtr.Zero;//DLL模块的句柄

		/// <summary>
		/// 获取DLL中的对象的指针
		/// </summary>
		/// <param name="proc_name">对象名称</param>
		/// <returns></returns>
		public IntPtr GetProcAddress(string proc_name)
		{
			return GetProcAddress(_hDll, proc_name);
		}

		private bool _freeSuccessfully = false;//标识是否成功释放DLL模块

		#region 导入kernel32.dll中的函数
		[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
		private static extern IntPtr LoadLibrary(string dll_file_path, int h, int flags);
		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr GetProcAddress(IntPtr h_dll, string proc_name);
		[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
		private static extern bool FreeLibrary(IntPtr h_dll);
		#endregion

		/// <summary>
		/// 释放DLL模块
		/// </summary>
		public void Dispose()
		{
			if (!_freeSuccessfully)
			{
				_freeSuccessfully = FreeLibrary(_hDll);
			}
		}
	}
}
