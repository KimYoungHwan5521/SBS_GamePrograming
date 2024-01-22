using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Heros
{
    internal class Golem : Character
    {
        public Golem() : base("골렘", 300, 6000, 230)
        {
            nowMP = 1000;
            cooldownNow = cooldownMax = 4;
            race = Race.Golem;
        }

        public override int Attack(Character[] enemy)
        {
            int damage = 0;
            if(nowMP > 250 && cooldownNow >= cooldownMax)
            {
                Console.WriteLine($"{this.name}의 지축 흔들기! ");
                for(int i = 0; i < enemy.Length; i++)
                {
                    if (enemy[i] == this || enemy[i].isDead) continue;
                    damage = enemy[i].TakeDamage(this, attackDamage, false);
                    Console.WriteLine($"{enemy[i].name}에게 {damage}의 데미지!");
                    if (enemy[i].isDead)
                    {
                        Console.WriteLine($"{enemy[i].name}은(는) 쓰러졌다!");
                        killCount++;
                    }
                }
                nowMP -= 250;
                cooldownNow = 0;
            }
            else
            {
                Random ran = new Random();
                int r = ran.Next(enemy.Length);
                int victim;
                while (enemy[r].isDead || enemy[r] == this)
                {
                    ran = new Random();
                    r = ran.Next(enemy.Length);
                }
                victim = r;

                damage = enemy[r].TakeDamage(this, attackDamage, false);
                Console.WriteLine($"{this.name}의 공격! {enemy[victim].name}에게 {damage}의 데미지!");
                if (enemy[victim].isDead)
                {
                    Console.WriteLine($"{enemy[victim].name}은(는) 쓰러졌다!");
                    killCount++;
                }
                cooldownNow++;
            }
            return damage;
        }
    }
}
