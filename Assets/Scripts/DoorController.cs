using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    public bool isLocked = true;
    public string nextSceneName;

    private PlayerInventory playerInventory;

    void Start()
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            if (isLocked)
            {
                if (playerInventory != null && playerInventory.UseKey()) // 直接用 UseKey 消耗钥匙
                {
                    isLocked = false;
                    Debug.Log("Door unlocked!");
                    LoadNextScene();
                }
                else
                {
                    Debug.Log("Door is locked, you need a key.");
                }
            }
            else
            {
                LoadNextScene();
            }
        }
    }

    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
