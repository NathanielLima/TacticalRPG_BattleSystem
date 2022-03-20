using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleSystem
{
    public class NumberEffect : MonoBehaviour
    {
        RectTransform m_rectTransform = null;
        float m_currentTime = 0.0f;
        float m_maxTime = 0.75f;

        // Start is called before the first frame update
        void Start()
        {
            Destroy(gameObject, m_maxTime);
        }

        // Update is called once per frame
        void Update()
        {
            m_currentTime += Time.deltaTime;
            m_rectTransform.localPosition = Vector3.up * 32.0f * Mathf.Sin(m_currentTime * Mathf.PI / m_maxTime);
        }

        public void SetEffectValues(string _text, Color _color)
        {
            Text text = GetComponent<Text>();
            m_rectTransform = GetComponent<RectTransform>();
            text.text = _text;
            text.color = _color;
        }
    }
}