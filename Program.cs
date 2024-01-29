using CS.Heros;
using System.ComponentModel;

namespace CS
{
    internal class Program
    {
        // 그냥 변수를 만들면
        // 각자 하나씩 들고 있는 것!
        // 하나만 존재하게 하려면 static
        // 이렇게 인스턴스를 관리하는 변수 딱 하나를 두고, 여러개를 안 만들고 싶어요
        // => "싱글턴"
        public static InfinityDungeon game;
        static void Main(string[] args)
        {
            game = new InfinityDungeon();
            game.MainScreen();

        }

        static void BattleRoyal2()
        {
            int characterNumber = 7;
            int[,] ranks = new int[characterNumber, characterNumber];
            int[] totalKills = new int[characterNumber];
            for(int game = 0; game < 1000; game ++)
            {
                // Human은 Character는 Character지만 특별한 기능이 있는 Character
                // player는 Character 자료형
                // Human이 Character의 자식인 경우 Character로 취급해도 상관없음
                Character[] c = new Character[characterNumber];
                Character.survivors = 0;
                c[0] = new Human();
                c[1] = new Vampire();
                c[2] = new Golem();
                c[3] = new Elemental();
                c[4] = new Undead();
                c[5] = new Dwarf();
                c[6] = new Dragon();

                Random ran = new Random();
                int r;
                int attacker;
                while (Character.survivors > 1)
                {
                    Console.WriteLine("-------------------------");
                    Console.WriteLine("이름\t\t체력\t마나");
                    for(int i = 0; i < c.Length; i++)
                    {
                        if (c[i].isDead) Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"{c[i].name}\t");
                        if (c[i].name.Length < 4) Console.Write("\t");
                        Console.WriteLine($"{c[i].nowHP}\t{c[i].nowMP}");
                        Console.ResetColor();
                    }

                    Console.WriteLine();
                    ran = new Random();
                    r = ran.Next(c.Length);
                    while (c[r].isDead)
                    {
                        ran = new Random();
                        r = ran.Next(c.Length);
                    }
                    attacker = r;
                    //Console.ReadLine();
                    c[attacker].Attack(c);
                }
                for (int i = 0; i < c.Length; i++)
                {
                    if (!c[i].isDead)
                    {
                        Console.WriteLine($"승자 : {c[i].name}!");
                        c[i].rank = 1;
                    }
                    totalKills[i] += c[i].killCount;
                }
                Console.WriteLine() ;

                for(int i=0; i< c.Length; i++)
                {
                    ranks[i, c[i].rank - 1]++;
                }
                for (int i = -1; i < c.Length; i++)
                {
                    int rankSum = 0;
                    for (int j = -1; j < c.Length + 2; j++)
                    {
                        if(i == -1)
                        {
                            if (j == -1)
                            {
                                Console.Write("이름\t\t");
                            }
                            else if (j == c.Length)
                            {
                                Console.Write("평균등수");
                            }
                            else if (j == c.Length + 1)
                            {
                                Console.Write("평균킬수");

                            }
                            else
                            {
                                Console.Write($"{j + 1}위\t");
                            }
                        }
                        else
                        {
                            if(j == -1)
                            {
                                Console.Write($"{c[i].name}\t");
                                if (c[i].name.Length < 4) Console.Write("\t");
                            }
                            else if(j == c.Length)
                            {
                                Console.Write($"{(float)rankSum / (game + 1)}\t");
                            }
                            else if(j == c.Length + 1)
                            {
                                Console.Write($"{(float)totalKills[i] / (game + 1)}\t");
                            }
                            else
                            {
                                Console.Write($"{ranks[i, j]}\t");
                                rankSum += (j + 1) * ranks[i, j];
                            }

                        }
                    }
                    Console.WriteLine();
                }
                //Console.ReadLine();
            }
        }
           
        /*
        static void BattleRoyal()
        {
            Character[] c = new Character[10];
            c[0] = new Character("브록레스너", 33, 188, 2);
            c[1] = new Character("나", 19, 166, 2);
            c[2] = new Character("존시나", 18, 177, 2);
            c[3] = new Character("랜디오턴", 20, 155, 2);
            c[4] = new Character("로만레인즈", 25, 141, 2);
            c[5] = new Character("세스롤린즈", 19, 168, 2);
            c[6] = new Character("드류맥킨타이어", 20, 165, 2);
            c[7] = new Character("AJ스타일즈", 19, 186, 2);
            c[8] = new Character("코디로즈", 19, 170, 2);
            c[9] = new Character("트리플H", 31, 110, 2);

            int attacker;
            int victim;
            Random ran = new Random();
            int r = 0;
            while(Character.survivors > 1)
            {
                Console.WriteLine($"생존자 : {Character.survivors}명");
                for(int i = 0; i < c.Length; i++)
                {
                    if (c[i].nowHP == 0) Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{c[i].name} : {c[i].healthPercent * 100} %");
                    Console.ResetColor();
                }
                ran = new Random();
                r = ran.Next(c.Length);
                while (c[r].isDead)
                {
                    ran = new Random();
                    r = ran.Next(c.Length);
                }
                attacker = r;
                while (c[r].isDead || r == attacker)
                {
                    ran = new Random();
                    r= ran.Next(c.Length);
                }
                victim = r;
                Console.ReadLine();
                c[attacker].Attack(c);
                Console.WriteLine($"{c[attacker].name}이(가) {c[victim].name}을(를) 공격");
                Console.WriteLine();
            }
            for(int i = 0; i < c.Length; i++)
            {
                if (!c[i].isDead)Console.WriteLine($"승자 : {c[i].name}!");
            }
        }

        static void Array2D()
        {
            // a00 a01 a02 a03
            // a10 a11 a12 a13
            // a20 a21 a22 a23
            // a30 a31 a32 a33
            int[,] testScore = {
                {100, 90, 80, 70 },
                {100, 80, 65, 40},
                {40, 60, 80, 100 },
                {100, 100, 100, 0},
            };
            //int[,] testScore = new int[4,4];
            //Console.WriteLine(testScore[2, 3]);

            // 1. 1번학생의 전체 평균 점수를 구해주세요
            int sum = 0;
            for(int i = 0; i < testScore.GetLength(1); i++) 
            {
                sum += testScore[1, i];
            }
            Console.WriteLine($"1번 학생의 전체 평균 {(float)sum / 4}");
            // 2. 수학 전체 평균 점수를 구해주세요.
            sum = 0;
            for(int i=0; i< testScore.GetLength(0); i++)
            {
                sum += testScore[i, 2];
            }
            Console.WriteLine($"수학 전체 평균 {(float)sum / 4}");
            // 3. 수학점수가 가장 높은 학생을 구해주세요
            int max = 0;
            int maxIndex = 0;
            for(int i=0;i< testScore.GetLength(0); i++)
            {
                if (testScore[i, 2] > max)
                {
                    max = testScore[i, 2];
                    maxIndex = i;
                }
            }
            Console.WriteLine($"수학 점수가 가장 높은 학생 : {maxIndex}번 학생, 수학 점수 : {max}");
            // 4. 영어 점수가 2번째로 높은 학생을 구해주세요
            max = 0;
            maxIndex = 0;
            int maxSecond = 0, maxSecondIndex = 0;
            for(int i = 0; i<testScore.GetLength(0); i++)
            {
                if (testScore[i, 1] > max)
                {
                    maxSecond = max;
                    maxSecondIndex = maxIndex;
                    max = testScore[i, 1];
                    maxIndex = i;
                }
                else if (testScore[i, 1] > maxSecond)
                {
                    maxSecond = testScore[i, 1];
                    maxSecondIndex = i;
                }
            }
            Console.WriteLine($"영어 점수가 두 번째로 높은 학생 : {maxSecondIndex}번 학생, 영어 점수 : {maxSecond}");
            Console.WriteLine(); 

            bool[,] bingo =
            {
                {true, false, false, true, true },
                {true, false, false, true, false },
                {true, true, true, true, true },
                {false, true, false, true, false },
                {true, false, false, true, true },
            };

            int count;
            int bingoCount;
            int horizontalBingoCount = 0;
            int verticalBingoCount = 0;
            int diagonalBingoCount = 0;
            for(int i = 0; i < bingo.GetLength(0); i++) 
            {
                count = 0;
                for(int j = 0; j < bingo.GetLength(1); j++)
                {
                    if (bingo[i,j])
                    {
                        count++;
                    }
                }
                if(count == 5)
                {
                    horizontalBingoCount++;
                }
                count = 0;
                for(int j = 0; j < bingo.GetLength(1); j++)
                {
                    if (bingo[j, i]) 
                    { 
                        count++;
                    }
                }
                if(count == 5)
                {
                    verticalBingoCount++;
                }
            }
            count = 0;
            for(int i = 0; i < bingo.GetLength(0); i++)
            {
                if (bingo[i,i]) 
                { 
                    count++;
                }
            }
            if (count == 5) diagonalBingoCount++;
            count = 0;
            for (int i = 0; i < bingo.GetLength(0); i++)
            {
                if (bingo[i, bingo.GetLength(0) - 1 - i])
                {
                    count++;
                }
            }
            if (count == 5) diagonalBingoCount++;
            bingoCount = horizontalBingoCount + verticalBingoCount + diagonalBingoCount;
            Console.WriteLine($"가로 빙고 : {horizontalBingoCount}\n세로 빙고 : {verticalBingoCount}\n대각선 빙고 : {diagonalBingoCount}\n총 빙고 : {bingoCount}");
        }

        static void ArrayStart()
        {
            // 7마리의 하수인을 가질 수 있다.
            // 배열을 만들려면
            // 자료형[] 이름 = { A, B, C ... };
            // 자료형[] 이름 = new 자료형[개수];
            Character[] myField = new Character[7];

            // myField는 "캐릭터들의 집합" -> 번호매기기
            // 배열[가져오고 싶은 번호]
            // 7명. 마지막번호는 6.
            // 변수를 쓰듯이 그대로 쓰면 되긴 하지만, 번호도 붙이면 좋다
            // myField 3번(4번째) 나와! -> myField[3]
            Console.WriteLine( myField[3] );
            // 배열은 자리를 마련해 놓은것.
            myField[0] = new Character("김영환", 5, 1, 170f);
            myField[1] = new Character("Kim", 3, 3, 165f);
            myField[2] = new Character("John", 1, 5, 175f);
            myField[3] = new Character("소배압", 5, 5, 175f);
            myField[4] = new Character("야율융서", 2, 8, 173f);

            for(int i = 0; i < myField.Length; i++) 
            {
                // 예외처리
                // 예외 : 생각했던 것에서 벗어나는 것
                // Null Reference Exception
                // 가장 즁요하고 가장 많이 나오는 것 null check
                // null 이면 이번 내용을 스킵하고 다음 것으로 이동.
                if (myField[i] != null)
                {
                    Console.WriteLine( myField[i].name );
                }
                else
                {
                    continue;
                }

            }

            // 최대치가 없음 : List
            List<Character> characters = new List<Character>();
            characters.Add(new Character("핵쟁이", 999, 999, 999f));
            Console.WriteLine(characters[0].name );

            // 배열은 전체적으로 확인하는 일이 많을 때
            // 리스트는 전체 확인 보다는 추가/제거 하는 일이 많을 때

        }
         
        static void Repeater()
        {
            int myMoney = 0;
            Shop victim = new Shop("고물상", "엿", 100, 100);
            int normalItemSellPrice = 10000;
            victim.ShowInfo();
            Console.WriteLine($"내 돈 : {myMoney} 원");

            int sellAmount = 3;
            int count = 0;

            while(count < sellAmount) 
            {
                if(victim.HaveCash(normalItemSellPrice))
                {
                    victim.SellItem(normalItemSellPrice);
                    myMoney += normalItemSellPrice;
                    Console.WriteLine("팔았다.");
                    count++;
                }
                else
                {
                    Console.WriteLine("상인이 돈이 모자라다.");
                    break;
                }
            }
            Console.WriteLine($"내 돈 : {myMoney} 원");

            for(int i = 0; i < sellAmount; i++) 
            {
                Console.WriteLine($"반복한 횟수 : {i}");
            }

        }

        static void Manyak()
        {
            // bool : true or false
            float curHP = 100;
            bool warning = curHP < 30;
            bool full = curHP == 100;
            bool siegeMode = false;

            bool myFavor = false;
            bool yourFavor = true;
            int yourWealth = 3000000;
            if(myFavor == yourFavor) 
            {
                Console.WriteLine("돌아감");
            }
            else if(myFavor == true) 
            {
                Console.WriteLine("붙잡음");
            }
            else if(yourWealth > 300000000)
            {
                Console.WriteLine("설득함");
            }
            else 
            {
                Console.WriteLine("헤어짐");
            }

            bool ourBottomIsHuman = false;
            bool ourTopIsHuman = false;
            bool ourMidIsHuman = false;

            if(ourBottomIsHuman) 
            {
                Console.WriteLine("승리");
            }
            else if(ourTopIsHuman)
            {
                if(ourMidIsHuman) 
                {
                    Console.WriteLine("승리");
                }
                else
                {
                    Console.WriteLine("패배");
                }
            }
            else
            {
                Console.WriteLine("패배");
            }
        }

        static void ChooseShop()
        {
            
            Shop shop1 = new Shop("대장간", "검", 1000, 10);
            Shop shop2 = new Shop("잡화점", "지도", 300, 5);
            Shop shop3 = new Shop("주점", "술", 100, 50);
            Shop shop4 = new Shop("빵집", "빵", 30, 100);
            Shop shop5 = new Shop("약국", "각성제", 500, 2);
            bool loopEnd = false;
            while (true) 
            { 
                Console.WriteLine("1. 대장간");
                Console.WriteLine("2. 잡화점");
                Console.WriteLine("3. 주점");
                Console.WriteLine("4. 빵집");
                Console.WriteLine("5. 약국");
                Console.WriteLine("자세한 정보를 알고싶은 상점의 번호를 입력해 주세요.");
                string chooseShop = Console.ReadLine();
                // 분기분 (switch, if)
                // swtich : 전환 -> 전등스위치가 On -> 불이켜짐
                //                  전등스위치가 Off -> 불이꺼짐
                switch(chooseShop) 
                {
                    case "1":
                        shop1.ShowInfo();
                        break;
                    case "2":
                        shop2.ShowInfo();
                        break;
                    case "3":
                        shop3.ShowInfo();
                        break;
                    case "4":
                        shop4.ShowInfo();
                        break;
                    case "5":
                        shop5.ShowInfo();
                        break;
                    case "-1":
                        loopEnd= true;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("잘못 된 입력");
                        Console.ResetColor();
                        break;
                }
                if (loopEnd) break;
            }
        }

        static void ShopTest()
        {
            // 새로운 상점
            // 카멜 표기법 camelPyogibub
            // 파스칼 표기법 PascalPyogibub
            // 스네이크 표기법 snake_pyogibub
            // 헝가리안 표기법 iHungarianPyogibub
            Shop alchemistShop = new Shop("한방한약방", "녹용", 10000, 2);
                        
            alchemistShop.ShowInfo();

            Shop weaponShop = new Shop("한방총", "총", 20000, 0);

            weaponShop.ShowInfo();

            Shop nShop = new Shop("무기무기", "한놈만걸려라", 999999, 1, 300000);
            nShop.ShowInfo();
        }
        */
    }
}