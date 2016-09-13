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
	
	void LateUpdate ()
	{
        Bones.Update();
	}
    /// <summary>
    /// Saves the DNA for this character to disk.
    /// </summary>
    /// <param name="path">The file path for the DNA file</param>
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
    /// <summary>
    /// Loads a DNA file from disk
    /// </summary>
    /// <param name="path">The path to the file to be loaded</param>
    /// <returns></returns>
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
    /// <summary>
    /// Loads a DNA file from the Resources folder
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
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
    /// <summary>
    /// Gets a gene by name
    /// </summary>
    /// <param name="name">the name of the gene</param>
    /// <returns>returns the gene if found, null if not</returns>
    public Gene GetGene(string name)
    {
        for (int i = 0; i < Genes.Count; i++)
        {
            if (Genes[i].Name==name)
            {
                return Genes[i];
            }
        }
        return null;
    }
}
