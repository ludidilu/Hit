using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.csv;

public class BattlePlayerControl : MonoBehaviour{
	
	public HeroContainer heroContainer;
	
	public BattleControl battleControl;
	
	void Update(){
		
		if(!heroContainer.isSilent && battleControl.isMoving > 0 && (heroContainer.state == 0 || heroContainer.state == -1)){
			
			CastSkill();
		}
	}

	protected virtual void CastSkill(){


	}
}
