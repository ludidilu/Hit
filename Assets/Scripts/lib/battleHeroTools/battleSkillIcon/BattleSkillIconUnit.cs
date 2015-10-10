using System;
using UnityEngine;

namespace xy3d.tstd.lib.battleHeroTools
{
    public class BattleSkillIconUnit
    {
        public Vector3 pos = new Vector3();
        public Vector3 scale = new Vector3(1, 1, 1);
        public Quaternion rotation = Quaternion.Euler(0, 0, 0);
        private Matrix4x4 matrix = new Matrix4x4();

        private Vector4 posVec = new Vector4();
        private Vector4 fixVec = new Vector4();

        private int state;

        public int State
        {
            get { return state; }
            set { state = value; }
        }

        private bool isChange = true;

        public bool IsChange
        {
            get { return isChange; }
            set { isChange = value; }
        }
		
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
            if (go != null)
            {
                posVec.x = go.transform.position.x;
                posVec.y = go.transform.position.y + 1.2f;
                posVec.z = go.transform.position.z;
                posVec.w = 1;
            }
            return posVec;
        }

        public Matrix4x4 GetMatrix()
        {

            if (go != null)
            {
                //rotation = Quaternion.Euler(go.transform.eulerAngles.x, go.transform.eulerAngles.y, go.transform.eulerAngles.z);
                scale.x = go.transform.localScale.x;
                scale.y = go.transform.localScale.y;
                scale.z = go.transform.localScale.z;
                matrix.SetTRS(pos, rotation, scale);
            }
            return matrix;
        }

        public Vector4 GetFixVec()
        {
            if (go != null)
            {
                fixVec.x = uFix;
                fixVec.y = vFix;
                fixVec.z = alpha;
                fixVec.w = State;
            }
            return fixVec;
        }
    }
}
