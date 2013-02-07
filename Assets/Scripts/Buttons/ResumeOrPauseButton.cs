using UnityEngine;
using System.Collections;

public class ResumeOrPauseButton : Button
{
    public enum ButtonTypes { PauseButton, ResumeButton }
    public ButtonTypes buttonType = ButtonTypes.PauseButton;

    private IEnumerator Start()
    {
        if (buttonType == ButtonTypes.PauseButton)
        {
            UIButton uiButton = this.GetComponent<UIButton>();
            if (uiButton != null)
            {
                while (GameController.Instance.CurrentGameState != GameStates.GameOver)
                {
                    if ((GameController.Instance.CurrentGameState == GameStates.Pause) && (uiButton.isEnabled))
                        uiButton.isEnabled = false;
                    else if ((GameController.Instance.CurrentGameState == GameStates.Play) && (!uiButton.isEnabled))
                        uiButton.isEnabled = true;
                    yield return null;
                }
                uiButton.isEnabled = false;
            }
            else
            {
                Debug.LogError("In " + this.name + " component 'UIButton' hasn't found!");
                yield return null;
            }
        }
        else
            yield return null;
    }

    protected override void Click()
    {
        if (buttonType == ButtonTypes.PauseButton)
        {
            GameController.Instance.StopGame();
            LevelGUIController.Instance.GameMenuActivation(true);
        }
        else if (buttonType == ButtonTypes.ResumeButton)
        {
            GameController.Instance.ResumeGame();
            LevelGUIController.Instance.GameMenuActivation(false);
        }
    }
}
