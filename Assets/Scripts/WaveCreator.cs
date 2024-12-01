using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class WaveCreator : MonoBehaviour
{
    [SerializeField] private GameObject WavePanel;
    [SerializeField] private TMP_Text WaveText;
    [SerializeField] private WaveSpawner waveSpawner;
    [SerializeField] private GameObject firstButton;
    public int wave = 1;
    // Start is called before the first frame update
    void Start()
    {
        var waveScript = waveSpawner.GetComponent<WaveSpawner>();
        waveScript.onWaveEnd += StartWave;
        WavePanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
        Time.timeScale = 0f;
        WaveText.text = "Wave " + wave.ToString();
        
    }

    private void StartWave(object sender, System.EventArgs e) {
        Time.timeScale = 0f;
        WavePanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    // Update is called once per frame
    public void StartWave() {
        WavePanel.SetActive(false);
        Time.timeScale = 1f;
        waveSpawner.SpawnWave();
        wave += 1;
        waveSpawner.asteroidsPerWave += 5;
        WaveText.text = "Wave " + wave.ToString();
    }

}
