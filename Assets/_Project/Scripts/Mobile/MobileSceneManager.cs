using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MobileSceneManager : MonoBehaviour
{
    [SerializeField] private FrameRate targetFrameRate;
    [SerializeField] private float timeScale;

    private Canvas QuitCanvas;
    private Button exitButton;

   
    private void Awake()
    {
        Application.targetFrameRate = (int)targetFrameRate;
        Time.timeScale = timeScale;
    }
    private void OnDisable()
    {
        if (exitButton != null)
            exitButton.onClick.RemoveListener(QuitApp);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                Application.Quit();
            }
            else
            {
                Quitting();
            }
        }
           
    }

    private void Quitting()
    {
        if(QuitCanvas == null)
        {
            GameObject obj = Instantiate(Resources.Load("QuittingCanvas"), Vector3.zero, Quaternion.identity) as GameObject;
            QuitCanvas = obj.GetComponent<Canvas>();

            exitButton = obj.transform.Find("Exit Button").GetComponent<Button>();
            exitButton.onClick.AddListener(QuitApp);
        }
        QuitCanvas.enabled = true;
    }
    private void QuitApp()
    {
        Application.Quit();
    }
}
