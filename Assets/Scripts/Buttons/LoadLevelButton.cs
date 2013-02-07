using UnityEngine;

public class LoadLevelButton : Button
{
    public int levelNumber = 0;
    public string levelName = null;
    public LoadLevelTypes loadLevelType;
    public int activeIfPassedLevel = 0;

    protected override void Awake()
    {
        if (MemoryManager.Instance.GetParameter(GameParameters.LastPassedLevel) < activeIfPassedLevel)
            this.GetComponent<UIButton>().isEnabled = false;
    }

    protected override void Click()
    {
        if (levelNumber != 0)
            LoadingManager.Instance.Load(levelNumber);
        else if (string.IsNullOrEmpty(levelName))
            LoadingManager.Instance.Load(loadLevelType);
        else
            LoadingManager.Instance.Load(levelName);

        MemoryManager.Instance.Save();
    }
}
