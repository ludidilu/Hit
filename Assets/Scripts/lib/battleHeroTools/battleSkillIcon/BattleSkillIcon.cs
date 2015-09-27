using System;
using System.Collections.Generic;
using UnityEngine;
using xy3d.tstd.lib.superTween;
using xy3d.tstd.lib.textureFactory;

namespace xy3d.tstd.lib.battleHeroTools
{
    public class BattleSkillIcon
    {
		private const float ASSET_WIDTH = 512 / 64;
		private const float ASSET_HEIGHT = 512 / 64;

        private const float FONT_WIDTH = 128 / 64;
        private const float FONT_HEIGHT = 64 / 64;
		
		public const int unitNum = 40;
		
		private BattleSkillIconUnit[] unitVec;
		private List<BattleSkillIconUnit> unitUseVec;
		private List<BattleSkillIconUnit> unitFreeVec;

        private Material mat;

        private MeshRenderer mr;

        private Texture texture;

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
            init();
		}

		private void init(){
			
			unitVec = new BattleSkillIconUnit[unitNum];
			
			unitFreeVec = new List<BattleSkillIconUnit>();
			
			unitUseVec = new List<BattleSkillIconUnit>();
			
			Vector3[] vertices = new Vector3[unitNum * 4 ];
		    Vector3[] normals = new Vector3[unitNum * 4];
		    Vector2[] uvs = new Vector2[unitNum * 4];
		    Vector4[] tangents = new Vector4[unitNum * 4];
		    int[] triangles = new int[unitNum * 6];

			
			for(int i = 0 ; i < unitNum ; i++){
				
				vertices[i * 4] = new Vector3(-0.5f * FONT_WIDTH, -0.5f * FONT_HEIGHT, 0);
                vertices[i * 4 + 1] = new Vector3(0.5f * FONT_WIDTH, 0.5f * FONT_HEIGHT, 0);
                vertices[i * 4 + 2] = new Vector3(0.5f * FONT_WIDTH, -0.5f * FONT_HEIGHT, 0);
                vertices[i * 4 + 3] = new Vector3(-0.5f * FONT_WIDTH, 0.5f * FONT_HEIGHT, 0);
				
                uvs[i * 4] = new Vector2(0, 0);
                uvs[i * 4 + 1] = new Vector2(FONT_WIDTH / ASSET_WIDTH, FONT_HEIGHT / ASSET_HEIGHT);
				uvs[i * 4 + 2] = new Vector2(FONT_WIDTH / ASSET_WIDTH, 0);
				uvs[i * 4 + 3] = new Vector2(0, FONT_HEIGHT / ASSET_HEIGHT);
				
				tangents[i * 4] = new Vector4(i, 0, 0, 0);
                tangents[i * 4 + 1] = new Vector4(i, 0, 0, 0);
                tangents[i * 4 + 2] = new Vector4(i, 0, 0, 0);
                tangents[i * 4 + 3] = new Vector4(i, 0, 0, 0);

                triangles[i * 6] = i * 4;
                triangles[i * 6 + 1] = i * 4 + 1;
                triangles[i * 6 + 2] = i * 4 + 2;
                triangles[i * 6 + 3] = i * 4 + 1;
                triangles[i * 6 + 4] = i * 4;
                triangles[i * 6 + 5] = i * 4 + 3;

                unitVec[i] = new BattleSkillIconUnit();
                unitFreeVec.Add(unitVec[i]);
			}
			

            GameObject skillIconObj = new GameObject();
            skillIconObj.name = "SkillIconObj";
            MeshFilter mf = skillIconObj.AddComponent<MeshFilter>();
            mr = skillIconObj.AddComponent<MeshRenderer>();

		    mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		    mr.receiveShadows = false;
		    mr.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
		    mr.useLightProbes = false;

		
		    Mesh mesh = new Mesh();
		    mesh.bounds.SetMinMax(new Vector3(), new Vector3(999, 999, 999));
		    mf.sharedMesh = mesh;
		
		    mesh.vertices = vertices;
		    mesh.normals = normals;
		    mesh.tangents = tangents;
		    mesh.uv = uvs;
		
		    mesh.triangles = triangles;

		
		    Bounds bounds = mesh.bounds;
		
		    bounds.Expand(1000f);
		
		    mesh.bounds = bounds;

            mat = new Material(Shader.Find("Custom/BattleSkillIconPass"));
            mr.material = mat;
		}

        public void Update()
        {
            for (int i = 0; i < unitNum; i++)
            {
                BattleSkillIconUnit unit = unitVec[i];
                Vector4 pos = unit.GetPositionsVec();
                mr.sharedMaterial.SetVector("positions" + i.ToString(), pos);

                mr.sharedMaterial.SetVector("fix" + i.ToString(), unit.GetFixVec());
                mr.sharedMaterial.SetMatrix("myMatrix" + i.ToString(), unit.GetMatrix());
            }
        }
		
		public BattleSkillIconUnit getSkillIcon(string _name, float _time,GameObject _go, Action<BattleSkillIconUnit,Action> endBack, Action _callBack){
            if (unitFreeVec.Count > 0)
            {
                BattleSkillIconUnit unit = unitFreeVec[0];
                unit.Init(_go);
                Action<Sprite> callBack = delegate(Sprite _texture)
                {
                    mat.mainTexture = _texture.texture;

                    unitFreeVec.RemoveAt(0);

                    unit.endBack = endBack;
                    unit.callBack= _callBack;

                    unit.uFix = _texture.textureRect.x / 512;
                    unit.vFix = _texture.textureRect.y / 512;
                    unit.alpha = 1;

                    unitUseVec.Add(unit);

                    int tweenID = 0;

                    Action delayCall = delegate()
                    {
                        SuperTween.Instance.Remove(tweenID);
                        readyToDelSkillIcon(unit);
                    };

                    tweenID = SuperTween.Instance.DelayCall(_time, delayCall);
                };

                TextureFactory.Instance.GetTexture("Assets/PlayGround/icon/skillIcon/" + _name + ".png", callBack, true);
				
				return unit;
				
			}else{
				
				throw new Exception("SkillIcon is out of use!!!");
				
			}
		}
		
		private void readyToDelSkillIcon(BattleSkillIconUnit _unit){

            int tweenID = 0;

            Action<float> toCall = delegate(float value)
            {
                _unit.alpha = value;
            };

            Action endCall = delegate()
            {
                SuperTween.Instance.Remove(tweenID);
                delSkillIcon(_unit);
            };

            tweenID = SuperTween.Instance.To(_unit.alpha, 0, 1, toCall, endCall);
		}
		
		private void delSkillIcon(BattleSkillIconUnit _unit){
			
			unitUseVec.RemoveAt(unitUseVec.IndexOf(_unit));
			
			unitFreeVec.Add(_unit);

			if(_unit.endBack != null){

                Action<BattleSkillIconUnit, Action> endBack = _unit.endBack;
                endBack(_unit, _unit.callBack);
			}
		}

        public void clearAll()
        {
			
			while(unitUseVec.Count > 0){
				
				unitUseVec[0].alpha = 0;
				
				delSkillIcon(unitUseVec[0]);
			}
		}
		
		public void Dispose()
        {
		}
    }
}
