using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : Singleton<LoadingManager>
{
    public GameObject loadingScreenPrefab;
    public float delayLoadComplete;

    public Action OnLoadingComplete;
    public Action OnUnloadingComplete;
    private string currentScene;
    private GameObject LSClone;

    public Scene LoadedScene { get; internal set; }

    //for OnClick Listeners
    public void LoadSceneAsync(int index)
    {
        Instance.StartCoroutine(LoadByNameAsyncRoutine(IndexToSceneName(index)));
    }

    public void LoadSceneAsyncAdditive(int index)
    {
        Instance.StartCoroutine(LoadByNameAdditiveAsyncRoutine(IndexToSceneName(index)));
    }

    private string IndexToSceneName(int index)
    {
        string pathToScene = SceneUtility.GetScenePathByBuildIndex(index);
        return System.IO.Path.GetFileNameWithoutExtension(pathToScene);
    }
   
    public void LoadByNameAsync(string scene)
    {
        Instance.StartCoroutine(LoadByNameAsyncRoutine(scene));
    }

    private IEnumerator LoadByNameAsyncRoutine(string scene)
    {
        Instance.SpawnLoadingScreen();
        var handle = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        yield return new WaitWhile(() => handle.isDone);
        Debug.Log("Loading done");
        currentScene = scene;
        SceneManager.sceneLoaded += FireOnLoadingComplete;
    }

    private void LoadByNameAdditiveAsync(string scene)
    {
        Instance.StartCoroutine(LoadByNameAdditiveAsyncRoutine(scene));
    }

    private IEnumerator LoadByNameAdditiveAsyncRoutine(string scene)
    {
        Instance.SpawnLoadingScreen();
        var handle = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        yield return new WaitWhile(() => handle.isDone);
        Debug.Log("Loading done");
        currentScene = scene;
        SceneManager.sceneLoaded += FireOnLoadingComplete;
    }

    private void FireOnLoadingComplete(Scene scene, LoadSceneMode loadingMode)
    {
        SceneManager.sceneLoaded -= FireOnLoadingComplete;
        SceneManager.SetActiveScene(scene);
        Instance.StartCoroutine(DelayOnLoadComplete());
    }

    private IEnumerator DelayOnLoadComplete()
    {
        while(delayLoadComplete > 0)
        {
            delayLoadComplete -= Time.deltaTime;
            yield return null;
        }
        Instance.DestoryLoadingScreen();
        Instance.OnLoadingComplete?.Invoke();
    }

    private void SpawnLoadingScreen()
    {
        if (!LSClone && loadingScreenPrefab)
            LSClone = Instantiate(loadingScreenPrefab);
    }

    private void DestoryLoadingScreen()
    {
        if (LSClone)
            Destroy(LSClone);
    }

    internal void MoveGameObjectToCurrentScene(GameObject GO)
    {
        Scene scene = SceneManager.GetSceneByName(currentScene);
        if (scene.isLoaded)
            SceneManager.MoveGameObjectToScene(GO, scene);
        else
        {
            Debug.Log("Trying to move object to scene that is being loaded or being onloaded");
            Instance.StartCoroutine(MoveGameObjectToCurrentScene(GO, scene));
        }
    }

    private IEnumerator MoveGameObjectToCurrentScene(GameObject GO, Scene scene)
    {
        while(!scene.isLoaded)
        {
            yield return null;
        }
        SceneManager.MoveGameObjectToScene(GO, scene);
    }
}
