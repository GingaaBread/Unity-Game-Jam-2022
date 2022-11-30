using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Dialogue")]
public class TutorialSO : ScriptableObject{

    public Sprite[] speakers;
    [TextArea(6,6)]
    //added because Unity 2021 LTS problem with showing index 0 TextArea arrays :/
    [NonReorderable]
    public string[] lines;
    public Sprite[] screenshots;
    
}
