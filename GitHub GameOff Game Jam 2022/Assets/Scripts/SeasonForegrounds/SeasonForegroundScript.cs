using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonForegroundScript : MonoBehaviour
{

    public int RandomSeed;

    [Header("animation Offset")]
    public float CycleOffset;
    public bool RandomizeOffset;

    [Header("Parent position")]
    public float xPosOffsetMinBound;
    public float xPosOffsetMaxBound;
    public float yPosOffsetMinBound;
    public float yPosOffsetMaxBound;


    void OnEnable() {

        Random.InitState(RandomSeed);

        foreach (Transform positionOffsetObj in transform) {
            // set local position of posittion offset object
            positionOffsetObj.localPosition = new Vector3(
                Random.Range(xPosOffsetMinBound, xPosOffsetMaxBound),
                Random.Range(yPosOffsetMinBound, yPosOffsetMaxBound),
                positionOffsetObj.localPosition.z);

            // set animation offsets
            if (RandomizeOffset) {
                positionOffsetObj.GetComponentInChildren<Animator>().SetFloat("CycleOffset", Random.Range(0f, 1f));
            } else {
                positionOffsetObj.GetComponentInChildren<Animator>().SetFloat("CycleOffset", CycleOffset);
            }
        }

    }

}
