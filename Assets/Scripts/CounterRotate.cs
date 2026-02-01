using TMPro;
using UnityEngine;

public class CounterRotate : MonoBehaviour
{
    private Transform parent;

    private void Start()
    {
        parent = transform.parent;
        transform.SetParent(null);
        transform.rotation = Quaternion.Euler(15f, 0, 0);
        transform.SetParent(parent.transform);

        transform.localPosition = new Vector3(0, 0, -5f);
    }
}
