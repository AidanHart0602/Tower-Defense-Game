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
    [SerializeField] private Text _healthText;
    [SerializeField] private TMP_Text _moneyText;
    [SerializeField] private Text _waveNumText;
    [SerializeField] private Image[] _hUD;
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

    private void UIHealth()
    {
        if (health < 100 && health >= 40) 
        {
            foreach (Image UI in _hUD)
            {
                UI.color = Color.yellow;
            }
        }

        if(health <= 30)
        {
            foreach(Image UI in _hUD)
            {
                UI.color = Color.red;
            }
        }
    }

    public void waveUpdate(int waveNum)
    {
        _waveNumText.text = "" + waveNum;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Start_Level");
    }

}
