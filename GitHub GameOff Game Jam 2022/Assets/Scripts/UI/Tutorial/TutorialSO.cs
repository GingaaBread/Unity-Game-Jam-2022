using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Dialogue")]
public class TutorialSO : ScriptableObject{

    public Sprite[] speakers;
    [TextArea(6,6)]
    public string[] lines;
    public Sprite[] screenshots;
    
}
