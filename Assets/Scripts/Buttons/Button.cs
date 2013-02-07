using UnityEngine;

public abstract class Button : MonoBehaviour 
{
    protected virtual void Awake() { }

    private void OnEnable()
    {
        UIButtonPlayAnimation pa = this.GetComponent<UIButtonPlayAnimation>();
        if (pa == null)
        {
            Debug.LogError("In " + this.name + " component 'UIButtonPlayAnimation' isn't found!");
            return;
        }

        if (pa.eventReceiver == null)
            pa.eventReceiver = this.gameObject;

        if (string.IsNullOrEmpty(pa.callWhenFinished))
            pa.callWhenFinished = "Click";
    }

    protected abstract void Click();
}
