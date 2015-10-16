using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using xy3d.tstd.lib.gameObjectFactory;
using System;

public class HeroContainer : MonoBehaviour {

	public BattleControl battleControl;

	public int index;

	public RectTransform maskRectTransform;

	public RectTransform buffRectTransform;

	public BarContainer bar;

	public HitContainer hitContainer;

	public Text hpText;

	public Text comboText;

	[HideInInspector]public NpcCsv npcCsv;

	private float percent;

	[HideInInspector]public float speed;

	[HideInInspector]public int isSilent;

	private int isBlood;

	private int hittedIndex;

	private float damageFix;

	[HideInInspector]public int state;

	private int combo;
	
	private int hp;

	[HideInInspector]public SkillCsv csv;

	private float loseHpTime;

	private float loseHpValue = 0;

	private Dictionary<int, BattleBuff> buffDic = new Dictionary<int, BattleBuff> ();

	private List<GameObject> buffList = new List<GameObject> ();

	private List<BattleBuff> buffTimeList = new List<BattleBuff> ();

	public void Init(int _npcID){

		npcCsv = StaticData.GetData<NpcCsv> (_npcID);

		percent = 0;

		hittedIndex = -1;

		speed = 1;

		SetScale ();

		damageFix = 1;

		SetSilent (false);
		SetBlood (false);
		SetState (-1);

		SetCombo (0);
		SetHp (npcCsv.hp);
	}

	// Use this for initialization
	void Start () {
	
		maskRectTransform.offsetMax = new Vector2((transform as RectTransform).rect.width,0);
		maskRectTransform.offsetMin = new Vector2(0,-(transform as RectTransform).rect.height);
	}

	private void SetCombo(int _value){

		combo = _value;

		comboText.text = combo.ToString ();
	}

	private void SetHp(int _value){

		hp = _value;

		hpText.text = hp.ToString ();
	}

	public void CastSkill(int _skillID){

		if (state == 0) {
			
			if (combo  < BattleConstData.MAX_COMBO) {
				
				SetCombo (combo + 1);
			}
		}

		csv = StaticData.GetData<SkillCsv> (_skillID);

		bar.Init ();

		hitContainer.Init ();

		SetState (csv.type [0]);

		percent = 0;
		
		hittedIndex = -1;
	}

	public void BeDamage(int _value){

		SetHp (hp - (int)(_value * damageFix));

		if (hp < 1) {
			
			battleControl.PauseMove ();
		}
	}

	public void BeInterrupt(){

		if(state == 2){
			
			SkillOver();
		}
	}

	public void SetSpeed(float _speed){

		speed *= _speed;

		SetScale ();
	}

	private void SetScale(){

		bar.SetScale ();

		hitContainer.SetScale ();
	}

	public void SetSilent(bool _b){

		isSilent += _b ? 1 : -1;
	}

	public void SetBlood(bool _b){
		
		isBlood += _b ? 1 : -1;
	}

	public void SetDamageFix(float _value){

		damageFix *= _value;
	}

	private void SkillOver(){
		
		SetState (-1);

		SetCombo (0);

		csv = null;
		
		bar.Clear ();

		hitContainer.Clear ();
	}

	private void SetState(int _state){

		state = _state;

		if (npcCsv.loseHpWhenFree) {

			if (state == -1) {

				SetLoseHpTime (BattleConstData.LOSEHPWHENFREE_TIME);

			}else {
				
				loseHpValue = 0;
			}
		} 
	}

	public void GetHit(ref float _deltaTime,List<int> _hitReal,List<int> _hitIndex){

		if(state != -1){

			float addPercent = _deltaTime / csv.allTime * speed;

//			if(buffTimeList.Count > 0){
//
//				float passTime = 0;
//
//				foreach(BattleBuff buff in buffTimeList){
//
//					bool result = buff.CheckIsOver(ref _deltaTime,ref passTime,ref addPercent);
//
//					if(!result){
//
//						break;
//					}
//				}
//			}

			for(int m = hittedIndex + 1 ; m < csv.hitTime.Length ; m++){
				
				float tmp = csv.hitTime[m] / csv.allTime;
				
				if(percent < tmp){
					
					if(percent + addPercent >= tmp){
						
						float tmpHitTime = (tmp - percent) * csv.allTime / speed;
						
						if(tmpHitTime < _deltaTime){

							_hitReal.Clear();
							_hitIndex.Clear();

							_hitReal.Add(index);
							_hitIndex.Add(m);

							_deltaTime = tmpHitTime;

						}else if(tmpHitTime == _deltaTime){

							_hitReal.Add(index);
							_hitIndex.Add(m);
						}
					}
					
					break;
				}
			}
		}
	}

	public void MoveBar(float _deltaTime){

		List<int> delList = new List<int> ();

		foreach (KeyValuePair<int,BattleBuff> pair in buffDic) {

			bool del = pair.Value.PassTime(ref _deltaTime);

			if(del){

				delList.Add(pair.Key);
			}
		}

		foreach (int csvID in delList) {

			RemoveBuff (csvID);
		}

		if (state != -1) {

			float addPercent = _deltaTime / csv.allTime * speed;
			
			float tt = 0;
			
			bool isOver = false;
			
			for (int m = 0; m < csv.time.Length; m++) {
				
				float tmp = (tt + csv.time [m]) / csv.allTime;
				
				if (percent < tmp) {
					
					if (percent + addPercent >= tmp) {
						
						if (m < csv.time.Length - 1) {

							SetState (csv.type [m + 1]);

						} else {
							
							isOver = true;
						}
					}
					
					break;
				}
				
				tt = tt + csv.time [m];
			}
			
			if (!isOver) {
				
				percent = percent + addPercent;

				bar.Move (_deltaTime);

				hitContainer.Move (_deltaTime);

			} else {
				
				SkillOver ();
			}

		} else {

			if(npcCsv.loseHpWhenFree){

				loseHpTime = loseHpTime - _deltaTime;

				if(loseHpTime <= 0){

					BeDamage((int)(npcCsv.hp * loseHpValue));

					SetLoseHpTime(BattleConstData.LOSEHPWHENFREE_TIME + loseHpTime);
				}
			}
		}
	}

	public void Hit(int _index){

		if (isBlood > 0) {

			BeDamage((int)(hp * BattleConstData.BLOOD_VALUE));
		}

		hittedIndex = _index;

		HitCsv hitCsv = StaticData.GetData<HitCsv> (csv.hitID[hittedIndex]);

		float fix = 1 + (combo * BattleConstData.COMBO_VALUE);

		battleControl.BeDamage (index, (int)(hitCsv.damage * fix));

		if (hitCsv.interrupt) {

			battleControl.BeInterrupt (index);
		}

		for (int i = 0; i < hitCsv.buff.Length; i++) {

			BuffCsv buffCsv = StaticData.GetData<BuffCsv>(hitCsv.buff[i]);

			if(buffCsv.harm){

				battleControl.AddBuff(index,buffCsv.ID,hitCsv.buffTime[i]);

			}else{

				AddBuff(buffCsv.ID,hitCsv.buffTime[i]);
			}
		}
	}

	public void HitOver(){

		hitContainer.HitOver ();
	}

	public void AddBuff(int _buffID,float _buffTime){

		if (buffDic.ContainsKey (_buffID)) {

			BattleBuff buff = buffDic[_buffID];

			buff.AddTime(_buffTime);

		} else {

			GameObject go = GameObjectFactory.Instance.GetGameObject("Assets/Prefabs/Buff.prefab",null,true);

			BattleBuff buff = go.GetComponent<BattleBuff> ();
			
			buff.Init (this, _buffID, _buffTime);

			go.transform.SetParent(buffRectTransform,false);

			if(index == 1){

				(go.transform as RectTransform).anchoredPosition = new Vector2((go.transform as RectTransform).anchoredPosition.x,(1 + buffDic.Count) * (go.transform as RectTransform).rect.height);

			}else{

				(go.transform as RectTransform).anchoredPosition = new Vector2((go.transform as RectTransform).anchoredPosition.x,-buffDic.Count * (go.transform as RectTransform).rect.height);
			}

			buffDic.Add (_buffID, buff);

			buffList.Add (go);

			buffTimeList.Add(buff);
		}

		buffTimeList.Sort (BuffSort.Instance);
	}

	public void RemoveBuff(int _buffID){

		BattleBuff buff = buffDic [_buffID];

		buffDic.Remove (_buffID);

		int tmpIndex = buffList.IndexOf (buff.gameObject);

		buffList.RemoveAt (tmpIndex);

		buffTimeList.Remove (buff);

		for (int i = tmpIndex; i < buffList.Count; i++) {

			RectTransform rt = buffList[i].transform as RectTransform;

			if(index == 1){

				rt.anchoredPosition = new Vector2(rt.anchoredPosition.x,rt.anchoredPosition.y - rt.rect.height);

			}else{

				rt.anchoredPosition = new Vector2(rt.anchoredPosition.x,rt.anchoredPosition.y + rt.rect.height);
			}
		}

		GameObject.Destroy (buff.gameObject);
	}

	private void SetLoseHpTime(float _time){

		loseHpTime = _time;

		loseHpValue = loseHpValue + BattleConstData.LOSEHPWHENFREE_VALUE;
	}
}
