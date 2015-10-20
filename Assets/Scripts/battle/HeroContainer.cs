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

	public BattleHpBar hpBar;

	public Text hpText;

	public Text comboText;

	[HideInInspector]public NpcCsv npcCsv;

	private float percent;

	[HideInInspector]public float speed;

	[HideInInspector]public int isSilent;

	private int isReflect;

	private int isBlood;

	private float damageFix;

	private int nextHitIndex;
	
	private float nextHitPercent;

	[HideInInspector]public int state;

	private int nextStateIndex;

	private float nextStateTime;

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

		speed = 1;

		SetScale ();

		damageFix = 1;

		isSilent = 0;
		isBlood = 0;
		isReflect = 0;

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

		float percent = (float)hp / npcCsv.hp;

		if (percent < 0) {

			percent = 0;
		}

		hpBar.SetScale (percent);
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

		nextStateTime = 0;
		
		nextStateIndex = 0;

		SetState (csv.type [0]);

		percent = 0;
		
		nextHitIndex = 0;

		SetNextHitPercent ();
	}

	private void SetNextHitPercent(){

		if (nextHitIndex < csv.hitTime.Length) {
			
			nextHitPercent = csv.hitTime [nextHitIndex] / csv.allTime;
		}
	}

	public void BeDamageWithoutFix(int _value){

		SetHp (hp - _value);
	}

	public int BeDamage(int _value){

		int damage = (int)(_value * damageFix);

		BeDamageWithoutFix (damage);

		if (hp < 1) {
			
			battleControl.PauseMove ();
		}

		if (isReflect > 0) {

			return damage;

		} else {

			return 0;
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

	public void SetReflect(bool _b){
		
		isReflect += _b ? 1 : -1;
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

		if (state == -1) {

			nextStateTime = 0;

			nextStateIndex = 0;

		} else {

			nextStateTime += csv.time[nextStateIndex];

			nextStateIndex++;
		}

		if (npcCsv.loseHpWhenFree) {

			if (state == -1) {

				SetLoseHpTime (BattleConstData.LOSEHPWHENFREE_TIME);

			}else {
				
				loseHpValue = 0;
			}
		} 
	}

	public void GetHit(ref float _deltaTime,List<int> _hitReal){

		if(state != -1 && nextHitIndex < csv.hitTime.Length){

			float addPercent = 0;

			float passTime = 0;

			float tmpSpeed = speed;

			float lastAddPercent;

			foreach(BattleBuff buff in buffTimeList){
				
				if(buff.buffTime > _deltaTime){

					break;
				}

				if(buff.csv.speedFix != 1){

					lastAddPercent = addPercent;

					addPercent += (buff.buffTime - passTime) / csv.allTime * tmpSpeed;

					if(percent + addPercent >= nextHitPercent){

						float tmpHitTime = passTime + (nextHitPercent - percent - lastAddPercent) * csv.allTime / tmpSpeed;
						
						if(tmpHitTime < _deltaTime){
							
							_hitReal.Clear();
							
							_hitReal.Add(index);
							
							_deltaTime = tmpHitTime;
							
						}else if(tmpHitTime == _deltaTime){
							
							_hitReal.Add(index);
						}

						return;
					}

					passTime = buff.buffTime;

					tmpSpeed = tmpSpeed / buff.csv.speedFix;
				}
			}

			lastAddPercent = addPercent;

			addPercent += (_deltaTime - passTime) / csv.allTime * tmpSpeed;

			if(percent + addPercent >= nextHitPercent){

				float tmpHitTime = passTime + (nextHitPercent - percent - lastAddPercent) * csv.allTime / tmpSpeed;
				
				if(tmpHitTime < _deltaTime){

					_hitReal.Clear();

					_hitReal.Add(index);

					_deltaTime = tmpHitTime;

				}else if(tmpHitTime == _deltaTime){

					_hitReal.Add(index);
				}
			}
		}
	}

	public void MoveBar(float _deltaTime){

		List<int> delList = new List<int> ();

		float passTime = 0;

		float tmp = 0;

		if (state != -1) {
			
			tmp = nextStateTime / csv.allTime;
		}

		foreach(BattleBuff buff in buffTimeList){
			
			if(buff.buffTime > _deltaTime){

				buff.PassTime(_deltaTime);

			}else{

				delList.Add(buff.csv.ID);

				if (state != -1) {

					float tmpPassTime = buff.buffTime - passTime;

					float addPercent = tmpPassTime / csv.allTime * speed;
					
					percent = percent + addPercent;
					
					bar.Move (tmpPassTime);
					
					hitContainer.Move (tmpPassTime);

					if(percent >= tmp){

						if (nextStateIndex < csv.time.Length) {
							
							SetState (csv.type [nextStateIndex]);

						} else {
							
							SkillOver ();
						}
					}

					passTime = buff.buffTime;
				}

				buff.BuffRemove();
			}
		}

		foreach (int csvID in delList) {

			RemoveBuff (csvID);
		}

		if (state != -1) {

			float tmpPassTime = _deltaTime - passTime;

			float addPercent = tmpPassTime / csv.allTime * speed;

			percent = percent + addPercent;
			
			bar.Move (tmpPassTime);
			
			hitContainer.Move (tmpPassTime);

			if (percent >= tmp) {

				if (nextStateIndex < csv.type.Length) {

					SetState (csv.type [nextStateIndex]);

				} else {
					
					SkillOver ();
				}
			}

		} else {

			if(npcCsv.loseHpWhenFree){

				loseHpTime = loseHpTime - _deltaTime;

				if(loseHpTime <= 0){

					BeDamageWithoutFix((int)(npcCsv.hp * loseHpValue));

					SetLoseHpTime(BattleConstData.LOSEHPWHENFREE_TIME + loseHpTime);
				}
			}
		}
	}

	public void Hit(){

		if (isBlood > 0) {

			BeDamageWithoutFix((int)(hp * BattleConstData.BLOOD_VALUE));
		}

		HitCsv hitCsv = StaticData.GetData<HitCsv> (csv.hitID[nextHitIndex]);

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

		nextHitIndex++;

		SetNextHitPercent ();
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
