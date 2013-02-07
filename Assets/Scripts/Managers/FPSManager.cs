using UnityEngine;

public class FPSManager : MonoBehaviour
{
    public float updateInterval = 0.5F;
    private double lastInterval;
    private int frames = 0;
    private float fps;
    private int maxFPS = 0;
    private UILabel label;

    private void Start()
    {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
        label = this.GetComponent<UILabel>();
        if (label == null)
            Debug.LogError("FPS label isn't found!");
    }

    private void Update()
    {
        ++frames;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > lastInterval + updateInterval)
        {
            fps = (float)(frames / (timeNow - lastInterval));
            //fps = (float)(1f / Time.smoothDeltaTime);

            frames = 0;
            lastInterval = timeNow;

            if (((int)fps) >= maxFPS)
                maxFPS = (int)fps;
        }

        PrintFPS();
    }

    private void PrintFPS()
    {
        if (fps > (float)(maxFPS * 0.7))
            label.color = Color.green;
        else if ((fps > (float)(maxFPS * 0.3)) && (fps < (float)(maxFPS * 0.7)))
            label.color = Color.yellow;
        else if (fps < (float)(maxFPS * 0.3))
            label.color = Color.red;

        label.text = fps.ToString("f1");
    }
}