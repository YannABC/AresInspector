using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ares.Examples
{
    /// <summary>
    /// unity是不会序列化property的，我们也不想支持property的序列化，因为感觉没必要
    /// ADProperty不是为了序列化的，只是需要显示在编辑器的时候才加
    /// 可以在编辑器中修改，运行时能生效，但不会被序列化
    /// </summary>
    public class ExampleProperty : ScriptableObject, IAresObjectV
    {
        public int i;

        [ADProperty]
        int privateNumInt { get; set; }

        [ADProperty]
        [ACReadOnly]
        float privateNumFloat { get; set; }

        [ADProperty]
        public long privateNumLong { get; }

        [ADProperty]
        bool privateNumbool { get; set; }

        [ADButton]
        void IncInt()
        {
            privateNumInt += 1;
        }

        [ADButton]
        void DecInt()
        {
            privateNumInt -= 1;
        }

        [ADButton]
        void IncFLoat()
        {
            privateNumFloat += 1.1f;
        }

        [ADButton]
        void DecFloat()
        {
            privateNumFloat -= 1.1f;
        }
    }
}
