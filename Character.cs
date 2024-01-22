using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS
{
    enum Race
    {
        Human, Vampire, Golem, Elemental, Undead, Dwarf, Dragon
    }

    internal class Character
    {
        // static : 정적 변수
        // 멤버변수는 캐릭터 마다 하나씩
        // 정적변수는 캐릭터한테 주는게 아니라 프로그램 자체가 가지고 있는다.
        public static int survivors = 0;
        // 멤버변수 : 변수는 변수인데 인스턴스가 가질 변수
        // = 필드
        public string name;
        public Race race;
        public int attackDamage;
        public int rank;
        public int killCount = 0;

        // '_' 넣으면 자동완성 우선순위 밀림
        private int _nowHP;
        public int nowHP
        {
            /*
            get
            {
                return _nowHP;
            }
            */
            get => _nowHP;
            set
            {
                if(isDead) return;
                _nowHP = value;
                // 현재생명력이 0이하가 되면 죽는다.
                if(value <= 0)
                {
                    if(race == Race.Undead && nowMP >= 100)
                    {
                        maxHP /= 2;
                        _nowHP = maxHP;
                        nowMP -= 100;
                        Console.WriteLine($"{name}은(는) 부활했다!");
                    }
                    else
                    {
                        _nowHP = 0; // 퍼센트 이쁘게 보이게
                        isDead = true;
                        rank = survivors;
                        survivors--;
                    }
                }
                // 현재생명력이 최대생명력 이상이 되면 현재생명력은 최대생명력이 된다.
                else if(value > maxHP)
                {
                    nowHP = maxHP;
                }
            }
        }
        public int maxHP;
        public int nowMP;
        public int maxMP;
        // 총구 식혀야 해서 '쿨다운'
        public float cooldownNow = 0;
        public float cooldownMax;
        public float size;
        public bool isDead = false;

        // 변수 + 함수 = 프로퍼티
        // 만들기는 일단 변수처럼
        // 실제 기능은 함수처럼
        public float healthPercent
        {
            get // 값 가져오기
            {
                return (float) nowHP / maxHP;
            }

            set // 값 설정
            {
                // 퍼센트를 바꾸면 현재 체력이 바뀜
                // set 안쪽에서는 "value"라는 친구가 생긴다.
                // nowHP = (int)((float) maxHP * value);
                nowHP = (int)MathF.Ceiling(maxHP * value);
            }
        }
        

        public Character(string name, int attackDamage, int maxHP, float size)
        {
            this.name = name;
            this.attackDamage = attackDamage;
            // 대입은 항상 오른쪽을 모두 계산한 뒤에 왼쪽에 있는 변수에 넣는다.
            this.nowHP = this.maxHP = maxHP;
            this.size = size;
            survivors++;
        }

        // 기능 Function 함수
        // 실제 캐릭터가 실행할 함수(이동, 공격, 맞기, 죽기)
        // 인스턴스를 대상으로 하는 함수 -> 메서드

        // 공격할 거임 -> 공격 대상
        // 데미지 반환 -> 딜미터기, 피흡
        public virtual int Attack(Character[] enemy)
        {
            return enemy[0].TakeDamage(this, attackDamage, false);
        }

        // 스킬
        // bool 반환 : 성공했는가 실패했는가
        // virtual : "(자식이)이 기능을 바꿀 수 있다"라고 언급을 해놓는 것
        public virtual bool Skill(Character[] target)
        {
            if(nowMP < 50 || cooldownNow > 0)
            {
                return false;
            }
            Attack(target);
            Attack(target);
            nowMP -= 50;
            cooldownNow = cooldownMax;

            return true;
        }

        // 공격 받을 거임 -> 몇 뎀, 누가 때렸는지(반사뎀, 킬로그 등)
        public virtual int TakeDamage(Character from, int damage, bool isCritical) 
        {
            if(isCritical)
            {
                damage *= 2;
            }
            nowHP -= damage;
            return damage;
        }

    }
}
