using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{


    public Vector3 posVec;
    public Vector3 startPos;
    public float projCount = 30f;
    public float k = 1f;
    public float timeConst = 0.2f;
    public bool coolEq = false;
    public float iHateBen = 100f;
    [SerializeField] private GameObject ballPrefab;
    List<GameObject> ballsList = new List<GameObject>();


    //PREALLOCATING MEMORY
    private Rigidbody _rb;
    private Collider _col;
    private float _totTime;
    private float _time;
    private float _kSqrt;
    private float _offesetX;

    private float _l;
    private float _n;
    private float _m;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
        _col = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();

        _time = 0;
        _rb.useGravity = false;
        _rb.mass = 1f;      //This bitch lighter than your mum

        if (coolEq)
        {
            for(int i = 0; i< 50; i++)
            {

                CoolEqFunc();

                GameObject newBall = Instantiate(ballPrefab, new Vector3(_n, 0, _m), ballPrefab.transform.rotation);
                _time += 0.5f;
            }


        }
        else
        {
            


            for (int i = 0; i < projCount; i++)
            {
                _offesetX = (-iHateBen) / (2f) + (iHateBen / (projCount - 1)) * i;
                Vector3 spawnPos = startPos + new Vector3(_offesetX, 0f, 0f);
                //(-1 * l/2f) + (Mathf.Pow(i,2)/l)
                GameObject newBall = Instantiate(ballPrefab, new Vector3(_offesetX, 10f, startPos.y), ballPrefab.transform.rotation);
            
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

    // Update is called once per frame





/*
             Vector3 curPos = ballsList[i].GetComponent<Rigidbody>().position;
            curPos.z = Mathf.Sqrt(_time - Mathf.Pow(curPos.x, 2));
 */