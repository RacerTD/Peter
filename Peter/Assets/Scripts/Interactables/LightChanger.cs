using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChanger : MonoBehaviour
{
    [SerializeField] private List<LightSetting> lightSettings = new List<LightSetting>();
    private LightSetting lastLightSetting = new LightSetting();
    [SerializeField] protected Light lightToChange;
    private int currentIndex = 0;
    private int CurrentIndex
    {
        get => currentIndex;
        set
        {
            if (value >= lightSettings.Count)
            {
                isActive = false;
            }
            else
            {
                currentIndex = value;
                timeInCurrentState = 0f;
            }
        }
    }
    private bool isActive = false;
    private float totalTime = 0f;
    private float timeSinceActive = 0f;
    private float timeInCurrentState = 0f;

    private void Start()
    {
        float temp = 0f;
        foreach (LightSetting set in lightSettings)
        {
            temp += set.Time;
        }
        totalTime = temp;
        lastLightSetting = new LightSetting(lightToChange.intensity, lightToChange.range, lightToChange.color);
    }

    private void Update()
    {
        if (isActive)
        {
            timeSinceActive += Time.deltaTime;
            timeInCurrentState += Time.deltaTime;

            lightToChange.color = Color.Lerp(lastLightSetting.Color, lightSettings[CurrentIndex].Color, timeInCurrentState / lightSettings[CurrentIndex].Time);
            lightToChange.range = Mathf.Lerp(lastLightSetting.Radius, lightSettings[CurrentIndex].Radius, timeInCurrentState / lightSettings[CurrentIndex].Time);
            lightToChange.intensity = Mathf.Lerp(lastLightSetting.Intensity, lightSettings[CurrentIndex].Intensity, timeInCurrentState / lightSettings[CurrentIndex].Time);

            if (timeInCurrentState >= lightSettings[CurrentIndex].Time)
            {
                lastLightSetting = lightSettings[currentIndex];
                CurrentIndex++;
            }

            if (timeSinceActive >= totalTime)
            {
                isActive = false;
            }
        }
    }

    [ContextMenu("Test Light Stuff")]
    public void StartLightChanging()
    {
        if (!isActive)
        {
            lastLightSetting = new LightSetting(lightToChange.intensity, lightToChange.range, lightToChange.color);
            isActive = true;
            timeSinceActive = 0f;
            CurrentIndex = 0;
        }
    }

    [System.Serializable]
    public struct LightSetting
    {
        public float Intensity;
        public float Radius;
        public Color Color;
        public float Time;
        public LightSetting(float intensity, float radius, Color color, float time = 0f)
        {
            Intensity = intensity;
            Radius = radius;
            Color = color;
            Time = time;
        }
    }
}
