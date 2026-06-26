using System;
using Behaviour.Effects;
using Behaviour.UI.Tooltip;
using Source.MissionSystem;
using Source.MissionSystem.Objectives;
using Source.Player;
using UnityEngine;

namespace Behaviour.Travel
{
	// Token: 0x020002CD RID: 717
	public class TutorialJumpgate : MonoBehaviour
	{
		// Token: 0x06001A36 RID: 6710 RVA: 0x000A3064 File Offset: 0x000A1264
		private void Start()
		{
			this.fullHeight = this.completed.size.y;
			this.UpdateConstructionProgress();
			this.skeleton.size = new Vector2(this.skeleton.size.x, this.skeletonSize);
			this.plates.size = new Vector2(this.plates.size.x, this.platesSize);
			this.conduits.size = new Vector2(this.conduits.size.x, this.conduitsSize);
			Mission mission = GamePlayer.current.GetMission("tutorial_10");
			if (mission == null)
			{
				this.tooltip.enabled = false;
				return;
			}
			this.tooltip.enabled = !mission.isComplete;
		}

		// Token: 0x06001A37 RID: 6711 RVA: 0x000A3134 File Offset: 0x000A1334
		private void Update()
		{
			this.updateTimer -= Time.deltaTime;
			if (this.updateTimer < 0f)
			{
				this.updateTimer = 0.5f;
				this.UpdateConstructionProgress();
			}
			float maxDelta = this.constructionSpeed * Time.deltaTime;
			this.skeleton.size = new Vector2(this.skeleton.size.x, Mathf.MoveTowards(this.skeleton.size.y, this.skeletonSize, maxDelta));
			this.plates.size = new Vector2(this.plates.size.x, Mathf.MoveTowards(this.plates.size.y, this.platesSize, maxDelta));
			this.conduits.size = new Vector2(this.conduits.size.x, Mathf.MoveTowards(this.conduits.size.y, this.conduitsSize, maxDelta));
		}

		// Token: 0x06001A38 RID: 6712 RVA: 0x000A3234 File Offset: 0x000A1434
		private void UpdateConstructionProgress()
		{
			ValueTuple<float, float, float, float> constructionProgress = this.GetConstructionProgress();
			float item = constructionProgress.Item1;
			float item2 = constructionProgress.Item2;
			float item3 = constructionProgress.Item3;
			float item4 = constructionProgress.Item4;
			if (item4 == 1f)
			{
				this.completed.gameObject.SetActive(true);
				this.skeleton.gameObject.SetActive(false);
				this.plates.gameObject.SetActive(false);
				this.conduits.gameObject.SetActive(false);
			}
			else
			{
				this.skeletonSize = this.fullHeight * item;
				this.platesSize = this.fullHeight * item2;
				this.conduitsSize = this.fullHeight * item3;
				this.completed.gameObject.SetActive(false);
				this.skeleton.gameObject.SetActive(true);
				this.plates.gameObject.SetActive(true);
				this.conduits.gameObject.SetActive(true);
			}
			if (item4 == 0f)
			{
				this.jumpgateEffect.gameObject.SetActive(false);
				return;
			}
			this.jumpgateEffect.gameObject.SetActive(true);
		}

		// Token: 0x06001A39 RID: 6713 RVA: 0x000A3348 File Offset: 0x000A1548
		private ValueTuple<float, float, float, float> GetConstructionProgress()
		{
			if (GamePlayer.current.IsMissionCompleted("tutorial_10"))
			{
				return new ValueTuple<float, float, float, float>(1f, 1f, 1f, 1f);
			}
			Mission mission = GamePlayer.current.GetMission("tutorial_10");
			if (mission == null)
			{
				return new ValueTuple<float, float, float, float>(0f, 0f, 0f, 0f);
			}
			float item = 1f;
			float item2 = 1f;
			float item3 = 1f;
			float item4 = 0f;
			foreach (MissionStep missionStep in mission.steps)
			{
				foreach (MissionObjective missionObjective in missionStep.objectives)
				{
					TriggerObjective triggerObjective = missionObjective as TriggerObjective;
					if (triggerObjective != null)
					{
						MissionTrigger? triggeredBy = triggerObjective.triggeredBy;
						if (triggeredBy != null)
						{
							switch (triggeredBy.GetValueOrDefault())
							{
							case MissionTrigger.TutorialJumpgateStructure:
								item = triggerObjective.progress;
								break;
							case MissionTrigger.TutorialJumpgatePlates:
								item2 = triggerObjective.progress;
								break;
							case MissionTrigger.TutorialJumpgateConduit:
								item3 = triggerObjective.progress;
								break;
							case MissionTrigger.TutorialJumpgateBeacon:
								item4 = triggerObjective.progress;
								break;
							}
						}
					}
				}
			}
			return new ValueTuple<float, float, float, float>(item, item2, item3, item4);
		}

		// Token: 0x04001078 RID: 4216
		private const string MissionId = "tutorial_10";

		// Token: 0x04001079 RID: 4217
		[SerializeField]
		private SpriteRenderer skeleton;

		// Token: 0x0400107A RID: 4218
		[SerializeField]
		private SpriteRenderer plates;

		// Token: 0x0400107B RID: 4219
		[SerializeField]
		private SpriteRenderer conduits;

		// Token: 0x0400107C RID: 4220
		[SerializeField]
		private SpriteRenderer completed;

		// Token: 0x0400107D RID: 4221
		[SerializeField]
		private JumpGateEffect jumpgateEffect;

		// Token: 0x0400107E RID: 4222
		[SerializeField]
		private TooltipSource tooltip;

		// Token: 0x0400107F RID: 4223
		private float updateTimer;

		// Token: 0x04001080 RID: 4224
		private float fullHeight;

		// Token: 0x04001081 RID: 4225
		private float constructionSpeed = 1.5f;

		// Token: 0x04001082 RID: 4226
		private float skeletonSize;

		// Token: 0x04001083 RID: 4227
		private float platesSize;

		// Token: 0x04001084 RID: 4228
		private float conduitsSize;
	}
}
