using UnityEngine;
using TMPro;

public class DisplayFrameRate : MonoBehaviour
{
    public float updateRate = 1.5f;
    [SerializeField] private TMP_Text frameRateText;
    private int avgFrameRate;

    private float nextUpdateTime;

    private void OnEnable()
    {
        avgFrameRate = 0;
        nextUpdateTime = Time.time + updateRate;
    }
    private void Update()
    {
        if(nextUpdateTime <= Time.time)
        {
            nextUpdateTime = Time.time + updateRate;

            GetFrameRate();
        }
    }
    private void GetFrameRate()
    {
        float current = 0;
        current = Time.frameCount / Time.time;
        avgFrameRate = Mathf.RoundToInt(current);

        frameRateText.text = "fps " + avgFrameRate;
    }
}
