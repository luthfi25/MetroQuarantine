using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    public List<Transform> SpawnPositions;
    public List<RuntimeAnimatorController> NPCAnimations;
    public GameObject NPCPrefab;
    private List<RuntimeAnimatorController> staticNPCAnimations;

    [SerializeField] private string level;
    [SerializeField] private Dictionary<string, Vector3> initPositions;

    // Start is called before the first frame update
    void Start()
    {
        if(level == "House"){
            initPositions = new Dictionary<string, Vector3>();

            GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject Enemy in Enemies){
                int randPosIndex = Random.Range(0, SpawnPositions.Count);
                Enemy.transform.position = SpawnPositions[randPosIndex].position;
                initPositions.Add(Enemy.name, Enemy.transform.position);

                SpawnPositions.RemoveAt(randPosIndex);
            }
        } else if (level == "RTH"){
            staticNPCAnimations = new List<RuntimeAnimatorController>(NPCAnimations);
            InitNPC();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset(){
        GameObject[] NPCs = GameObject.FindGameObjectsWithTag("Enemy");
        NPCScript nPCScript;

        if(level == "House"){
            foreach(GameObject NPC in NPCs){
                NPC.transform.position = initPositions[NPC.name];
                
                if(NPC.TryGetComponent<NPCScript>(out nPCScript)){
                    nPCScript.Reset();
                }
            }
        } else if (level == "RTH"){
            foreach(GameObject NPC in NPCs){
                Destroy(NPC);
            }

            InitNPC();
        }
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

    public void ForceFreeze(){
        GameObject[] NPCs = GameObject.FindGameObjectsWithTag("Enemy");
        NPCScript nPCScript;

        foreach(GameObject NPC in NPCs){
            if(NPC.TryGetComponent<NPCScript>(out nPCScript)){
                nPCScript.SetFreeze(true);
            }
        }
    }

    public void UnFreeze(){
        GameObject[] NPCs = GameObject.FindGameObjectsWithTag("Enemy");
        NPCScript nPCScript;

        foreach(GameObject NPC in NPCs){
            if(NPC.TryGetComponent<NPCScript>(out nPCScript)){
                nPCScript.SetFreeze(false);
            }
        }
    }
}
