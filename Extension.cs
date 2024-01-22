using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS
{
    // 해당 자료형의 기능 확장
    // 개별 객체마다 있는 걸까?
    // 특별한 한 명 한테만 기능을 적용하는게 아니라 모두에게 적용
    internal static class Extension
    {
        // 숫자 하나를 가지고 약간의 조작을 해봅시다.
        // 정규화
        // normalize : 벡터의 길이를 1로 만들어서 방향을 나타낸다.
        // this는 딱 하나만 붙을 수 있다.
        // this가 붙은 경우에는 매개변수x 대상(자기자신)o
        public static int Normalize(this float value)
        {
            if (value < 0) return -1;
            else if (value > 0) return 1;
            else return 0;
        }
    }
}
