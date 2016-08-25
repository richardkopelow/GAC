using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BoneCollection
{
    public Bone Root { get; set; }
    public BoneCollection()
    {
        Root = new Bone();
    }

    public BoneCollection(Transform transform)
    {
        Root = new Bone(transform);
    }

    public void SetBoneScale(string boneName,string scaleName, Vector3 scale)
    {
        GetBone(boneName).SetScale(scaleName,scale);
    }

    public Bone GetBone(string name)
    {
        return FindBoneByName(Root, name);
    }

    Bone FindBoneByName(Bone root, string name)
    {
        if (root.Name==name)
        {
            return root;
        }
        else
        {
            foreach (Bone b in root.Children)
            {
                Bone ret= FindBoneByName(b, name);
                if (ret!=null)
                {
                    return ret;
                }
            }
        }
        return null;
    }

    public void Update()
    {
        Root.Update();
    }
}
