using UnityEngine;

public class MainMenuUIComponent : MonoBehaviour
{
    [SerializeField] private GameObject _aboutPanel;

    public void OnClickPlay()
    {
        GameManager.Instance.StartGame();
        _aboutPanel.SetActive(false);
    }

    public void OnClickAbout()
    {
        _aboutPanel.SetActive(true);
    }

    public void OnClickCloseAbout()
    {
        _aboutPanel.SetActive(false);
    }

    public void OnClickQuit()
    {
        GameManager.Instance.Quit();
    }
}
