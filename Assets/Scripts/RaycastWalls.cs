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
                currentlyBlocking.Add(wallRenderer);

                //Checks to see if it can be disabled via canvas group or renderer
                if(wallRenderer.GetComponentInParent<CanvasGroup>() != null && 
                    wallRenderer.GetComponentInParent<CanvasGroup>().alpha == 1f)
                {
                    SetTransparent(wallRenderer, transparentVal);
                }
                else if (wallRenderer.GetComponentInParent<CanvasGroup>() == null &&
                    wallRenderer.gameObject.GetComponent<Renderer>().enabled == true)
                {
                    SetTransparent(wallRenderer, transparentVal);
                }
            }
        }

        foreach (var rend in lastFrameBlocking)
        {
            if (!currentlyBlocking.Contains(rend))
            {
                SetOpaque(rend);
            }
                
        }
    }

    private void SetTransparent(Renderer rend, float alpha)
    {
        Debug.Log("transparent attemp");
        CanvasGroup cg = rend.GetComponentInParent<CanvasGroup>();
        if(cg != null)
        {
            cg.alpha = alpha;
           
        }
        else
        {
            Debug.LogWarning($"The {rend.name} object does not have a valid canvas group on its parent: Disabling Default");
            rend.gameObject.GetComponent<Renderer>().enabled = false;
        }
    }

    private void SetOpaque(Renderer rend)
    {
        CanvasGroup cg = rend.GetComponentInParent<CanvasGroup>();
        
        if(cg != null)
        {
            cg.alpha = 1.0f;
        }
        else
        {
            Debug.LogWarning($"The {rend.name} object does not have a valid canvas group on its parent: Enabling Default");
            rend.gameObject.GetComponent<Renderer>().enabled = true;
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
