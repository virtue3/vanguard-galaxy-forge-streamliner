using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Source.AudioSystem
{
	// Token: 0x0200018E RID: 398
	[Serializable]
	public class SoundData
	{
		// Token: 0x040007E5 RID: 2021
		public AudioClip clip;

		// Token: 0x040007E6 RID: 2022
		public AudioMixerGroup mixerGroup;

		// Token: 0x040007E7 RID: 2023
		public float volume = 1f;

		// Token: 0x040007E8 RID: 2024
		public bool loop;

		// Token: 0x040007E9 RID: 2025
		public bool playOnAwake;

		// Token: 0x040007EA RID: 2026
		public float minPitch;

		// Token: 0x040007EB RID: 2027
		public float maxPitch;

		// Token: 0x040007EC RID: 2028
		public bool frequentSound;

		// Token: 0x040007ED RID: 2029
		public bool thrusterSound;

		// Token: 0x040007EE RID: 2030
		public float minZoomVolume = 0.2f;
	}
}
