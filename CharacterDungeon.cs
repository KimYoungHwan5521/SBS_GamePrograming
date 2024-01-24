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
        public int nowHP
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
        public int maxMP;
        private int _nowMP;
        public int nowMP
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

        public CharacterDungeon(string name, int maxHP, int maxMP, int attackDamage, int defense)
        {
            this.name = name;
            this.nowHP = this.maxHP = maxHP;
            this.nowMP = this.maxMP = maxMP;
            this.attackDamage = attackDamage;
            this.defense = defense;
        }

        public virtual int Attack(CharacterDungeon enemy, bool isIgnoreDefense = false)
        {
            return enemy.TakeDamage(this, attackDamage, isIgnoreDefense);
        }

        public virtual int TakeDamage(CharacterDungeon from, int damage, bool isIgnoreDefense = false)
        {
            if(isIgnoreDefense)
            {
                nowHP -= damage;
            }
            else
            {
                nowHP -= damage - defense;
            }
            return damage;
        }

        public virtual bool Skill()
        {
            return false;
        }
    }
}
