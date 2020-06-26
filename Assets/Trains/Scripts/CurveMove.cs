using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveMove : MonoBehaviour
{
    [SerializeField]
    public BezierCurve curve;
    public Transform moving;
    public float t;
    // Start is called before the first frame update
    void Start()
    {
        t = 0;       
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if(t>1)
        {
            t = 0;
        }
        moving.position = curve.GetPoint(t);
    }
}
