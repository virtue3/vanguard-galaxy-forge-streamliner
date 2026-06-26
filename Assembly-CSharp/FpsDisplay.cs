using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x0200000C RID: 12
public class FpsDisplay : MonoBehaviour
{
	// Token: 0x060000B4 RID: 180 RVA: 0x000055EC File Offset: 0x000037EC
	private void Awake()
	{
		for (int i = 0; i < this._cacheNumbersAmount; i++)
		{
			this.CachedNumberStrings[i] = i.ToString();
		}
		this._frameRateSamples = new int[this._averageFromAmount];
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x00005630 File Offset: 0x00003830
	private void Update()
	{
		FpsDisplay.DeltaTimeType deltaType = this.DeltaType;
		float num;
		if (deltaType != FpsDisplay.DeltaTimeType.Smooth)
		{
			if (deltaType != FpsDisplay.DeltaTimeType.Unscaled)
			{
				num = Time.unscaledDeltaTime;
			}
			else
			{
				num = Time.unscaledDeltaTime;
			}
		}
		else
		{
			num = Time.smoothDeltaTime;
		}
		int num2 = (int)Math.Round((double)(1f / num));
		this._frameRateSamples[this._averageCounter] = num2;
		float num3 = 0f;
		foreach (int num4 in this._frameRateSamples)
		{
			num3 += (float)num4;
		}
		this._currentAveraged = (int)Math.Round((double)(num3 / (float)this._averageFromAmount));
		this._averageCounter = (this._averageCounter + 1) % this._averageFromAmount;
		TMP_Text text = this.Text;
		int currentAveraged = this._currentAveraged;
		string text2;
		if (currentAveraged >= 0 && currentAveraged < this._cacheNumbersAmount)
		{
			text2 = this.CachedNumberStrings[currentAveraged];
		}
		else if (currentAveraged >= this._cacheNumbersAmount)
		{
			text2 = string.Format("> {0}", this._cacheNumbersAmount);
		}
		else if (currentAveraged < 0)
		{
			text2 = "< 0";
		}
		else
		{
			text2 = "?";
		}
		text.text = text2;
	}

	// Token: 0x04000071 RID: 113
	public TMP_Text Text;

	// Token: 0x04000072 RID: 114
	[Tooltip("Unscaled is more accurate, but jumpy, or if your game modifies Time.timeScale. Use Smooth for smoothDeltaTime.")]
	public FpsDisplay.DeltaTimeType DeltaType;

	// Token: 0x04000073 RID: 115
	private Dictionary<int, string> CachedNumberStrings = new Dictionary<int, string>();

	// Token: 0x04000074 RID: 116
	private int[] _frameRateSamples;

	// Token: 0x04000075 RID: 117
	private int _cacheNumbersAmount = 300;

	// Token: 0x04000076 RID: 118
	private int _averageFromAmount = 30;

	// Token: 0x04000077 RID: 119
	private int _averageCounter;

	// Token: 0x04000078 RID: 120
	private int _currentAveraged;

	// Token: 0x020003F0 RID: 1008
	public enum DeltaTimeType
	{
		// Token: 0x04001739 RID: 5945
		Smooth,
		// Token: 0x0400173A RID: 5946
		Unscaled
	}
}
