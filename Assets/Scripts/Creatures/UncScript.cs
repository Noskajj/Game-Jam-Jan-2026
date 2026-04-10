using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

public class UncScript : MonoBehaviour
{

        public Vector3 posVec;
        public Vector3 startPos;
        public float scale = 5f;
        public float k = 1f;
        public float a = 0f;


        //PREALLOCATING MEMORY
        private Rigidbody _rb;
        private Collider _col;
        private float _totTime;
        private float _time;
        private float _kSqrt;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _col = GetComponent<Collider>();
            _rb = GetComponent<Rigidbody>();


            _rb.useGravity = false;
            _rb.mass = 1f;      //This bitch lighter than your mum

            _totTime = 0f;
            posVec = new Vector3(0 + a, 0, 0);
            startPos = transform.position;
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            float _dt = Time.fixedDeltaTime;
            _totTime += _dt;
            _kSqrt = Mathf.Sqrt(k);

            Path_Eq();

            _rb.position = new Vector3(startPos.x + posVec.x,0,(startPos.z + posVec.z));     //Updated location. Will move on next physics tick
        }


        public void Path_Eq()
        {

            posVec.x = (((posVec.x) % (_kSqrt * 2) ) - _kSqrt); //Domain eq
            Debug.Log(posVec.x);
            posVec.z = Mathf.Sqrt(k - Mathf.Pow(posVec.x, 2f));
            Debug.Log(posVec.z);
            k += 0.1f;
        }
}
