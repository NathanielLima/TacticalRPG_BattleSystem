using UnityEngine;

namespace BattleSystem
{
    [CreateAssetMenu(fileName = "Item", menuName = "BattleSystem/Item", order = 4)]
    public class Item : ScriptableObject
    {
        public string m_name = "";
        public int m_count = 1;

        public int m_life = 0;
        public int m_mana = 0;

        public static Item CreateCopy(Item _source)
        {
            Item item = CreateInstance<Item>();
            item.m_name = _source.m_name;
            item.m_count = _source.m_count;
            item.m_life = _source.m_life;
            item.m_mana = _source.m_mana;
            return item;
        }
    }
}
