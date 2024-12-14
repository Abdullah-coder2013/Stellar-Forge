using TMPro;
using UnityEngine.EventSystems;
using UnityEngine;

[System.Serializable]
public class UIController : MonoBehaviour
{
    public TMP_Text MaterialShower;
    public TMP_Text EnergyShower;
    public TMP_Text XpShower;
    public int collectedExp;
    [SerializeField] private Canvas inGameUI;
    [SerializeField] private GameObject PauseUI;
    [SerializeField] private GameObject firstButton;

    private void Start(){
        MaterialShower.text = "0";
        EnergyShower.text = "0";
        XpShower.text = "0";
        PauseUI.gameObject.SetActive(false);

    }
    private int cInt(string text){
        return int.Parse(text);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        PauseUI.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        PauseUI.gameObject.SetActive(false);
    }

    public void ToBuild()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    public void UpdateMaterial(int amount){
        
        if (MaterialShower != null) {
        
        MaterialShower.text = (cInt(MaterialShower.text) + amount).ToString();
    } else {
        Debug.LogError("MaterialShower is null!");
    }
    }
    public void UpdateEnergy(float amount){
        if (SaveSystem.LoadUpgrade("Energy Increase") != null) {
            amount += float.Parse(SaveSystem.LoadUpgrade("Energy").level+1.5f);
        }
        var floatEnergy = float.Parse(EnergyShower.text);
        EnergyShower.text = (floatEnergy + amount).ToString();
    }

    public void UpdateExp(int amount)
    {
        collectedExp += amount;
        if (XpShower != null)
        {
            XpShower.text = (cInt(XpShower.text) + amount).ToString();
        }
        else
        {
            print("XpShower is null!");
        }
         
    }
    public void DisableInGameUI(){
        inGameUI.gameObject.SetActive(false);
    }
    public void EnableInGameUI(){
        inGameUI.gameObject.SetActive(true);
    }
}
