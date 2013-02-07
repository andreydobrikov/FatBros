/*Описание.
 *Скрипт подгружает уровни и показывает 
 *окно загрузки. Поместите прифабы с окнами
 *загрузки в папку "Resources/Prefabs/Loading"
 *и назовите из LoadingBack и LoadingBackAndroid.
 *Если вы используете плагину NGUI то нужно назначить
 *переменную NGUI_LAYER с номером слоя который использует
 *NGUI камера.
 */

using UnityEngine;
using System.Collections;

public enum LoadLevelTypes
{
    Shop,
    MainMenu,
    Restart,
    SelectLevel,
    Next,
    none
}

public class LoadingManager
{
    #region Variables

    private static LoadingManager instance = null;
    private GameObject loadingPrefab = null, loadingObject = null;
    private int? NGUI_LAYER = (int)8;
    private Camera NGUICamera = null;
    private AsyncOperation currentAsyncOperation = null;
    private bool loadingIsOver = true;

    #endregion

    #region Properties

    public static LoadingManager Instance
    {
        get 
        {
            if (instance == null)
                instance = new LoadingManager();
            return instance;
        }
    }

    public AsyncOperation CurrentAsyncOperation
    {
        get { return currentAsyncOperation; }
        set { currentAsyncOperation = value; }
    }

    public bool LoadingIsOver
    {
        get { return loadingIsOver; }
        private set { loadingIsOver = value; }
    }

    #endregion

    #region Functions

    public LoadingManager()
    {
        loadingPrefab = (GameObject)Resources.Load("Prefabs/Loading/LoadingBack");
        NGUICamera = FindCamera(NGUI_LAYER);
    }

    public void Load(LoadLevelTypes levelType)
    {
        switch (levelType)
        {
            case LoadLevelTypes.Restart:
                //Load(Application.loadedLevel);
                break;
            case LoadLevelTypes.Next:
                Load(Application.loadedLevel + 1);
                break;
            default:
                Load(levelType.ToString());
                break;
        }
    }

    public void Load(string levelName)
    {
        if (Application.CanStreamedLevelBeLoaded(levelName))
        {
            if (NGUICamera != null)
                NGUICamera.enabled = false;
            GameObject go = new GameObject();
            GameObject.DontDestroyOnLoad(go);
            go.AddComponent<MonoBehaviour>().StartCoroutine(LoadingProgress(go, levelName, null));
        }
        else
            Debug.LogWarning("Level " + levelName + " can not be loaded!");
    }

    public void Load(int levelNumber)
    {
        if (Application.CanStreamedLevelBeLoaded(levelNumber))
        {
            if (NGUICamera != null)
                NGUICamera.enabled = false;
            GameObject go = new GameObject();
            GameObject.DontDestroyOnLoad(go);
            go.AddComponent<MonoBehaviour>().StartCoroutine(LoadingProgress(go, null, (int)levelNumber));
        }
        else
            Debug.LogWarning("Level with number " + levelNumber + " can not be loaded!");
    }

    private IEnumerator LoadingProgress(GameObject go, string levelName, int? levelNumber)
    {
        AsyncOperation async = null;
       
        BlinkScreen(true);
        while (loadingObject.animation.isPlaying)
            yield return null;

        if (!string.IsNullOrEmpty(levelName))
            async = Application.LoadLevelAsync(levelName);
        else if (levelNumber != null)
            async = Application.LoadLevelAsync((int)levelNumber);
        else
        {
            Debug.LogError("Invalid arguments in LoadingProgress functions!!!");
            Debug.Break();
        }
        CurrentAsyncOperation = async;
        while (!async.isDone)
            yield return null;

        BlinkScreen(false);
        BlinkScreen(true);

        while (loadingObject.animation.isPlaying)
            yield return null;

        BlinkScreen(false);
        NGUICamera = FindCamera(NGUI_LAYER);
        GameObject.Destroy(go);
    }

    private void BlinkScreen(bool activation)
    {
        if (loadingObject == null)
        {
            if (loadingPrefab == null)
            {
                Debug.LogWarning("'RestartBack' isn't found! Create folder 'Resources/Prefabs/Loading' and set prefab name 'RestartBack'.");
                return;
            }
            else
            {
                loadingObject = (GameObject)GameObject.Instantiate(loadingPrefab);
                GameObject.DontDestroyOnLoad(loadingObject);
            }
        }
        else
            loadingObject.SetActive(activation);
    }

    private Camera FindCamera(int? layer)
    {
        if (NGUI_LAYER != null)
            return NGUITools.FindCameraForLayer((int)NGUI_LAYER);
        else
            return null;
    }

    #endregion
}
