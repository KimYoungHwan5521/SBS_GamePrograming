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
        public virtual void Enter(CharacterDungeon target) 
        { 
            if (target == null) { }
        }
        public virtual void Exit(CharacterDungeon target) 
        {
            if (target == null) { }
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
        public Store(string greetingMessage, string goodByeMessage, params GoodsInfo[] goods)
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
                    Exit(target);
                    break;
            }
        }

        public override void Exit(CharacterDungeon target)
        {
            base.Exit(target);
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
            if (target.nowHP >= target.maxHP || target.isDead) return false;
            target.nowHP += value;
            return true;
        }
    }

    internal class Shelter
    {
    }
}
