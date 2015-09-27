using System;
using UnityEngine;

namespace xy3d.tstd.lib.battleHeroTools
{
    public class BattleSkillIconUnit
    {
        public Vector3 pos = new Vector3();
        public Vector3 scale = new Vector3(0.5f, 0.5f, 0.5f);
        public Quaternion rotation = Quaternion.Euler(0, 0, 0);
        private Matrix4x4 matrix = new Matrix4x4();
		
		public float uFix = 0;
		public float vFix = 0;
		
		public float alpha = 0;

		public Action<BattleSkillIconUnit, Action> endBack;

        public Action callBack;

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
                vec.y = go.transform.position.y + 1.8f;
                vec.z = go.transform.position.z;
                vec.w = alpha;
            }
            return vec;
        }

        public Matrix4x4 GetMatrix()
        {

            if (go != null)
            {
                matrix.SetTRS(pos, rotation, scale);
            }
            return matrix;
        }

        public Vector4 GetFixVec()
        {
            Vector4 vec = new Vector4();
            if (go != null)
            {
                vec.x = uFix;
                vec.y = vFix;
            }
            return vec;
        }
    }
}
