using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageOptions : MonoBehaviour
{
    public Sprite nullImage;
    public List<Sprite> images;

    public Sprite GetImage(int id)
    {
        if(id > 0 && id < images.Count-1)
        {
            return images[id];
        }
        else
        {
            Debug.Log("That isn't right!");
            return nullImage;
        }
    }
}
