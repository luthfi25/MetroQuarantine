using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    public List<Transform> SpawnPositions;
    public List<RuntimeAnimatorController> NPCAnimations;
    public GameObject NPCPrefab;

    // Start is called before the first frame update
    void Start()
    {
        foreach(RuntimeAnimatorController npcAnim in NPCAnimations){
            int randIndex = Random.Range(0, SpawnPositions.Count);
            GameObject npc = Instantiate(NPCPrefab, SpawnPositions[randIndex].position, transform.rotation);
            
            Animator npcAnimator;
            if(npc.TryGetComponent<Animator>(out npcAnimator)){
                npcAnimator.runtimeAnimatorController = npcAnim;
            }

            SpawnPositions.RemoveAt(randIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
