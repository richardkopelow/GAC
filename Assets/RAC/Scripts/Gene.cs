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
    /// <summary>
    /// The value of the gene
    /// </summary>
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
    /// <summary>
    /// Creates a new gene with an empty name
    /// </summary>
    public Gene():this("")
    {
    }
    /// <summary>
    /// Creates a new gene with a given name
    /// </summary>
    /// <param name="name">The name of the gene</param>
    public Gene(string name)
    {
        Name = name;
        Bones = new List<BoneAxisPair>();
    }
    /// <summary>
    /// Called by the RAC system to update the bones with this gene.
    /// </summary>
    /// <returns>true: if successful</returns>
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
