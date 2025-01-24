using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    public int money = 500;
    public int health = 100;
    [SerializeField] private Text _healthText, _waveNumText, _statusText, _lifeStatusText;
    [SerializeField] private TMP_Text _moneyText;
    [SerializeField] private Image[] _hUD;
    [SerializeField] private GameObject _gatlingUpgradePopUp, _missilePopUp, _status, _endStatus;

    // Update is called once per frame
    void Update()
    {
        _moneyText.text = "" + money;
        _healthText.text = "" + health;
        UIHealth();
    }
    
    public void DecreaseUiHP()
    {
        health -= 10;
    }

    public void UpgradeGatlingPopUp(bool Active)
    {
        _gatlingUpgradePopUp.SetActive(Active);
    }

    public void StatusPopUp(bool Active)
    {
        _status.SetActive(Active);
        _statusText.text = "Prepare for the Next Wave!";
    }

    public void UpgradeMissilePopUp(bool Active)
    {
        _missilePopUp.SetActive(Active);
    }

    private void UIHealth()
    {
        if (health < 90 && health >= 40) 
        {
            foreach (Image UI in _hUD)
            {
                UI.color = Color.yellow;
                _lifeStatusText.text = "Damaged";
            }
        }

        if(health <= 30)
        {
            foreach(Image UI in _hUD)
            {
                UI.color = Color.red;
                _lifeStatusText.text = "DANGER";
            }
        }
        
        if(health < 1)
        {
            StartCoroutine(ResetScene());
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void ResumeOrDoubleSpeed(int scale)
    {
        Time.timeScale = scale;
    }

    public void waveUpdate(int waveNum)
    {
        if(waveNum <= 10)
        {
            _waveNumText.text = "" + waveNum;
        }

        if(waveNum > 10)
        {
            _endStatus.SetActive(true);
        }
        
        StartCoroutine(WaveStatus(waveNum));
    }

    public void Restart()
    {
        SceneManager.LoadScene("Start_Level");
    }

    IEnumerator WaveStatus(int waveNum)
    {
        if(waveNum <= 10)
        {
            _status.SetActive(true);
            _statusText.text = "Next Wave Incoming!";
            yield return new WaitForSeconds(3.0f);
            _status.SetActive(false);
        }
    }

    IEnumerator ResetScene() 
    {
        _status.SetActive(true);
        _statusText.text = "Failed to repel enemies...";
        yield return new WaitForSeconds(3);
        Restart();
    }
}
