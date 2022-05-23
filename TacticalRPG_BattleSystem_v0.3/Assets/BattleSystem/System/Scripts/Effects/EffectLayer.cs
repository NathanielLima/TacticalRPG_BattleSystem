using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem
{
    public class EffectLayer : MonoBehaviour
    {
        [SerializeField] GameObject m_numberEffectPrefab = null;

        // Start is called before the first frame update
        void Start()
        {
            EffectManager.GetSingleton().SetEffectLayer(this);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CreateNumberEffect(int _value, Color _color, Vector3 _position)
        {
            GameObject numberEffect = Instantiate(m_numberEffectPrefab, transform);
            numberEffect.transform.position = Camera.main.WorldToScreenPoint(_position);
            numberEffect.GetComponentInChildren<NumberEffect>().SetEffectValues(_value.ToString(), _color);
        }

        public void CreateTextEffect(string _text, Color _color, Vector3 _position)
        {
            GameObject numberEffect = Instantiate(m_numberEffectPrefab, transform);
            numberEffect.transform.position = Camera.main.WorldToScreenPoint(_position);
            numberEffect.GetComponentInChildren<NumberEffect>().SetEffectValues(_text, _color);
        }
    }
}