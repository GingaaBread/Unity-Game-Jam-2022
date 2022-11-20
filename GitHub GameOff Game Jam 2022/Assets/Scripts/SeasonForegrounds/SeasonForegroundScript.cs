using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonForegroundScript : MonoBehaviour
{
    [Header("animation Offset")]
    public float CycleOffset;
    public bool RandomizeOffset;

    [Header("Parent position")]
    public float xPosOffsetMinBound;
    public float xPosOffsetMaxBound;
    public float yPosOffsetMinBound;
    public float yPosOffsetMaxBound;


    void OnEnable() {

        Random.InitState(gameObject.GetInstanceID());

        if (RandomizeOffset) {
            this.GetComponent<Animator>().SetFloat("CycleOffset", Random.Range(0f,1f));
        } else {
            this.GetComponent<Animator>().SetFloat("CycleOffset", CycleOffset);
        }

        float parentXPos = Random.Range(xPosOffsetMinBound, xPosOffsetMaxBound);
        float parentYPos = Random.Range(yPosOffsetMinBound, yPosOffsetMaxBound);
        transform.parent.localPosition = new Vector3(parentXPos, parentYPos, transform.parent.localPosition.z);

    }

}
