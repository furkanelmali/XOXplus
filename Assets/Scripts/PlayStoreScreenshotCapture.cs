using System;
using System.Collections;
using System.IO;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayStoreScreenshotCapture : MonoBehaviour
{
    [Header("Capture")]
    [SerializeField] private KeyCode captureKey = KeyCode.F12;
    [SerializeField] private KeyCode alternateCaptureKey = KeyCode.P;
    [SerializeField, Min(1)] private int superSize = 1;
    [SerializeField] private bool captureOnStart;
    [SerializeField, Min(0f)] private float captureOnStartDelay = 1f;

    [Header("Output")]
    [SerializeField] private string outputFolderName = "PlayStoreScreenshots";
    [SerializeField] private string filePrefix = "xoxplus_playstore";
    [SerializeField] private bool includeResolutionInFileName = true;

    private bool isCapturing;

    private void Start()
    {
        if (captureOnStart)
        {
            StartCoroutine(CaptureAfterDelay());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(captureKey) || Input.GetKeyDown(alternateCaptureKey))
        {
            Capture();
        }
    }

    public void Capture()
    {
        if (!isCapturing)
        {
            StartCoroutine(CaptureRoutine());
        }
    }

    private IEnumerator CaptureAfterDelay()
    {
        yield return new WaitForSecondsRealtime(captureOnStartDelay);
        Capture();
    }

    private IEnumerator CaptureRoutine()
    {
        isCapturing = true;
        yield return new WaitForEndOfFrame();

        var path = BuildScreenshotPath();
        ScreenCapture.CaptureScreenshot(path, superSize);
        Debug.Log($"Play Store screenshot saved: {path}");

        isCapturing = false;
    }

    private string BuildScreenshotPath()
    {
        var outputDirectory = GetOutputDirectory();
        Directory.CreateDirectory(outputDirectory);

        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var resolution = includeResolutionInFileName ? $"_{Screen.width}x{Screen.height}" : string.Empty;
        var safePrefix = MakeSafeFileName(filePrefix);
        return Path.Combine(outputDirectory, $"{safePrefix}{resolution}_{timestamp}.png");
    }

    private string GetOutputDirectory()
    {
#if UNITY_EDITOR
        var projectRoot = Directory.GetParent(Application.dataPath).FullName;
        return Path.Combine(projectRoot, outputFolderName);
#else
        return Path.Combine(Application.persistentDataPath, outputFolderName);
#endif
    }

    private static string MakeSafeFileName(string value)
    {
        foreach (var invalidChar in Path.GetInvalidFileNameChars())
        {
            value = value.Replace(invalidChar, '_');
        }

        return string.IsNullOrWhiteSpace(value) ? "playstore_screenshot" : value;
    }
}
