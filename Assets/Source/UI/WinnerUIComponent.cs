using UnityEngine;
using UnityEngine.SceneManagement;

public class WinnerUIComponent : MonoBehaviour
{
    public void OnClickRetry()
    {
        SceneManager.LoadScene(0);
    }

    public void OnClickQuit()
    {
        GameManager.Instance.Quit();
    }
}
