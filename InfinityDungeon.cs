using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS
{
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
            DrawText("아주 먼 옛날 무시무시한 일이 있었다.");
            NextPage();
            DrawText("아주... 무서운... 일이다...");
            NextPage();
            DrawText("이건... 그 이야기이다....");
            NextPage();
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
            for(int i=0; i<wantString.Length; i++)
            {
                Console.Write(wantString[i]);
                if (wantString[i] == '.') Thread.Sleep(delay * 6);
                else Thread.Sleep(delay);
            }
            Console.WriteLine();
        }
    }
}
