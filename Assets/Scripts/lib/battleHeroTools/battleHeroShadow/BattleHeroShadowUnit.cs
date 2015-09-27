using System;
using UnityEngine;

namespace xy3d.tstd.lib.battleHeroTools
{
    class BattleHeroShadowUnit
    {
        public Vector3 pos = new Vector3();
        public Vector3 scale = new Vector3(0.3f, 0.3f, 0.3f);
        public Quaternion rotation = Quaternion.Euler(90, 0, 0);
        private Matrix4x4 matrix = new Matrix4x4();
        
		
		public float alpha = 0;

        private GameObject go;

        public void Init(GameObject _go)
        {
            go = _go;
        }

        public Vector4 GetPositionsVec()
        {
            Vector4 vec = new Vector4();
            if (go != null)
            {
                vec.x = go.transform.position.x;
                vec.y = 0;
                vec.z = go.transform.position.z;
                vec.w = alpha;
            }
            return vec;
        }

        public Matrix4x4 GetMatrix()
        {

            if (go != null)
            {
                pos.x = go.transform.position.x;
                pos.y = 0.2f;
                pos.z = go.transform.position.z;
                matrix.SetTRS(pos, rotation, scale);
            }
            return matrix;
        }
    }
}
