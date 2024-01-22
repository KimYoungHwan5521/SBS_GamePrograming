using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CS.Heros
{
    // 웨폰마스터 (공격, 스킬) 아펠리오스
    // 현재 무기의 공격횟수를 모두 사용하면 다음 무기로 이동
    // 현재 무기에 맞는 스킬을 발동하고 바로 다음 무기로 이동
    // 공격횟수를 거의 다 썼다고 판단하면 빠르게 스킬 써서 최대 화력

    // Human 은 Character의 일종이다. -> 상속 / 자식(Child)
    internal class Human : Character
    {
        // Length를 맨 마지막에 놓으면 자연스럽게 갯수를 표현
        public enum WeaponType { Gun, Sword, Granade, Staff, Length }

        public WeaponType curWeapon;
        public int weaponCount;
        // Human은 Character의 속성을 가지고있다.
        // Character 부분을 먼저 생성하고, Human의 부분을 생성
        public Human() : base("고길동", 400, 4000, 175)
        {
            nowMP = maxMP = 0;
            cooldownMax = 10;
            race = Race.Human;

            ChangeWeapon(WeaponType.Gun);


        }
        // 부모의 Attack() -> base라고 하는 키워드를 통해 부모의 기능을 갖다 쓸 수 있음
        // 부모가 만들어놓은 Attack()을 내 맘 대로 바꿀겁니다.
        // override 무시(기각)하다
        public override int Attack(Character[] enemy)
        {
            if(weaponCount <= 0)
            {
                Console.WriteLine($"{curWeapon}의 내구도가 모두 소모되었다.");
                NextWeapon();
                Console.WriteLine($"{curWeapon}을(를) 꺼내들었다.");
            }
            weaponCount--;

            Random ran = new Random();
            int r = ran.Next(enemy.Length);
            int victim;
            while (enemy[r].isDead || enemy[r] == this)
            {
                ran = new Random();
                r = ran.Next(enemy.Length);
            }
            victim = r;

            switch (curWeapon)
            {
                case WeaponType.Gun:
                    {
                        ran = new Random();
                        int allDamage = 0;
                        for(int i = 0; i < 3; i++)
                        {
                            float rate = ran.NextSingle();
                            if (rate < 0.1)
                            {
                                // 헤드샷
                                int totalDamage = enemy[victim].TakeDamage(this, attackDamage, true);
                                Console.WriteLine($"헤드샷! {this.name}이(가) {enemy[victim].name}에게 {totalDamage}의 피해!");
                                allDamage += totalDamage;
                            }
                            else if (rate < 0.7)
                            {
                                // 적중
                                int totalDamage = enemy[victim].TakeDamage(this, attackDamage, false);
                                Console.WriteLine($"총 적중! {this.name}이(가) {enemy[victim].name}에게 {totalDamage}의 피해!");
                                allDamage += totalDamage;
                            }
                            else
                            {
                                // 빗맞음
                                Console.WriteLine($"{this.name}의 총은 빗맞았다.");
                            }
                        }
                        Console.WriteLine($"{this.name}은(는) {enemy[victim].name}에게 총 {allDamage}의 피해!");
                        if (enemy[victim].isDead)
                        {
                            Console.WriteLine($"{enemy[victim].name}은(는) 쓰러졌다!");
                            killCount++;
                        }
                        return allDamage;
                    }
                case WeaponType.Sword:
                    {
                        int damage = enemy[victim].TakeDamage(this, attackDamage, false);
                        Console.WriteLine($"{this.name}의 칼 공격! {enemy[victim].name}에게 {damage}의 데미지");
                        if (enemy[victim].isDead)
                        {
                            Console.WriteLine($"{enemy[victim].name}은(는) 쓰러졌다!");
                            killCount++;
                        }
                        return damage;
                    }
                case WeaponType.Granade:
                    {
                        int damage = 0;
                        Console.WriteLine($"{this.name}의 수류탄 공격! ");
                        for(int i = 0; i < enemy.Length; i++)
                        {
                            if (enemy[i] == this || enemy[i].isDead) continue;
                            damage = enemy[i].TakeDamage(this, attackDamage, true);
                            Console.WriteLine($"{enemy[i].name}에게 {damage}의 데미지");
                            if (enemy[i].isDead)
                            {
                                Console.WriteLine($"{enemy[i].name}은(는) 쓰러졌다!");
                                killCount++;
                            }
                        }
                        return damage;
                    }
                case WeaponType.Staff:
                    {
                        int damage = 0;
                        if (enemy[victim].race == Race.Golem)
                        {
                            damage = enemy[victim].TakeDamage(this, attackDamage, true);
                            Console.WriteLine($"{this.name}의 마법 공격! {enemy[victim].name}는 골렘, 골렘에게는 치명타 {damage}의 데미지!");
                        }
                        else
                        {
                            damage = enemy[victim].TakeDamage(this, attackDamage, false);
                            Console.WriteLine($"{this.name}의 마법 공격! {enemy[victim].name}에게 {damage}의 데미지");

                        }
                        if (enemy[victim].isDead)
                        {
                            Console.WriteLine($"{enemy[victim].name}은(는) 쓰러졌다!");
                            killCount++;
                        }
                        return damage;
                    }
                default:
                    {
                        ChangeWeapon(WeaponType.Sword);
                        return Attack(enemy);
                    }
            }
            // return base.Attack(enemy);
        }

        public WeaponType ChangeWeapon(WeaponType wantWeapon)
        {
            curWeapon = wantWeapon;
            switch(wantWeapon)
            {
                case WeaponType.Gun:
                    weaponCount = 3;
                    break;
                case WeaponType.Sword:
                    weaponCount = 5; 
                    break;
                case WeaponType.Granade:
                    weaponCount = 1; 
                    break;
                case WeaponType.Staff:
                    weaponCount = 3;
                    break;
                default:
                    // 잘못된 무기가 들어왔을때 다른무기 줘버림
                    // 본인을 다시 호출 (재귀함수)
                    return ChangeWeapon(WeaponType.Sword);
            }
            // 바꾸고 나서 들고 있는 무기를 알려줘야함
            return curWeapon;
        }

        public WeaponType NextWeapon()
        {
            return ChangeWeapon((WeaponType) (((int)curWeapon + 1) % (int)WeaponType.Length));
        }
    }
}
