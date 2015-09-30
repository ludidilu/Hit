using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.csv;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		StaticData.Load<NpcCsv> ("npc");
		StaticData.Load<SkillCsv> ("skill");
		StaticData.Load<HitCsv> ("hit");
		StaticData.Load<BuffCsv> ("buff");

		GameObject g1 = GameObject.Find ("OppContainer");
//
//		g1.GetComponent<Bar> ().Init (1);

		GameObject g2 = GameObject.Find ("MyContainer");

//		g2.GetComponent<Bar> ().Init (2);

		GetComponent<BattleControl> ().Init (new GameObject[]{g1,g2},new int[]{1,2});

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
