using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Ares
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    [Conditional("UNITY_EDITOR")]
    public class AresGroup : AresAttribute
    {
        public int id;
        public int parentId;
        public List<AresGroup> groups = new List<AresGroup>();
        public List<AresMember> members = new List<AresMember>();

        public AresGroup(int id, int parentId)
        {
            this.id = id;
            this.parentId = parentId;
        }
    }
}
