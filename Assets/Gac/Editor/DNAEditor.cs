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

    private List<AnimBool> geneBools = new List<AnimBool>();
    private List<AnimBool> bonesBools = new List<AnimBool>();
    private Vector2 scrollPos;
    private string filePath = "";

    [MenuItem("Window/GAC/DNA Editor")]
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
        if (GUILayout.Button("New"))
        {
            Genes = new List<Gene>();
            geneBools = new List<AnimBool>();
            bonesBools = new List<AnimBool>();
            scrollPos = Vector2.zero;
        }
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
                geneBools = new List<AnimBool>(Genes.Count);
                bonesBools = new List<AnimBool>(Genes.Count);
                for (int i = 0; i < Genes.Count; i++)
                {
                    geneBools.Add(new AnimBool(false));
                    bonesBools.Add(new AnimBool(false));
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
            EditorGUILayout.BeginHorizontal();
            geneBools[i].target = EditorGUILayout.Foldout(geneBools[i].target, Genes[i].Name);
            if (GUILayout.Button("Remove Gene"))
            {
                Genes.RemoveAt(i);
                geneBools.RemoveAt(i);
                bonesBools.RemoveAt(i);
                i--;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel++;

            if (EditorGUILayout.BeginFadeGroup(geneBools[i].faded))
            {
                Genes[i].Name = EditorGUILayout.TextField("Name", Genes[i].Name);
                Genes[i].Value = EditorGUILayout.FloatField("Value", Genes[i].Value);
                bonesBools[i].target = EditorGUILayout.Foldout(bonesBools[i].target, "Bones");
                EditorGUI.indentLevel++;
                if (EditorGUILayout.BeginFadeGroup(bonesBools[i].faded))
                {
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
                    if (GUILayout.Button("Add Bone"))
                    {
                        Genes[i].Bones.Add(new BoneAxisPair());
                    }
                }
                EditorGUILayout.EndFadeGroup();
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndFadeGroup();
            
        }

        if (GUILayout.Button("Add Gene"))
        {
            Genes.Add(new Gene("(New Gene)"));
            AnimBool geneBool = new AnimBool(true);
            geneBools.Add(geneBool);
            geneBool.valueChanged.AddListener(Repaint);
            AnimBool boneBool = new AnimBool(false);
            bonesBools.Add(boneBool);
            boneBool.valueChanged.AddListener(Repaint);
        }
        EditorGUILayout.EndScrollView();
    }
}
