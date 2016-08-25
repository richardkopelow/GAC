using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Bone
{
    public string Name { get; set; }
    public Bone Parent { get; set; }
    public List<Bone> Children { get; set; }
    public Transform Trans;

    public Dictionary<string,Vector3> Scales { get; set; }

    public Vector3 GlobalScale
    {
        get
        {
            if (localScale!=Trans.localScale)
            {
                animationScale = Trans.localScale;
            }
            Vector3 net = animationScale;
            foreach (Vector3 scale in Scales.Values)
            {
                net = new Vector3(net.x * scale.x, net.y * scale.y, net.z * scale.z);
            }
            return net;
        }
    }

    private Vector3 localScale;
    private Vector3 animationScale;

    public Bone()
    {
        Name = "";
        Children = new List<Bone>();
        Scales = new Dictionary<string,Vector3>();
        Scales.Add("",new Vector3(1,1,1));
    }

    public Bone(Transform transform)
        : this()
    {
        Name = transform.name;
        Trans = transform;
        for (int i = 0; i < transform.childCount; i++)
        {
            Bone child = new Bone(transform.GetChild(i));
            child.Parent = this;
            Children.Add(child);
        }
    }

    public void CalculateScale()
    {
        Vector3 globalScale = GlobalScale;
        Vector3 identity = new Vector3(1, 1, 1);
        Vector3 parentGlobalScale = Parent != null ? Parent.GlobalScale - identity : identity;
        //Quaternion relativeRotation = Trans.rotation;
        //Quaternion relativeRotation = Quaternion.Inverse(Trans.rotation)*Parent.Trans.rotation;
        //Vector3 localSpaceDistortion = Trans.localRotation*parentGlobalScale;

        if (Parent != null)
        {
            float i1i1 = Math.Abs(Vector3.Dot(Trans.right, Parent.Trans.right));
            float i1i2 = Math.Abs(Vector3.Dot(Trans.right, Parent.Trans.up));
            float i1i3 = Math.Abs(Vector3.Dot(Trans.right, Parent.Trans.forward));
            float i2i1 = Math.Abs(Vector3.Dot(Trans.up, Parent.Trans.right));
            float i2i2 = Math.Abs(Vector3.Dot(Trans.up, Parent.Trans.up));
            float i2i3 = Math.Abs(Vector3.Dot(Trans.up, Parent.Trans.forward));
            float i3i1 = Math.Abs(Vector3.Dot(Trans.forward, Parent.Trans.right));
            float i3i2 = Math.Abs(Vector3.Dot(Trans.forward, Parent.Trans.up));
            float i3i3 = Math.Abs(Vector3.Dot(Trans.forward, Parent.Trans.forward));

            Vector3 i1 = new Vector3(i1i1, i1i2, i1i3);
            Vector3 i2 = new Vector3(i2i1, i2i2, i2i3);
            Vector3 i3 = new Vector3(i3i1, i3i2, i3i3);

            Vector3 localSpaceDistortion = new Vector3(Vector3.Dot(i1, parentGlobalScale), Vector3.Dot(i2, parentGlobalScale), Vector3.Dot(i3, parentGlobalScale));

            localSpaceDistortion += identity;

            localScale = new Vector3(globalScale.x / localSpaceDistortion.x, globalScale.y / localSpaceDistortion.y, globalScale.z / localSpaceDistortion.z);
        }
        else
        {
            localScale = globalScale;
        }

        //localScale = new Vector3(_globalScale.x / Parent.GlobalScale.x, _globalScale.y / Parent.GlobalScale.y, _globalScale.z / Parent.GlobalScale.z);
        //localScale = new Vector3(_globalScale.x / Parent.Trans.lossyScale.x, _globalScale.y / Parent.Trans.lossyScale.y, _globalScale.z / Parent.Trans.lossyScale.z);
        Trans.localScale = localScale;
    }

    public void Update()
    {
        CalculateScale();
        foreach (Bone child in Children)
        {
            child.Update();
        }
    }

    public void SetScale(string name, Vector3 scale)
    {
        if (Scales.ContainsKey(name))
        {
            Scales[name] = scale;
        }
        else
        {
            Scales.Add(name, scale);
        }
    }
}
