using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWalls : MonoBehaviour
{
    public LayerMask wallLayer;
    private HashSet<Renderer> currentlyBlocking = new HashSet<Renderer>();
    private HashSet<Renderer> lastFrameBlocking = new HashSet<Renderer>();
    private Material wallMat;

    private void Update()
    {
        lastFrameBlocking.Clear();
         foreach(var rend in currentlyBlocking)
        {
           lastFrameBlocking.Add(rend);
        }

         currentlyBlocking.Clear();

        Vector3 direction = transform.parent.position - transform.position;
        float dist = direction.magnitude;

        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction.normalized, dist, wallLayer);
        foreach(var hit in hits)
        {
            Renderer wallRenderer = hit.collider.GetComponent<Renderer>();
            if (wallRenderer != null)
            {
                currentlyBlocking.Add(wallRenderer);

                wallRenderer.enabled = false;
            }
        }       

       foreach(var rend in lastFrameBlocking)
        {
            if (!currentlyBlocking.Contains(rend))
                rend.enabled = true;
        }
    }

    private void SetTransparent(Renderer rend, float alpha)
    {
        Debug.Log("transparent attemp");
        Material mat = rend.material;
        mat.SetFloat("_Surface", 1);
        Color color = mat.color;
        color.a = alpha;
        mat.color = color;
    }

    private void SetOpaque(Renderer rend, float alpha)
    {
        Material mat = rend.material;
        mat.SetFloat("_Surface", 0);
        Color color = mat.color;
        color.a = alpha;
        mat.color = color;
    }



    //Not implemented yet
    private IEnumerator FadeWall(Renderer wallRenderer, int targetVal, int fadeTime)
    {
        Material mat = wallRenderer.material;
        Color color = mat.color;
        float startAlpha = color.a;
        float timer = 0;

        while(timer < fadeTime)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetVal, timer / fadeTime);
            mat.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        mat.color = new Color(color.r, color.g, color.b, targetVal);
    }
}
