using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.csv;

public class BattleAI : MonoBehaviour{

	public HeroContainer heroContainer;

	public BattleControl battleControl;

	void Update(){

		if(battleControl.isMoving > 0 && (heroContainer.state == 0 || heroContainer.state == -1)){

			if (UnityEngine.Random.Range (0, heroContainer.npcCsv.level) < 1) {
				
				int skillID = heroContainer.npcCsv.skill[(int)(heroContainer.npcCsv.skill.Length * UnityEngine.Random.value)];

				heroContainer.CastSkill(skillID);
			}
		}
	}
}
