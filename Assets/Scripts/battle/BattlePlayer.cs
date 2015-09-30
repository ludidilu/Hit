using UnityEngine;
using System.Collections;

public class BattlePlayer : BattlePlayerControl {

	protected override void CastSkill ()
	{
		if(Input.GetKeyDown(KeyCode.Alpha1)){
			
			heroContainer.CastSkill(heroContainer.npcCsv.skill[0]);
			
		}else if(Input.GetKeyDown(KeyCode.Alpha2)){
			
			heroContainer.CastSkill(heroContainer.npcCsv.skill[1]);
			
		}else if(Input.GetKeyDown(KeyCode.Alpha3)){
			
			heroContainer.CastSkill(heroContainer.npcCsv.skill[2]);
			
		}else if(Input.GetKeyDown(KeyCode.Alpha4)){
			
			heroContainer.CastSkill(heroContainer.npcCsv.skill[3]);
		}
	}
}
