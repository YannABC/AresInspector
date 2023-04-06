using System;
using System.Diagnostics;

namespace Ares
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    [Conditional("UNITY_EDITOR")]
    public class ACReadOnly : System.Attribute
    {
        public ACReadOnly()
        {

        }
    }
}