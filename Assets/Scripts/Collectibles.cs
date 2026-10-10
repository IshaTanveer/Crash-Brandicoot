using TMPro;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI livesTMP;
    [SerializeField] TextMeshProUGUI wumpaTMP;
    int wumpaFruits = 0;
    static int lives = 0;
    AudioManager audioManager;
    private void Awake() {
        audioManager = FindFirstObjectByType<AudioManager>();
    }
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Wumpa")
        {
            if(wumpaFruits % 100 == 0)
            {
                increaseLives();
            }
            DestroyCollectible(ref wumpaFruits, other.gameObject, livesTMP);
        }
        if(other.tag == "Head")
        {
            DestroyCollectible(ref lives, other.gameObject, wumpaTMP);
        }
    }
    void DestroyCollectible(ref int numOfCollectibles, GameObject gameObject, TextMeshProUGUI TMP)
    {
        //play animations
        audioManager.PlaySounds(AudioManager.Sound.Collect);
        numOfCollectibles += 1;
        TMP.text = numOfCollectibles.ToString();
        Destroy(gameObject);
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
    }
    public void dcreaseLives()
    {
        if(lives > 0)
        {
            lives = lives - 1;
        }
    }
}
