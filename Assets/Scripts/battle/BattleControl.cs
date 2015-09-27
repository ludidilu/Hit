using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.superTween;
using System;

public class BattleControl : MonoBehaviour{

	private bool isMoving;
	private float barContainerWidth;

	private GameObject barContainer1;
	private GameObject barContainer2;

	private GameObject bar1;
	private GameObject bar2;

	private float percent1;
	private float percent2;

	private float speed1 = 1;
	private float speed2 = 1;

	private int hittedIndex1 = -1;
	private int hittedIndex2 = -1;

	private int state1;
	private int state2;

	public void Init(GameObject _barContainer1,GameObject _barContainer2){

		barContainerWidth = (_barContainer1.transform as RectTransform).rect.width;

		barContainer1 = _barContainer1;
		barContainer2 = _barContainer2;

		bar1 = barContainer1.GetComponent<Bar> ().bar;
		bar2 = barContainer2.GetComponent<Bar> ().bar;
	}

	public void StartMove(){

		percent1 = 0;
		percent2 = 0;

		isMoving = true;
	}

	public void PauseMove(){

		isMoving = false;
	}

	public void ResumeMove(){

		isMoving = true;
	}

	public void SetSpeed1(float _speed){

		speed1 = _speed;

		barContainer1.GetComponent<Bar> ().SetScale (speed1);
	}

	public void SetSpeed2(float _speed){
		
		speed2 = _speed;
		
		barContainer2.GetComponent<Bar> ().SetScale (speed2);
	}

	void Update(){

		if (isMoving) {

			float deltaTime = Time.deltaTime;

			int hitIndex1 = -1;
			float hitTime1 = 0;

			int hitIndex2 = -1;
			float hitTime2 = 0;

			if(barContainer1.GetComponent<Bar>().csv != null){

				SkillCsv csv = barContainer1.GetComponent<Bar>().csv;
				
				float addPercent = deltaTime / csv.allTime * speed1;

				for(int i = hittedIndex1 + 1 ; i < csv.hitTime.Length ; i++){

					if(csv.hitTime[i] / csv.allTime > percent1){
						
						if(csv.hitTime[i] / csv.allTime <= percent1 + addPercent){

							hitIndex1 = i;

							hitTime1 = (percent1 + addPercent - csv.hitTime[i] / csv.allTime) * csv.allTime * speed1;

							break;
						}
					}
				}
			}

			if(barContainer2.GetComponent<Bar>().csv != null){
				
				SkillCsv csv = barContainer2.GetComponent<Bar>().csv;
				
				float addPercent = deltaTime / csv.allTime * speed2;
				
				for(int i = hittedIndex2 + 1 ; i < csv.hitTime.Length ; i++){
					
					if(csv.hitTime[i] / csv.allTime > percent2){
						
						if(csv.hitTime[i] / csv.allTime <= percent2 + addPercent){
							
							hitIndex2 = i;
							
							hitTime2 = (percent2 + addPercent - csv.hitTime[i] / csv.allTime) * csv.allTime * speed2;
							
							break;
						}
					}
				}
			}

			if(hitIndex1 != -1 && hitIndex2 != -1){

				if(hitTime1 < hitTime2){

					hitIndex2 = -1;

					SkillCsv csv = barContainer1.GetComponent<Bar>().csv;

					deltaTime = (csv.hitTime[hitIndex1] / csv.allTime - percent1) * csv.allTime * speed1;

				}else if(hitTime1 > hitTime2){

					hitIndex1 = -1;

					SkillCsv csv = barContainer2.GetComponent<Bar>().csv;
					
					deltaTime = (csv.hitTime[hitIndex2] / csv.allTime - percent2) * csv.allTime * speed2;

				}else{

					SkillCsv csv = barContainer1.GetComponent<Bar>().csv;
					
					deltaTime = (csv.hitTime[hitIndex1] / csv.allTime - percent1) * csv.allTime * speed1;
				}

			}else if(hitIndex1 != -1){

				SkillCsv csv = barContainer1.GetComponent<Bar>().csv;
				
				deltaTime = (csv.hitTime[hitIndex1] / csv.allTime - percent1) * csv.allTime * speed1;

			}else if(hitIndex2 != -1){

				SkillCsv csv = barContainer2.GetComponent<Bar>().csv;
				
				deltaTime = (csv.hitTime[hitIndex2] / csv.allTime - percent2) * csv.allTime * speed2;
			}

			if(barContainer1.GetComponent<Bar>().csv != null){

				SkillCsv csv = barContainer1.GetComponent<Bar>().csv;

				float addPercent = deltaTime / csv.allTime * speed1;

				float tt = 0;

				bool isOver = false;

				for(int i = 0 ; i < csv.time.Length ; i++){

					if((tt + csv.time[i]) / csv.allTime > percent1){

						if((tt + csv.time[i]) / csv.allTime <= percent1 + addPercent){

							if(i < csv.time.Length - 1){

								state1 = csv.type[i + 1];

							}else{

								isOver = true;
							}
						}

						break;
					}

					tt = tt + csv.time[i];
				}

				if(!isOver){

					percent1 = percent1 + addPercent;

					(bar1.transform as RectTransform).anchoredPosition = new Vector2((bar1.transform as RectTransform).anchoredPosition.x - deltaTime / Bar.MAX_TIME * barContainerWidth,(bar1.transform as RectTransform).anchoredPosition.y);

				}else{

					hittedIndex1 = -1;

					barContainer1.GetComponent<Bar>().Clear();
				}
			}

			if(barContainer2.GetComponent<Bar>().csv != null){
				
				SkillCsv csv = barContainer2.GetComponent<Bar>().csv;
				
				float addPercent = deltaTime / csv.allTime * speed2;
				
				float tt = 0;
				
				bool isOver = false;
				
				for(int i = 0 ; i < csv.time.Length ; i++){
					
					if((tt + csv.time[i]) / csv.allTime > percent2){
						
						if((tt + csv.time[i]) / csv.allTime <= percent2 + addPercent){
							
							if(i < csv.time.Length - 1){
								
								state2 = csv.type[i + 1];
								
							}else{
								
								isOver = true;
							}
						}
						
						break;
					}
					
					tt = tt + csv.time[i];
				}
				
				if(!isOver){
					
					percent2 = percent2 + addPercent;
					
					(bar2.transform as RectTransform).anchoredPosition = new Vector2((bar2.transform as RectTransform).anchoredPosition.x - deltaTime / Bar.MAX_TIME * barContainerWidth,(bar2.transform as RectTransform).anchoredPosition.y);
					
				}else{

					hittedIndex2 = -1;
					
					barContainer2.GetComponent<Bar>().Clear();
				}
			}

			if(hitIndex1 != -1 && hitIndex2 != -1){

				hittedIndex1 = hitIndex1;
				hittedIndex2 = hitIndex2;

				PauseMove();

				SuperTween.Instance.DelayCall(2,ResumeMove);

			}else if(hitIndex1 != -1){

				hittedIndex1 = hitIndex1;

				PauseMove();
				
				SuperTween.Instance.DelayCall(2,ResumeMove);

			}else if(hitIndex2 != -1){

				hittedIndex2 = hitIndex2;

				PauseMove();
				
				SuperTween.Instance.DelayCall(2,ResumeMove);
			}
		}
	}
}
