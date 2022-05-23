using System;
using UnityEngine;

namespace BattleSystem
{
    [Serializable]
    public class CharacterSheet
    {
        public string m_name = "";

        public Sprite m_face = null;

        public RuntimeAnimatorController m_controller = null;
        public AnimationClip[] m_idle = new AnimationClip[4];
        public AnimationClip[] m_walk = new AnimationClip[4];
        /*public AnimationClip[] m_attack = new AnimationClip[4];
        public AnimationClip[] m_cast = new AnimationClip[4];
        public AnimationClip[] m_use = new AnimationClip[4];
        public AnimationClip[] m_dead = new AnimationClip[4];*/

        public string m_description = "";

        public string m_weaponName = "";
        public Item m_weapon = null;

        public int m_maxLife = 0;
        public int m_maxMana = 0;

        public int m_str = 0;
        public int m_def = 0;
        public int m_magStr = 0;
        public int m_magDef = 0;
        public int m_sp = 0;
    }
}
