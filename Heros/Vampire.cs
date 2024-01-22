using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Heros
{
    internal class Vampire : Character
    {
        public Vampire() : base("노스페라투", 800, 3000, 190)
        {
            nowMP = maxMP = 0;
            cooldownNow = cooldownMax = 2;
            race = Race.Vampire;
        }

        public override int Attack(Character[] enemy)
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

            int damage = enemy[victim].TakeDamage(this, attackDamage, false);
            Console.WriteLine($"{this.name}의 공격! {enemy[victim].name}에게 {damage}의 데미지!");
            if(cooldownNow >= cooldownMax)
            {
                cooldownNow = 0;
                if (enemy[victim].race == Race.Golem)
                {
                    Console.WriteLine($"{this.name}의 흡혈! 하지만 {enemy[victim].name}은(는) 골렘 흡혈할 피는 없었다!");
                }
                else
                {
                    nowHP += damage;
                    Console.WriteLine($"{this.name}의 흡혈! {damage}만큼의 체력 회복!");
                }
            }
            else
            {
                cooldownNow++;
            }
            if (enemy[victim].isDead)
            {
                Console.WriteLine($"{enemy[victim].name}은(는) 쓰러졌다!");
                killCount++;
            }
            return damage;
        }
    }
}
