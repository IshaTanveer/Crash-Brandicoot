using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI livesTMP;
    [SerializeField] TextMeshProUGUI wumpaTMP;
    int wumpaFruits = 0;
    static int lives = 0;
    AudioManager audioManager;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatics(){  lives = 0;}
    private void Awake() {
        audioManager = FindFirstObjectByType<AudioManager>();
    }
    private void Start() {
        UpdateUI(lives, livesTMP);
    }
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Wumpa")
        {
            if(wumpaFruits != 0 && wumpaFruits % 100 == 0)
            {
                increaseLives();
            }
            DestroyCollectible(ref wumpaFruits, other.gameObject, wumpaTMP, AudioManager.Sound.CollectLives);
        }
        if(other.tag == "Head")
        {
            DestroyCollectible(ref lives, other.gameObject, livesTMP, AudioManager.Sound.CollectWumpa);
        }
    }

    void DestroyCollectible(ref int numOfCollectibles, GameObject gameObject, TextMeshProUGUI TMP, AudioManager.Sound collect)
    {
        //play animations
        audioManager.PlaySounds(collect);
        numOfCollectibles += 1;
        UpdateUI(numOfCollectibles, TMP);
        Destroy(gameObject);
    }

    private void UpdateUI(int numOfCollectibles, TextMeshProUGUI TMP)
    {
        TMP.text = numOfCollectibles.ToString();
    }

    public int GetLivesNums()
    {
        return lives;
    }
    public int GetWumpaNums()
    {
        return wumpaFruits;
    }
    public void increaseLives()
    {
        lives = lives + 1;
        UpdateUI(lives, livesTMP);
    }
    public void dcreaseLives()
    {
        if(lives > 0)
        {
            lives = lives - 1;
            UpdateUI(lives, livesTMP);
        }
    }
}
