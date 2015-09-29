using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.superTween;
using System;
using System.Collections.Generic;
using xy3d.tstd.lib.csv;
using UnityEngine.UI;
using xy3d.tstd.lib.gameObjectFactory;

public class BattleControl : MonoBehaviour{

	public int isMoving = 0;

	private HeroContainer[] barContainer;

	public void Init(GameObject[] _barContainer,int[] _npcID){

		barContainer = new HeroContainer[_barContainer.Length];

		for (int i = 0; i < barContainer.Length; i++) {

			barContainer [i] = _barContainer [i].GetComponent<HeroContainer> ();

			barContainer [i].Init(this,i,_npcID[i]);
		}

		GameObjectFactory.Instance.GetGameObject ("Assets/Prefabs/Buff.prefab", null, false);
	}

	public void StartMove(){

		isMoving++;
	}

	public void PauseMove(){

		isMoving--;
	}

	public void ResumeMove(){

		isMoving++;
	}

	public void SetSpeed(int _index,float _speed){

		barContainer [_index].SetSpeed (_speed);
	}

	void Update(){

		if(Input.GetKeyDown(KeyCode.A)){

			barContainer[0].AddBuff(1,3);
		}

		if(Input.GetKeyDown(KeyCode.B)){
			
			barContainer[0].AddBuff(2,4);
		}

		if (isMoving > 0) {

			float deltaTime = Time.deltaTime;

			List<int> hitReal = new List<int>();
			List<int> hitIndex = new List<int>();
			
			float max = float.MaxValue;

			for(int i = 0 ; i < barContainer.Length ; i++){

				barContainer[i].GetHit(ref deltaTime,ref max,hitReal,hitIndex);
			}

			for(int i = 0 ; i < barContainer.Length ; i++){

				barContainer[i].MoveBar(deltaTime);
			}

			if(hitReal.Count > 0){

				PauseMove();

				SuperTween.Instance.DelayCall(2,ResumeMove);

				for(int i = 0 ; i < hitReal.Count ; i++){

					barContainer[hitReal[i]].Hit(hitIndex[i]);
				}
			}
		}
	}

	private void SkillOver(int _index){

		barContainer [_index].SkillOver ();
	}

	private void HpChange(int _index,int _change){

		barContainer [_index].HpChange (_change);
	}

	public void Hit(int _index,HitCsv _csv){

		for(int m = 0 ; m < barContainer.Length ; m++){

			if(m != _index){

				int damage = (int)(_csv.damage * (1 + barContainer[_index].combo * 0.4f));

				if(barContainer[m].state == 2){

					damage = damage * 2;

					SkillOver(m);
				}

				HpChange(m,-damage);
			}
		}
	}
}
