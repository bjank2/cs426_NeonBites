using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance {get; private set; }
    public int Money { get; private set; } = 200;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    public void AddMoney(int amount)
    {
        Money += amount;
    }
}
