using UnityEngine;
using System.Collections;

public class BattlePlayerControl : MonoBehaviour{
	
	public HeroContainer heroContainer;
	
	public BattleControl battleControl;
	
	void Update(){
		
		if(heroContainer.isSilent < 1 && battleControl.isMoving > 0 && (heroContainer.state == 0 || heroContainer.state == -1)){
			
			CastSkill();
		}
	}

	protected virtual void CastSkill(){


	}
}
