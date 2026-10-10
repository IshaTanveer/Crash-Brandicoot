using System;
using UnityEngine;

public class BreakCrate : MonoBehaviour
{
    CharacterMovementAndAnimationController controller;
    [SerializeField] GameObject wumpaFruitPrefab;
    [SerializeField] GameObject AkuAkuPrefab;
    [SerializeField] GameObject CharacterHeadPrefab;
    [SerializeField] float spawnheight = 0.5f;
    [SerializeField] float wumpaRadius = 0.3f;
    [SerializeField] ParticleSystem breakCrateParticles;
    AudioManager audioManager;
    bool broken = false;
    bool playerinside = false;
    private void Awake() {
        controller = FindFirstObjectByType<CharacterMovementAndAnimationController>();
        audioManager = FindFirstObjectByType<AudioManager>();
    }
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player")
        {
            playerinside = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player")
        {
            playerinside = false;
        }
    }
    void Update()
    {
        BrakeThecrate();
    }
    void BrakeThecrate()
    {
        if(broken) return;
        if(controller.GetIsSpining() && playerinside)
        {
            //play aniamtion
            //play sound
            //play particles
            broken = true;
            if(this.tag == "SpecialWumpaCrate")
            {
                SpawnCollectibles(9, wumpaFruitPrefab);
            }
            else if(this.tag == "SimpleWumpaCrate")
            {
                SpawnCollectibles(9, wumpaFruitPrefab);
            }
            else if(this.tag == "AkuAkuCrate")
            {
                SpawnCollectibles(1, AkuAkuPrefab);
            }
            else if(this.tag == "LifeCrate")
            {
                SpawnCollectibles(1, CharacterHeadPrefab);
            }
            if (breakCrateParticles != null)
            {
                Instantiate(breakCrateParticles, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            }
            audioManager.PlaySounds(AudioManager.Sound.BreakCrate);
            Destroy(this.gameObject);
        }
    }

    private void SpawnCollectibles(int numOfCollectibles, GameObject prefab)
    {
        for(int i = 0; i < numOfCollectibles; i++)
        {
            Vector2 offset = UnityEngine.Random.insideUnitCircle * wumpaRadius;
            Vector3 pos = transform.position + Vector3.up * spawnheight + new Vector3(offset.x, 0f, offset.y);
            Instantiate(prefab, pos, Quaternion.identity);
            Debug.Log("wumpa Spawned");
        }
    }
}
