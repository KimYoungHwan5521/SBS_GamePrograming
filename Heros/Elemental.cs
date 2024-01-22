using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Heros
{
    internal class Elemental : Character
    {
        public Elemental() : base("정령왕", 600, 3200, 200)
        {
            nowMP = maxMP = 2000;
            cooldownNow = cooldownMax = 1;
            race = Race.Elemental;
        }


        public override int Attack(Character[] enemy)
        {
            Random ran = new Random();
            int r = ran.Next(3);
            if(cooldownNow >= cooldownMax)
            {
                switch(r)
                {
                    case 0:
                        this.name = "정령왕(화염)";
                        Console.WriteLine("정령왕의 폼체인지 정령왕(화염)이 되었다.");
                        break;
                    case 1:
                        this.name = "정령왕(냉기)";
                        Console.WriteLine("정령왕의 폼체인지 정령왕(냉기)이 되었다.");
                        break;
                    case 2:
                        this.name = "정령왕(번개)";
                        Console.WriteLine("정령왕의 폼체인지 정령왕(번개)이 되었다.");
                        break;
                }
            }
            int damage = 0;
            int victim;
            switch (this.name)
            {
                case "정령왕(화염)":
                    int bullet;
                    if(nowMP >= 500)
                    {
                        bullet = 3;
                        nowMP -= 500;
                    }
                    else if(nowMP >= 200)
                    {
                        bullet = 2;
                        nowMP -= 200;
                    }
                    else { bullet = 1; }
                    for(int i=0 ; i< bullet; i++)
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
                        Console.WriteLine($"{this.name}의 화염구! {enemy[victim].name}에게 {damage}의 데미지!");
                        if (enemy[victim].isDead)
                        {
                            Console.WriteLine($"{enemy[victim].name}은(는) 쓰러졌다!");
                            killCount++;
                        }
                    }
                    break;
                case "정령왕(냉기)":
                    if(nowMP < 1000)
                    {
                        nowMP += 1000;
                        Console.WriteLine($"{this.name}의 물마시기! 1000의 MP를 회복!");
                    }
                    else
                    {
                        Console.WriteLine($"{this.name}의 쓰나미! ");
                        for (int i = 0; i < enemy.Length; i++)
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
                        nowMP -= 1000;
                    }
                    break;
                case "정령왕(번개)":
                    bool isCritical;
                    if(nowMP >= 200)
                    {
                        isCritical = true;
                    }
                    else { isCritical= false; }
                    while (enemy[r].isDead || enemy[r] == this)
                    {
                        ran = new Random();
                        r = ran.Next(enemy.Length);
                    }
                    victim = r;

                    damage = enemy[victim].TakeDamage(this, attackDamage, isCritical);
                    Console.WriteLine($"{this.name}의 공격! {enemy[victim].name}에게 {damage}의 데미지!");
                    if (enemy[victim].isDead)
                    {
                        Console.WriteLine($"{enemy[victim].name}은(는) 쓰러졌다!");
                        killCount++;
                    }
                    break;
                default:
                    Console.WriteLine($"잘못된 이름 \"{this.name}\"");
                    break;
            }
            return damage;
        }
    }
}
