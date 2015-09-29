using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.csv;
using UnityEngine.UI;
using System.Collections.Generic;
using xy3d.tstd.lib.gameObjectFactory;
using System;

public class HeroContainer : MonoBehaviour {

	private BattleControl battleControl;

	private int index;

	public GameObject maskContainer;

	public GameObject buffContainer;

	public BarContainer bar;

	public Text hpText;

	public Text comboText;

	[HideInInspector]public NpcCsv npcCsv;

	private float percent;

	private float speed;

	private int hittedIndex;

	[HideInInspector]public int state;

	[HideInInspector]public int combo;
	
	private int hp;

	[HideInInspector]public SkillCsv csv;

	private Dictionary<int, BattleBuff> buffDic = new Dictionary<int, BattleBuff> ();

	private List<GameObject> buffList = new List<GameObject> ();

	public void Init(BattleControl _battleControl,int _index,int _npcID){

		index = _index;

		battleControl = _battleControl;

		npcCsv = StaticData.GetData<NpcCsv> (_npcID);

		percent = 0;
		speed = 1;
		hittedIndex = -1;
		state = -1;
		
		SetCombo (0);
		SetHp (npcCsv.hp);
	}

	// Use this for initialization
	void Start () {
	
		(maskContainer.transform as RectTransform).offsetMax = new Vector2((transform as RectTransform).rect.width,0);
		(maskContainer.transform as RectTransform).offsetMin = new Vector2(0,-(transform as RectTransform).rect.height);

		(bar.transform as RectTransform).offsetMax = new Vector2((transform as RectTransform).rect.width,0);
		(bar.transform as RectTransform).offsetMin = new Vector2(0,-(transform as RectTransform).rect.height);
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

		state = csv.type [0];
		
		percent = 0;
		
		hittedIndex = -1;
	}

	public void HpChange(int _value){

		SetHp (hp + _value);

		if(hp < 1){
			
			battleControl.PauseMove();
		}
	}

	public void SetSpeed(float _speed){

		speed = _speed;

		bar.SetScale (1 / _speed);
	}

	public void SkillOver(){
		
		state = -1;

		SetCombo (0);

		csv = null;
		
		bar.Clear ();
	}

	public void GetHit(ref float _deltaTime,ref float _max,List<int> _hitReal,List<int> _hitIndex){

		if(state != -1){

			float addPercent = _deltaTime / csv.allTime * speed;
			
			for(int m = hittedIndex + 1 ; m < csv.hitTime.Length ; m++){
				
				float tmp = csv.hitTime[m] / csv.allTime;
				
				if(tmp > percent){
					
					if(tmp <= percent + addPercent){
						
						float tmpHitTime = (percent + addPercent - tmp) * csv.allTime * speed;
						
						if(tmpHitTime < _max){

							_hitReal.Clear();
							_hitIndex.Clear();

							_hitReal.Add(index);
							_hitIndex.Add(m);

							_max = tmpHitTime;

							_deltaTime = (tmp - percent) * csv.allTime * speed;
							
						}else if(tmpHitTime == _max){

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

		if(state != -1){

			float addPercent = _deltaTime / csv.allTime * speed;
			
			float tt = 0;
			
			bool isOver = false;
			
			for(int m = 0 ; m < csv.time.Length ; m++){
				
				float tmp = (tt + csv.time[m]) / csv.allTime;
				
				if(tmp >= percent){
					
					if(tmp < percent + addPercent){
						
						if(m < csv.time.Length - 1){
							
							state = csv.type[m + 1];
							
						}else{
							
							isOver = true;
						}
					}
					
					break;
				}
				
				tt = tt + csv.time[m];
			}
			
			if(!isOver){
				
				percent = percent + addPercent;
				
				(bar.transform as RectTransform).anchoredPosition = new Vector2((bar.transform as RectTransform).anchoredPosition.x - _deltaTime / BattleConstData.MAX_TIME * (transform as RectTransform).rect.width,(bar.transform as RectTransform).anchoredPosition.y);
				
			}else{
				
				SkillOver();
			}
		}
	}

	public void Hit(int _index){

		hittedIndex = _index;

		HitCsv hitCsv = StaticData.GetData<HitCsv> (csv.hitID[hittedIndex]);
		
		battleControl.Hit (index, hitCsv);
	}

	public void AddBuff(int _buffID,float _buffTime){

		if (buffDic.ContainsKey (_buffID)) {

			BattleBuff buff = buffDic[_buffID];

			buff.AddTime(_buffTime);

		} else {

			GameObject go = GameObjectFactory.Instance.GetGameObject("Assets/Prefabs/Buff.prefab",null,true);

			BattleBuff buff = go.GetComponent<BattleBuff> ();
			
			buff.Init (this, _buffID, _buffTime);

			go.transform.SetParent(buffContainer.transform,false);

			(go.transform as RectTransform).anchoredPosition = new Vector2((go.transform as RectTransform).anchoredPosition.x,-buffDic.Count * (go.transform as RectTransform).rect.height);

			buffDic.Add (_buffID, buff);

			buffList.Add (go);
		}
	}

	private void RemoveBuff(int _buffID){

		GameObject go = buffDic [_buffID].gameObject;

		buffDic.Remove (_buffID);

		int tmpIndex = buffList.IndexOf (go);

		buffList.RemoveAt (tmpIndex);

		for (int i = tmpIndex; i < buffList.Count; i++) {

			(buffList[i].transform as RectTransform).anchoredPosition = new Vector2((buffList[i].transform as RectTransform).anchoredPosition.x,(buffList[i].transform as RectTransform).anchoredPosition.y + (buffList[i].transform as RectTransform).rect.height);
		}

		GameObject.Destroy (go);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
