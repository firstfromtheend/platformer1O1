using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] float timeToWaitBeforeLoad = 1f;

    public void LoadScene(int sceneIndexInBuild)
    {
        StartCoroutine(LoadSceneByIndex(sceneIndexInBuild));
    }

    IEnumerator LoadSceneByIndex(int sceneIndexInBuild)
    {
        yield return new WaitForSeconds(timeToWaitBeforeLoad);
        SceneManager.LoadScene(sceneIndexInBuild);
    }
}
