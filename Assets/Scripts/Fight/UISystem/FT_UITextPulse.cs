using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FT_UITextPulse : MonoBehaviour
{

    // Grow parameters
    public float approachSpeed = 0.001f;
    public float growthBound = 1.0f;
    public float shrinkBound = 0.5f;
    public float currentRatio = 1;


    // The text object we're trying to manipulate
    private Image image;

    // And something to do the manipulating
    private Coroutine routine;
    private bool keepGoing = true;
    private bool activePulse = true;


    void OnEnable()
    {
        this.image = this.gameObject.GetComponent<Image>();

        this.routine = StartCoroutine(this.Pulse());
    }

    void OnDisable()
    {
        
    }

    IEnumerator Pulse()
    {
        // Run this indefinitely
        while (keepGoing)
        {
            // Get bigger for a few seconds
            while (this.currentRatio != this.growthBound)
            {
                // Determine the new ratio to use
                currentRatio = Mathf.MoveTowards(currentRatio, growthBound, approachSpeed);

                // Update our text element
                this.image.transform.localScale = Vector3.one * currentRatio;

                yield return new WaitForEndOfFrame();
            }

            // Shrink for a few seconds
            while (this.currentRatio != this.shrinkBound)
            {
                // Determine the new ratio to use
                currentRatio = Mathf.MoveTowards(currentRatio, shrinkBound, approachSpeed);

                // Update our text element
                this.image.transform.localScale = Vector3.one * currentRatio;

                yield return new WaitForEndOfFrame();
            }
        }
    }
}