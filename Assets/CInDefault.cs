using Ares;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CInDefault : MonoBehaviour
{
    public enum EA
    {
        None = 0,
        A = 1,
        B = 2,
    }
    [AresField(label = "ttt", show = "aaa")]
    [Range(33, 44)]
    [Space(100)]
    public int i;

    public EA A;
    public List<int> B;
    public void Start()
    {

    }
}
