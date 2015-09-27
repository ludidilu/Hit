using UnityEngine;

namespace xy3d.tstd.lib.battleHeroTools
{
    public class BattleSpeedBarUnit
    {
        public float u = 0;
        public float v = 0;
        public float frameU = 0;
        public float frameV = 0;

        private Vector4 v4 = new Vector4();

        public Vector4 GetUV()
        {
            v4.x = u;
            v4.y = v;
            return v4;
        }

        public Vector4 GetFrameUV()
        {
            v4.x = frameU;
            v4.y = frameV;
            return v4;
        }
    }
}
