using UnityEngine;
using System;
using System.Collections;

public class LevelGUIController : MonoBehaviour
{
    #region Variables

    public Action ShowWinLoseMenu;

    private static LevelGUIController instance;

    private UILabel gameBulletsLabel, gameScoreLabel;
    private Transform winLoseMenu, gameMenu;
    private Transform[] winMenuStars;
    private UIButton[] winMenuButtons;

    private int score = 0, bullets = 0, countOfKilledEnemies = 0, maxCountOfEnemies = 0;

    private enum GameOverTypes
    {
        Lose = 0,
        Normal = 1,
        Good = 2,
        Great = 3
    }

    #endregion

    #region Properties

    public static LevelGUIController Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("\'LevelGUIController\' instance isn't assigned");
            return instance;
        }
        private set { instance = value; }
    }

    public int ScoresCount
    {
        get { return score; }
        set
        {
            if (gameScoreLabel == null)
                gameScoreLabel = FindGUIElement<UILabel>("GameScoreCountLabel");
            score = value;
            gameScoreLabel.text = score.ToString();
        }
    }

    public int BulletsCount
    {
        get { return bullets; }
        set
        {
            if (gameBulletsLabel == null)
                gameBulletsLabel = FindGUIElement<UILabel>("BulletsCountLabel");
            bullets = value;
            gameBulletsLabel.text = bullets.ToString();
        }
    }

    public int MaxCountOfEnemies
    {
        get { return maxCountOfEnemies; }
        set { maxCountOfEnemies = value; }
    }

    public int CountOfKilledEnemies
    {
        get { return countOfKilledEnemies; }
        set { countOfKilledEnemies = value; }
    }

    #endregion

    #region Unity functions

    private void Awake()
    {
        Instance = this;
        WinLoseMenuActivation(false);
        GameMenuActivation(false);
        ShowWinLoseMenu = () => { StartCoroutine(WinLoseMenuActivation()); };
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    #endregion

    #region GUIController functions

    private T FindGUIElement<T>(string elementName) where T : Component
    {
        T[] elementsArray = this.GetComponentsInChildren<T>();
        for (int i = 0; i < elementsArray.Length; i++)
        {
            if (elementsArray[i].name.Contains(elementName))
                return elementsArray[i];
        }

        Debug.LogWarning("GUI Element " + elementName + " isn't found!");
        return null;
    }

    public void GameMenuActivation(bool activation)
    {
        if (gameMenu == null)
            gameMenu = FindGUIElement<Transform>("GameMenu");

        if (gameMenu == null)
        {
            Debug.LogError("GameObject 'GameMenu' isn't found!");
            return;
        }

        gameMenu.gameObject.SetActive(activation);
    }

    private IEnumerator WinLoseMenuActivation()
    {
        GameOverTypes currentType = GameOverTypes.Lose;
        if (CountOfKilledEnemies >= ((float)MaxCountOfEnemies * 0.9f))
            currentType = GameOverTypes.Great;
        else if ((CountOfKilledEnemies >= ((float)MaxCountOfEnemies * 0.7f)) && (CountOfKilledEnemies < ((float)MaxCountOfEnemies * 0.9f)))
            currentType = GameOverTypes.Good;
        else if ((CountOfKilledEnemies >= ((float)MaxCountOfEnemies * 0.5f)) && (CountOfKilledEnemies < ((float)MaxCountOfEnemies * 0.7f)))
            currentType = GameOverTypes.Normal;
        string winTitle = currentType.ToString();

        WinLoseMenuActivation(true);
        WinMenuButtonsActivation(false);

        UILabel congradulationLabel = FindGUIElement<UILabel>("CongradulationLabel");
        UILabel finalScoreLabel = FindGUIElement<UILabel>("FinalScoreLabel");
        if (congradulationLabel == null || finalScoreLabel == null)
        {
            Debug.LogError("WinLoseMenu labels aren't found!");
            yield return null;
        }
        else
        {
            congradulationLabel.text = string.Empty;
            StartCoroutine(ShowStars((int)currentType));

            float delay = 0.001f;
            int workScoresCount = 0;
            while (ScoresCount > 0)
            {
                workScoresCount++;
                ScoresCount--;
                if (Input.GetMouseButtonDown(0))
                {
                    workScoresCount += ScoresCount;
                    ScoresCount = 0;
                }
                finalScoreLabel.text = workScoresCount.ToString();
                yield return new WaitForSeconds(delay);
            }

            foreach (Char c in winTitle)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    congradulationLabel.text = winTitle;
                    break;
                }
                congradulationLabel.text += c.ToString();
                NGUITools.PlaySound(AudioManager.Instance.GetClip("WriteLetter"));
                yield return new WaitForSeconds(0.03f);
            }

            WinMenuButtonsActivation(currentType);
            SaveData(currentType);
        }
    }

    private void WinLoseMenuActivation(bool activation)
    {
        if (winLoseMenu == null)
            winLoseMenu = FindGUIElement<Transform>("WinLoseMenu");
        
        if (winLoseMenu == null)
        {
            Debug.LogError("GameObject 'WinLoseMenu' isn't found!");
            return;
        }

        if (winMenuStars == null)
        {
            winMenuStars = new Transform[3];
            for (int i = 0; i < winMenuStars.Length; i++)
                winMenuStars[i] = winLoseMenu.FindChild("Star_" + (i + 1).ToString());
        }

        if (winMenuButtons == null)
            winMenuButtons = winLoseMenu.GetComponentsInChildren<UIButton>();

        winLoseMenu.gameObject.SetActive(activation);
    }

    private IEnumerator ShowStars(int howMany)
    {
        if (winMenuStars == null)
        {
            Debug.LogError("Stars array isn't assigned!");
            yield return null;
        }
        else
        {
            for (int i = 0; i < winMenuStars.Length; i++)
                winMenuStars[i].gameObject.SetActive(false);

            for (int i = 0; i < howMany; i++)
            {
                winMenuStars[i].gameObject.SetActive(true);
                NGUITools.PlaySound(AudioManager.Instance.GetClip("Star"));
                yield return new WaitForSeconds(0.3f);
            }
        }
    }

    private void WinMenuButtonsActivation(GameOverTypes currentType)
    {
        if (currentType == GameOverTypes.Lose)
        {
            for (int i = 0; i < winMenuButtons.Length; i++)
            {
                if (!winMenuButtons[i].name.Contains("NextButton"))
                    winMenuButtons[i].isEnabled = true;
            }
        }
        else
            WinMenuButtonsActivation(true);
    }

    private void WinMenuButtonsActivation(bool activation)
    {
        if (winMenuButtons == null)
        {
            Debug.LogError("WinLoseMenu buttons aren't found!");
            return;
        }

        for (int i = 0; i < winMenuButtons.Length; i++)
            winMenuButtons[i].isEnabled = activation;
    }

    private void SaveData(GameOverTypes currentType)
    {
        //if (currentType != GameOverTypes.Lose)
        //{
        //    int currentStarsCount = (int)currentType;
        //    int levelStarsCount = (int)MemoryManager.Instance.GetParameter(Application.loadedLevelName + "Stars");
        //    int maxStarsCount = (int)MemoryManager.Instance.GetParameter(GameParameters.AllStars);
        //    if (currentStarsCount > levelStarsCount)
        //    {
        //        maxStarsCount += (currentStarsCount - levelStarsCount);
        //        MemoryManager.Instance.SetParameter(Application.loadedLevelName + "Stars", currentStarsCount);
        //        MemoryManager.Instance.SetParameter(GameParameters.AllStars, maxStarsCount);
        //    }

        //    if (Application.loadedLevel > MemoryManager.Instance.GetParameter(GameParameters.LastPassedLevel))
        //        MemoryManager.Instance.SetParameter(GameParameters.LastPassedLevel, Application.loadedLevel);
        //}
        //else
        //    Debug.Log("You lose! Njthing to save.");
    }

    #endregion
}
