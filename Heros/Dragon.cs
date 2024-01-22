using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Heros
{
    internal class Dragon : Character
    {
        public Dragon() : base("용의알", 500, 3200, 400)
        {
            nowMP = 0;
            cooldownNow = cooldownMax = 0;
            race = Race.Dragon;
        }

        public int count = 3;
        public bool isAwaken = false;
        public override int Attack(Character[] enemy)
        {
            int damage = 0;
            if(count > 0)
            {
                Console.WriteLine($"알이 꿈틀 거린다.({count} 남음)");
                count--;
            }
            else
            {
                if (!isAwaken)
                {
                    Console.WriteLine("용이 깨어났다!");
                    this.name = "깨어난 용";
                    isAwaken= true;
                }

                Random ran = new Random();
                int r = ran.Next(enemy.Length);
                int victim;
                while (enemy[r].isDead || enemy[r] == this)
                {
                    ran = new Random();
                    r = ran.Next(enemy.Length);
                }
                victim = r;

                damage = enemy[victim].TakeDamage(this, attackDamage, true);
                Console.WriteLine($"{this.name}의 브레스! {enemy[victim].name}에게 {damage}의 데미지!");
                if (enemy[victim].isDead)
                {
                    Console.WriteLine($"{enemy[victim].name}은(는) 쓰러졌다!");
                    killCount++;
                }
                for (int i=0; i<enemy.Length; i++)
                {
                    if (enemy[i] != this && !enemy[i].isDead && i != victim)
                    {
                        damage = enemy[i].TakeDamage(this, attackDamage, false);
                        Console.WriteLine($"{enemy[i].name}에게 {damage}의 데미지!");
                        if (enemy[i].isDead)
                        {
                            Console.WriteLine($"{enemy[i].name}은(는) 쓰러졌다!");
                            killCount++;
                        }

                    }
                }
            }
            return damage;
        }
    }
}
