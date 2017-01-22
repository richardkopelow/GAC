using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDesignerHUD : MonoBehaviour
{
    public DNAController controller;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnHeightChange(float height)
    {
        controller.GetGene("Height").Value = height;
    }
	public void OnLowerBodyChange(float strength)
	{
		controller.GetGene("LowerBody").Value = strength;
	}
	public void OnUpperBodyChange(float strength)
	{
		controller.GetGene("UpperBody").Value = strength;
	}
}
