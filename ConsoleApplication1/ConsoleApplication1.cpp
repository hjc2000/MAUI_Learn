#include <iostream>
/*下面使用了相对路径来找到 Dll1 项目中的 Math.h 文件。因为标准库中
有一个 math.h 所以如果将 Math.h 复制到控制台项目的“头文件”文件夹中，
然后直接

#include"Math.h"

会报错，使用相对路径

#include"./Math.h"

也会报错，编译器都以为是要找标准库的 math.h
*/
#include "../Dll1/Math.h"

void Print(void);

int main()
{
    Print();
    std::cout << myLib::Math::Add(665, 1) << std::endl;
    getchar();
    getchar();
    return 0;
}