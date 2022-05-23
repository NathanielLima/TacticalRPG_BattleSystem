using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem
{
    [Serializable]
    public class Inventory
    {
        public BaseInventory m_baseInventoy = null;
        Dictionary<string, Item> m_items = new Dictionary<string, Item>();
        [NonSerialized] public List<string> m_itemNames = new List<string>();
        //Dictionary<string, Item> m_weapons = new Dictionary<string, Item>();
        [NonSerialized] public List<string> m_weaponNames = new List<string>();

        public void SetBaseInventory()
        {
            foreach (Item current in BattleSystemManager.GetSingleton().m_database.m_baseInventory.m_items)
            {
                AddItem(current);
            }
        }

        public void AddItem(Item _item)
        {
            if (m_items.ContainsKey(_item.m_name))
            {
                m_items[_item.m_name].m_count += _item.m_count;
            }
            else
            {
                m_items.Add(_item.m_name, _item);
                switch (_item.m_itemType)
                {
                    case ItemType.CONSUMABLE:
                        m_itemNames.Add(_item.m_name);
                        break;
                    case ItemType.WEAPON:
                        m_weaponNames.Add(_item.m_name);
                        break;
                    default:
                        break;
                }
            }
        }

        public Item RemoveItem(string _name)
        {
            if (m_items.ContainsKey(_name))
            {
                Item remove = m_items[_name];
                --m_items[_name].m_count;
                if (m_items[_name].m_count == 0)
                {
                    m_items.Remove(_name);
                    m_itemNames.Remove(_name);
                }
                return remove;
            }
            return new Item();
        }

        public int GetItemCount(string _name)
        {
            if (m_items.ContainsKey(_name))
            {
                return m_items[_name].m_count;
            }
            return 0;
        }

        public void AddWeapon(Item _weapon)
        {
            if (m_items.ContainsKey(_weapon.m_name))
            {
                m_items[_weapon.m_name].m_count += _weapon.m_count;
            }
            else
            {
                m_items.Add(_weapon.m_name, _weapon);
                m_weaponNames.Add(_weapon.m_name);
            }
        }

        public Item RemoveWeapon(string _name)
        {
            if (m_items.ContainsKey(_name))
            {
                Item remove = m_items[_name];
                --m_items[_name].m_count;
                if (m_items[_name].m_count == 0)
                {
                    m_items.Remove(_name);
                    m_weaponNames.Remove(_name);
                }
                return remove;
            }
            return new Item();
        }

        public int GetWeaponCount(string _name)
        {
            if (m_items.ContainsKey(_name))
            {
                return m_items[_name].m_count;
            }
            return 0;
        }
    }
}
