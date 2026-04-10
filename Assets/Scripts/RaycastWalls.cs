using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWalls : MonoBehaviour
{
    public LayerMask wallLayer;
    private HashSet<CanvasGroup> currentlyBlocking = new HashSet<CanvasGroup>();
    private HashSet<CanvasGroup> lastFrameBlocking = new HashSet<CanvasGroup>();
    private Material wallMat;

    [SerializeField]
    private float transparentVal = 0.2f, fadeTime = 1f;


    private void Update()
    {
        CheckContacts();
    }

    private void CheckContacts()
    {
        lastFrameBlocking.Clear();
        foreach (var rend in currentlyBlocking)
        {
            lastFrameBlocking.Add(rend);
        }

        currentlyBlocking.Clear();

        Vector3 direction = transform.parent.position - transform.position;
        float dist = direction.magnitude;

        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction.normalized, dist, wallLayer);
        foreach (var hit in hits)
        {
            Renderer wallRenderer = hit.collider.GetComponent<Renderer>();

            if (wallRenderer != null)
            {
                CanvasGroup cg = wallRenderer.GetComponentInParent<CanvasGroup>();

                if (cg == null)
                    continue;

                currentlyBlocking.Add(cg);

                //Checks to see if it can be disabled via canvas group or renderer
                if(wallRenderer.GetComponentInParent<CanvasGroup>() != null && 
                    wallRenderer.GetComponentInParent<CanvasGroup>().alpha == 1f)
                {
                    SetTransparent(cg, transparentVal);
                }
                else if (wallRenderer.GetComponentInParent<CanvasGroup>() == null &&
                    wallRenderer.gameObject.GetComponent<Renderer>().enabled == true)
                {
                    SetTransparent(cg, transparentVal);
                }
            }
        }

        foreach (var cg in lastFrameBlocking)
        {
            if (!currentlyBlocking.Contains(cg))
            {
                SetOpaque(cg);
            }
                
        }
    }

    private void SetTransparent(CanvasGroup cg, float alpha)
    {
        if(cg != null)
        {
            foreach(var renderer in cg.GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = false;
            }
           
        }
    }

    private void SetOpaque(CanvasGroup cg)
    {
        
        if(cg != null)
        {
            foreach (var renderer in cg.GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = true;
            }
        }

    }

    //Not implemented yet
    private IEnumerator FadeWall(CanvasGroup cg, float startVal, float targetVal, int fadeTime)
    {
        float timer = 0;

        while(timer < fadeTime)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startVal, targetVal, timer / fadeTime);
            cg.alpha = alpha;
            yield return null;
        }

        cg.alpha = targetVal;
    }
}
