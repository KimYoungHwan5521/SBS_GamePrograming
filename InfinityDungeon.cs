using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS
{
    public enum InputKey
    {
        None, UP, RIGHT, DOWN, LEFT, SELECT, CANCEL
    }

    internal class InfinityDungeon
    {
        // 플레이할 캐릭터 한 명
        CharacterDungeon player;
        // 적 한며 명
        CharacterDungeon enemy;

        // 게임 시작
        public void GameStart(CharacterDungeon whoPlayer)
        {
            player = whoPlayer;
            if (player == null)
            {
                MainScreen();
                return;
            }
            Console.Clear();
            DrawText("LOADING...");
            Place.Initialize();
            
            Place.palletTown.Enter(player);
            
        }

        public void Prologue()
        {
            // 내가 만든 클래스가 아닌, 모든 클래스에다가 기능을 추가하기
            // C#의 모든 요소에는 메소드가 들어가 있습니다.
            // string에도 기능을 넣어보자.
            Console.Clear();
            DrawText($"아주 먼 옛날 {"무시무시".Color(Extension.TextColor.Red)}한 일이 있었다.");
            NextPage();
            DrawText("이건... 그 이야기이다....");
            NextPage();
            GameStart(CharacterSettingScreen());

        }

        public void MainScreen()
        {
            int main = Select("Infinity Dungeon", new string[] { "게임시작", "옵션", "게임종료" });
            switch(main)
            {
                case 0:
                    Prologue();
                    break;
                case 1:
                    OptionScreen();
                    break;
                case 2:
                    break;

            }
        }

        public CharacterDungeon CharacterSettingScreen()
        {
            int select = 0;
            string name = "";
            int gender = 0;
            while(true)
            {
                Console.Clear();
                Console.WriteLine("====================================================");
                Console.WriteLine("                     캐릭터 생성                     ");
                Console.WriteLine("====================================================");

                Console.WriteLine("  이름을 입력해 주세요");
                if (select == 0) Console.Write("->");
                else Console.Write("  ");
                Console.WriteLine($"[{name}]");

                Console.WriteLine("           성별");
                if (select == 1) Console.Write("->");
                else Console.Write("  ");
                if(gender == 0) Console.WriteLine("    남성    /    \x1b[90m여성\x1b[0m    ");
                else Console.WriteLine("    \x1b[90m남성\x1b[0m    /    여성    ");

                if (select == 2) Console.Write("->");
                else Console.Write("  ");
                Console.WriteLine("완료");

                if (select == 3) Console.Write("->");
                else Console.Write("  ");
                Console.WriteLine("취소");
                switch(select)
                {
                    case 0:
                        // 1. 입력을 두 번 받음
                        // 2. Z나 X 등을 누를 때 VerticalSelect가 안되야됌
                        if(GetCharDown(ref name, out ConsoleKey key) == false)
                        {
                            // VerticalSelect에는 내가 준 키를 기준으로 하는 것도 생각
                            VerticalSelect(ref select, 4, FunctionKey(key));
                        }
                        break;
                    case 1:
                        InputKey input = GetKeyDown();
                        HorizontalSelect(ref gender, 2, input);
                        VerticalSelect(ref select, 4, input);
                        break;
                    case 2:
                        if (VerticalSelect(ref select, 4) == true)
                        {
                            // 이름 제대로 적었는지 체크
                            if(name.Length == 0 || name.Length > 8)
                            {
                                Console.WriteLine("<<이름은 1글자 이상 8글자 이하로 적어주세요>>".RedColor());
                                Console.ReadKey();
                            }
                            else
                            {
                                return new CharacterDungeon(name, 500, 500, 50, 0);
                            }

                        }
                        break;
                    case 3:
                        if (VerticalSelect(ref select, 4) == true)
                        {
                            return null;
                        }
                        break;
                }
            }
        }

        public void OptionScreen(int bar = 0)
        {
            int page = Select("환경 설정", new string[] { "게임플레이", "조작".DarkColor(), "사운드".DarkColor(), "그래픽".DarkColor(), "언어".DarkColor(), "뒤로" }, bar);
            switch(page)
            {
                case 0: OptionGamePlayScreen(); break;
                case 5: MainScreen(); break;
                default: OptionScreen(page); break;
            }
        }
        
        static bool fastPrint = false;
        public void OptionGamePlayScreen()
        {
            int page = Select("환경설정", new string[] { $"대화 속도 : {(fastPrint ? "빠름" : "보통")}", "뒤로" });
            switch(page)
            {
                case 0: 
                    fastPrint = !fastPrint; 
                    OptionGamePlayScreen();
                    break;
                case 1: OptionScreen(); break;
            }
        }

        // Q: 길가다 쓰러져있는 사람을 만났습니다.
        // [공격한다]
        // [무시한다]
        // [환대한다]
        public static int Select(string question, string[] choices, int select = 0)
        {
            while(true)
            {
                Console.Clear();
                Console.WriteLine("==============================================");
                Console.Write("    ");
                Console.WriteLine(question);
                Console.WriteLine("==============================================");
                for(int i=0; i < choices.Length; i++)
                {
                    if (i == select) Console.Write("->");
                    else Console.Write("  ");
                    Console.WriteLine(choices[i]);
                }

                // ref(reference) : 참조 (본인이 직접 접근)
                if(VerticalSelect(ref select, choices.Length) == true)
                {
                    return select;
                }
            }
        }

        // 1. 눌려진 키가 뭔지 아는 상태
        // 키를 받았는데 위아래도 아니고 글자도 아니면?
        // 2. 키가 눌리지 않았음 -> 내가 키 받을게
        public static bool HorizontalSelect(ref int select, int length, InputKey key)
        {
            // 많으면 순환, 적으면 막힘
            bool isManySelections = length > 3;
            if (key == InputKey.RIGHT)
            {
                if (!isManySelections)
                {
                    select = Math.Clamp(select + 1, 0, length - 1);
                }
                else
                {
                    select = (select + 1) % length;
                }
            }
            else if (key == InputKey.LEFT)
            {
                if (!isManySelections)
                {
                    select = Math.Clamp(select - 1, 0, length - 1);
                }
                else
                {
                    select += length - 1;
                    select = select % length;
                }
            }
            else return false;
            return true;
        }


        // 예/아니오/모른다
        // [자료형?] => nullable
        // 가지고 있는 축구 유니폼의 번호
        // 5 / 7 / 없다
        // 예 / 아니오 / 다른 키
        // overload (과적) : 같은 이름의 함수에 매개변수가 다른 기능을 쌓아놓는것

        /// <summary>
        /// 수직으로 된 옵션을 선택하는 함수
        /// </summary>
        /// <param name="select">현재 옵션의 위치</param>
        /// <param name="length">전체 옵션의 개수</param>
        /// <param name="input">들어온 키 입력</param>
        /// <returns>선택 완료 여부</returns>
         
        public static bool? VerticalSelect(ref int select, int length)
        {
            // 키가 안들어온 VerticalSelect
            bool? result = null;

            while(result == null)
            {
                result = VerticalSelect(ref select, length, GetKeyDown());
            }
            return result;
        }
        
        public static bool? VerticalSelect(ref int select, int length, InputKey input)
        {
            bool isManySelections = length > 3;
            if (input == InputKey.SELECT)
            {
                return true;
            }
            else if (input == InputKey.CANCEL)
            {
                // 취소를 누르면 맨 아래 선택지
                select = length - 1;
                return true;
            }
            else if (input == InputKey.UP)
            {
                // A : 끝에 가면 막기
                if (!isManySelections)
                {
                    select = Math.Clamp(select - 1, 0, length - 1);
                }
                else
                {
                    // B : 반대쪽으로 가기
                    // 나머지를 사용하면 돌아갈 수 있다.
                    // ※ 모듈로 연산을 쓸 때에 음수 값이 될 수 있는 여지가 있으면 전체 개수를 숫자에 더해놓는다.
                    select += length - 1;
                    select = select % length;
                }
                return false;

            }
            else if (input == InputKey.DOWN)
            {
                if (!isManySelections)
                {
                    select = Math.Clamp(select + 1, 0, length - 1);
                }
                else
                {
                    select = (select + 1) % length;
                }
                return false;
            }
            else return null;
        }
        


        // bool < 글자를 입력했다면 다른데에서 키를 쓸 수도 있어야 해서 키를 알려줘야하고 글자를 입력했다라고 알려줄 수도 있어야 함.
        // ref : 밖에 있는 걸 갖다 쓰겠다.
        // out : 밖에다가 주는 용도                      
        public static bool GetCharDown(ref string result, out ConsoleKey key)
        {
            ConsoleKeyInfo inputKey = Console.ReadKey();
            bool isCharDown = true;
            // Key 코드가 A 이상 Z 이하
            if ((ConsoleKey.A <= inputKey.Key && ConsoleKey.Z >= inputKey.Key) || (ConsoleKey.D0 <= inputKey.Key && ConsoleKey.D9 >= inputKey.Key))
            {
                // ascii Code : Char 자료형!
                result = result + (char)inputKey.Key;
            }
            else if (inputKey.Key == ConsoleKey.Backspace)
            {
                // string에는 문자열을 바꿀 수 있는 많은 함수들이 있다.
                // 실행한다고 원본이 바뀌는 것은 아니다.
                // 결과를 다시 변수에 담아야한다.
                if(result.Length > 0) result = result.Remove(result.Length - 1);
            }
            else isCharDown = false;

            // 약속한 장소에 약속한 물건을 놓고 가야함
            key = inputKey.Key;
            return isCharDown;
        }

        // GetKeyDown 기능을 두 개로 분리
        // 1. 키 입력 요청
        // 2. 키 판단
        public static InputKey GetKeyDown()
        {
            ConsoleKeyInfo inputKey = Console.ReadKey();
            return FunctionKey(inputKey.Key);
        }

        public static InputKey FunctionKey(ConsoleKey key) 
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:    return InputKey.UP;
                case ConsoleKey.DownArrow:  return InputKey.DOWN;
                case ConsoleKey.RightArrow:  return InputKey.RIGHT;
                case ConsoleKey.LeftArrow:  return InputKey.LEFT;
                case ConsoleKey.Spacebar:
                case ConsoleKey.Enter:
                case ConsoleKey.Z:          return InputKey.SELECT;
                case ConsoleKey.Escape:
                case ConsoleKey.X:          return InputKey.CANCEL;
                default:                    return InputKey.None;
            }
        }

        public static void NextPage() 
        {
            Console.WriteLine("[계속 : Enter]");
            Console.ReadLine();
            Console.Clear();
        }

        // 텍스트를 받았을 때 한 번에 출력하는 것이 아니라 속도에 맞춰 천천히 출력해주기
        // InfinityDungeon이라는 객체가 존재하지 않아도 어디에서든 잘 출력 되었으면 좋겠다.
        // 객체와 관계없이 프로그램에 종속되어서 어디에서든 사용할 수 있는 static
        public static void DrawText(string wantString, int delay = 50)
        {
            bool skip = false;
            for(int i=0; i<wantString.Length; i++)
            {
                Console.Write(wantString[i]);
                if (wantString[i] == '\x1b') skip = true;
                if(skip && wantString[i] == 'm') skip = false;
                if(skip || fastPrint) Thread.Sleep(1);
                else if (wantString[i] == '.') Thread.Sleep(delay * 6);
                else Thread.Sleep(delay);
            }
            Console.WriteLine();
        }
    }
}
