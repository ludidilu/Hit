using System;
using System.Collections.Generic;
using UnityEngine;
using xy3d.tstd.lib.gameObjectFactory;
using xy3d.tstd.lib.superTween;
using xy3d.tstd.lib.textureFactory;

namespace xy3d.tstd.lib.battleHeroTools
{
    public class BattleSkillIcon
    {
        public const float ASSET_WIDTH = 512f / 128f;
        public const float ASSET_HEIGHT = 512f / 128f;

        public const float FONT_WIDTH = 128f / 128f;
        public const float FONT_HEIGHT = 64f / 128f;
		
		public const int unitNum = 40;
		
		private BattleSkillIconUnit[] unitVec;

        private Material mat;

        private MeshRenderer mr;

        private GameObject skillIconGO;
        private GameObject container;

        private static BattleSkillIcon _Instance;

        public static BattleSkillIcon Instance
        {

            get
            {

                if (_Instance == null)
                {

                    _Instance = new BattleSkillIcon();
                }

                return _Instance;
            }
        }
		
		public  BattleSkillIcon(){
		}

		public void Init(GameObject con){

            container = con;

            if (skillIconGO)
            {
                skillIconGO.SetActive(true);
                return;
            }
			
			unitVec = new BattleSkillIconUnit[unitNum];
			
			
			for(int i = 0 ; i < unitNum ; i++){
				
                unitVec[i] = new BattleSkillIconUnit();
			}

            Action<GameObject> loadGameObject = delegate(GameObject _go)
            {
                skillIconGO = _go;
                skillIconGO.transform.SetParent(container.transform, false);
                mr = _go.GetComponent<MeshRenderer>();
                mat = mr.material;
            };

            GameObjectFactory.Instance.GetGameObject("Assets/Arts/battle/BattleTool/SkillIcon.prefab", loadGameObject, true);       
		}

        public void Update()
        {
			if(mr != null){

	            for (int i = 0; i < unitNum; i++)
	            {
	                BattleSkillIconUnit unit = unitVec[i];
	                if (unit.State == 1 || unit.IsChange)
	                {
	                    if (unit.IsChange) unit.IsChange = false;
	                    Vector4 pos = unit.GetPositionsVec();
	                    mr.material.SetVector("positions" + i.ToString(), pos);

						mr.material.SetVector("fix" + i.ToString(), unit.GetFixVec());
						mr.material.SetMatrix("myMatrix" + i.ToString(), unit.GetMatrix());
	                }
	            }
			}
        }
		
		public BattleSkillIconUnit GetSkillIcon(string _name, float _time,GameObject _go, Action<BattleSkillIconUnit,Action> endBack, Action _callBack){
            for(int i = 0; i < unitNum; i++)
            {
                if(unitVec[i].State == 0)
                {
                    BattleSkillIconUnit unit = unitVec[i];
                    unit.Init(_go);

                    unit.alpha = 1;
                    unit.State = 1;

                    Action<Sprite> callBack = delegate(Sprite _texture)
                    {
                        mat.mainTexture = _texture.texture;

                        unit.endBack = endBack;
                        unit.callBack = _callBack;

                        unit.uFix = _texture.textureRect.x / 512;
                        unit.vFix = _texture.textureRect.y / 512;
                        unit.alpha = 1;

                        Action delayCall = delegate()
                        {
                            readyToDelSkillIcon(unit);
                        };

                        SuperTween.Instance.DelayCall(_time, delayCall);
                    };

                    TextureFactory.Instance.GetTexture("Assets/Arts/ui/skillIcon/" + _name + ".png", callBack, true);

                    return unit;
                }
                
				
			}

			throw new Exception("SkillIcon is out of use!!!");
		}
		
		private void readyToDelSkillIcon(BattleSkillIconUnit _unit){

            Action<float> toCall = delegate(float value)
            {
                _unit.alpha = value;
            };

            Action endCall = delegate()
            {
                DelSkillIcon(_unit);
            };

            SuperTween.Instance.To(_unit.alpha, 0, 1, toCall, endCall);
		}
		
		public void DelSkillIcon(BattleSkillIconUnit _unit){

            _unit.alpha = 0;
            _unit.State = 0;
            _unit.IsChange = true;

            if (_unit.endBack != null)
            {

                Action<BattleSkillIconUnit, Action> endBack = _unit.endBack;
                endBack(_unit, _unit.callBack);
            }
		}

        public void clearAll()
        {
			
			foreach(BattleSkillIconUnit unit in unitVec){

                DelSkillIcon(unit);
			}
		}
		
		public void Dispose()
        {
            skillIconGO.SetActive(false);
		}
    }
}
