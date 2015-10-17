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

		Application.targetFrameRate = 60;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
