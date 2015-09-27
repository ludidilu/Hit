using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using xy3d.tstd.lib.superTween;
using xy3d.tstd.lib.gameObjectFactory;

namespace xy3d.tstd.lib.battleHeroTools
{
    public class BattleDamageNum
    {


        public const float ASSET_WIDTH = 512.0f / 20;
        public const float ASSET_HEIGHT = 128.0f / 20;

        public const float FONT_WIDTH = 32.0f / 20;
        public const float FONT_HEIGHT = 32.0f / 20;

        public const int damageUnitNum = 80;

        public const int damageGroupNum = 10;

        private const int damageDrawNum = 3;
        
		private BattleDamageNumGroup[,] groupVec;
		private BattleDamageNumGroup[] groupFreeVec;
		private BattleDamageNumGroup[] groupUseVec;
		private BattleDamageNumUnit[,] unitVec;
        private BattleDamageNumUnit[] unitFreeVec;

        private const float flyTime = 1;			//伤害数字飞行时间
		private const float flyHeight = 1;
		private const float largeTime = 0.1f;
		private const float targetScale = 2f;
		private const float small1Time = 0.1f;
		private const float small1Scale = 0.75f;
		private const float waitTimeConst = 0.8f;
		private const float smallTime = 0.1f;
		
		private const float randomRadios = 1f;

        private MeshRenderer[] mrList;

        private Dictionary<BattleDamageNumGroup, int> tweenDic = new Dictionary<BattleDamageNumGroup, int>();
        private int twID;


        private static BattleDamageNum _Instance;

        public static BattleDamageNum Instance
        {

            get
            {

                if (_Instance == null)
                {

                    _Instance = new BattleDamageNum();
                }

                return _Instance;
            }
        }

        public BattleDamageNum()
        {
            Init();
        }


        private void Init()
        {
			
			unitVec = new BattleDamageNumUnit[damageDrawNum,damageUnitNum];
			
			unitFreeVec = new BattleDamageNumUnit[damageDrawNum];
			
			groupVec = new BattleDamageNumGroup[damageDrawNum,damageGroupNum];
			
			groupFreeVec = new BattleDamageNumGroup[damageDrawNum];
			
			groupUseVec = new BattleDamageNumGroup[damageDrawNum];
			
            int i,m;
			for(i = 0 ; i < damageDrawNum ; i++){
				
				for(m = 0 ; m < damageUnitNum ; m++){
					
					unitVec[i, m] = new BattleDamageNumUnit();
					
					if(m > 0){
						
						unitVec[i, m - 1].nextUnit = unitVec[i, m];
					}
				}
				
				unitFreeVec[i] = unitVec[i, 0];
				
				for(m = 0 ; m < damageGroupNum ; m++){
					
					groupVec[i, m] = new BattleDamageNumGroup(i,m);
                    groupVec[i, m].bdm = this;
					
					if(m > 0){
						
						groupVec[i, m - 1].nextGroup = groupVec[i, m];
					}
				}
				
				groupFreeVec[i] = groupVec[i, 0];
			}
			
            mrList = new MeshRenderer[damageDrawNum];

            Action<GameObject> loadGameObject = delegate(GameObject _go)
            {
//                PopUpManager.Instance.AddPopUp(damageGO, PopUp2LayerType.Type_Main);
                mrList[0] = _go.GetComponent<MeshRenderer>();
            };

            GameObjectFactory.Instance.GetGameObject("Assets/PlayGround/BattleTool/DamageNum.prefab", loadGameObject, true);      
		}

        public void Update()
        {
            
            for (int i = 0; i < damageDrawNum; i++)
            {
                if (mrList[i] != null)
                {
                    MeshRenderer tempMR = mrList[i];
                    for (int m = 0; m < damageUnitNum; m++)
                    {
                        BattleDamageNumUnit tempUnit = unitVec[i,m];
                        Vector4 temp = tempUnit.GetFix();
                        tempMR.sharedMaterial.SetVector("fix" + m.ToString(), temp);
                    }

                    for (int n = 0; n < damageGroupNum; n++)
                    {
                        BattleDamageNumGroup tempGroup = groupVec[i, n];
                        tempMR.sharedMaterial.SetVector("positions" + n.ToString(), tempGroup.GetPositionsVec());
                        tempMR.sharedMaterial.SetMatrix("myMatrix" + n.ToString(), tempGroup.GetMatrix());
                    }
                }
            }
        }


        public void addDamageNum(string _str, int _color, Vector3 _pos, bool _randomDirection = false, bool _isFly = false)
        {
            _pos.y = _pos.y + 1.8f;
			int nowDrawIndex = -1;
			
			for(int i = 0 ; i < damageDrawNum ; i++){
				
				if(groupFreeVec[i] != null){
					
					nowDrawIndex = i;
					
					break;
				}
			}
			
			//if(groupUseVec[nowDrawIndex] == null){
                //mrList[nowDrawIndex] = GameObject.Instantiate(damageGO).GetComponent<MeshRenderer>();
			//}
			
			BattleDamageNumGroup unit = groupFreeVec[nowDrawIndex];

            unit.init(_str, _color);
			
			unit.alpha = 1;

			groupFreeVec[nowDrawIndex] = unit.nextGroup;
			
			unit.nextGroup = groupUseVec[nowDrawIndex];
			
			unit.preGroup = null;
			
			groupUseVec[nowDrawIndex] = unit;
			
			if(unit.nextGroup != null){
				
				unit.nextGroup.preGroup = unit;
			}

            if(!_randomDirection){

                unit.pos.x = _pos.x;

                unit.pos.y = _pos.y;

                unit.pos.z = _pos.z;

            }else{

                unit.pos.y = _pos.y +randomRadios - (UnityEngine.Random.value * 2 * randomRadios) ;

                unit.pos.x = _pos.x + randomRadios - (UnityEngine.Random.value * 2 * randomRadios);

                unit.pos.z = _pos.z + randomRadios - (UnityEngine.Random.value * 2 * randomRadios);
            }

            if(!_isFly){
                Action scaleCall = delegate()
                {
                    SuperTween.Instance.Remove(tweenDic[unit]);
                    tweenDic.Remove(unit);
                    Samll1(unit);
                };

                Action<float> setScaleCall = delegate(float value)
                {
                    unit.SetScale(value);
                };
                twID = SuperTween.Instance.To(unit.nowScale, targetScale, largeTime, setScaleCall, scaleCall);
                tweenDic.Add(unit, twID);

            }else{
                 Action moveCall = delegate()
                {
                    SuperTween.Instance.Remove(tweenDic[unit]);
                    tweenDic.Remove(unit);
                    MoveOver(unit);
                };

                Action<float> setMoveCall = delegate(float value)
                {
                    unit.pos.y = value;
                    unit.alpha = 1- (value - _pos.y)/flyHeight;
                };
                twID = SuperTween.Instance.To(unit.pos.y, unit.pos.y + flyHeight, flyTime, setMoveCall, moveCall);
                 tweenDic.Add(unit, twID);
            }
		}

        public void Samll1(BattleDamageNumGroup _unit)
        {
            Action scaleCall = delegate()
            {
                SuperTween.Instance.Remove(tweenDic[_unit]);
                tweenDic.Remove(_unit);
                Small1Over(_unit);
            };

            Action<float> setScaleCall = delegate(float value)
            {
                _unit.SetScale(value);
            };
            twID = SuperTween.Instance.To(_unit.nowScale, small1Scale, small1Time, setScaleCall, scaleCall);
             tweenDic.Add(_unit, twID);
        }

        public void Small1Over(BattleDamageNumGroup _unit)
        {
            Action waitCall = delegate()
            {
                SuperTween.Instance.Remove(tweenDic[_unit]);
                tweenDic.Remove(_unit);
                Small(_unit);
            };
            twID = SuperTween.Instance.DelayCall(waitTimeConst, waitCall);
             tweenDic.Add(_unit, twID);
        }

        public void Small(BattleDamageNumGroup _unit)
        {

            Action scaleCall = delegate()
            {
                SuperTween.Instance.Remove(tweenDic[_unit]);
                tweenDic.Remove(_unit);
                MoveOver(_unit);
            };

            Action<float> setScaleCall = delegate(float value)
            {
                _unit.SetScale(value);
            };
            twID = SuperTween.Instance.To(_unit.nowScale, 0, smallTime, setScaleCall, scaleCall);
             tweenDic.Add(_unit, twID);
        }

        public void MoveOver(BattleDamageNumGroup _unit)
        {
            delDamageGroup(_unit);
        }

        public BattleDamageNumUnit getDamageUnit(BattleDamageNumGroup _group)
        {

            BattleDamageNumUnit unit = unitFreeVec[_group.drawIndex];
			
			unitFreeVec[_group.drawIndex] = unit.nextUnit;
			
			unit.SetGroup(_group.index);
			
			return unit;
		}

        public void delDamageGroup(BattleDamageNumGroup _unit)
        {
			
			_unit.alpha = 0;
			
			for(int i = 0 ; i < _unit.unitVec.Length ; i++){
				
				_unit.unitVec[i].SetGroup(-1);
				
				_unit.unitVec[i].nextUnit = unitFreeVec[_unit.drawIndex];
				
				unitFreeVec[_unit.drawIndex] = _unit.unitVec[i];
			}

            _unit.unitVec = null;
			
			if(_unit.preGroup != null){
				
				_unit.preGroup.nextGroup = _unit.nextGroup;
				
				if(_unit.nextGroup != null){
					
					_unit.nextGroup.preGroup = _unit.preGroup;
				}
				
			}else{
				
				groupUseVec[_unit.drawIndex] = _unit.nextGroup;
				
				if(_unit.nextGroup != null){
					
					_unit.nextGroup.preGroup = null;
				}
			}
			
			_unit.nextGroup = groupFreeVec[_unit.drawIndex];
			
			groupFreeVec[_unit.drawIndex] = _unit;
			
			if(groupUseVec[_unit.drawIndex] == null){
				
				//GameObject.Destroy(meshList[_unit.drawIndex]);
			}
		}
		
		public void clearAll()
        {
			
			for(int i = 0 ; i < damageDrawNum ; i++){
				
				BattleDamageNumGroup tmpGroup = groupUseVec[i];
				
				while(tmpGroup != null){
					
					BattleDamageNumGroup tmpGroup2 = tmpGroup;
					
					tmpGroup = tmpGroup.nextGroup;
					
					delDamageGroup(tmpGroup2);
				}
			}
		}
		
		public void dispose()
        {
			
		}
    }
}
