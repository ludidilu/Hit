using UnityEngine;
using System;
using xy3d.tstd.lib.gameObjectFactory;

namespace xy3d.tstd.lib.battleHeroTools
{
    class BattleHeroShadow
    {

        private const int shadowNum = 40;
        private BattleHeroShadowUnit[] unitVec;
        private bool[] unitUseVec;
        private int useNum = 0;

        private MeshRenderer mr;

        private static BattleHeroShadow _Instance;

        public static BattleHeroShadow Instance
        {

            get
            {

                if (_Instance == null)
                {

                    _Instance = new BattleHeroShadow();
                }

                return _Instance;
            }
        }

        public BattleHeroShadow()
        {
            Init();
        }

        private void Init()
        {

            unitVec = new BattleHeroShadowUnit[shadowNum];

            unitUseVec = new bool[shadowNum];

            for (int i = 0; i < shadowNum; i++)
            {
                unitVec[i] = new BattleHeroShadowUnit();
                unitUseVec[i] = false;
            }

            Action<GameObject> loadGameObject = delegate(GameObject _go)
            {
//                PopUpManager.Instance.AddPopUp(_go, PopUp2LayerType.Type_Main);
                mr = _go.GetComponent<MeshRenderer>();
            };

            GameObjectFactory.Instance.GetGameObject("Assets/PlayGround/BattleTool/Shadow.prefab", loadGameObject, true);            
        }

        public BattleHeroShadowUnit getShadow(GameObject _go)
        {
			BattleHeroShadowUnit unit = null;

			useNum++;
			
			for(int i = 0 ; i < shadowNum ; i++){
				
				if(!unitUseVec[i])
                {
					
					unit = unitVec[i];
					
					unitUseVec[i] = true;

                    unit.Init(_go);
			
					break;
				}
			}
			
			return unit;
		}

        public void Update()
        {
            for (int i = 0; i < shadowNum; i++)
            {
                BattleHeroShadowUnit unit = unitVec[i];
                mr.sharedMaterial.SetVector("positions" + i.ToString(), unit.GetPositionsVec());
                mr.sharedMaterial.SetMatrix("myMatrix" + i.ToString(), unit.GetMatrix());
            }
        }

        public void delShadow(BattleHeroShadowUnit _unit)
        {
			
			_unit.alpha = 0;
			
			unitUseVec[Array.IndexOf(unitVec, _unit)] = false;
			
			useNum--;
			
		}
		
		public void setContainer()
        {
		}
		
		public void dispose()
        {
			
		}
    }
}
