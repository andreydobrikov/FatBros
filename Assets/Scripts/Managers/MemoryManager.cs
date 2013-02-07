/*ќписание.
 * —крип сохран€ет(загружает) игровые параметры
 * в(из) реестр(а). ¬ enum GameParameters нужно
 * занести имена всех параметров игры, а потом
 * дублировать их в массив allParameters. ќстальное
 * оставить без изменений.
 */

using UnityEngine;
using System.Collections.Generic;

public enum GameParameters
{
    LastPassedLevel,
    Level_1Stars,
    Level_2Stars,
    Level_3Stars,
    Level_4Stars,
    Level_5Stars,
    Level_6Stars,
    Level_7Stars,
    Level_8Stars,
    Level_9Stars,
    Level_10Stars,
    AllStars
}

public class MemoryManager
{
    #region Variables

    private static MemoryManager instance;

    private GameParameters[] allParameters = 
    {
        GameParameters.LastPassedLevel,
        GameParameters.Level_1Stars,
        GameParameters.Level_2Stars,
        GameParameters.Level_3Stars,
        GameParameters.Level_4Stars,
        GameParameters.Level_5Stars,
        GameParameters.Level_6Stars,
        GameParameters.Level_7Stars,
        GameParameters.Level_8Stars,
        GameParameters.Level_9Stars,
        GameParameters.Level_10Stars,
        GameParameters.AllStars
    };
    private Dictionary<GameParameters, float> additionalParameters = new Dictionary<GameParameters, float>();

    #endregion

    #region Properties

    public static MemoryManager Instance
    {
        get 
        {
            if (instance == null)
                instance = new MemoryManager();
            return instance;
        }
        set { instance = value; }
    }

    #endregion

    #region Functions

    public MemoryManager() { Load(); }

    public void SetParameter(GameParameters parameter, float parameterValue)
    {
        if (additionalParameters.ContainsKey(parameter))
            additionalParameters.Remove(parameter);

        additionalParameters.Add(parameter, parameterValue);
    }

    public void SetParameter(string parameter, float parameterValue)
    {
        GameParameters? curPar = null;
        foreach (GameParameters p in allParameters)
        {
            if (parameter == p.ToString())
            {
                curPar = (GameParameters)p;
                break;
            }
        }
        if (curPar != null)
            SetParameter((GameParameters)curPar, parameterValue);
        else
            Debug.LogWarning("SetParameter warning: Parameter " + parameter + " isn't declared! Please describe this parameter in GameParameters enum.");
    }

    public float GetParameter(GameParameters parameter)
    {
        float val = 0f;
        if (additionalParameters.TryGetValue(parameter, out val))
            return val;
        else
        {
            Debug.LogWarning("Parameter " + parameter.ToString() + " hasn't found. Please add all parameters from 'GameParameters' enum to the 'allParameters' array!");
            return 0;
        }
    }

    public float GetParameter(string parameter)
    {
        GameParameters? curPar = null;
        foreach (GameParameters p in allParameters)
        {
            if (parameter == p.ToString())
            {
                curPar = (GameParameters)p;
                break;
            }
        }
        if (curPar != null)
            return GetParameter((GameParameters)curPar);
        else
        {
            Debug.LogWarning("GetParameter warning: Parameter " + parameter + " isn't declared! Please describe this parameter in GameParameters enum.");
            return 0f;
        }
    }

    public void Save()
    {
        foreach (GameParameters key in additionalParameters.Keys)
        {
            float val = 0;
            if (additionalParameters.TryGetValue(key, out val))
                PlayerPrefs.SetFloat(key.ToString(), val);
        }
    }

    private void Load()
    {
        foreach (GameParameters param in allParameters)
        {
            float val = 0;
            if (PlayerPrefs.HasKey(param.ToString()))
                val = PlayerPrefs.GetFloat(param.ToString());
            additionalParameters.Add(param, val);
        }
    }

    #endregion
}
