using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FPS : MonoBehaviour
{
    private int FramesPerSec;
    private float frequency = 1.0f;
    private TextMeshProUGUI textMeshProUGUI;
    void Start()
    {
        this.textMeshProUGUI = this.GetComponent<TextMeshProUGUI>();
        StartCoroutine(FPSCoroutine());
    }

    private IEnumerator FPSCoroutine()
    {
        for (; ; )
        {
            // Capture frame-per-second
            int lastFrameCount = Time.frameCount;
            float lastTime = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(frequency);
            float timeSpan = Time.realtimeSinceStartup - lastTime;
            int frameCount = Time.frameCount - lastFrameCount;

            // Display it

            this.textMeshProUGUI.text = string.Format("FPS: {0}", Mathf.RoundToInt(frameCount / timeSpan));
        }
    }

}
