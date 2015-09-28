using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.superTween;
using System;
using System.Collections.Generic;
using xy3d.tstd.lib.csv;
using UnityEngine.UI;

public class BattleControl : MonoBehaviour{

	private static int MAX_HP = 2000;

	private static int MAX_COMBO = 5;

	private int isMoving = 0;

	private float barContainerWidth;

	private NpcCsv[] npcCsv;

	private Bar[] barContainer;

	private Text[] hpText;

	private Text[] comboText;

	private float[] percent;

	private float[] speed;

	private int[] hittedIndex;

	private int[] state;

	private int[] combo;

	private int[] hp;

	public void Init(GameObject[] _barContainer,GameObject[] _hpText,GameObject[] _comboText,int[] _npcID){

		barContainerWidth = (_barContainer[0].transform as RectTransform).rect.width;

		barContainer = new Bar[_barContainer.Length];
		
		npcCsv = new NpcCsv[barContainer.Length];

		hpText = new Text[barContainer.Length];

		comboText = new Text[barContainer.Length];

		percent = new float[barContainer.Length];
		
		speed = new float[barContainer.Length];

		hittedIndex = new int[barContainer.Length];

		state = new int[barContainer.Length];

		combo = new int[barContainer.Length];

		hp = new int[barContainer.Length];

		for (int i = 0; i < barContainer.Length; i++) {

			barContainer [i] = _barContainer [i].GetComponent<Bar> ();

			hpText[i] = _hpText[i].GetComponent<Text>();

			hpText[i].text = MAX_HP.ToString();

			comboText[i] = _comboText[i].GetComponent<Text>();

			comboText[i].text = "0";

			percent[i] = 0;

			speed[i] = 1;

			hittedIndex[i] = -1;

			state[i] = -1;

			npcCsv[i] = StaticData.GetData<NpcCsv>(_npcID[i]);

			combo[i] = 0;

			hp[i] = npcCsv[i].hp;
		}
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

		speed [_index] = _speed;

		barContainer [_index].SetScale (_speed);
	}

	public void CastSkill(int _index,int _skillID){

		if (state [_index] == 0) {
			
			if (combo [_index] < MAX_COMBO) {
				
				ComboChange (_index, combo [_index] + 1);
			}
		}

		barContainer [_index].Init (_skillID);

		state [_index] = barContainer [_index].csv.type [0];

		percent [_index] = 0;

		hittedIndex [_index] = -1;
	}

	void Update(){

		if (isMoving > 0) {

			float deltaTime = Time.deltaTime;

			List<int> hitReal = new List<int>();
			List<int> hitIndex = new List<int>();
			
			float max = float.MaxValue;

			for(int i = 0 ; i < barContainer.Length ; i++){

				if(state[i] != -1){
					
					SkillCsv csv = barContainer[i].csv;
					
					float addPercent = deltaTime / csv.allTime * speed[i];
					
					for(int m = hittedIndex[i] + 1 ; m < csv.hitTime.Length ; m++){

						float tmp = csv.hitTime[m] / csv.allTime;

						if(tmp > percent[i]){
							
							if(tmp <= percent[i] + addPercent){

								float tmpHitTime = (percent[i] + addPercent - csv.hitTime[m] / csv.allTime) * csv.allTime * speed[i];

								if(tmpHitTime < max){
									
									hitReal.Clear();

									hitIndex.Clear();
									
									hitReal.Add(i);

									hitIndex.Add(m);
									
									max = tmpHitTime;
									
								}else if(tmpHitTime == max){
									
									hitReal.Add(i);

									hitIndex.Add(m);
								}
							}

							break;
						}
					}
				}
			}

			if(hitReal.Count > 0){

				SkillCsv csv = barContainer[hitReal[0]].csv;
				
				deltaTime = (csv.hitTime[hitIndex[0]] / csv.allTime - percent[hitReal[0]]) * csv.allTime * speed[hitReal[0]];
			}

			for(int i = 0 ; i < barContainer.Length ; i++){

				if(state[i] != -1){
					
					SkillCsv csv = barContainer[i].csv;
					
					float addPercent = deltaTime / csv.allTime * speed[i];
					
					float tt = 0;
					
					bool isOver = false;
					
					for(int m = 0 ; m < csv.time.Length ; m++){

						float tmp = (tt + csv.time[m]) / csv.allTime;

						if(tmp >= percent[i]){
							
							if(tmp < percent[i] + addPercent){
								
								if(m < csv.time.Length - 1){

									state[i] = csv.type[m + 1];

								}else{
									
									isOver = true;
								}
							}
							
							break;
						}
						
						tt = tt + csv.time[m];
					}

					if(!isOver){
						
						percent[i] = percent[i] + addPercent;
						
						(barContainer[i].bar.transform as RectTransform).anchoredPosition = new Vector2((barContainer[i].bar.transform as RectTransform).anchoredPosition.x - deltaTime / Bar.MAX_TIME * barContainerWidth,(barContainer[i].bar.transform as RectTransform).anchoredPosition.y);
						
					}else{

						SkillOver(i);
					}
				}
			}

			if(hitReal.Count > 0){

				PauseMove();

				SuperTween.Instance.DelayCall(2,ResumeMove);

				for(int i = 0 ; i < hitReal.Count ; i++){

					SkillCsv csv = barContainer[hitReal[i]].csv;

					hittedIndex[hitReal[i]] = hitIndex[i];

					Hit(hitReal[i],csv.hitID[hitIndex[i]]);
				}
			}

			if(state[1] == 0 || state[1] == -1){

				if(Input.GetKeyDown(KeyCode.Alpha1)){

					CastSkill(1,npcCsv[1].skill[0]);

				}else if(Input.GetKeyDown(KeyCode.Alpha2)){
					
					CastSkill(1,npcCsv[1].skill[1]);
					
				}else if(Input.GetKeyDown(KeyCode.Alpha3)){
					
					CastSkill(1,npcCsv[1].skill[2]);
					
				}else if(Input.GetKeyDown(KeyCode.Alpha4)){
					
					CastSkill(1,npcCsv[1].skill[3]);
				}
			}

			if(state[0] == 0 || state[0] == -1){

				int skillID = AiCastSkill(0);

				if(skillID != -1){

					CastSkill(0,skillID);
				}
			}
		}
	}

	private void SkillOver(int _index){

		state[_index] = -1;
		
		barContainer[_index].Clear();
		
		ComboChange (_index, 0);
	}

	private void HpChange(int _index,int _change){

		hp [_index] = hp [_index] + _change;

		hpText [_index].text = hp [_index].ToString ();

		
		if(hp[_index] < 1){
			
			PauseMove();
		}
	}

	private void ComboChange(int _index,int _change){

		combo [_index] = _change;
		
		comboText [_index].text = combo [_index].ToString ();
	}

	private void Hit(int _index,int _hitID){

		HitCsv csv = StaticData.GetData<HitCsv> (_hitID);

		for(int i = 0 ; i < csv.type.Length ; i++){

			switch(csv.type[i]){

			case 0:

				for(int m = 0 ; m < barContainer.Length ; m++){

					if(m != _index){

						int damage = (int)(csv.data[i] * (1 + combo[_index] * 0.4f));

						if(state[m] == 2){

							damage = damage * 2;

							SkillOver(m);
						}

						HpChange(m,-damage);
					}
				}

				break;
			}
		}
	}

	public int AiCastSkill(int _index){
		
		if (UnityEngine.Random.Range (0, npcCsv[_index].level) < 1) {
			
			return npcCsv[_index].skill [(int)(npcCsv[_index].skill.Length * UnityEngine.Random.value)];
			
		} else {
			
			return -1;
		}
	}
}
