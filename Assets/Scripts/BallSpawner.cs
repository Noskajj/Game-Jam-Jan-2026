using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public enum Shape_Function 
{ 

    Fourier, 
    SemiCircle

}

public class BallSpawner : MonoBehaviour
{


    public Vector3 posVec;
    public Vector3 startPos;
    public float projCount = 5f;
    private float k = 0f;
    public float timeConst = 0.2f;
    public Shape_Function ShapeFunction = Shape_Function.Fourier;
    public float iHateBen = 100f;
    [SerializeField] private GameObject ballPrefab;
    


    //PREALLOCATING MEMORY
    private Rigidbody _rb;
    private Collider _col;
    private float _totTime;
    private float _time;
    private float _kSqrt;
    private float _offesetX;

    private List<GameObject> spawnedBalls = new List<GameObject>();

    private float Fourier_Length;
    private float Fourier_x;
    private float Fourier_y;
    private float tmax = 20;
    private float t = 0;
    private float tstep = 0.03f;
    private float tpause = 0.05f;
    private float ttransition = 0.05f;
    private float kmax = 100f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        /*startPos = transform.position;
        _col = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
        Debug.Log("started");
        _time = 0;
        _rb.useGravity = false;
        _rb.mass = 1f; */     //This bitch lighter than your mum

        if (ShapeFunction == Shape_Function.Fourier)
        {

            StartCoroutine(AttFourier());
            //IAttFourier();
      
        }

        else
        
        {
            


            for (int i = 0; i < projCount; i++)
            {
                _offesetX = (-iHateBen) / (2f) + (iHateBen / (projCount - 1)) * i;
                Vector3 spawnPos = startPos + new Vector3(_offesetX, 0f, 0f);
   
                GameObject newBall = Instantiate(ballPrefab, new Vector3(_offesetX, 10f, startPos.y), ballPrefab.transform.rotation);
            
            }
        }
      

}



    IEnumerator AttFourier()
    {
        for (float i = 0; i < tmax; i += tstep)
        {
            GameObject newBall = Instantiate(ballPrefab);
            spawnedBalls.Add(newBall);
        }

        while (k < kmax)
        {
            int ballIndex = 0;

            while (t < tmax)
            {
                Fourier_Length = k * Mathf.Sin(t * 0.5f * Mathf.PI);
                Fourier_x = 2f * (Mathf.Cos(t)) + Fourier_Length * Mathf.Cos(t);
                Fourier_y = 2f * Mathf.Sin(t) + Fourier_Length * Mathf.Sin(t);
                spawnedBalls[ballIndex].transform.position = new Vector3(Fourier_x, 1, Fourier_y);

                t = t + tstep;
                ballIndex++;
            }

            k = k + 0.1f;
            t = 0;
            yield return new WaitForSeconds(ttransition);
        }
    }

    void IAttFourier()
    {
        
        while (t < tmax)
        {

            Fourier_Length = k * Mathf.Sin(t * 10f * Mathf.PI);
            Fourier_x = 2f * (Mathf.Cos(t)) + Fourier_Length * Mathf.Cos(t);
            Fourier_y = 2f * Mathf.Sin(t) + Fourier_Length * Mathf.Sin(t);
            GameObject newBall = Instantiate(ballPrefab, new Vector3(Fourier_x, 0, Fourier_y), ballPrefab.transform.rotation);
            spawnedBalls.Add(newBall);

            t = t + tstep;



        }

    }

    /*IEnumerator AttHalfCircle()
    {
        
    }*/
}