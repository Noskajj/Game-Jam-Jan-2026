using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWalls : MonoBehaviour
{
    public LayerMask wallLayer;
    private HashSet<Renderer> disabledRenders = new HashSet<Renderer>();

    private void Update()
    {
         foreach(var rend in disabledRenders)
        {
            if(rend != null)
                rend.enabled = true;
        }
         disabledRenders.Clear();

        Vector3 direction = transform.parent.position - transform.position;
        float dist = direction.magnitude;

        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction.normalized, dist, wallLayer);
       foreach(var hit in hits)
        {
            Renderer wallRenderer = hit.collider.GetComponent<Renderer>();
            if (wallRenderer != null)
            {
                wallRenderer.enabled = false;
                disabledRenders.Add(wallRenderer);
            }
        }       
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
