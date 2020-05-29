using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    public List<Transform> SpawnPositions;
    public List<RuntimeAnimatorController> NPCAnimations;
    public GameObject NPCPrefab;
    private List<RuntimeAnimatorController> staticNPCAnimations;

    // Start is called before the first frame update
    void Start()
    {
        staticNPCAnimations = new List<RuntimeAnimatorController>(NPCAnimations);
        InitNPC();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset(){
        GameObject[] NPCs = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject NPC in NPCs){
            Destroy(NPC);
        }

        InitNPC();
    }

    void InitNPC(){
        NPCScript nPCScript;

        for(int i = 0; i < SpawnPositions.Count; i++){
            GameObject npc = Instantiate(NPCPrefab, SpawnPositions[i].position, transform.rotation);

            if(i < 5){
                int randIndex = Random.Range(0, NPCAnimations.Count);
                if(npc.TryGetComponent<NPCScript>(out nPCScript)){
                    nPCScript.InitAnimator(NPCAnimations[randIndex]);
                }

                NPCAnimations.RemoveAt(randIndex);
            } else {
                int randIndex = Random.Range(0, 5);
                if(npc.TryGetComponent<NPCScript>(out nPCScript)){
                    nPCScript.InitAnimator(staticNPCAnimations[randIndex]);
                }
            }
        }

        NPCAnimations = new List<RuntimeAnimatorController>(staticNPCAnimations);
    }
}
