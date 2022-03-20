using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleSystem
{
    public class Logger : MonoBehaviour
    {
        Text m_text = null;
        string m_endLog = "\n------------------------------------------------------\n";

        // Start is called before the first frame update
        void Start()
        {
            LogManager.GetSingleton().SetLogger(this);
            m_text = GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Log(string _log)
        {
            string[] text =  m_text.text.Split('\n');
            string[] output = (_log + m_endLog).Split('\n');
            if (text.Length >= 34)
            {
                m_text.text = "";
                for (int i = output.Length - 1; i < text.Length - 1; i++)
                {
                    m_text.text += text[i] + "\n";
                }
            }
            m_text.text += _log + m_endLog;
        }
    }
}