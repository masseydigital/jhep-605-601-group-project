using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverControl : MonoBehaviour
{
    public HoverState state;
    public Image hoverImage;
    public int transparencyAmount = 45;

    private Color goodColor = Color.green;
    private Color badColor = Color.red;
    private Color transparentColor = new Color(0, 0, 0, 0);
    private Color visibleColor = new Color(255, 255, 255, 45);

    // Start is called before the first frame update
    void Start()
    {
        state = HoverState.bad;
        hoverImage.color = transparentColor;

        goodColor = new Color(goodColor.r, goodColor.g, goodColor.b, transparencyAmount);
        badColor = new Color(badColor.r, badColor.g, badColor.b, badColor.a);
    }

    // Checks the state and changes the image based on result
    public void CheckState()
    {
        switch (state)
        {
            case HoverState.good:
                hoverImage.color = goodColor;
                break;
            case HoverState.bad:
                hoverImage.color = badColor;
                break;
            default:
                break;
        }
    }

    // When the mouse hovers over the icon
    public void Enter()
    {
        hoverImage.color = visibleColor;
        CheckState();
    }

    // When the mouse exits hover
    public void Exit()
    {
        // Make the image invisible
        hoverImage.color = transparentColor;
    }

    // When the mouse is clicked over the object
    public void Click()
    {
        Debug.Log("Clicked!");
    }
}

public enum HoverState
{
    good = 0,
    bad = 1
};

