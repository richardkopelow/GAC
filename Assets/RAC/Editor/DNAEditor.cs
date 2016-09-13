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

    private List<AnimBool> animBools = new List<AnimBool>();
    private Vector2 scrollPos;
    private string filePath = "";

    [MenuItem("Window/RAC/DNA Editor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(DNAEditor));
    }
    void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        GUILayout.Label("File", EditorStyles.boldLabel);
        filePath = EditorGUILayout.TextField("File Path", filePath);
        GUILayout.BeginHorizontal();
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
        GUILayout.EndHorizontal();
        GUILayout.Label("Genes", EditorStyles.boldLabel);
        for (int i = 0; i < Genes.Count; i++)
        {
            animBools[i].target = EditorGUILayout.Foldout(animBools[i].target, Genes[i].Name);
            EditorGUI.indentLevel++;

            if (EditorGUILayout.BeginFadeGroup(animBools[i].faded))
            {
                Genes[i].Name = EditorGUILayout.TextField("Name", Genes[i].Name);
                Genes[i].Value = EditorGUILayout.FloatField("Value", Genes[i].Value);
                EditorGUILayout.PrefixLabel("Bones", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                for (int j = 0; j < Genes[i].Bones.Count; j++)
                {
                    BoneAxisPair pair = Genes[i].Bones[j];
                    GUILayout.BeginHorizontal();
                    pair.Name = EditorGUILayout.TextField("Bone Name", pair.Name);
                    if (GUILayout.Button("Remove Bone"))
                    {
                        Genes[i].Bones.RemoveAt(j);
                        j--;
                    }
                    GUILayout.EndHorizontal();
                    pair.Mask = EditorGUILayout.Vector3Field("Scale Mask", pair.Mask);
                    
                    EditorGUILayout.Space();
                }
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Add Bone"))
                {
                    Genes[i].Bones.Add(new BoneAxisPair());
                }
                if (GUILayout.Button("Remove Gene"))
                {
                    Genes.RemoveAt(i);
                    i--;
                }
                GUILayout.EndHorizontal();
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
        EditorGUILayout.EndScrollView();
    }
}
