using UnityEngine;
using System.Collections.Generic;

public class ComponentLoader : MonoBehaviour {
    public string ArmatureName = "Armature";
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
    /// <summary>
    /// Destroys all of the character components and recreates them all
    /// </summary>
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
    /// <summary>
    /// Removes a character component
    /// </summary>
    /// <param name="part">The character component to remove</param>
    public void Remove(GameObject part)
    {
        for (int i = 0; i < Parts.Count; i++)
        {
            if (Parts[i]==part||generatedChildren[i]==part)
            {
                Destroy(generatedChildren[i]);
                Parts.RemoveAt(i);
                generatedChildren.RemoveAt(i);
            }
        }
    }
    /// <summary>
    /// Removes a character component at a given index
    /// </summary>
    /// <param name="index">the index of the item to remove</param>
    public void RemoveAt(int index)
    {
        Destroy(generatedChildren[index]);
        Parts.RemoveAt(index);
        generatedChildren.RemoveAt(index);
    }
    /// <summary>
    /// Adds a character component to a character
    /// </summary>
    /// <param name="part">The component to be added (a copy of this object will be added)</param>
    public GameObject Add(GameObject part)
    {
        Parts.Add(part);
        GameObject go = Instantiate(part);
        generatedChildren.Add(go);

        skinMesh(go);
        return go;
    }
    /// <summary>
    /// Adds a character component to the character at a given index
    /// </summary>
    /// <param name="index">The index to add the component</param>
    /// <param name="part">The component to be added (a copy of this object will be added)</param>
    public GameObject Insert(int index,GameObject part)
    {
        Parts.Insert(index,part);
        GameObject go = Instantiate(part);
        generatedChildren.Insert(index,go);

        skinMesh(go);
        return go;
    }
    void skinMesh(GameObject go)
    {
        Transform goTrans = go.transform;
        goTrans.parent = trans;
        Destroy(goTrans.Find(ArmatureName).gameObject);
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
