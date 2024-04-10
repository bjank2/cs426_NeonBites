using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance {get; private set; }
    public int Money { get; private set; } = 200;
    public GameObject moneyChangePrefab;
    public Transform moneyChangeParent;

    public AudioClip moneySound;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void AddMoney(int amount)
    {
        Money += amount;
        ShowMoneyChange(amount);

        if(moneySound != null && audioSource !=null) audioSource.PlayOneShot(moneySound);
    }

    void ShowMoneyChange(int amount) {
        if (moneyChangePrefab != null && moneyChangeParent != null) {
            GameObject moneyChange = Instantiate(moneyChangePrefab, moneyChangeParent);
            MoneyAnim moneyAnim = moneyChange.GetComponent<MoneyAnim>();
            moneyAnim.Initialize(amount);
        }
    }
}
