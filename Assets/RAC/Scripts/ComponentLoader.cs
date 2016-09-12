using UnityEngine;
using System.Collections.Generic;

public class ComponentLoader : MonoBehaviour {

    public List<GameObject> Parts;
    List<GameObject> generatedChildren;
	// Use this for initialization
    Transform trans;
	void Start ()
	{
        trans = transform;
        generatedChildren = new List<GameObject>();

        Rebuild();
	}

    public void Rebuild()
    {
        foreach (GameObject go in generatedChildren)
        {
            Destroy(go);
        }
        foreach (GameObject template in Parts)
        {
            GameObject go = Instantiate(template);
            generatedChildren.Add(go);
            skinMesh(go);
        }
    }

    public void Remove(GameObject part)
    {
        for (int i = 0; i < Parts.Count; i++)
        {
            if (Parts[i]==part||generatedChildren[i]==part)
            {
                Parts.RemoveAt(i);
                generatedChildren.RemoveAt(i);
            }
        }
    }
    public void RemoveAt(int index)
    {
        Parts.RemoveAt(index);
        generatedChildren.RemoveAt(index);
    }
    public void Add(GameObject part)
    {
        Parts.Add(part);
        GameObject go = Instantiate(part);
        generatedChildren.Add(go);

        skinMesh(go);
    }
    public void Insert(int index,GameObject part)
    {
        Parts.Insert(index,part);
        GameObject go = Instantiate(part);
        generatedChildren.Insert(index,go);

        skinMesh(go);
    }
    void skinMesh(GameObject go)
    {
        Transform goTrans = go.transform;
        goTrans.parent = trans;
        Destroy(goTrans.Find("Armature").gameObject);
        SkinnedMeshRenderer[] renderers = go.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer ren in renderers)
        {
            Transform[] mainBones = new Transform[ren.bones.Length];
            for (int i = 0; i < ren.bones.Length; i++)
            {
                mainBones[i] = findChildByName(trans, ren.bones[i].name);
            }
            ren.bones = mainBones;
            ren.rootBone = mainBones[0];
        }
    }

    Transform findChildByName(Transform parent, string name)
    {
        if (parent.name == name)
        {
            return parent;
        }
        else
        {
            for (int j = 0; j < parent.childCount; j++)
            {
                Transform t = findChildByName(parent.GetChild(j), name);
                if (t != null)
                {
                    return t;
                }
            }
        }

        return null;
    }
}
