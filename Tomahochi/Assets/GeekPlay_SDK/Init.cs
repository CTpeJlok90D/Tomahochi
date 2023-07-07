//#define INIT_DEBUG;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saving;
using System.IO;
using System;

public enum Platform 
{
    Editor,
    Yandex, 
    VK,
    GameArter
}

public class Init : MonoBehaviour
{
    public bool SoundOn = true; //Звук включен?
    public PlayerData PlayerData;

    [Space(50)]
    [Header("Publisher Settings")]
    public Platform Platform; //Платформа
    [SerializeField] private GameObject gameArterPrefab; //Префаб площадки GameArter
    public GameObject leaderboardBtn; //КНОПКА, ОТКРЫВАЮЩАЯ ЛИДЕРБОРД
    [SerializeField] private string colorDebug; //Цвет Дебага

    [Header("Not touch")]
    public static Init Instance;
    private string rewardTag; //Тэг награды
    private bool adOpen; //Реклама открыта?
    private string purchasedTag; //Тэг покупки
    public bool wasLoad; //Игра загружалась?
    private bool canShowAd = true; //Можно ли проигрывать рекламу на вк?
    private string developerNameYandex = "GeeKid%20-%20школа%20программирования";

    [Header("Localization")]
    public string language;

    [Header("Mobile")]
    public bool mobile; //Устройство игрока мобильное?


	//ВЫ МЕНЯЕТЕ
	public delegate void RewardHandler();
	private Dictionary<string, RewardHandler> _rewardDelates = new();
	public void OnRewarded() //ВОЗНАГРАЖДЕНИЕ ПОСЛЕ ПРОСМОТРА РЕКЛАМЫ
    {
        _rewardDelates[rewardTag].Invoke();
        _rewardDelates.Remove(rewardTag);

		Debug.Log($"<color=yellow>REWARD:</color> {rewardTag}");
        Save();
        StartCoroutine(RewardLoad());
    }

    public void ShowRewardedAd(RewardHandler callback)
    {
        string id = Guid.NewGuid().ToString();
		_rewardDelates.Add(id, callback);
        ShowRewardedAd(id);
	}

    //ВЫ ВЫЗЫВАЕТЕ
    public void ShowInterstitialAd() //МЕЖСТРАНИЧНАЯ РЕКЛАМА - ПОКАЗАТЬ
    {
        switch (Platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>INTERSTITIAL SHOW</color>");
                break;
            case Platform.Yandex:
                Utils.AdInterstitial();
                break;
            case Platform.VK:
                if (canShowAd)
                {
                    canShowAd = false;
                    StartCoroutine(CanAdShow());
                    Utils.VK_Interstitial();
                }
                break;
        }
    }
    public void ShowRewardedAd(string idOrTag) //РЕКЛАМА С ВОЗНАГРАЖДЕНИЕМ - ПОКАЗАТЬ
    {
        switch (Platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>REWARD SHOW</color>");
                rewardTag = idOrTag;
                OnRewarded();
                break;
            case Platform.Yandex:
                rewardTag = idOrTag;
                Utils.AdReward();
                break;
            case Platform.VK:
                    canShowAd = false;
                    StartCoroutine(CanAdShow());
                    rewardTag = idOrTag;
                    Utils.VK_Rewarded();
                break;
        }
    }
    public void OpenOtherGames() //ССЫЛКА НА ДРУГИЕ ИГРЫ
    {
        switch (Platform)
        {
            case Platform.Editor:
#if INIT_DEBUG
                Debug.Log($"<color={colorDebug}>OPEN OTHER GAMES</color>");
#endif
				break;
            case Platform.Yandex:
                var domain = Utils.GetDomain();
                Application.OpenURL($"https://yandex.{domain}/games/developer?name=" + developerNameYandex);
                break;
            case Platform.VK:
            	rewardTag = "Group";
                Utils.VK_ToGroup();
                break;
        }
    }
    public void RateGameFunc() //ПРОСЬБА ОЦЕНИТЬ ИГРУ
    {
        switch (Platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>REWIEV GAME</color>");
                break;
            case Platform.Yandex:
                Utils.RateGame();
                break;
        }
    }
    public void Save() //СОХРАНЕНИЕ
    {
        string jsonString = "";

        switch (Platform)
        {
            case Platform.Editor:
                EditorSave();
				// В данном случае мне так удобнее. Что бы в тестовых билдах можно было сейвы смотреть
#if INIT_DEBUG
                Debug.Log($"<color={colorDebug}>SAVE</color>");
#endif
				break;
            case Platform.Yandex:
                if (wasLoad == false)
                {
                    break;
				}
				jsonString = JsonUtility.ToJson(PlayerData);
				Utils.SaveExtern(jsonString);
				Debug.Log("Save");
				break;
            case Platform.VK:
                if (wasLoad == false)
                {
                    break;
                }
				jsonString = JsonUtility.ToJson(PlayerData);
				Utils.VK_Save(jsonString);
				break;
        }
    }

    private void EditorSave()
    {
		string directory = Directory.GetCurrentDirectory() + "/save.txt";
		File.WriteAllText(directory, JsonUtility.ToJson(PlayerData, true));
	}

    public void Leaderboard(string leaderboardName, int value) //ЗАНЕСТИ В ЛИДЕРБОРД
    {
        switch (Platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>SET LEADERBOARD:</color> {value}");
                break;
            case Platform.Yandex:
                Utils.SetToLeaderboard(value, leaderboardName);
                break;
            case Platform.VK:
                if (mobile)
                    Utils.VK_OpenLeaderboard(value);
                break;
        }
    }
    public void ToStarGame() //ДОБАВИТЬ В ИЗБРАННОЕ (ВК)
    {
    	switch (Platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>GAME TO STAR</color>");
                break;
            case Platform.Yandex:
                break;
            case Platform.VK:
                Utils.VK_Star();
                break;
        }
    }
    public void ShareGame() //ПОДЕЛИТЬСЯ ИГРОЙ (ВК)
    {
    	switch (Platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>SHARE</color>");
                break;
            case Platform.Yandex:
                break;
            case Platform.VK:
                Utils.VK_Share();
                break;
        }
    }
    public void InvitePlayers() //ПРИГЛАСИТЬ ИГРОКОВ (ВК)
    {
    	switch (Platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>INVITE</color>");
                break;
            case Platform.Yandex:
                break;
            case Platform.VK:
                Utils.VK_Invite();
                break;
        }
    }



    //ДЛЯ РАБОТЫ - ТРОГАТЬ НЕ НАДО
	protected void Awake()
    {
#if UNITY_EDITOR
        Platform = Platform.Editor;
#endif
        //Синглтон
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        //Game Arter
        if (Platform != Platform.GameArter)
        {
            Destroy(gameArterPrefab);
        }
        else
        {
            gameArterPrefab.SetActive(true);
        }

        //Старт площадок
        switch (Platform)
        {
            case Platform.Editor:
                ShowBannerAd();
                StartCoroutine(BannerVK());
                EditorLoad();
                language = "tr"; //ВЫБРАТЬ ЯЗЫК ДЛЯ ТЕСТОВ. ru/en/tr\
                Localization();
                break;
            case Platform.Yandex:
                language = Utils.GetLang();
                if (wasLoad)
                    Utils.LoadExtern();
                Localization();
                ShowInterstitialAd();
                break;
            case Platform.VK:
                language = "ru";
                Localization();
                StartCoroutine(BannerVK());
                StartCoroutine(RewardLoad());
                StartCoroutine(InterLoad());
                if (wasLoad)
                    Utils.VK_Load();
                break;
        }
    }

    public void ItIsMobile()
    {
        mobile = true;
        leaderboardBtn.SetActive(true);
    }

    IEnumerator RewardLoad()
    {
    	yield return new WaitForSeconds(15);
    	switch (Platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>REWARD LOAD</color>");
                break;
            case Platform.VK:
                Utils.VK_AdRewardCheck();
                break;
        }
    }

    IEnumerator InterLoad()
    {
    	while (true)
    	{	
    		yield return new WaitForSeconds(15);
	    	switch (Platform)
	        {
	            case Platform.Editor:
	                Debug.Log($"<color={colorDebug}>INTERSTITIAL LOAD</color>");
	                break;
	            case Platform.VK:
	                Utils.VK_AdInterCheck();
	                break;
	        }
    	}
    }


    IEnumerator BannerVK()
    {
    	yield return new WaitForSeconds(5);
    	ShowBannerAd();
    }

    public void ShowBannerAd() 
    {
        switch (Platform)
        {
            case Platform.Editor:
#if INIT_DEBUG
                Debug.Log($"<color={colorDebug}>BANNER SHOW</color>");
#endif
                break;
            case Platform.VK:
                Utils.VK_Banner();
                break;
        }
    }

    public void StopMusAndGame()
    {
        adOpen = true;
        canShowAd = false;
        StartCoroutine(CanAdShow());
        AudioListener.volume = 0;
        Time.timeScale = 0;
    }

    public void ResumeMusAndGame()
    {
        adOpen = false;
        AudioListener.volume = 1;
        Time.timeScale = 1;
        if (!SoundOn)
            AudioListener.volume = 0;
    }

    public void Localization()
    {

    }

    public void SetPlayerData(string value)
    {
        PlayerData = JsonUtility.FromJson<PlayerData>(value);
    }

    public void Load()
    {
        switch (Platform)
        {
            case Platform.Editor:
                // А тут не было загузки вообще для Editor'а. 
                EditorLoad();
#if INIT_DEBUG
                Debug.Log($"<color={colorDebug}>LOAD</color>");
#endif
				break;
            case Platform.Yandex:
                if (wasLoad)
                {   
                    Debug.Log("LOAD");
                    Utils.LoadExtern();
                }
                break;
        }
    }

    private void EditorLoad()
    {
		try
		{
			string playerDataJson = File.ReadAllText(Directory.GetCurrentDirectory() + "/save.txt");
			PlayerData = JsonUtility.FromJson<PlayerData>(playerDataJson);
		}
		catch
		{
			PlayerData = new();
		}
	}

    void OnApplicationFocus(bool hasFocus)
    {
        Silence(!hasFocus);
    }

    void OnApplicationPause(bool isPaused)
    {
        Silence(isPaused);
    }

    private void Silence(bool silence)
    {
        AudioListener.volume = silence ? 0 : 1;
        Time.timeScale = silence ? 0 : 1;

        if (adOpen)
        {
            Time.timeScale = 0;
            AudioListener.volume = 0;
        }
        if (!SoundOn)
        {
            AudioListener.volume = 0;
        }
    }
    IEnumerator CanAdShow()
    {
        yield return new WaitForSeconds(60);
        canShowAd = true;
    }



    

    //В ДОРАБОТКЕ
    public void RealBuyItem(string idOrTag) //открыть окно покупки
    {
        switch (Platform)
        {
            case Platform.Editor:
                purchasedTag = idOrTag;
                OnPurchasedItem();
                Debug.Log($"<color={colorDebug}>PURCHASE: </color> {purchasedTag}");
                break;
            case Platform.Yandex:
                purchasedTag = idOrTag;
                Utils.BuyItem(idOrTag);
                break;
            case Platform.VK:
                purchasedTag = idOrTag;
                Debug.Log($"<color={colorDebug}>PURCHASE VK</color>");
                break;
        }
    }

    public void CheckBuysOnStart(string idOrTag) //проверить покупки на старте
    {
        Utils.CheckBuyItem(idOrTag);
    }

    private void OnPurchasedItem() //начислить покупку (при удачной оплате)
    {
        if (purchasedTag == "purchasedID")
        {
            
        }
    }

    public void SetPurchasedItem() //начислить уже купленные предметы на старте
    {
        if (purchasedTag == "purchasedID")
        {
            
        }
    }

    public void LeaderboardBtn(int value) //Для кнопки лидерборда в VK
    {
    	//value = playerData.Level;
        switch (Platform)
        {
            case Platform.Editor:
                Debug.Log($"<color={colorDebug}>SET LEADERBOARD:</color> {value}");
                break;
            case Platform.Yandex:
                break;
            case Platform.VK:
                Utils.VK_OpenLeaderboard(value);
                break;
        }
    }
}
