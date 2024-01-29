using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CS
{
    class Place
    {
        public List<Place> connectedPlaces;
        public string name;

        public static PalletTown palletTown;
        public static PalletMountain palletMountain;
        public static SecondTown secondTown;
        public static SecondMountain secondMountain;
        public static Mokpo mokpo;
        public static ThirdMountain thirdMountain;
        public static PawnIsland pawnIsland;
        public static PawnForest pawnForest;
        public static BadcoldTown badcoldTown;
        public static BadcoldMountain badcoldMountain;
        public static FluTown fluTown;

        public static PalletTownStore palletTownStore;
        // 애들이 위에서 아래로 차례차례 만들어 지니까
        // 1. 모든 애들을 다 만들어 놓고 (initialize)
        // 2. 그 다음에 연결하기
        public static void Initialize()
        {
            palletTown      = new ();
            palletMountain  = new ();
            secondTown      = new ();
            secondMountain  = new ();
            mokpo           = new ();
            thirdMountain   = new ();
            pawnIsland      = new ();
            pawnForest      = new ();
            badcoldTown     = new ();
            badcoldMountain = new ();
            fluTown         = new ();

            palletTownStore = new ();

            palletTown.Connect(palletMountain, palletTownStore);
            palletMountain.Connect(palletTown, secondTown);
            secondTown.Connect(palletMountain, secondMountain, thirdMountain);
            secondMountain.Connect(secondTown, mokpo);
            thirdMountain.Connect(secondTown);
            mokpo.Connect(secondMountain, pawnIsland, badcoldTown);
            pawnIsland.Connect(mokpo, pawnForest, badcoldTown);
            pawnForest.Connect(pawnIsland);
            badcoldTown.Connect(badcoldMountain, mokpo, pawnIsland);
            badcoldMountain.Connect(badcoldTown, fluTown);
            fluTown.Connect(badcoldMountain);

            palletTownStore.Connect(palletTown);
        }

        public virtual void Connect(params Place[] places)
        {
            // 연결된 지역들에다가 더해주기
            // Add는 하나만 넣기
            // AddRange : 배열이나 리스트를 통으로 붙이기
            connectedPlaces.AddRange(places);
        }

        public Place(string name, params Place[] connectedPlaces) 
        {
            this.name = name;
            // 배열 -> 리스트
            this.connectedPlaces = new List<Place>(connectedPlaces);
            // 리스트 -> 배열
            // connectedPlace.ToArray();
        }

        public virtual void Enter(CharacterDungeon target) 
        { 
            if (target != null) { }
        }
        public virtual void Exit(CharacterDungeon target) 
        {
            if (target == null) { }
        }
        public virtual void Move(CharacterDungeon target)
        {
            string[] placeNames = new string[connectedPlaces.Count + 1];
            placeNames[connectedPlaces.Count] = $"뒤로({name})";
            for(int i = 0; i < connectedPlaces.Count; i++)
            {
                placeNames[i] = connectedPlaces[i].name;
            }
            
            int selected = InfinityDungeon.Select("어디로 갈까요", placeNames);
            if(selected == connectedPlaces.Count)
            {
                // 뒤로
                Enter(target);
            }
            else
            {
                // 1. null 체크
                // 2. 있던 곳에서 나가기
                // 3. 고른 번호의 장소에 Enter
                if (connectedPlaces[selected] != null)
                {
                    // 2. Exit
                    Exit(target);
                    // 3. Enter
                    connectedPlaces[selected].Enter(target);

                }
                else
                {
                    // 1. null 이면 Move
                    Console.WriteLine("<<!잘못된 장소>>".RedColor());
                    Console.ReadKey();
                    Move(target);
                }
            }
        }
    }

    class Store : Place
    {
        // 판매할 아이템의 배열
        public GoodsInfo[] goods;
        public string greetingMessage;
        public string goodByeMessage;
        // 메서드에 매개변수로 "배열"을 넣어야 할 때
        //     만들 때                         실행할 때
        // Store(int[] values)         =>  new Stroe(new int[] { 1,2,3,});
        // Store(params int[] values)  =>  new Store(1,2,3);
        public Store(string name, string greetingMessage, string goodByeMessage, GoodsInfo[] goods, params Place[] connectedPlaces)
            : base(name, connectedPlaces)
        {
            this.greetingMessage = greetingMessage;
            this.goodByeMessage= goodByeMessage;
            this.goods = goods;
        }

        public override void Enter(CharacterDungeon target)
        {
            base.Enter(target);
            switch(InfinityDungeon.Select(greetingMessage, new string[] { "구매", "판매".DarkColor(), "나가기" }))
            {
                case 0:
                    // 아이템들의 이름을 리스트로
                    string[] itemList = new string[goods.Length + 1];
                    while(true)
                    {
                        // 리스트를 하나씩 반복해서 채우기
                        for(int i = 0;i < goods.Length;i++)
                        {
                            itemList[i] = goods[i].ToString();
                        }
                        itemList[goods.Length] = "뒤로";

                        Console.Clear();
                        int selected = InfinityDungeon.Select($"구매할 아이템을 선택 해주세요.\n[소지금 : {target.gold} G]\n  이름\t\t가격\t재고", itemList);
                        if (selected == itemList.Length - 1)
                        {
                            Enter(target);
                            break;
                        }
                        else
                        {
                            // selected 번째 상품 구매

                            // 1. 아이템이 있는가
                            // 2. 아이템 재고가 있는가
                            // 3. 살 가격이 있는가

                            // 4. 돈을 줌
                            // 5. 상점 재고 - 1
                            // 6. 인벤토리에 아이템 넣기
                            if (goods[selected].item == null)
                            {
                                Console.WriteLine("<<!알 수 없는 아이템>>".RedColor());
                                Console.ReadKey();
                            }
                            else if (goods[selected].quantity <= 0)
                            {
                                Console.WriteLine("품절입니다.");
                                Console.ReadKey();
                            }
                            else if (target.gold < goods[selected].price)
                            {
                                Console.WriteLine("Gold가 부족합니다.");
                                Console.ReadKey();
                            }
                            else
                            {
                                target.gold -= goods[selected].price;
                                goods[selected].quantity--;
                                target.inventory.Add(goods[selected].item);
                                Console.WriteLine($"{goods[selected].item.name}을(를) 구매했다.");
                                Console.ReadKey();
                            }
                        }
                    }

                    break;
                case 1:
                    Enter(target);
                    break;
                case 2:
                    Move(target);
                    break;
            }
        }


        /*
        Store BeginnersShop = new Store(
            new GoodsInfo[] 
            {
                new GoodsInfo() { item = ItemBase.SmallPotion, price = 50, quantity = 100 },
                new GoodsInfo() { item = ItemBase.knife, price = 100, quantity = 1 },
            }
        );
        */
    }

    // class : 중간에 없어질 수도 있는 친구
    //         플레이어가 없다 << 없는 것도 정보
    // struct : "없다"로 무마할 수 없다.
    //          변수가 선언이 되면 그 안에 내용은 무조건 채워져있어야 한다.
    //          상점은 여러 종류의 물건을 가지고 있습니다.
    //          각 물건을 몇 개씩 가지고 있나
    //          "정보"를 저장하는 친구
    struct GoodsInfo
    {
        public ItemBase item;
        public int price;
        public int quantity;

        // GoodsInfo를 글자로 표현하면 어떻게될까
        // [이름      가격      재고]
        // 재고가 0일때는 품절을 띄우고 취소선
        // 상품이 없으면 [--없음--] 출력
        public override string ToString()
        {
            if(item == null)
            {
                return "[          물건이 없습니다.          ]".DarkColor();
            }
            else if(quantity <= 0)
            {
                return $"{item.name}\t{price} G\t품절".DarkStrikeThrough();
            }
            else
            {
                return $"{item.name}\t{price} G\t{quantity}개";
            }
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            // 둘 중에 하나만 널이면 false, 여기선 GoodsInfo가 struct라 null일 수 없기 때문에 생략
            // if(obj == null ^ this == null);
            if(obj == null) return false;
            if (typeof(GoodsInfo) != obj.GetType()) return false;
            return ((GoodsInfo)obj).item == item;
        }
    }

    // 아이템은 기능도 있다
    // ItemBase는 정말로 있는 걸까
    // "농기구"                        "추상적 개념" abstract
    // "낫", "호미", "삽", "갈퀴"...
    abstract class ItemBase
    {
        public string name = "!잘못된 아이템 이름";

        // 왜 bool ? -> 생명력 포션을 쓰고 싶어요 : 제 생명력이 가득 찬 상태라면 사용x
        public abstract bool Use(CharacterDungeon user, CharacterDungeon target);

        // 클래스 -> 개념 / 인스턴스 -> 객체
        // "빨간 포션, 50 회복"이라는 내용을 캐릭터마다 따로 적어놓을 필요가 없다.
        // 모든 곳에서 동일하게 사용할 수 있게, 없어지지 않는 단 하나의 존재
        public static HealingPotion SmallPotion = new HealingPotion("소형 포션", 200);
        public static HealingPotion MediumPotion = new HealingPotion("중형 포션", 400);
        public static HealingPotion LargePotion = new HealingPotion("대형 포션", 800);
        public static Sword knife = new Sword("나이프", 50);
        public static Sword LongSword = new Sword("장검", 100);
        
    }

    public enum Location { Head, Body, RightHand, LeftHand, Legs, Feets}
    interface Equipable
    {

        // interface 인스턴스 선언 불가능, 프로퍼티, 메서드는 가능
        // 그냥 변수에 => 사용하면 get이라고 취급.
        // set은 불가능 하니 바꿀순 없음
        public virtual Location targetLocation => Location.Head;

        // interface는 "값"이라고 하는 것이 존재하지 않는다.
        // 메서드에도 "내용"이 존재하지 않는 것이 좋다.
        // 이런 기능이 있다 까지만.
        public bool Equip(CharacterDungeon target);
        public bool UnEquip(CharacterDungeon target);
    }

    // 검을 착용할 때, 해제할 때
    // 검은 "아이템"의 일종인데, 그 중 "장비 할 수 있는"의 일종
    // "장비 할 수 있는" 형용사
    // 얻어 맞을 수 있는 -> 맞는다 (기능에 대한 이야기)
    class Sword : ItemBase, Equipable
    {
        public int value;

        public virtual Location targetLocation => Location.RightHand;

        public Sword(string name, int value) 
        {
            this.name = name;
            this.value = value;
        }

        public bool Equip(CharacterDungeon target)
        {
            target.attackDamage += value;
            return true;
        }

        public bool UnEquip(CharacterDungeon target)
        {
            target.attackDamage -= value;
            return true;
        }

        public override bool Use(CharacterDungeon user, CharacterDungeon target)
        {
            // 장비하는 순서
            // 1. 무기를 들고있는지 확인
            // true : 들고있는 무기를 해제
            // 2. 무기 장착
            if (target == null) return false;
            // 장착하려는 위치에 대상이 있다 = 없지 않다.
            if(Equip(target))
            {
                if (target.equipables[(int)targetLocation] != null)
                {
                    target.equipables[(int)targetLocation].UnEquip(target);
                    target.equipables[(int)targetLocation] = null;
                }
                target.equipables[(int)targetLocation] = this;
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }

    class HealingPotion : ItemBase
    {
        public int value;

        public HealingPotion(string name, int value)
        {
            this.name = name;
            this.value = value;
        }

        public override bool Use(CharacterDungeon user, CharacterDungeon target)
        {
            // strunct는 무조건 있는거 , class는 없을 수도 있음
            if(target == null) return false;
            if (target.NowHP >= target.maxHP || target.isDead) return false;
            target.NowHP += value;
            return true;
        }
    }

    class Town : Place
    {
        public Town(string name, params Place[] places) : base(name, places) 
        {
            
        }

        public override void Enter(CharacterDungeon target)
        {
            Move(target); // 마을은 자유롭게 이동할 수 있는 공간
        }
    }

    class Field : Place
    {
        protected CharacterDungeon enemy = null;
        
        public Field(string name, params Place[] connectedPlaces) : base(name, connectedPlaces)
        {

        }

        public override void Enter(CharacterDungeon target)
        {
            base.Enter(target);
            ChoiceAction(target);
        }

        // 아예 남들은 쓰면 안되는데, Field의 자식들은 바꾸긴 해야한다
        //            public(x)                  private(x)
        // => protected
        protected string conditionExplain = "아무 일도 없었다.";
        public virtual void ChoiceAction(CharacterDungeon player)
        {
            // 현재 상황에 대해 파악을 하고, 선택
            int selected = InfinityDungeon.Select(conditionExplain, new string[] { "탐색", "이동"});
            switch(selected)
            {
                case 0: Search(player); break; // 첫 번째 선택지 : 탐색
                case 1: Move(player); break; // 두 번째 탐색지 : 이동
            }
        }

        public virtual void Search(CharacterDungeon player)
        {
            ChoiceAction(player);
        }

        //                                          1. 플레이어가 강하면 강한 몬스터
        //                                          2. 이벤트 아이템을 들고 왔으면 이벤트가 실행
        //                                          3. 도플갱어
        //                                          4. 몬스터 소환될 때 플레이어에게 영향
        public virtual CharacterDungeon SpawnMonster(CharacterDungeon player)
        {
            Random ran = new Random();
            float value = ran.NextSingle();
            if(value > 0.7f)
            {
                return new CharacterDungeon("슬라임", 150, 0, 10, 0);
            }
            else
            {
                return new CharacterDungeon("스켈톤", 200, 0, 30, 10);
            }
        }

        public virtual void BattleTurn(CharacterDungeon player)
        {
            // select는 무한반복 : 플레이어는 맘대로 눌러버릴 거니까
            while(true)
            {
                // 상태창
                // 행동 선택
                int selected = InfinityDungeon.Select
                (
                    $"\t{player.name}\t\t{enemy.name}\n"
                    + $"체력\t{player.NowHP}/{player.maxHP}\t\t{enemy.NowHP}/{enemy.maxHP}\n"
                    + $"마나\t{player.NowMP}/{player.maxMP}\t\t{enemy.NowMP}/{enemy.maxMP}"
                    , new string[] {"공격", "스킬", "아이템", "방어", "도망"}
                );
                
                if(selected == 0)
                {
                    // 공격
                    player.Attack(enemy);
                    break;
                }
                else if(selected == 1 && player.Skill(enemy))
                {
                    // 스킬
                    break;
                }
                else if(selected == 2)
                {
                    // 아이템
                    // 턴제 게임에서 아이템 사용에 대한 이야기
                    // "턴"을 소모할 것인가 말 것인가.
                    // 턴을 쓴다. : break;
                    // 보조 턴을 쓴다.
                    // -> 턴을 안쓴다.
                    player.UseItem(player.SelectItem(), player, enemy);
                }
                else if(selected == 3)
                {
                    // 방어
                    player.Defense(enemy);
                    break;
                }
                else if(selected == 4)
                {
                    // 도망
                    return;
                }
            }

            // 둘 다 안죽었으면 
            if (!player.isDead && !enemy.isDead)
            {
                enemy.isDefense= false;
                enemy.AI(player);
            }

            InfinityDungeon.NextPage();
            // 적 턴 후에도 모두 안죽었으면
            if (!player.isDead && !enemy.isDead)
            {
                player.isDefense = false;
                BattleTurn(player);
            }
            else if(enemy.isDead) // 몬스터가 죽었으면
            {
                InfinityDungeon.DrawText($"{enemy.name}을(를) 쓰러트렸다!");
            }
            else
            {
                InfinityDungeon.DrawText($"{player.name}은(는) 쓰러졌다!");
                Program.game.GameOver(player);
            }
        }
    }

    // 태초마을
    internal class PalletTown : Town
    {
        bool isEventMoney = false;
        public PalletTown(params Place[] connectedPlaces) : base("태초마을", connectedPlaces)
        {

        }

        public override void Enter(CharacterDungeon target)
        {
            if(!isEventMoney) GiveMoney(target);
            base.Enter(target);
        }

        public void GiveMoney(CharacterDungeon target)
        {
            Console.Clear();
            InfinityDungeon.DrawText("반갑네 나는 이 마을 이장일세");
            InfinityDungeon.DrawText("일단 100G를 받게");
            InfinityDungeon.NextPage();
            target.gold += 100;
            InfinityDungeon.DrawText("100G를 얻었다!");
            InfinityDungeon.NextPage();
            InfinityDungeon.DrawText("상점에서 아이템을 구매해보게");
            InfinityDungeon.NextPage();
            isEventMoney= true;
        }
    }

    internal class PalletTownStore : Store
    {
        public PalletTownStore(params Place[] connectedPlaces) 
            : base(
                  "태초 상점", 
                  "ㅎㅇ", 
                  "ㅂㅂ",
                  new GoodsInfo[]
                  {
                      new GoodsInfo(){item = ItemBase.SmallPotion, price = 50, quantity = 100},
                      new GoodsInfo(){item = ItemBase.knife, price = 100, quantity = 1},
                  },
                  connectedPlaces
                  )
        {

        }
    }

    internal class PalletMountain : Field
    {
        public PalletMountain(params Place[] connectedPlaces) : base("태초산", connectedPlaces)
        {
            conditionExplain = "약한 야생 몬스터들이 서식하는 작은 산입니다.";
        }

        public override void Search(CharacterDungeon player)
        {
            if(new Random().NextSingle() < 0.5)
            {
                InfinityDungeon.DrawText("10골드를 획득했다!");
                player.gold += 10;
                Console.ReadKey();
            }
            else
            {
                enemy = SpawnMonster(player);
                InfinityDungeon.DrawText($"수풀에서 {enemy.name}이(가) 튀어 나왔다!");
                Console.ReadKey();
                BattleTurn(player);
            }

            base.Search(player);
        }
    }

    internal class SecondTown : Town
    {
        public SecondTown(params Place[] connectedPlaces) : base("세컨드 타운", connectedPlaces)
        {

        }
    }
    internal class SecondMountain : Field
    {
        public SecondMountain(params Place[] connectedPlaces) : base("세컨드 마운틴", connectedPlaces)
        {

        }
    }
    internal class ThirdMountain : Field
    {
        public ThirdMountain(params Place[] connectedPlaces) : base("써드 마운틴", connectedPlaces)
        {

        }
    }
    internal class Mokpo : Town
    {
        public Mokpo(params Place[] connectedPlaces) : base("목포", connectedPlaces)
        {

        }
    }
    internal class PawnIsland : Town
    {
        public PawnIsland(params Place[] connectedPlaces) : base("폰섬", connectedPlaces)
        {

        }
    }
    internal class PawnForest : Field
    {
        public PawnForest(params Place[] connectedPlaces) : base("폰숲", connectedPlaces)
        {

        }
    }
    internal class BadcoldTown : Town
    {
        public BadcoldTown(params Place[] connectedPlaces) : base("감기 마을", connectedPlaces)
        {

        }
    }
    internal class BadcoldMountain : Field
    {
        public BadcoldMountain(params Place[] connectedPlaces) : base("감기산", connectedPlaces)
        {

        }
    }
    internal class FluTown : Town
    {
        public FluTown(params Place[] connectedPlaces) : base("독감 마을", connectedPlaces)
        {

        }
    }
}
