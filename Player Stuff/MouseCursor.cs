using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    public Texture2D crosshair;
    public Texture2D menuCursor;

    Texture2D currentCursor;

    Vector2 crosshairOffset;
    Vector2 menuCursorOffset;
    // Start is called before the first frame update
    void Start()
    {
        //Set the offsets for the mouse cursors
        crosshairOffset = new Vector2(crosshair.width / 2, crosshair.height / 2);
        menuCursorOffset = Vector2.zero;
        currentCursor = crosshair;
        SubscribeToEvents();
    }

    void SubscribeToEvents()
    {
        //GameEvents.current.onWeaponSelected += ChangeCurrentPlayerCursor;
    }

    //Referenced by the Game Controller
    public void SetPlayerCursor()
    {
        //Set the center of the new crosshair
        float xspot = currentCursor.width / 2;
        float yspot = currentCursor.height / 2;

        Vector2 hotspot = new Vector2(xspot, yspot);

        Cursor.SetCursor(currentCursor, hotspot, CursorMode.Auto);
    }

    public void SetMenuCursor()
    {
        Cursor.SetCursor(menuCursor, menuCursorOffset, CursorMode.Auto);
    }

    /*//Change player cursor to standard cursor or missile target box cursor
    public void ChangeCurrentPlayerCursor(Texture2D _newCrosshair)
    {
        currentCursor = _newCrosshair;

        if (currentCursor == null)
        {
            currentCursor = crosshair;
        }

        SetPlayerCursor();
    }*/

    public void MouseVisibility(bool _visible)
    {
        Cursor.visible = _visible;
    }
}
