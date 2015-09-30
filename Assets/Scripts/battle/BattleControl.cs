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

			barContainer[1].AddBuff(1,3);
		}

		if(Input.GetKeyDown(KeyCode.B)){
			
			barContainer[1].AddBuff(2,4);
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

	public void BeDamage(int _index,int _damage){

		for(int m = 0 ; m < barContainer.Length ; m++){

			if(m != _index){

				barContainer [m].BeDamage(_damage);
			}
		}
	}

	public void AddBuff(int _index,int _buffID,float _buffTime){

		for(int m = 0 ; m < barContainer.Length ; m++){
			
			if(m != _index){
				
				barContainer [m].AddBuff(_buffID,_buffTime);
			}
		}
	}
}
