using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreatePoseEndEvent{

	[MenuItem("Animation/在动作最后一帧抛出结束事件")]
	public static void Start(){

		Object[] gos = Selection.objects;

		foreach(Object go in gos){

			if(go is AnimationClip){

				AnimationClip clip = (AnimationClip) go;

				if(!clip.isLooping){

					AnimationEvent e = new AnimationEvent();

					e.functionName = "DispatchAnimationEvent";
					e.stringParameter = "poseStop";

					e.time = clip.length;

					AnimationUtility.SetAnimationEvents(clip,new AnimationEvent[]{e});
				}
			}
		}
	}
}
