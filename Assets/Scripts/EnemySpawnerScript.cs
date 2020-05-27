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
        RuntimeAnimatorController[] NPCAnimationsDup = new RuntimeAnimatorController[NPCAnimations.Count];
        NPCAnimations.CopyTo(NPCAnimationsDup);
        Animator npcAnimator;

        for(int i = 0; i < SpawnPositions.Count; i++){
            GameObject npc = Instantiate(NPCPrefab, SpawnPositions[i].position, transform.rotation);

            if(i < 5){
                int randIndex = Random.Range(0, NPCAnimations.Count);
                if(npc.TryGetComponent<Animator>(out npcAnimator)){
                    npcAnimator.runtimeAnimatorController = NPCAnimations[randIndex];
                }

                NPCAnimations.RemoveAt(randIndex);
            } else {
                int randIndex = Random.Range(0, 5);
                if(npc.TryGetComponent<Animator>(out npcAnimator)){
                    npcAnimator.runtimeAnimatorController = NPCAnimationsDup[randIndex];
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
