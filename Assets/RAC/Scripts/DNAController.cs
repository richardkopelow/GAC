using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

public class DNAController : MonoBehaviour {

    Transform trans;
    public BoneCollection Bones { get; set; }
    public List<Gene> Genes;
    public string Path;
    public bool Resource;

    // Use this for initialization
    void Start()
    {
        trans = transform;
        Transform armature = trans.FindChild("Armature");
        Bones = new BoneCollection(armature);

        if (Path == "")
        {
            Genes = new List<Gene>();
        }
        else
        {
            if (Resource)
            {
                LoadDNAResource(Path);
            }
            else
            {
                LoadDNAFile(Path);
            }
        }
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
    public bool LoadDNAFile(string path)
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
    public bool LoadDNAResource(string path)
    {
        TextAsset geneFile=Resources.Load<TextAsset>(path);
        if (geneFile==null)
        {
            return false;
        }
        XmlSerializer xmls = new XmlSerializer(typeof(List<Gene>));
        Genes = (List<Gene>)xmls.Deserialize(new StringReader(geneFile.text));

        foreach (Gene g in Genes)
        {
            g.Controller = this;
            g.UpdateBones();
        }
        return true;
    }
}
