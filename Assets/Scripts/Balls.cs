using System.Collections.Generic;
using UnityEngine;

public class Balls : MonoBehaviour
{


    public Vector3 posVec;
    public Vector3 startPos;
    public float scale = 5f;
    public float k = 1f;
    public bool coolEq = false;

    public float timeConst = 2f;

    [SerializeField] private GameObject ballPrefab;
    List<GameObject> ballsList = new List<GameObject>();


    //PREALLOCATING MEMORY
    private Rigidbody _rb;
    private Collider _col;
    private float _totTime;
    private float _time;
    private float _kSqrt;
    private Vector3 _center;

    private float _l;
    private float _n;
    private float _m;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _col = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
        Transform daddySpawner = transform.parent;
        _center = daddySpawner.position;
        _time = 0;
        _rb.useGravity = false;
        _rb.mass = 1f;      //This bitch lighter than your mum
        if (coolEq)
        {
            _n = GetComponent<Rigidbody>().position.x;
            _m = GetComponent<Rigidbody>().position.z;
        }

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 curPos = GetComponent<Rigidbody>().position;
        _time += timeConst;
        float x = curPos.x - _center.x;
        if (coolEq)
        {
            //CoolEqFunc();
            //curPos = new Vector3(_n, curPos.y, _m);
            //GetComponent<Rigidbody>().position = curPos;
        }
        else
        {
            if (Mathf.Abs(x) <= Mathf.Sqrt(_time))
            {

                GetComponent<Rigidbody>().position = new Vector3(_center.x + x, 20f , Mathf.Sqrt(_time - (x * x)));
            }
        }
    }
    
    void CoolEqFunc()
    {
        _l = k * Mathf.Sin(0.5f * Mathf.PI) * _time;
        _n = 2f * (Mathf.Cos(_time)) + _l * Mathf.Cos(_time);
        _m = 2f * Mathf.Sin(_time) + _l * Mathf.Sin(_time);
    }


}


/*
 * |(x - l)/(n^2/l)|
 * 
 */