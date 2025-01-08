using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using VRFPSKit;
using static AYellowpaper.SerializedCollections.SerializedDictionarySample;

public class MemeGameController : MonoBehaviour
{
    public static MemeGameController Instance { get; private set; }

    public float time = 301f;

    public TextMeshProUGUI timeText;

    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI highScoreText;

    [Header("Game UI Buttons")]

    [SerializeField] GameObject statusBtn;
    [SerializeField] GameObject statusBtn2;
    [SerializeField] GameObject pausedText;
    [SerializeField] GameObject pausedText2;

    [SerializeField] GameObject isPlayText;
    [SerializeField] GameObject isPlayText2;

    [SerializeField] GameObject restartBtn;
    [SerializeField] GameObject restartText;

    [SerializeField] bool isPlay = false;
    //public bool IsPlay { get { return isPlay; } }

    [SerializeField] GameObject shotgunVideo;
    //[SerializeField] GameObject shotgunVideo2;

    AudioManager audioManager;

    public int score { get; private set; }

    [SerializedDictionary("meme name", "kill count")]
    public SerializedDictionary<string, int> KilledDict = new();

    private void Awake()
    {
        Instance = this;
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    private void Start()
    {
        isPlay = false;

        ToggleRestartBtn(false);

        if (shotgunVideo != null)
        {
            ToggleTutorialVideo(true);
        }



        CreateKilledCountDict();
        AddScore(0);
        highScoreText.text = "Top: " + GetScore().ToString();
        UIGameManager.Instance.Setup();
        MemeSpawner.Instance.DisableMemeSpawner();
    }

    private void Update()
    {
        if (isPlay)
        {
            TimeCount();

            // considering 
            //ToggleTutorialVideo(false);
            //ToggleIsPlayText(true);
            //ToggleIsPausedText(false);

            //MemeSpawner.Instance.ReactivateMemeSpawner();
        }
        //else if (!isPlay)
        //{
        //    ToggleIsPlayText(false);
        //    ToggleIsPausedText(true);
        //    ToggleTutorialVideo(true);

        //    MemeSpawner.Instance.DisableMemeSpawner();
        //}
    }

    public void AddScore(int val)
    {
        score += val;

        scoreText.text = "Score: " + score.ToString();
    }

    void CreateKilledCountDict()
    {
        KilledDict.Clear();
        foreach (Sprite sprite in MemeSpawner.Instance.memeSprites)
        {
            KilledDict.Add(sprite.name, 0);
        }
    }

    public void AddKilled(string spriteName)
    {
        if (KilledDict.TryGetValue(spriteName, out int value))
        {
            KilledDict[spriteName] += 1;
        }
        else
        {
            KilledDict.Add(spriteName, 1);
        }
    }

    public int GetScore()
    {
        return PlayerPrefs.GetInt("score", 0);
    }

    public void SetScore(int val)
    {
        PlayerPrefs.SetInt("score", val);
    }

    public void UpdateHighScore()
    {
        if (score > GetScore())
        {
            SetScore(score);
        }
    }

    void TimeCount()
    {
        if (isPlay)
        {

            if (time > 0f)
            {
                time -= Time.deltaTime;

                if (time <= 0f)
                {
                    if (time < 0f)
                    {
                        time = 0f;
                    }

                    GameOver();
                }

                int minute = (int)(time / 60f);

                int second = (int)(time - (minute * 60f));

                timeText.text = minute.ToString("00") + ":" + second.ToString("00");
            }
        }
    }

    void GameOver()
    {
        if (audioManager != null)
        {
            audioManager.DisableBGM();
            audioManager.PlayVictorySFX();
        }

        UIGameManager.Instance.GameOver();

        MemeSpawner.Instance.DisableMemeSpawner();

        ToggleStatusBtn(false);
        ToggleIsPausedText(false);
        ToggleIsPlayText(false);
        ToggleRestartBtn(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ToggleIsPlay()
    {
        
        audioManager.PlayButtonPressedSFX();

        if (isPlay)
        {
            isPlay = false;

            ToggleIsPlayText(false);
            ToggleIsPausedText(true);
            ToggleTutorialVideo(true);

            MemeSpawner.Instance.DisableMemeSpawner();

        }
        else if (!isPlay)
        {

            isPlay = true;

            // tắt tutorial video khi vào game
            ToggleTutorialVideo(false);
            ToggleIsPlayText(true);
            ToggleIsPausedText(false);

            MemeSpawner.Instance.ReactivateMemeSpawner();

        }
    }

    void ToggleTutorialVideo(bool value)
    {
        if (shotgunVideo != null)
        {
            shotgunVideo.SetActive(value);
            //shotgunVideo2.SetActive(value);

        }
    }

    void ToggleRestartBtn(bool value)
    {
        if (restartBtn && restartText != null)
        {
            restartBtn.SetActive(value);
            restartText.SetActive(value);

        }
    }

    void ToggleStatusBtn(bool value)
    {
        statusBtn.SetActive(value);
        statusBtn2.SetActive(value);
    }

    void ToggleIsPlayText(bool value)
    {

        isPlayText.SetActive(value);
        isPlayText2.SetActive(value);
    }

    void ToggleIsPausedText(bool value)
    {
        pausedText.SetActive(value);
        pausedText2.SetActive(value);
    }
}
