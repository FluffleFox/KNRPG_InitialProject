using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexWave : MonoBehaviour
{

    void Update()
    {
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    LeanTween.moveY(transform.GetChild(i).gameObject, Random.Range(0, 2), 1);
        //}
        foreach (Transform t in transform)
        {
            t.position = new Vector3(t.position.x, Mathf.Abs(Mathf.Sin(t.position.x + Time.realtimeSinceStartup)), t.position.z);

        }
    }
}
