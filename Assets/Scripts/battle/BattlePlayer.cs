using UnityEngine;
using System.Collections;

public class BattlePlayer : MonoBehaviour {

	public HeroContainer heroContainer;
	
	public BattleControl battleControl;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
	
		if(battleControl.isMoving > 0 && (heroContainer.state == 0 || heroContainer.state == -1)){
			
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
}
