using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public DBConnection dataRet;
    public TMP_Dropdown theme_dropdown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FindGame()
    {
        SceneManager.LoadScene(1);
    }

    // Communicates the the game data to update the theme
    public void UpdateTheme()
    {
        dataRet.RetrieveData(theme_dropdown.value);
    }
}
