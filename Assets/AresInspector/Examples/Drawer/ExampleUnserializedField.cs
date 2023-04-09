using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ares.Examples
{
    public class ExampleUnserializedField : ScriptableObject, IAresObjectV
    {
        public int i;

        [ADUnserializedField]
        int m_PrivateNumInt;

        [ADUnserializedField]
        [ACReadOnly]
        float m_PrivateNumFloat;

        [ADUnserializedField]
        long m_PrivateNumLong;

        [ADUnserializedField]
        bool m_PrivateNumbool;

        [ADButton]
        void IncInt()
        {
            m_PrivateNumInt += 1;
        }

        [ADButton]
        void DecInt()
        {
            m_PrivateNumInt -= 1;
        }

        [ADButton]
        void IncFLoat()
        {
            m_PrivateNumFloat += 1.1f;
        }

        [ADButton]
        void DecFloat()
        {
            m_PrivateNumFloat -= 1.1f;
        }
    }
}
