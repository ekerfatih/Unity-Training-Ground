using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject options,startBtn,optionsBtn,quitBtn;
    public void OptionsMenu()
    {
        options.SetActive(true);
        startBtn.SetActive(false);
        optionsBtn.SetActive(false);
        quitBtn.SetActive(false);
    }
    public void BackBtn()
    {
        options.SetActive(false);
        startBtn.SetActive(true);
        optionsBtn.SetActive(true);
        quitBtn.SetActive(true);
    }
    public void PlayBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
