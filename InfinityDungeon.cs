using System;
using System.Collections.Generic;
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
        Character player;
        // 적 한며 명
        Character enemy;

        // 게임 시작
        public void GameStart(Character whoPlayer)
        {
            player = whoPlayer;
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
            

        }

        public void MainScreen()
        {
            int main = Select("Infinity Dungeon", new string[] { "게임시작", "옵션", "게임종료" });
            switch(main)
            {
                case 0:
                    break;
                case 1:
                    OptionScreen();
                    break;
                case 2:
                    break;

            }
        }

        public void OptionScreen(int bar = 0)
        {
            int page = Select("환경 설정", new string[] { "게임플레이", "\x1b[90m조작\x1b[0m", "\x1b[90m사운드\x1b[0m", "\x1b[90m그래픽\x1b[0m", "\x1b[90m언어\x1b[0m", "뒤로" }, bar);
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
                Console.Write("  ");
                Console.WriteLine(question);
                Console.WriteLine("==============================================");
                for(int i=0; i < choices.Length; i++)
                {
                    if (i == select) Console.Write("->");
                    else Console.Write("  ");
                    Console.WriteLine(choices[i]);
                }

                InputKey input = InputKey.None;
                while(true)
                {
                    input = GetKeyDown();
                    if(input == InputKey.SELECT)
                    {
                        return select;
                    }
                    else if(input == InputKey.CANCEL)
                    {
                        // 취소를 누르면 맨 아래 선택지
                        return choices.Length - 1;
                    }
                    else if(input == InputKey.UP) 
                    {
                        // A : 끝에 가면 막기
                        // if (select > 0) select--;
                        select = Math.Clamp(select - 1, 0, choices.Length - 1);

                        // B : 반대쪽으로 가기
                        // 나머지를 사용하면 돌아갈 수 있다.
                        // ※ 모듈로 연산을 쓸 때에 음수 값이 될 수 있는 여지가 있으면 전체 개수를 숫자에 더해놓는다.
                        // if (select < 0) select += choices.Length;
                        // select = select % choices.Length - 1;
                        break;
                    }
                    else if(input == InputKey.DOWN)
                    {
                        select = Math.Clamp(select + 1, 0, choices.Length - 1);
                        break;
                    }
                }
            }
        }

        public static InputKey GetKeyDown()
        {
            ConsoleKeyInfo inputKey = Console.ReadKey();
            switch (inputKey.Key)
            {
                case ConsoleKey.UpArrow:
                    return InputKey.UP;
                case ConsoleKey.DownArrow:
                    return InputKey.DOWN;
                case ConsoleKey.Spacebar:
                case ConsoleKey.Enter:
                case ConsoleKey.Z:
                    return InputKey.SELECT;
                case ConsoleKey.Escape:
                case ConsoleKey.X:
                    return InputKey.CANCEL;
                default:
                    return InputKey.None;
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
