using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Heros
{
    internal class Dwarf : Character
    {
        public Dwarf() : base("드워프", 100, 4200, 120)
        {
            nowMP = 1000;
            cooldownNow = cooldownMax = 0;
        }

        public int tower = -1;
        public override int Attack(Character[] enemy)
        {
            int damage = 0;
            if(tower == -1)
            {
                Console.WriteLine($"{this.name}은(는) 타워를 설치했다.");
                tower++;
            }
            else
            {
                if(nowMP >= 100)
                {
                    Console.WriteLine($"{this.name}은(는) 타워를 강화했다.");
                    tower++;
                    nowMP -= 100;
                }
                Random ran;
                int r;
                int victim;

                for (int i = 0; i < tower; i++)
                {
                    ran = new Random();
                    r = ran.Next(enemy.Length);

                    int check = 0;
                    while (enemy[r].isDead || enemy[r] == this)
                    {
                        ran = new Random();
                        r = ran.Next(enemy.Length);
                        check++;
                        if (check > 1000) break;
                    }
                    victim = r;
                    damage = enemy[victim].TakeDamage(this, attackDamage, false);
                    Console.WriteLine($"{this.name}의 포탑공격! {enemy[victim].name}에게 {damage}의 데미지!");
                    if (enemy[victim].isDead)
                    {
                        Console.WriteLine($"{enemy[victim].name}은(는) 쓰러졌다!");
                        killCount++;
                    }
                }

            }
            return damage;
        }
    }
}
