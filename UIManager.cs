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

    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        moneyText.text = "" + money;    
    }

    public void Restart()
    {
        SceneManager.LoadScene("Start_Level");
    }

}
