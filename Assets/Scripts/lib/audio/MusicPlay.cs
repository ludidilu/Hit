using UnityEngine;
using System.Collections;
using System;

namespace xy3d.tstd.lib.assetManager{

	public class MusicPlay{

		private static MusicPlay _Instance;

		public static MusicPlay Instance{

			get{

				if(_Instance == null){

					_Instance = new MusicPlay();
				}

				return _Instance;
			}
		}

		private GameObject musicPlayer;

		private AudioSource audioSource;

		public MusicPlay(){

			musicPlayer = new GameObject();

			musicPlayer.name = "MusicPlayer";

			audioSource = musicPlayer.AddComponent<AudioSource>();
            audioSource.mute = true;
		}

		public void Play(string _path,bool _loop){

			Action<AudioClip> callBack = delegate(AudioClip obj) {

				GetClip(obj,_loop);
			};

			AssetManager.Instance.GetAsset<AudioClip>(_path,callBack);
		}

		private void GetClip(AudioClip _clip,bool _loop){

			audioSource.clip = _clip;

			audioSource.loop = _loop;

			audioSource.Play();
		}
	}
}