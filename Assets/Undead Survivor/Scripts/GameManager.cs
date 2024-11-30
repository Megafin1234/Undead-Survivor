using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("#Game Control")]
    public bool isLive;
    public float dayPhaseTimer;  
    public float nightPhaseTimer; 
    public float dayPhaseDuration = 180f; 
    public float nightPhaseDuration = 120f; 
    
      
    [Header("#Player Info")]
    public int playerId;
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 360, 450, 600};
    [Header("#Game Object")]

    public Player player;
    public PoolManager pool;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject enemyCleaner;
    public Weapon weapon;

    private bool isDayPhase = true;

    void Awake(){
        instance = this;
    }
    public void GameStart(int id)
    {
        playerId = id;
        health = maxHealth;
        player.gameObject.SetActive(true);
        uiLevelUp.Select(playerId % 2);
        Resume();
        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        dayPhaseTimer = dayPhaseDuration;
    }
    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }
    IEnumerator GameOverRoutine()
    {
        isLive =false;
        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.lose();
        Stop();
        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);

    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }
    IEnumerator GameVictoryRoutine()
    {
        isLive =false;
        enemyCleaner.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();
        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);

    }
    public void GameRetry()

    {
        SceneManager.LoadScene(0);

    }
    void Update(){
        if (!isLive)
            return;
        /*gameTime += Time.deltaTime;

        if(gameTime > maxGameTime){
            gameTime = maxGameTime;
            GameVictory();
        } */
        if (isDayPhase)
        {
            dayPhaseTimer -= Time.deltaTime;
            if (dayPhaseTimer <= 0)
            {
                DayToNight();
            }
        }
        else
        {
            nightPhaseTimer -= Time.deltaTime;
            if (nightPhaseTimer <= 0)
            {
                NightToDay();
            }
        }

    }

public void DayToNight()
{
    isDayPhase = false;
    nightPhaseTimer = nightPhaseDuration;
    isLive = false;

    UIManager.instance.FadeOut(() =>
    {
        Debug.Log("페이드 아웃 완료 후 작업 실행");
        UIManager.instance.ShowNightPhaseText();
        UIManager.instance.FadeIn(() =>
        {
            Debug.Log("페이드 인 완료 후 작업 실행");
            isLive = true;
            StartNightPhase();
        });
    });
}

public void NightToDay()
{
    isDayPhase = true;
    isLive = false;
    dayPhaseTimer = dayPhaseDuration;

    UIManager.instance.FadeOut(() =>
    {
        UIManager.instance.ShowDayPhaseText();
        UIManager.instance.FadeIn(() =>
        {
            isLive = true;
            StartDayPhase();
        });
    });
}

// 낮/밤 페이즈 시작 로직
private void StartDayPhase()
{
    Debug.Log("Day Phase Started");
    // 낮 페이즈 관련 초기화 로직
}

private void StartNightPhase()
{
    Debug.Log("Night Phase Started");
    // 밤 페이즈 관련 초기화 로직
}


    public void GetExp()
    {   
        if (!isLive)
            return;
        exp++;

        if(exp == nextExp[Mathf.Min(level,nextExp.Length-1)]){
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }
    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }
}
