using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.superTween;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using xy3d.tstd.lib.gameObjectFactory;
using xy3d.tstd.lib.textureFactory;

public class BattleControl : MonoBehaviour{

	[HideInInspector] public int isMoving = 0;

	public HeroContainer[] heroContainer;

	public void Init(int[] _npcID){

		for (int i = 0; i < heroContainer.Length; i++) {

			heroContainer [i].Init(_npcID[i]);
		}

		GameObjectFactory.Instance.GetGameObject ("Assets/Prefabs/Buff.prefab", null, false);

		GameObjectFactory.Instance.GetGameObject ("Assets/Prefabs/Bar.prefab", null, false);

		GameObjectFactory.Instance.GetGameObject ("Assets/Prefabs/Hit.prefab", null, false);

		TextureFactory.Instance.GetTexture<Sprite> ("Assets/Textures/cangtianyudijian.png", null, false);

		TextureFactory.Instance.GetTexture<Sprite> ("Assets/Textures/binghuangyanfengzhang.png", null, false);
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

	void Update(){

		if (isMoving > 0) {

			float deltaTime = Time.deltaTime;

			List<int> hitReal = new List<int>();

			for(int i = 0 ; i < heroContainer.Length ; i++){

				heroContainer[i].GetHit(ref deltaTime,hitReal);
			}

			for(int i = 0 ; i < heroContainer.Length ; i++){

				heroContainer[i].MoveBar(deltaTime);
			}

			if(hitReal.Count > 0){

				PauseMove();

				for(int i = 0 ; i < hitReal.Count ; i++){

					heroContainer[hitReal[i]].Hit();
				}

				Action callBack = delegate() {

					for(int i = 0 ; i < hitReal.Count ; i++){
						
						heroContainer[hitReal[i]].HitOver();
					}

					ResumeMove();
				};
				
				SuperTween.Instance.DelayCall(2,callBack);
			}
		}
	}

	public void BeDamage(int _index,int _damage){

		for(int m = 0 ; m < heroContainer.Length ; m++){

			if(m != _index){

				heroContainer [m].BeDamage(_damage);
			}
		}
	}

	public void BeInterrupt(int _index){
		
		for(int m = 0 ; m < heroContainer.Length ; m++){
			
			if(m != _index){
				
				heroContainer [m].BeInterrupt();
			}
		}
	}

	public void AddBuff(int _index,int _buffID,float _buffTime){

		for(int m = 0 ; m < heroContainer.Length ; m++){
			
			if(m != _index){
				
				heroContainer [m].AddBuff(_buffID,_buffTime);
			}
		}
	}
}
