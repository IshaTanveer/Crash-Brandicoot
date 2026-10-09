using TMPro;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI livesTMP;
    [SerializeField] TextMeshProUGUI wumpaTMP;
    int wumpaFruits = 0, lives = 0;
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Wumpa")
        {
            DestroyCollectible(ref wumpaFruits, other.gameObject, livesTMP);
        }
        if(other.tag == "Head")
        {
            DestroyCollectible(ref lives, other.gameObject, wumpaTMP);
        }
    }
    void DestroyCollectible(ref int numOfCollectibles, GameObject gameObject, TextMeshProUGUI TMP)
    {
        numOfCollectibles += 1;
        TMP.text = numOfCollectibles.ToString();
        Destroy(gameObject);
    }
}
