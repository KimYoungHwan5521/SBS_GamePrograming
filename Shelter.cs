using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS
{
    // class : 중간에 없어질 수도 있는 친구
    //         플레이어가 없다 << 없는 것도 정보
    // struct : "없다"로 무마할 수 없다.
    //          변수가 선언이 되면 그 안에 내용은 무조건 채워져있어야 한다.
    //          상점은 여러 종류의 물건을 가지고 있습니다.
    //          각 물건을 몇 개씩 가지고 있나
    //          "정보"를 저장하는 친구
    struct SellInfo
    {
        public ItemBase item;
        public int price;
        public int quantity;
    }

    // 아이템은 기능도 있다
    // ItemBase는 정말로 있는 걸까
    // "농기구"                        "추상적 개념"
    // "낫", "호미", "삽", "갈퀴"...
    abstract class ItemBase
    {
        public string name = "!잘못된 아이템 이름";

        // 왜 bool ? -> 생명력 포션을 쓰고 싶어요 : 제 생명력이 가득 찬 상태라면 사용x
        public abstract bool Use(CharacterDungeon user, Character target);
    }

    internal class Shelter
    {
    }
}
