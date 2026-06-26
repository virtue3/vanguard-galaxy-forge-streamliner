using System;
using System.Collections;
using Behaviour.Bootstrap;
using Behaviour.Item.Builder;
using Behaviour.Item.Usable;
using Behaviour.UI.Main_Menu;
using Behaviour.Util;
using Source.Crew;
using Source.Player;
using Source.Simulation.Story;
using Source.SpaceShip;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Main
{
	// Token: 0x0200025F RID: 607
	public class NewGame : MonoBehaviour
	{
		// Token: 0x06001658 RID: 5720 RVA: 0x0008E118 File Offset: 0x0008C318
		private void Start()
		{
			this.SetupPersonalStep();
			this.personalHistoryStep.gameObject.SetActive(false);
			this.shipCarousel.gameObject.SetActive(false);
			this.shipCarousel.SetCameraOverride(false);
			GameObject gameObject = this.step4;
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
			GameObject gameObject2 = this.step5;
			if (gameObject2 != null)
			{
				gameObject2.SetActive(false);
			}
			this.cameraMovement = Camera.main.GetComponent<CameraMovement>();
			this.cameraMovement.ShipyardZoom();
			this.tutorialButtonColor = this.tutorialButton.color;
			this.ChooseTutorial();
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x0008E1B0 File Offset: 0x0008C3B0
		private void Update()
		{
			if (this.currentStep == 3 && Singleton<BackdropManager>.Instance && Singleton<BackdropManager>.Instance.screenSize != this.screenSize)
			{
				this.screenSize = Singleton<BackdropManager>.Instance.screenSize;
				this.shipCarousel.SetMainCameraPos();
			}
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x0008E204 File Offset: 0x0008C404
		private void OnDestroy()
		{
			CameraMovement cameraMovement = this.cameraMovement;
			if (cameraMovement == null)
			{
				return;
			}
			cameraMovement.NormalZoom(GameplayerPrefs.GetZoom());
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x0008E21B File Offset: 0x0008C41B
		public void SetupPersonalStep()
		{
			this.personalStep.gameObject.SetActive(true);
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x0008E230 File Offset: 0x0008C430
		public void TrySubmitPersonalStep()
		{
			if (this.personalStep.firstName == null || this.personalStep.firstName == "")
			{
				return;
			}
			this.personalStep.gameObject.SetActive(false);
			this.personalHistoryStep.gameObject.SetActive(true);
			this.currentStep = 2;
			this.stepProgression.SetStepState("1", StepState.Completed);
			this.stepProgression.SetStepState("2", StepState.Current);
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x0008E2B0 File Offset: 0x0008C4B0
		private void SubmitPersonalHistoryStep()
		{
			this.personalHistoryStep.gameObject.SetActive(false);
			this.personalHistoryData = new PersonalHistoryData(this.personalHistoryStep.selectedPersonalHistory);
			this.ToggleBackgroundImages(false);
			this.shipCarousel.gameObject.SetActive(true);
			this.shipCarousel.SetStarterShips(this.personalHistoryData.starterShips);
			this.currentStep = 3;
			this.stepProgression.SetStepState("2", StepState.Completed);
			this.stepProgression.SetStepState("3", StepState.Current);
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x0008E33C File Offset: 0x0008C53C
		private void TrySubmitChooseShipStep()
		{
			if (!this.shipCarousel.selectedShipData.shipClass)
			{
				return;
			}
			this.SetShip();
			this.shipCarousel.gameObject.SetActive(false);
			this.step4.SetActive(true);
			this.currentStep = 4;
			this.ToggleBackgroundImages(true);
			this.stepProgression.SetStepState("3", StepState.Completed);
			this.stepProgression.SetStepState("4", StepState.Current);
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x0008E3B4 File Offset: 0x0008C5B4
		private void TrySubmitCustomShipName()
		{
			this.step4.SetActive(false);
			this.step5.SetActive(true);
			SpaceShipData selectedShipData = this.shipCarousel.selectedShipData;
			CustomShipName customShipName = this.customShipName;
			selectedShipData.customShipName = ((customShipName != null) ? customShipName.shipName : null);
			this.currentStep = 5;
			this.nextLabel.TL("@NGStart", Array.Empty<object>());
			this.stepProgression.SetStepState("4", StepState.Completed);
			this.stepProgression.SetStepState("5", StepState.Current);
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x0008E439 File Offset: 0x0008C639
		private void SetShip()
		{
			this.personalHistoryData.starterShipName = this.shipCarousel.selectedShipData.name;
			this.customShipName.UpdateShipName(this.shipCarousel.selectedShipData.GetShipName());
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x0008E474 File Offset: 0x0008C674
		public void SubmitInput()
		{
			switch (this.currentStep)
			{
			case 1:
				this.TrySubmitPersonalStep();
				return;
			case 2:
				this.SubmitPersonalHistoryStep();
				return;
			case 3:
				this.TrySubmitChooseShipStep();
				return;
			case 4:
				this.TrySubmitCustomShipName();
				return;
			case 5:
				base.StartCoroutine(this.StartGame());
				return;
			default:
				Debug.LogWarning("Unknown step: " + this.currentStep.ToString());
				return;
			}
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x0008E4EC File Offset: 0x0008C6EC
		public void Back()
		{
			this.ToggleBackgroundImages(true);
			switch (this.currentStep)
			{
			case 1:
				PersistentSingleton<SceneLoader>.Instance.LoadScene("Main Menu", false);
				PersistentSingleton<SceneLoader>.Instance.UnloadScene("Start - New Game");
				return;
			case 2:
				this.personalStep.gameObject.SetActive(true);
				this.personalHistoryStep.gameObject.SetActive(false);
				this.currentStep = 1;
				this.stepProgression.SetStepState("2", StepState.Incomplete);
				this.stepProgression.SetStepState("1", StepState.Current);
				return;
			case 3:
				this.personalHistoryStep.gameObject.SetActive(true);
				this.shipCarousel.gameObject.SetActive(false);
				this.currentStep = 2;
				this.stepProgression.SetStepState("3", StepState.Incomplete);
				this.stepProgression.SetStepState("2", StepState.Current);
				return;
			case 4:
			{
				SpaceShipData selectedShipData = this.shipCarousel.selectedShipData;
				CustomShipName customShipName = this.customShipName;
				selectedShipData.customShipName = ((customShipName != null) ? customShipName.shipName : null);
				this.shipCarousel.gameObject.SetActive(true);
				this.shipCarousel.ShowShip(false);
				this.step4.SetActive(false);
				this.ToggleBackgroundImages(false);
				this.currentStep = 3;
				this.stepProgression.SetStepState("4", StepState.Incomplete);
				this.stepProgression.SetStepState("3", StepState.Current);
				return;
			}
			case 5:
				this.step5.SetActive(false);
				this.step4.SetActive(true);
				this.currentStep = 4;
				this.stepProgression.SetStepState("5", StepState.Incomplete);
				this.stepProgression.SetStepState("4", StepState.Current);
				this.nextLabel.TL("@NGNext", Array.Empty<object>());
				return;
			default:
				Debug.LogWarning("Unknown step: " + this.currentStep.ToString());
				return;
			}
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x0008E6D0 File Offset: 0x0008C8D0
		public void ChooseTutorial()
		{
			this.startTutorial = true;
			this.tutorialButton.color = new Color(0.3f, 0.3f, 0.3f, 0.6f);
			this.sandboxButton.color = this.tutorialButtonColor;
			this.readyText.TL("@NGReadyTextTutorial", Array.Empty<object>());
		}

		// Token: 0x06001664 RID: 5732 RVA: 0x0008E730 File Offset: 0x0008C930
		public void ChooseSandbox()
		{
			this.startTutorial = false;
			this.tutorialButton.color = this.tutorialButtonColor;
			this.sandboxButton.color = new Color(0.3f, 0.3f, 0.3f, 0.6f);
			this.readyText.TL("@NGReadyTextSandbox", Array.Empty<object>());
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x0008E78E File Offset: 0x0008C98E
		private void ToggleBackgroundImages(bool enabled = true)
		{
			this.bottomDivider.GetComponent<Image>().enabled = enabled;
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x0008E7A1 File Offset: 0x0008C9A1
		private IEnumerator StartGame()
		{
			LoadingScreen.Show("@UICreatingNewGame");
			yield return null;
			this.SaveInputs();
			this.cameraMovement.NormalZoom(GameplayerPrefs.GetZoom());
			PersistentSingleton<GameManager>.Instance.StartNewGame();
			yield break;
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x0008E7B0 File Offset: 0x0008C9B0
		private void SaveInputs()
		{
			GamePlayer.CreateNewGamePlayer(this.personalHistoryData, this.startTutorial);
			GamePlayer current = GamePlayer.current;
			CommanderData commander = current.commander;
			commander.SetName(this.personalStep.firstName, this.personalStep.callsign, this.personalStep.lastName);
			commander.SetIcon(this.personalStep.SelectedIcon);
			commander.gender = (this.personalStep.SelectedIcon.isMale ? Gender.Male : Gender.Female);
			commander.SetPersonalHistory(this.personalHistoryStep.selectedPersonalHistory);
			TitleDefinition titleDefinition;
			switch (this.personalHistoryStep.selectedPersonalHistory)
			{
			case PersonalHistory.Miner:
				titleDefinition = Titles.Miner;
				break;
			case PersonalHistory.NavyCaptain:
				titleDefinition = Titles.NavyCaptain;
				break;
			case PersonalHistory.Salvaging:
				titleDefinition = Titles.Salvager;
				break;
			case PersonalHistory.Hauler:
				titleDefinition = Titles.Hauler;
				break;
			case PersonalHistory.BountyHunter:
				titleDefinition = Titles.BountyHunter;
				break;
			case PersonalHistory.Pirate:
				titleDefinition = Titles.Pirate;
				break;
			default:
				titleDefinition = null;
				break;
			}
			TitleDefinition titleDefinition2 = titleDefinition;
			if (titleDefinition2 != null)
			{
				current.GrantTitle(titleDefinition2.identifier);
				commander.SetTitle(titleDefinition2.GetDisplayName(), titleDefinition2.color);
			}
			current.spaceShips.Add(this.shipCarousel.selectedShipData);
			this.shipCarousel.selectedShipData.currentHullHP *= 0.7f;
			if (this.personalHistoryData.startWithAmmo)
			{
				this.shipCarousel.selectedShipData.GiveAmmo(600, true);
			}
			current.SetSpaceShipData(this.shipCarousel.selectedShipData);
			if (this.startTutorial)
			{
				current.AddStoryteller(new Tutorial(current), true);
			}
			else
			{
				current.AddStoryteller(new Sandbox(current), true);
				current.AddStoryteller(new Puppeteers(current), true);
				current.AddStoryteller(new Economy(current), true);
				current.commander.bonusSkillPoints++;
				current.currentSpaceShip.RepairHullHp(99999f);
				current.currentSpaceShip.AddCargo(ItemBuilder.Get("WarpFuel").CreateWarpFuel(WarpFuelItem.WarpFuelType.PlasmaCell, 1f), 1, false);
				current.currentSpaceShip.AddCargo("PoiBeacon", 1, false);
			}
			current.AddStoryteller(new Default(current), true);
		}

		// Token: 0x04000D8F RID: 3471
		public PersonalStep personalStep;

		// Token: 0x04000D90 RID: 3472
		public PersonalHistoryStep personalHistoryStep;

		// Token: 0x04000D91 RID: 3473
		public ShipCarousel shipCarousel;

		// Token: 0x04000D92 RID: 3474
		public GameObject step4;

		// Token: 0x04000D93 RID: 3475
		public GameObject step5;

		// Token: 0x04000D94 RID: 3476
		[SerializeField]
		private CustomShipName customShipName;

		// Token: 0x04000D95 RID: 3477
		[SerializeField]
		private GameObject bottomDivider;

		// Token: 0x04000D96 RID: 3478
		[SerializeField]
		private GameObject padding;

		// Token: 0x04000D97 RID: 3479
		[SerializeField]
		private StepProgression stepProgression;

		// Token: 0x04000D98 RID: 3480
		[SerializeField]
		private TMP_Text nextLabel;

		// Token: 0x04000D99 RID: 3481
		[SerializeField]
		private Image tutorialButton;

		// Token: 0x04000D9A RID: 3482
		[SerializeField]
		private Image sandboxButton;

		// Token: 0x04000D9B RID: 3483
		[SerializeField]
		private TMP_Text readyText;

		// Token: 0x04000D9C RID: 3484
		private PersonalHistoryData personalHistoryData;

		// Token: 0x04000D9D RID: 3485
		private int currentStep = 1;

		// Token: 0x04000D9E RID: 3486
		private CameraMovement cameraMovement;

		// Token: 0x04000D9F RID: 3487
		private Color tutorialButtonColor;

		// Token: 0x04000DA0 RID: 3488
		private bool startTutorial;

		// Token: 0x04000DA1 RID: 3489
		private Vector2 screenSize;
	}
}
