using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

public class DNAController : MonoBehaviour {

    Transform trans;
    public BoneCollection Bones { get; set; }
    public List<Gene> Genes;

	// Use this for initialization
	void Start ()
	{
        trans = transform;
        Transform armature = trans.FindChild("Armature");
        Bones = new BoneCollection(armature);

        Genes = new List<Gene>();
        /*
        Gene height = new Gene();
        height.Controller = this;
        height.Name = "Height";
        height.Bones.Add(new BoneAxisPair("Cube",new Vector3(0,0,1)));
        height.Value = 2;
        */
        //*
        Gene height = new Gene();
        height.Controller = this;
        height.Name = "Height";
        //height.Bones.Add(new BoneAxisPair("hips", new Vector3(1, 0, 0)));
        height.Bones.Add(new BoneAxisPair("spine", new Vector3(1, 0, 0)));
        height.Bones.Add(new BoneAxisPair("spine-1", new Vector3(1, 0, 0)));
        height.Bones.Add(new BoneAxisPair("chest", new Vector3(1, 0, 0)));
        height.Bones.Add(new BoneAxisPair("chest-1", new Vector3(1, 0, 0.2f)));
        //height.Bones.Add(new BoneAxisPair("clavicle_L", new Vector3(0, 1, 0)));
        //height.Bones.Add(new BoneAxisPair("clavicle_R", new Vector3(0, 1, 0)));
        height.Bones.Add(new BoneAxisPair("thigh_L", new Vector3(1, 0, 0)));
        height.Bones.Add(new BoneAxisPair("shin_L", new Vector3(1, 0, 0)));
        height.Bones.Add(new BoneAxisPair("thigh_R", new Vector3(1, 0, 0)));
        height.Bones.Add(new BoneAxisPair("shin_R", new Vector3(1, 0, 0)));
        Genes.Add(height);
        height.Value = 0.6f;

        Gene armLength = new Gene();
        armLength.Controller = this;
        armLength.Name = "ArmLength";
        armLength.Bones.Add(new BoneAxisPair("upper_arm_L",new Vector3(1,0,0)));
        armLength.Bones.Add(new BoneAxisPair("forearm_L", new Vector3(1,0,0)));
        armLength.Bones.Add(new BoneAxisPair("upper_arm_R", new Vector3(1, 0, 0)));
        armLength.Bones.Add(new BoneAxisPair("forearm_R", new Vector3(1, 0, 0)));
        Genes.Add(armLength);
        armLength.Value = 0.6f;

        Gene breastSize = new Gene();
        breastSize.Controller = this;
        breastSize.Name = "BreastSize";
        breastSize.Bones.Add(new BoneAxisPair("breast_L",new Vector3(1,1,1)));
        breastSize.Bones.Add(new BoneAxisPair("breast_R", new Vector3(1, 1, 1)));
        Genes.Add(breastSize);
        breastSize.Value = 1f;
        //*/

        LoadDNA(@"C:\Users\Class2018\Desktop\geneSaveTest.xml");
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
        Bones.Update();
	}

    public void SaveDNA(string path)
    {
        FileInfo fi = new FileInfo(path);
        if (!fi.Exists)
        {
            fi.Create().Close();
        }
        using (StreamWriter sw = new StreamWriter(path))
        {
            XmlSerializer xmls = new XmlSerializer(typeof(List<Gene>));
            xmls.Serialize(sw, Genes);
        }
    }
    public bool LoadDNA(string path)
    {
        FileInfo fi = new FileInfo(path);
        if (!fi.Exists)
        {
            return false;
        }
        using (StreamReader sr = new StreamReader(path))
        {
            XmlSerializer xmls = new XmlSerializer(typeof(List<Gene>));
            Genes=(List<Gene>)xmls.Deserialize(sr);
        }
        foreach (Gene g in Genes)
        {
            g.Controller = this;
            g.UpdateBones();
        }
        return true;
    }
}
