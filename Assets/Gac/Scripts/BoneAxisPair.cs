using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BoneAxisPair
{
    public string Name { get; set; }
    public Vector3 Mask { get; set; }

    public BoneAxisPair()
    {
        Name="";
    }
    public BoneAxisPair(string name, Vector3 mask)
    {
        Name = name;
        Mask = mask;
    }
}
