using UnityEngine;
using System;
using xy3d.tstd.lib.gameObjectFactory;

namespace xy3d.tstd.lib.battleHeroTools
{
    public class BattleHeroShadow
    {

        public const int shadowNum = 40;

        public const float pngWidth = 230f / 128f;
        public const float pngHeight = 230f / 128f;

        private BattleHeroShadowUnit[] unitVec;

        private MeshRenderer mr;

        private GameObject shadowGO;
        private GameObject container;

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

        }

        public void Init(GameObject con)
        {

            container = con;

            if (shadowGO)
            {
                shadowGO.SetActive(true);
                return;
            }

            unitVec = new BattleHeroShadowUnit[shadowNum];

            for (int i = 0; i < shadowNum; i++)
            {
                unitVec[i] = new BattleHeroShadowUnit();
            }

            Action<GameObject> loadGameObject = delegate(GameObject _go)
            {
                shadowGO = _go;
                shadowGO.transform.SetParent(container.transform, false);
                mr = _go.GetComponent<MeshRenderer>();
            };

            GameObjectFactory.Instance.GetGameObject("Assets/Arts/battle/BattleTool/Shadow.prefab", loadGameObject, true);            
        }

        public BattleHeroShadowUnit GetShadow(GameObject _go)
        {
			BattleHeroShadowUnit unit = null;

			for(int i = 0 ; i < shadowNum ; i++){
				
				if(unitVec[i].State == 0)
                {
					
					unit = unitVec[i];

                    unit.Init(_go);

                    unit.State = 1;
			
					break;
				}
			}
			
			return unit;
		}

        public void Update()
        {
			if(mr != null){

	            for (int i = 0; i < shadowNum; i++)
	            {
	                BattleHeroShadowUnit unit = unitVec[i];
	                if (unit.State == 1 || unit.IsChange)
	                {
	                    if (unit.IsChange) unit.IsChange = false;
	                    mr.material.SetVector("stateInfo" + i.ToString(), unit.GetStateInfoVec());
						mr.material.SetMatrix("myMatrix" + i.ToString(), unit.GetMatrix());
	                }
	                
	            }
			}
        }

        public void DelShadow(BattleHeroShadowUnit _unit)
        {
			
			_unit.Alpha = 0;
            _unit.State = 0;
            _unit.IsChange = true;
			
			
		}
		
		public void Dispose()
        {
            shadowGO.SetActive(false);
		}

    }
}
