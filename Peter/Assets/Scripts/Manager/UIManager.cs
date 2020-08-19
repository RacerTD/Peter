using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class UIManager : ManagerModule<UIManager>
{
    #region NormalUI
    [Header("Normal UI")]
    [SerializeField] protected GameObject uI;
    [Header("Health Bar")]
    [SerializeField] protected Slider healthBar;
    [Header("Switch Dimension Timer")]
    [SerializeField] protected Slider switchDimensionBar;

    #region HitMarker
    [Header("Hit Marker")]
    [SerializeField] protected List<Image> hitMarkers = new List<Image>();
    [SerializeField] protected Transform hitMarkerParent;
    [SerializeField] private Color hitMarkerStartColor = new Color(0, 0, 0, 0);
    [SerializeField] private Color hitMarkerActiveColor = Color.white;
    [SerializeField] private Color hitMarkerEndColor = new Color(0, 0, 0, 0);
    private float hitMarkerStartSize = 1f;
    [SerializeField] private float hitMarkerEndSize = 2f;
    [SerializeField] private float hitMarkerMaxSize = 10f;
    //[SerializeField] private float hitMarkerDamageSizeMultiplicator = 1f;
    [SerializeField] [Tooltip("Percent of the hit Marker active time the marker is in the stage of getting bigger and mor non transparent")] private float hitMarkerGetBiggerTimePercent = 0.7f;
    private bool hitMarkerIsActive = false;
    private int shotsShortTimeTogether = 0;
    [SerializeField] private float hitMarkerTotalTime = 0f;
    private float hitMarkerTotalTimer = 0f;
    #endregion
    #endregion

    #region DeathScreen

    [Header("DeathScreen")]
    [SerializeField] protected GameObject deathScreen;
    [SerializeField] private List<FadingImage> deathScreenObjects = new List<FadingImage>();
    [SerializeField] protected Image thingOverLogo;
    [SerializeField] private float startLength = 0f;
    [SerializeField] private Vector2 startPositon = Vector2.zero;
    [SerializeField] private float endLength = 0f;
    [SerializeField] private Vector2 endPositon = Vector2.zero;
    [SerializeField] private float thingOverLogoTimeTillStart = 2f;
    [SerializeField] private float thingOverLogoTimeToDo = 3f;
    [SerializeField] protected AudioClip deathScreenSound;
    private float timeTillDead = 0f;

    #endregion

    #region BlackScreen

    [Header("Black Screen")]
    [SerializeField] protected Image blackScreen;
    [SerializeField] protected Color blackScreenStartColor;
    [SerializeField] protected Color blackScreenEndColor;
    private float timeToBlack = 2f;
    private bool blackScreenIsActive = false;
    private float blackScreenTimer = 0f;
    private float timeInThisScene = 0f;

    #endregion

    private void Start()
    {
        if (hitMarkers.Count > 0)
        {
            hitMarkerStartSize = hitMarkerParent.localScale.x;
        }

        if (deathScreen != null)
        {
            deathScreen.SetActive(false);
        }
    }

    private void Update()
    {
        timeInThisScene += Time.deltaTime;

        HandleHitMarker();

        if (GameManager.Instance.CurrentGameState == GameState.Dead)
        {
            HandleDeathScreen();
        }

        HandleFadeToBlack();

        if (timeInThisScene <= timeToBlack * 3)
        {
            HandleGameStart();
        }
    }

    #region healthBar
    /// <summary>
    /// Updates the health bar
    /// </summary>
    /// <param name="value">Value, between 1 and 0</param>
    public void UpdateHealthBar(float value)
    {
        if (healthBar != null)
        {
            if (Mathf.Abs(healthBar.value) - Mathf.Abs(value) < 0.05)
            {
                return;
            }
        }
    }
    #endregion

    #region SwitchDimensionSlider
    /// <summary>
    /// Updates the switch dimension bar bar
    /// </summary>
    /// <param name="value">Value, between 1 and 0</param>
    public void UpdateSwitchDimensionSlider(float value)
    {
        if (switchDimensionBar != null)
        {
            switchDimensionBar.value = value;
        }
    }
    #endregion

    #region FadeToBlack
    /// <summary>
    /// Starts the fade to black
    /// </summary>
    /// <param name="time">How long the fade should take</param>
    public void StartFadeToBlack(float time)
    {
        blackScreenIsActive = true;
        blackScreenTimer = 0f;
        timeToBlack = time;
    }

    /// <summary>
    /// Handles everything for the fade to black
    /// </summary>
    public void HandleFadeToBlack()
    {
        blackScreenTimer += Time.deltaTime;

        if (blackScreenIsActive && blackScreen != null)
        {
            blackScreen.color = Color.Lerp(blackScreenStartColor, blackScreenEndColor, blackScreenTimer / timeToBlack);
        }
    }
    #endregion

    #region HitMarker
    /// <summary>
    /// Starts the hit marker
    /// </summary>
    public void ActivateHitMarker()
    {
        if (hitMarkers.Count > 0)
        {
            hitMarkerIsActive = true;
            hitMarkerTotalTimer = 0f;
            shotsShortTimeTogether++;
            /*
            foreach (Image rend in hitMarkers)
            {
                rend.rectTransform.localScale = Vector3.one * hitMarkerStartSize;
            }
            */
        }
    }

    /// <summary>
    /// Handles all the things needed for a smooth hit marker
    /// </summary>
    private void HandleHitMarker()
    {
        if (hitMarkerIsActive)
        {
            if (hitMarkerTotalTimer <= hitMarkerTotalTime * hitMarkerGetBiggerTimePercent)
            {
                foreach (Image rend in hitMarkers)
                {
                    rend.color = Color.Lerp(hitMarkerStartColor, hitMarkerActiveColor, hitMarkerTotalTimer / (hitMarkerTotalTime * hitMarkerGetBiggerTimePercent));
                }

                hitMarkerParent.localScale = Vector3.Lerp(hitMarkerParent.localScale, Vector3.one * Mathf.Clamp(hitMarkerEndSize * shotsShortTimeTogether, 0f, hitMarkerMaxSize), hitMarkerTotalTimer / (hitMarkerTotalTime * hitMarkerGetBiggerTimePercent));
            }
            else
            {
                foreach (Image rend in hitMarkers)
                {
                    rend.color = Color.Lerp(hitMarkerActiveColor, hitMarkerEndColor, (hitMarkerTotalTimer - hitMarkerTotalTime * hitMarkerGetBiggerTimePercent) / (hitMarkerTotalTime * hitMarkerGetBiggerTimePercent));
                }

                hitMarkerParent.localScale = Vector3.Lerp(hitMarkerParent.localScale, Vector3.one * hitMarkerStartSize, (hitMarkerTotalTimer - hitMarkerTotalTime * hitMarkerGetBiggerTimePercent) / (hitMarkerTotalTime * hitMarkerGetBiggerTimePercent));
            }

            if (hitMarkerTotalTimer >= hitMarkerTotalTime)
            {
                hitMarkerIsActive = false;
                hitMarkerParent.localScale = Vector3.one * hitMarkerStartSize;
                shotsShortTimeTogether = 0;

                foreach (Image rend in hitMarkers)
                {
                    rend.color = hitMarkerEndColor;
                }
            }

            hitMarkerTotalTimer += Time.deltaTime;
        }
    }
    #endregion

    #region DeathScreen
    /// <summary>
    /// Activated the deathscreen
    /// </summary>
    public void ActivateDeathScreen()
    {
        deathScreen.SetActive(true);
        uI.SetActive(false);
        if (deathScreenSound != null)
        {
            AudioManager.Instance.PlayNewSound(AudioType.Music, deathScreenSound);
        }
    }

    /// <summary>
    /// Handles all the things needed for the deathscreen
    /// </summary>
    public void HandleDeathScreen()
    {
        foreach (FadingImage fadingImage in deathScreenObjects)
        {
            fadingImage.Picture.color = Color.Lerp(fadingImage.StartColor, fadingImage.EndColor, (timeTillDead - fadingImage.TimeTillStart) / fadingImage.TimeToFade);
        }

        thingOverLogo.rectTransform.sizeDelta = new Vector2(Mathf.Lerp(startLength, endLength, (timeTillDead - thingOverLogoTimeTillStart) / thingOverLogoTimeToDo), thingOverLogo.rectTransform.rect.height);

        thingOverLogo.rectTransform.localPosition = new Vector3(Mathf.Lerp(startPositon.x, endPositon.x, (timeTillDead - thingOverLogoTimeTillStart) / thingOverLogoTimeToDo), Mathf.Lerp(startPositon.y, endPositon.y, (timeTillDead - thingOverLogoTimeTillStart) / thingOverLogoTimeToDo), 0);

        timeTillDead += Time.deltaTime;
    }
    #endregion

    #region GameStart
    /// <summary>
    /// Does all the stuff needed for the gameto start
    /// </summary>
    public void HandleGameStart()
    {
        blackScreenTimer += Time.deltaTime;

        if (blackScreen != null)
        {
            blackScreen.color = Color.Lerp(blackScreenEndColor, blackScreenStartColor, blackScreenTimer / timeToBlack);
        }
    }
    #endregion

    [System.Serializable]
    public struct FadingImage
    {
        public Image Picture;
        public float TimeTillStart;
        public float TimeToFade;
        public Color StartColor;
        public Color EndColor;

        public FadingImage(Image picture, float timeTillStart, float timeToFade, Color startColor, Color endColor)
        {
            Picture = picture;
            TimeTillStart = timeTillStart;
            TimeToFade = timeToFade;
            StartColor = startColor;
            EndColor = endColor;
        }
    }
}
