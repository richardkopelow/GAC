using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEditor.AnimatedValues;

public class DNAEditor : EditorWindow
{
    public List<Gene> Genes = new List<Gene>();

    private List<AnimBool> animBools;

    private string filePath="";

    [MenuItem("Window/RAC/DNA Editor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(DNAEditor));
    }
    void OnEnable()
    {
    }
    void OnGUI()
    {
        GUILayout.Label("File", EditorStyles.boldLabel);
        filePath = EditorGUILayout.TextField("File Path", filePath);
        if (GUILayout.Button("Load"))
        {
            FileInfo fi = new FileInfo(filePath);
            if (fi.Exists)
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    XmlSerializer xmls = new XmlSerializer(typeof(List<Gene>));
                    Genes = (List<Gene>)xmls.Deserialize(sr);
                }
            }
        }

        if (GUILayout.Button("Save"))
        {
            FileInfo fi = new FileInfo(filePath);
            if (!fi.Exists)
            {
                fi.Create().Close();
            }
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                XmlSerializer xmls = new XmlSerializer(typeof(List<Gene>));
                xmls.Serialize(sw, Genes);
            }
        }

        for (int i = 0; i < Genes.Count; i++)
        {
            animBools[i].target = EditorGUILayout.Foldout(animBools[i].target, "Show extra fields");
            EditorGUI.indentLevel++;

            if (EditorGUILayout.BeginFadeGroup(animBools[i].faded))
            {
                Genes[i].Name = EditorGUILayout.TextField("Name",Genes[i].Name);
            }
            EditorGUILayout.EndFadeGroup();
            EditorGUI.indentLevel--;
        }

        if (GUILayout.Button("Add Gene"))
        {
            Genes.Add(new Gene());
            animBools.Add(new AnimBool(true));
            animBools[animBools.Count - 1].valueChanged.AddListener(Repaint);
        }
    }
}
