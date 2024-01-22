using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS
{
    internal class Shop
    {
        // private, public
        public string name = "Shop";
        public string consumableItem = "Health Potion";
        public int price = 4000;
        public int stock = 10;
        private int cash = 100000;


        // 로딩 -> 데이터를 RAM에 올려 놓는것
        // 생성자
        public Shop(string name, string consumableItem, int price, int stock, int cash = 100000) 
        { 
            this.name = name;
            this.consumableItem = consumableItem;
            this.price = price;
            this.stock = stock;
            this.cash = cash;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"안녕하세요 {name} 입니다.");
            Console.WriteLine($"상점 자산 : {cash} 원");
            Console.WriteLine("----------판매 목록------------");
            if(stock == 0) Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{consumableItem} : {price} 원\t| 재고 : {stock}");
            Console.ResetColor();
            Console.WriteLine();
        }

        public bool HaveCash(int money)
        {
            return cash >= money;
        }

        public int SellItem(int itemPrice)
        {
            if(HaveCash(itemPrice))
            {
                cash -= itemPrice;
                return itemPrice;
            }
            else
            {
                Console.WriteLine("Not enough cash");
                return 0;
            }
        }

    }
}
