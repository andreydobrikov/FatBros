/*Описание.
 *Скрипт подгружает аудио клипы из ресурсов
 *и записывает их к себе в словарь для даль-
 *нешего использования. В папке "Resources"
 *нужно создать папку "Sounds" куда и скла-
 *дывать все нужные аудио клипы.
 */
using UnityEngine;
using System.Collections.Generic;

public class AudioManager
{
    #region Variables

    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    private static AudioManager instance = null;

    #endregion

    #region Properties

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
                instance = new AudioManager();
            return instance;
        }
    }

    #endregion

    #region Functions

    public AudioClip GetClip(string clipName)
    {
        AudioClip clip = null;
        if (audioClips.TryGetValue(clipName, out clip)) return clip;
        else
        {
            clip = (AudioClip)Resources.Load("Sounds/" + clipName);
            if (clip != null)
            {
                audioClips.Add(clipName, clip);
                return clip;
            }
            else
            {
                Debug.LogWarning("Audio clip '" + clipName + "' isn't found! Create 'Sounds' folder in the 'Resources' folder or check clip name.");
                return null;
            }
        }
    }

    public void RemoveClip(string clipName) { audioClips.Remove(clipName); }

    public void ClearDictionary()
    {
        audioClips.Clear();
        Debug.Log("All clips was removed from AudioManager");
    }

    #endregion
}
