using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.csv;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		StaticData.Load<SkillCsv> ("skill");

		GameObject g1 = GameObject.Find ("BarContainer1");

		g1.GetComponent<Bar> ().Init (1);

		GameObject g2 = GameObject.Find ("BarContainer2");

		g2.GetComponent<Bar> ().Init (2);

		GetComponent<BattleControl> ().Init (g1, g2);

		GetComponent<BattleControl> ().StartMove ();

		GetComponent<BattleControl> ().SetSpeed1 (0.5f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
