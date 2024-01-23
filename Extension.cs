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

        //    생명력    추가 공격력(%)
        // fromA   fromB   toA  toB
        // (1500 ~ 500) ->  (0.1 ~ 0.9)
        // ex)  1250    ->  약 0.25
        //      1000    ->   0.5
        public static float GetPercent(this float value, float fromA, float fromB, float toA, float toB)
        {
            // 빼고 나눔
            float fromPercent = (fromB - value) / (fromB - fromA);
            // 0 ~ 1 범위로 만든 것을 0.1 ~ 0.9로
            // 빼고 나눔 -> 곱하고 더함
            float result = fromPercent * (toB - toA) + toA;

            return result;
        }

        // 열거형은 기본적으로 정수로 0부터 저장됨
        // 강제로 수를 바꿔 줄수 있음
        public enum TextStyle
        {
            Defalut = 0,
            Bold = 1, 
            UnderLine = 4, 
            StrikeThrough = 9,
        }

        public enum TextColor
        {
            Black = 30,
            Red = 31,
            Green = 32,
            Yellow = 33,
            Blue = 34,
            Magenta = 35,
            Cyan = 36,
            White = 37,
            Default = 39,

        }

        public static string Color(this string target, TextColor textColor = TextColor.Default, TextStyle textStyle = TextStyle.Bold, TextColor backgroundColor = TextColor.Default)
        {
            // https://gist.github.com/fnky/458719343aabd01cfb17a3a4f7296797

            // 빨강 + 밑줄
            return $"\x1b[{(int)textColor};{(int)backgroundColor + 10};{(int)textStyle}m" + target + "\x1b[0m";
        }

        public static string SelectedColor(this string target) => target.Color(TextColor.Black, TextStyle.Bold, TextColor.White);
        public static string GreenColor(this string target) => target.Color(TextColor.Green);
        public static string RedColor(this string target) => target.Color(TextColor.Red);
    }
}
