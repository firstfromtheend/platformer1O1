using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float timeToWait = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(WaitAndLoad());
        }
    }


    private IEnumerator WaitAndLoad()
    {
        yield return new WaitForSeconds(timeToWait);
        Debug.Log("asdad");
        //SceneManager.LoadScene();
    }
}
