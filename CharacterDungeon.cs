using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS
{
    internal class CharacterDungeon
    {
        public string name;
        public Equipable[] equipables = new Equipable[6];
        public List<ItemBase> inventory = new List<ItemBase>();
        public int gold;
        public bool isDead = false;
        public int maxHP;
        private int _nowHP;
        public int NowHP
        {
            get { return _nowHP; } 
            set 
            { 
                if(isDead) return;
                _nowHP = value;
                if(value <= 0)
                {
                    _nowHP = 0;
                    isDead = true;
                }
                else if(value > maxHP)
                {
                    _nowHP = maxHP;
                }
            }
        }
        public float HealthPercent
        {
            get
            {
                return (float)NowHP / maxHP;
            }

            set
            {
                NowHP = (int)MathF.Ceiling(maxHP * value);
            }
        }
        public int maxMP;
        private int _nowMP;
        public int NowMP
        {
            get { return _nowMP; }
            set
            {
                _nowMP = value;
                if(value <= 0)
                {
                    _nowMP = 0;
                }
                else if(value > maxMP)
                {
                    _nowMP = maxMP;
                }
            }
        }
        public int attackDamage;
        public int defense;
        public bool isDefense;
        public virtual float DefenseRate => 0.5f;

        public ItemBase SelectItem()
        {
            // var : 자료형 쓰기 귀찮을 때
            // 중간중간 바뀌거나 너무 긴 자료형은 쓰기 귀찮습니다!
            // foreach는 자료형도 딱 정해져있다.
            // 자동으로 정해주기 때문에 쓸데 없는 것도 정해줄 수 있다.
            // object는 자료형을 바꿔서 넣어줄수도 있지만,
            // var는 선언시 자료형을 정해줘야하고 한 번 정해주면 바꿀 수 없음.
            string[] itemNames = new string[inventory.Count + 1];
            itemNames[itemNames.Length - 1] = "뒤로";
            for(int i=0; i < inventory.Count; i++)
            {
                itemNames[i] = inventory[i].name;
            }

            int selected = InfinityDungeon.Select("아이템을 선택 해주세요", itemNames);
            // 아이템을 골랐으니 내보낼건데, "뒤로"가 있다.
            if(selected == inventory.Count)
            {
                return null;
            }
            else
            {
                return inventory[selected];
            }
        }

        // 
        public bool UseItem(ItemBase item, params CharacterDungeon[] target)
        {
            // 1. 아이템이 있는지 확인
            if (item == null) return false; // 아이템이 없다.
            // 2. 고른다.
            // 2-1. 대상을 선택
            // 일단 선택한 캐릭터는 아직 없다.
            CharacterDungeon selectCharacter = null;
            string[] characterNames = new string[target.Length + 1];
            characterNames[characterNames.Length-1] = "취소";
            for(int i=0; i<target.Length; i++)
            {
                characterNames[i] = target[i].name;
            }
            int selected = InfinityDungeon.Select("대상을 선택 해주세요", characterNames);
            if(selected < target.Length)
            {
                selectCharacter = target[selected];
            }
            
            if(selectCharacter == null)
            {
                return false;
            }
            // 3. 사용
            // 4. 아이템이 제대로 사용 되었는지 확인한 후에
            else
            {
                // 5. 사용이 되었다면 인벤토리에서 제거
                if(item.Use(this, selectCharacter))
                {
                    inventory.Remove(item);
                    return true;
                }
                else { return false; }
            }

        }

        public CharacterDungeon(string name, int maxHP, int maxMP, int attackDamage, int defense)
        {
            this.name = name;
            this.NowHP = this.maxHP = maxHP;
            this.NowMP = this.maxMP = maxMP;
            this.attackDamage = attackDamage;
            this.defense = defense;
        }

        public virtual int Attack(CharacterDungeon enemy, bool isIgnoreDefense = false)
        {
            InfinityDungeon.DrawText($"{name}은(는) 공격했다!");
            return enemy.TakeDamage(this, attackDamage, isIgnoreDefense);
        }

        public virtual int TakeDamage(CharacterDungeon from, int damage, bool isIgnoreDefense = false)
        {
            if(!isIgnoreDefense)
            {
                damage -= defense;
            }
            if(isDefense)
            {
                damage /= 2;
            }
            InfinityDungeon.DrawText($"{name}에게 {damage}의 데미지!");
            NowHP -= damage;
            return damage;
        }

        public virtual bool Skill(CharacterDungeon target)
        {
            return false;
            InfinityDungeon.DrawText($"{name}의 스킬!");
        }

        public virtual void Defense(CharacterDungeon target)
        {
            InfinityDungeon.DrawText($"{name}은(는) 방어했다");
            isDefense= true;
        }

        // Artificial Intelligence
        // 이 친구는 어떤 논리적인 사고로 생각을 하게 될까
        public virtual void AI(CharacterDungeon enemy)
        {
            Random ran = new Random();
            float value = ran.NextSingle();
            // 공격, 스킬, 방어
            // 언제?
            //   공격적    수비적
            // 공격 스킬    방어
            //            내가 위험 할 때
            //                                                             막아도 죽으면 막지 않겠다.
            if(value < HealthPercent.GetPercent(0.1f, 0.5f, 0.9f, 0.1f) && enemy.attackDamage * DefenseRate < HealthPercent)
            {
                // 1. 생명력 50% 미만 && 50% 확률
                // 2. 상대가 계속 때리고 있다
                // 3. 내가 공격을 할 수 없을 때
                // 4. 생명력이 50%가 되면 슬슬 막아야 되나? 생명력이 20%쯤 된다면?
                // 생명력이 떨어질수록 더 막고 싶어지지 않을까?
                // 상대방의 공격력이 막으면 막힐 것 같을 때
                Defense(enemy);
            }
            else
            {
                // 방어하지 않았다면
                // 공격 혹은 스킬
                // 스킬 반환값 : bool
                if(Skill(enemy) == false)
                {
                    Attack(enemy);
                }
            }
        }
    }
}
