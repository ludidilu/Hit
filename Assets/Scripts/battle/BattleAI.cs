using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.csv;

public class BattleAI : BattlePlayerControl{

	protected override void CastSkill(){

		if (UnityEngine.Random.Range (0, heroContainer.npcCsv.level) < 1) {
			
			int skillID = heroContainer.npcCsv.skill [(int)(heroContainer.npcCsv.skill.Length * UnityEngine.Random.value)];
			
			heroContainer.CastSkill (skillID);
		}
	}
}
