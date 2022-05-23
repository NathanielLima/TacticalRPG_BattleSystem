using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem
{
    public class EffectManager
    {
        static EffectManager m_instance = null;
        EffectLayer m_effectLayer = null;

        public static EffectManager GetSingleton()
        {
            if (m_instance == null)
            {
                m_instance = new EffectManager();
            }
            return m_instance;
        }

        public void SetEffectLayer(EffectLayer _effectLayer)
        {
            m_effectLayer = _effectLayer;
        }

        public void CreateNumberEffect(int _value, Color _color, Vector3 _position)
        {
            if (m_effectLayer)
            {
                m_effectLayer.CreateNumberEffect(_value, _color, _position);
            }
        }

        public void CreateTextEffect(string _text, Color _color, Vector3 _position)
        {
            if (m_effectLayer)
            {
                m_effectLayer.CreateTextEffect(_text, _color, _position);
            }
        }
    }
}