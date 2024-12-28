using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    public int money;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text enemyNumText;
    [SerializeField] private TMP_Text waveNumText;
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        moneyText.text = "" + money;    
    }

    public void enemyNumChange(int numOfEnemies)
    {
        enemyNumText.text = "" + numOfEnemies;
    }

    public void waveUpdate(int waveNum)
    {
        waveNumText.text = "" + waveNum;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Start_Level");
    }

}
