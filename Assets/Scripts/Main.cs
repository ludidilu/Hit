using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		StaticData.Load<NpcCsv> ("npc");
		StaticData.Load<SkillCsv> ("skill");
		StaticData.Load<HitCsv> ("hit");
		StaticData.Load<BuffCsv> ("buff");

		GetComponent<BattleControl> ().Init (new int[]{1,2});

		GetComponent<BattleControl> ().StartMove ();

//		GetComponent<BattleControl> ().SetSpeed (0,0.5f);

//		GetComponent<BattleControl> ().CastSkill (0, 1);
//
//		GetComponent<BattleControl> ().CastSkill (1, 2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
