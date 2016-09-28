using UnityEngine;
using System.Collections;

public class TestGUI : MonoBehaviour
{
    public ComponentLoader MaleLoader;
    public GameObject Armor;
    public GameObject MaleShirt;

    bool armor=true;

    public void OnToggleMaleArmor()
    {
        if (armor)
        {
            MaleLoader.Remove(MaleShirt);
            MaleLoader.Add(Armor);
        }
        else
        {
            MaleLoader.Remove(Armor);
            MaleLoader.Add(MaleShirt);
        }
        armor = !armor;
    }
}
