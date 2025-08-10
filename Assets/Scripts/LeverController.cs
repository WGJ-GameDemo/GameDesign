using UnityEngine;
using UnityEngine.Events;

public class LeverController : MonoBehaviour
{
    public bool isPulled = false;

    public UnityEvent onLeverPulled;
    public UnityEvent onLeverReset;


    public void PullLever()
    {
        if (isPulled) return;

        isPulled = true;
        Debug.Log("Lever pulled");

        if (onLeverPulled != null)
            onLeverPulled.Invoke();
    }

    public void ResetLever()
    {
        if (!isPulled) return;

        isPulled = false;
        Debug.Log("Lever reset");

        if (onLeverReset != null)
            onLeverReset.Invoke();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            if (!isPulled)
                PullLever();
            else
                ResetLever();
        }
    }
}

