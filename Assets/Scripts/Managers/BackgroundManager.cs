using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    private void Start() { SetDiamentions(); }

    private void SetDiamentions()
    {
        float scale_Z = (float)(Camera.mainCamera.orthographicSize / 5f);
        float scale_Y = 1f;
        float scale_X = (float)System.Math.Round(scale_Z / Screen.height * Screen.width, 4);

        this.transform.localScale = new Vector3(scale_X, scale_Y, scale_Z);
    }
}
