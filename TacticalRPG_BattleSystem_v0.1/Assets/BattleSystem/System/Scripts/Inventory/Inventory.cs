using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem
{
    [CreateAssetMenu(fileName = "Inventory", menuName = "BattleSystem/Inventory", order = 3)]
    public class Inventory : ScriptableObject
    {
        public BaseInventory m_baseInventoy = null;
        Dictionary<string, Item> m_items = new Dictionary<string, Item>();
        [NonSerialized] public List<string> m_itemNames = new List<string>();
        Dictionary<string, Weapon> m_weapons = new Dictionary<string, Weapon>();
        [NonSerialized] public List<string> m_weaponNames = new List<string>();

        public void SetBaseInventory()
        {
            foreach (Item current in m_baseInventoy.m_items)
            {
                AddItem(current);
            }
            foreach (Weapon current in m_baseInventoy.m_weapons)
            {
                AddWeapon(current);
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
                m_items.Add(_item.m_name, Item.CreateCopy(_item));
                m_itemNames.Add(_item.m_name);
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

        public void AddWeapon(Weapon _weapon)
        {
            if (m_weapons.ContainsKey(_weapon.m_name))
            {
                m_weapons[_weapon.m_name].m_count += _weapon.m_count;
            }
            else
            {
                m_weapons.Add(_weapon.m_name, Weapon.CreateCopy(_weapon));
                m_weaponNames.Add(_weapon.m_name);
            }
        }

        public Weapon RemoveWeapon(string _name)
        {
            if (m_weapons.ContainsKey(_name))
            {
                Weapon remove = m_weapons[_name];
                --m_weapons[_name].m_count;
                if (m_weapons[_name].m_count == 0)
                {
                    m_weapons.Remove(_name);
                    m_weaponNames.Remove(_name);
                }
                return remove;
            }
            return new Weapon();
        }

        public int GetWeaponCount(string _name)
        {
            if (m_weapons.ContainsKey(_name))
            {
                return m_weapons[_name].m_count;
            }
            return 0;
        }
    }
}
