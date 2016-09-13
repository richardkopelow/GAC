using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

public class Gene
{
    [XmlIgnore]
    public DNAController Controller { get; set; }
    public string Name { get; set; }

    private float _value = 1;
    public float Value
    {
        get { return _value; }
        set
        {
            _value = value;
            if (Controller != null)
            {
                UpdateBones();
            }
        }
    }

    public List<BoneAxisPair> Bones { get; set; }
    public Gene()
    {
        Name = "";
        Bones = new List<BoneAxisPair>();
    }

    public bool UpdateBones()
    {
        try
        {
            foreach (BoneAxisPair pair in Bones)
            {
                Controller.Bones.SetBoneScale(pair.Name, Name, (_value - 1) * pair.Mask+new Vector3(1,1,1));
            }
            return true;
        }
        catch
        {
            Debug.LogError("Error assigning scale to bone in gene " + Name);
            return false;
        }
    }
}
