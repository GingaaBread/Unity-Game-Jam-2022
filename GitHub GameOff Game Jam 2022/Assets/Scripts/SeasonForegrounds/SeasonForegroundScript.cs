using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonForegroundScript : MonoBehaviour
{

    public float CycleOffset;

    void OnEnable()
    {
        this.GetComponent<Animator>().SetFloat("CycleOffset", CycleOffset);
    }

}
