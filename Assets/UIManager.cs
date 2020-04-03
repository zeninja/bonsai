using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public RectTransform selections;

    public UISelection ui_selection_prefab;
    UISelection active_selection;

    public void HandleMouseDown()
    {
        // Vector2 screen_pos = Input.mousePosition;

        active_selection = Instantiate(ui_selection_prefab);
        active_selection.transform.SetParent(selections); 
        active_selection.OnCreated();

        // Debug.Log(screen_pos);
    }

    public void HandleMouseHeld()
    {
        // Vector2 screen_pos = Input.mousePosition;
        active_selection.OnUpdate();
    }

    public void HandleMouseUp()
    {
        active_selection.OnDestroyed();
    }
}
