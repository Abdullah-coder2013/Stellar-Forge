using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;


public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Transform playerSpawnPosition;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject firstButton;
    [SerializeField] private TMPro.TMP_Text materials;
    [SerializeField] private TMPro.TMP_Text energy;
    [SerializeField] private Experience experience;
    [SerializeField] private AdsManager adsManager;
    private string newLifeId = "ca-app-pub-7134863660692852/2428370755";
    private GameObject UIController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    private void Awake()
    {
        UIController = GameObject.Find("UIController");
    }
    
    private void Start()
    {
        var player = Instantiate(playerPrefab, playerSpawnPosition.position, playerSpawnPosition.rotation);
        var playerScript = player.GetComponent<Player>();
        playerScript.PlayerDeath += GameOver;
        gameOverPanel.SetActive(false);
    }

    private void GameOver( object sender, System.EventArgs e)
    {
        materials.text = UIController.GetComponent<UIController>().MaterialShower.text;
        energy.text = UIController.GetComponent<UIController>().EnergyShower.text;
        experience.AddExperience(UIController.GetComponent<UIController>().collectedExp);
        UIController.GetComponent<UIController>().DisableInGameUI();   
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);    
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        gameOverPanel.SetActive(false);
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex ) ;
    }

    public void Respawn()
    {
        if (adsManager.ShowRewardedAd(newLifeId))
        {
            UIController.GetComponent<UIController>().EnableInGameUI();
            var player = Instantiate(playerPrefab, playerSpawnPosition.position, playerSpawnPosition.rotation);
            player.GetComponent<Player>().PlayerDeath += GameOver;
            Time.timeScale = 1f;
            gameOverPanel.SetActive(false);
        }
        
    }

    public void MainMenu() {
        var m = 0;
        var e = 0f;
        if (SaveSystem.LoadData() != null) {
            var dat = SaveSystem.LoadData();
            m = (int)dat.material;
            e = dat.energy;
        }
        var exdata = SaveSystem.LoadData();
        var data = new Data(m, e, 0, 1, 0, 100, exdata.money, exdata.oil, exdata.incomeMultiplier);
        if (exdata != null) {
            data = new Data(m+long.Parse(UIController.GetComponent<UIController>().MaterialShower.text), e+float.Parse(UIController.GetComponent<UIController>().EnergyShower.text), experience.totalExperience, experience.currentlevel, experience.previousLevelsExperience, experience.nextLevelsExperience, exdata.money, exdata.oil, exdata.incomeMultiplier);
        }
        else {
            data = new Data(m+=(int)long.Parse(UIController.GetComponent<UIController>().MaterialShower.text), e+=float.Parse(UIController.GetComponent<UIController>().EnergyShower.text), experience.totalExperience, experience.currentlevel, experience.previousLevelsExperience, experience.nextLevelsExperience, 0, 0, 1f);
        }
        

        SaveSystem.SaveData(data);
        Time.timeScale = 1f;
        gameOverPanel.SetActive(false);
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex + 1 ) ;
    }

}
