using System;
using System.Collections.Generic;
using UnityEngine;

namespace xy3d.tstd.lib.effect
{
//	[ExecuteInEditMode] // Make water live-update even when not in play mode
	public class Water : MonoBehaviour
	{
		public Material material;

		// This just sets up some matrices in the material; for really
		// old cards to make water texture scroll.
		void Update()
		{

			Vector4 waveSpeed = material.GetVector("WaveSpeed");
			float waveScale = material.GetFloat("_WaveScale");
			Vector4 waveScale4 = new Vector4(waveScale, waveScale, waveScale * 0.4f, waveScale * 0.45f);
			
			// Time since level load, and do intermediate calculations with doubles
			double t = Time.timeSinceLevelLoad / 20.0;
			Vector4 offsetClamped = new Vector4(
				(float)Math.IEEERemainder(waveSpeed.x * waveScale4.x * t, 1.0),
				(float)Math.IEEERemainder(waveSpeed.y * waveScale4.y * t, 1.0),
				(float)Math.IEEERemainder(waveSpeed.z * waveScale4.z * t, 1.0),
				(float)Math.IEEERemainder(waveSpeed.w * waveScale4.w * t, 1.0)
				);
			
			material.SetVector("_WaveOffset", offsetClamped);
			material.SetVector("_WaveScale4", waveScale4);
		}

	}
}