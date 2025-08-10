using UnityEngine;

public class PressurePlateController : MonoBehaviour
{
    public GameObject cover;  // cover the full health recovery heart
    private int objectsOnPlate = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MovableBox"))
        {
            objectsOnPlate++;
            UpdateCover();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MovableBox"))
        {
            objectsOnPlate--;
            UpdateCover();
        }
    }

    void UpdateCover()
    {
        if (objectsOnPlate > 0)
        {
            if (cover != null && cover.activeSelf)
            {
                cover.SetActive(false);  // cover disppear
                Debug.Log("Cover hidden, heart is accessible");
            }
        }
        else
        {
            if (cover != null && !cover.activeSelf)
            {
                cover.SetActive(true);   // cover the heart
                Debug.Log("Cover shown, heart is protected");
            }
        }
    }
}
