using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Heros
{
    internal class Undead : Character
    {
        public Undead() : base("좀비", 400, 1600, 170)
        {
            nowMP = 400;
            cooldownNow = cooldownMax = 3;
            race = Race.Undead;
        }

        public override int Attack(Character[] enemy)
        {
            int damage = 0;

            Random ran = new Random();
            int r = ran.Next(enemy.Length);
            int victim;
            while (enemy[r].isDead || enemy[r] == this)
            {
                ran = new Random();
                r = ran.Next(enemy.Length);
            }
            victim = r;

            if (cooldownNow >= cooldownMax)
            {
                cooldownNow = 0;
                damage = enemy[victim].TakeDamage(this, attackDamage, true);
                Console.WriteLine($"{this.name}의 치명타! {enemy[victim].name}에게 {damage}만큼의 피해!");

            }
            else
            {
                cooldownNow++; 
                damage = enemy[victim].TakeDamage(this, attackDamage, false);
                Console.WriteLine($"{this.name}의 공격! {enemy[victim].name}에게 {damage}만큼의 피해!");

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
