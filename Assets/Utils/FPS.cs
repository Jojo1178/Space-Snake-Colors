using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FPS : MonoBehaviour
{
    private int FramesPerSec;
    private float frequency = 1.0f;
    private TextMeshProUGUI textMeshProUGUI;

    private void Start()
    {
        this.textMeshProUGUI = this.GetComponent<TextMeshProUGUI>();
        StartCoroutine(FPSCoroutine());
    }

    private IEnumerator FPSCoroutine()
    {
        while (true)
        {
            int lastFrameCount = Time.frameCount;
            float lastTime = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(frequency);
            float timeSpan = Time.realtimeSinceStartup - lastTime;
            int frameCount = Time.frameCount - lastFrameCount;
            this.textMeshProUGUI.text = string.Format("FPS: {0}", Mathf.RoundToInt(frameCount / timeSpan));
        }
    }
}