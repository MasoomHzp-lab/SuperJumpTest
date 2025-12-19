public void OnPointerDown(PointerEventData eventData)
{
    if (Time.unscaledTime - lastTapTime > multiTapWindow)
        tapCount = 0;

    tapCount++;
    lastTapTime = Time.unscaledTime;

    if (tapCount >= tapsRequired)
    {
        tapCount = 0;
        if (CheatManager.Instance != null)
            CheatManager.Instance.ForceNextRoll(forcedValue, forceTimeoutSeconds);
    }
}