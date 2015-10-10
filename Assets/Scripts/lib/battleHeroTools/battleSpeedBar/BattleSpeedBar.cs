using System;
using System.Collections.Generic;
using UnityEngine;

namespace xy3d.tstd.lib.battleHeroTools
{
    public class BattleSpeedBar
    {
		public const float allTextureWidth = 512;
		public const float allTextureHeight = 512;
		
		public const int seg = 5;
		
		public const float oneTextureWidth = 100;
		public const float oneTextureHeight = 100;
		
		
		private const string myFrameName = "blue1";
		private const string oppFrameName = "red1";
		
		private const float scale0 = 1.4f;
		private const float scale1 = 1.2f;
		
		public const int unitNum = 10;

		private BattleSpeedBarUnit[] unitVec;
		
		private RenderTexture texture;
		
		private float oppHeroFixU;
		
		private BattleSpeedBarHelper helper;
		
		private List<String> nameVec;

        private Material mat;

        private MeshRenderer mr;

        private GameObject speedBarGO;
        private GameObject container;

        private static BattleSpeedBar _Instance;

        public static BattleSpeedBar Instance
        {

            get
            {

                if (_Instance == null)
                {

                    _Instance = new BattleSpeedBar();
                }

                return _Instance;
            }
        }
		
		public BattleSpeedBar()
		{
			
			helper = new BattleSpeedBarHelper();
            //init(75, 75, 85, 85, 20, 100, 8);
		}
		
		public void Init(float _picWidth, float _picHeight, float _frameWidth, float _frameHeight,  float _fixX, float _fixY, float _gap, GameObject con)
        {

            container = con;

            if (speedBarGO)
            {
                speedBarGO.SetActive(true);
                return;
            }

			
			oppHeroFixU = oneTextureWidth / allTextureWidth;

            texture = helper.init();

            float sw = Screen.width;
            float sh = Screen.height;
            
			float picWidth = _picWidth / sw;
			float picHeight = _picHeight / sh;
			
			float frameWidth = _frameWidth / sw;
			float frameHeight = _frameHeight / sh;
			
			float gap = _gap / sh;
			
			_fixX = _fixX / sw;
			_fixY = _fixY / sh;
			
			unitVec = new BattleSpeedBarUnit[unitNum];
			
			
            Vector3[] vertices = new Vector3[unitNum * 4 * 2];
		    Vector3[] normals = new Vector3[unitNum * 4 * 2];
		    Vector2[] uvs = new Vector2[unitNum * 4 * 2];
		    Vector4[] tangents = new Vector4[unitNum * 4 * 2];
		    int[] triangles = new int[unitNum * 6 * 2];
			
			for(int i = 0 ; i < unitNum ; i++){
				
				if(i == 0){
                    //cc
                    vertices[i * 4] = new Vector3(-0.5f * frameWidth - 1 + frameWidth * 0.5f + _fixX, -0.5f * frameHeight - (i - (unitNum - 1) * 0.5f) * (frameHeight + gap) + _fixY + frameHeight * (scale1 - 1), 0);
                    vertices[i * 4 + 1] = new Vector3(0.5f * frameWidth - 1 + frameWidth * 0.5f + _fixX + frameWidth * (scale0 - 1), 0.5f * frameHeight - (i - (unitNum - 1) * 0.5f) * (frameHeight + gap) + _fixY + frameHeight * (scale1 - 1) + frameHeight * (scale0 - 1), 0);
                    vertices[i * 4 + 2] = new Vector3(0.5f * frameWidth - 1 + frameWidth * 0.5f + _fixX + frameWidth * (scale0 - 1), -0.5f * frameHeight - (i - (unitNum - 1) * 0.5f) * (frameHeight + gap) + _fixY + frameHeight * (scale1 - 1), 0);
                    vertices[i * 4 + 3] = new Vector3(-0.5f * frameWidth - 1 + frameWidth * 0.5f + _fixX, 0.5f * frameHeight - (i - (unitNum - 1) * 0.5f) * (frameHeight + gap) + _fixY + frameHeight * (scale1 - 1) + frameHeight * (scale0 - 1), 0);


                    vertices[i * 4 + vertices.Length / 2] = new Vector3(-0.5f * picWidth - 1 + frameWidth * 0.5f + _fixX, -0.5f * picHeight - (i - (unitNum - 1) * 0.5f) * (frameHeight + gap) + _fixY + frameHeight * (scale1 - 1), 0);
                    vertices[i * 4 + 1 + vertices.Length / 2] = new Vector3(0.5f * picWidth - 1 + frameWidth * 0.5f + _fixX + frameWidth * (scale0 - 1), 0.5f * picHeight - (i - (unitNum - 1) * 0.5f) * (frameHeight + gap) + _fixY + frameHeight * (scale1 - 1) + frameHeight * (scale0 - 1), 0);
                    vertices[i * 4 + 2 + vertices.Length / 2] = new Vector3(0.5f * picWidth - 1 + frameWidth * 0.5f + _fixX + frameWidth * (scale0 - 1), -0.5f * picHeight - (i - (unitNum - 1) * 0.5f) * (frameHeight + gap) + _fixY + frameHeight * (scale1 - 1), 0);
                    vertices[i * 4 + 3 + vertices.Length / 2] = new Vector3(-0.5f * picWidth - 1 + frameWidth * 0.5f + _fixX, 0.5f * picHeight - (i - (unitNum - 1) * 0.5f) * (frameHeight + gap) + _fixY + frameHeight * (scale1 - 1) + frameHeight * (scale0 - 1), 0);
					
				}else if(i == 1){

                    vertices[i * 4] =  new Vector3(-0.5f * frameWidth - 1 + frameWidth * 0.5f + _fixX, -0.5f * frameHeight - (i - (unitNum - 1) * 0.5f) * (frameHeight + gap) + _fixY, 0);
                    vertices[i * 4 + 1] = new Vector3(0.5f * frameWidth - 1 + frameWidth * 0.5f + _fixX + frameWidth * (scale1 - 1), 0.5f * frameHeight - (i - (unitNum - 1) * 0.5f) * (frameHeight + gap) + _fixY + frameHeight * (scale1 - 1), 0);
                    vertices[i * 4 + 2] =  new Vector3(0.5f * frameWidth - 1 + frameWidth * 0.5f + _fixX + frameWidth * (scale1 - 1), -0.5f * frameHeight - (i - (unitNum - 1) * 0.5f) * (frameHeight + gap) + _fixY, 0);
                    vertices[i * 4 + 3] = new Vector3(-0.5f * frameWidth - 1 + frameWidth * 0.5f + _fixX, 0.5f * frameHeight - (i - (unitNum - 1) * 0.5f) * (frameHeight + gap) + _fixY + frameHeight * (scale1 - 1), 0);

					
					vertices[i * 4 + vertices.Length / 2] =  new Vector3(-0.5f * picWidth - 1 + frameWidth * 0.5f + _fixX,  -0.5f * picHeight - (i - (unitNum - 1) * 0.5f) * (frameHeight + gap) + _fixY, 0);
                    vertices[i * 4 + 1 + vertices.Length / 2] = new Vector3(0.5f * picWidth - 1 + frameWidth * 0.5f + _fixX + frameWidth * (scale1 - 1), 0.5f * picHeight - (i - (unitNum - 1) * 0.5f) * (frameHeight + gap) + _fixY + frameHeight * (scale1 - 1), 0);
					vertices[i * 4 + 2 + vertices.Length / 2] =  new Vector3(0.5f * picWidth - 1 + frameWidth * 0.5f + _fixX + frameWidth * (scale1 - 1), -0.5f * picHeight - (i - (unitNum - 1) * 0.5f) * (frameHeight + gap) + _fixY, 0);
                    vertices[i * 4 + 3 + vertices.Length / 2] = new Vector3(-0.5f * picWidth - 1 + frameWidth * 0.5f + _fixX, 0.5f * picHeight - (i - (unitNum - 1) * 0.5f) * (frameHeight + gap) + _fixY + frameHeight * (scale1 - 1), 0);
				
				}else{

                    vertices[i * 4] = new Vector3(-0.5f * frameWidth - 1 + frameWidth * 0.5f + _fixX, -0.5f * frameHeight - (i - (unitNum - 1) * 0.5f) * (frameHeight + gap) + _fixY, 0);
                    vertices[i * 4 + 1] = new Vector3(0.5f * frameWidth - 1 + frameWidth * 0.5f + _fixX, 0.5f * frameHeight - (i - (unitNum - 1) * 0.5f) * (frameHeight + gap) + _fixY, 0);
                    vertices[i * 4 + 2] = new Vector3(0.5f * frameWidth - 1 + frameWidth * 0.5f + _fixX, -0.5f * frameHeight - (i - (unitNum - 1) * 0.5f) * (frameHeight + gap) + _fixY, 0);
                    vertices[i * 4 + 3] = new Vector3(-0.5f * frameWidth - 1 + frameWidth * 0.5f + _fixX, 0.5f * frameHeight - (i - (unitNum - 1) * 0.5f) * (frameHeight + gap) + _fixY, 0);


                    vertices[i * 4 + vertices.Length / 2] = new Vector3(-0.5f * picWidth - 1 + frameWidth * 0.5f + _fixX, -0.5f * picHeight - (i - (unitNum - 1) * 0.5f) * (frameHeight + gap) + _fixY, 0);
                    vertices[i * 4 + 1 + vertices.Length / 2] = new Vector3(0.5f * picWidth - 1 + frameWidth * 0.5f + _fixX, 0.5f * picHeight - (i - (unitNum - 1) * 0.5f) * (frameHeight + gap) + _fixY, 0);
                    vertices[i * 4 + 2 + vertices.Length / 2] = new Vector3(0.5f * picWidth - 1 + frameWidth * 0.5f + _fixX, -0.5f * picHeight - (i - (unitNum - 1) * 0.5f) * (frameHeight + gap) + _fixY, 0);
					vertices[i * 4 + 3 + vertices.Length / 2] = new Vector3(-0.5f * picWidth - 1 + frameWidth * 0.5f + _fixX, 0.5f * picHeight - (i - (unitNum - 1) * 0.5f) * (frameHeight + gap) + _fixY, 0);

				}


                tangents[i * 4] = new Vector4(i, 0, 0, 0);
                tangents[i * 4 + 1] = new Vector4(i, 0, 0, 0);
                tangents[i * 4 + 2] = new Vector4(i, 0, 0, 0);
                tangents[i * 4 + 3] = new Vector4(i, 0, 0, 0);

                tangents[i * 4 +tangents.Length / 2] = new Vector4(i + unitNum, 0, 0, 0);
                tangents[i * 4 + 1 +tangents.Length / 2] = new Vector4(i + unitNum, 0, 0, 0);
                tangents[i * 4 + 2 +tangents.Length / 2] = new Vector4(i + unitNum, 0, 0, 0);
                tangents[i * 4 + 3 +tangents.Length / 2] = new Vector4(i + unitNum, 0, 0, 0);
				
				triangles[i * 6] = i * 4;
				triangles[i * 6 + 1] = i * 4 + 1;
				triangles[i * 6 + 2] = i * 4 + 2;
				triangles[i * 6 + 3] = i * 4 + 1;
				triangles[i * 6 + 4] = i * 4;
				triangles[i * 6 + 5] = i * 4 + 3;
				
				triangles[i * 6 + triangles.Length / 2] = i * 4 + unitNum * 4;
				triangles[i * 6 + 1 + triangles.Length / 2] = i * 4 + 1 + unitNum * 4;
				triangles[i * 6 + 2 + triangles.Length / 2] = i * 4 + 2 + unitNum * 4;
				triangles[i * 6 + 3 + triangles.Length / 2] = i * 4 + 1+ unitNum * 4;
				triangles[i * 6 + 4 + triangles.Length / 2] = i * 4 + unitNum * 4;
				triangles[i * 6 + 5 + triangles.Length / 2] = i * 4 + 3 + unitNum * 4;
				
				uvs[i * 4 + uvs.Length / 2] = uvs[i * 4] = new Vector2(0, 1- oneTextureHeight / allTextureHeight);
				uvs[i * 4 + 1 + uvs.Length / 2] = uvs[i * 4 + 1] = new Vector2(oneTextureWidth / allTextureWidth, 1);
				uvs[i * 4 + 2 + uvs.Length / 2] = uvs[i * 4 + 2] = new Vector2(oneTextureWidth / allTextureWidth, 1- oneTextureHeight / allTextureHeight);
				uvs[i * 4 + 3 + uvs.Length / 2] = uvs[i * 4 + 3] = new Vector2(0, 1);

				
				unitVec[i] = new BattleSpeedBarUnit();
			}

            speedBarGO = new GameObject();
            speedBarGO.name = "SpeedObject";
            MeshFilter mf = speedBarGO.AddComponent<MeshFilter>();
            mr = speedBarGO.AddComponent<MeshRenderer>();

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

            mat = new Material(Shader.Find("Custom/BattleSpeedPass"));

            mat.mainTexture = texture;

            mr.material = mat;

            speedBarGO.transform.SetParent(container.transform, false);
            speedBarGO.SetActive(false);
		}

        public void Update()
        {
            if (mr != null)
            {

                for (int i = 0; i < unitNum * 2; i++)
                {
                    BattleSpeedBarUnit unit;
                    Vector4 uv;
                    if (i < unitNum)
                    {
                        unit = unitVec[i];
                        uv = unit.GetFrameUV();
                    }
                    else
                    {

                        unit = unitVec[i - unitNum];
                        uv = unit.GetUV();
                    }
                    mr.material.SetVector("fix" + i.ToString(), uv);
                }
            }
        }
		
		private void setIcon(int _index, string _name, bool _isMyHero)
        {

			int index = nameVec.IndexOf(_name);
			
			unitVec[_index].u = ((int)(index % seg)) * oneTextureWidth / allTextureWidth;
			unitVec[_index].v =  - ((int)(index / seg)) * oneTextureHeight / allTextureWidth;

			if(_isMyHero){
				
				unitVec[_index].frameU = 0;
				
			}else{
				
				unitVec[_index].frameU = oppHeroFixU;
			}
		}
		
		public void Start(List<string> _nameVec){
			
			nameVec = new List<string>(_nameVec.Count + 2);
			
			nameVec.Add(myFrameName);
			nameVec.Add(oppFrameName);
			
			for(int i = 0 ; i < _nameVec.Count ; i++){
				
				nameVec.Add(_nameVec[i]);
			}

            helper.start(nameVec, TextureLoadOK); ;
		}
		
		private void reloadTextureReal()
        {
			
			texture = helper.init();
			
			mat.mainTexture = texture;
			
			if(nameVec != null){

                helper.start(nameVec, TextureLoadOK);
			}
		}

        private void TextureLoadOK()
        {
            speedBarGO.SetActive(true);
        }
		
		public void SetIcons(List<string> _vec, List<bool> _isMyHeroVec)
        {
			
			for(int i = 0 ; i < unitNum ; i++){
				
				setIcon(i,_vec[i],_isMyHeroVec[i]);
			}
		}
		
		public void Dispose()
        {
            nameVec = null;
            speedBarGO.SetActive(false);
		}
    }
}
