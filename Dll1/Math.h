#pragma once

#ifdef DLL1_EXPORTS
	#define API __declspec(dllexport)
#else
	#define API __declspec(dllimport)
#endif // DLL1_EXPORTS

namespace myLib
{
	class API Math
	{
	public:
		static int Add(int a, int b);
	};
}