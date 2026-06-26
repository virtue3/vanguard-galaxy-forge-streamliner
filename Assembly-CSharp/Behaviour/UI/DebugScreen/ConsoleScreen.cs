using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Behaviour.Crew;
using Behaviour.GalaxyMap;
using Behaviour.Item;
using Behaviour.Item.Builder;
using Behaviour.Item.Usable;
using Behaviour.Managers;
using Behaviour.UI.HUD;
using Behaviour.UI.Main_Menu;
using Behaviour.UI.Spacestation;
using Behaviour.UI.Spacestation.Bar;
using Behaviour.Unit;
using Behaviour.Util;
using Source.Combat;
using Source.Crew;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Mining;
using Source.MissionSystem;
using Source.MissionSystem.Story;
using Source.Player;
using Source.Simulation.Story;
using Source.Simulation.World;
using Source.Simulation.World.System;
using Source.SpaceShip;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Behaviour.UI.DebugScreen
{
	// Token: 0x02000296 RID: 662
	public class ConsoleScreen : Singleton<ConsoleScreen>
	{
		// Token: 0x06001846 RID: 6214 RVA: 0x00097E50 File Offset: 0x00096050
		protected override void Awake()
		{
			base.Awake();
			this.Toggle();
			this.commands.Add(new ConsoleCommand("AddCredits", "ac", new Action<string>(this.AddCredits), "*"));
			this.commands.Add(new ConsoleCommand("RemoveCredits", "rc", new Action<string>(this.RemoveCredits), "*"));
			this.commands.Add(new ConsoleCommand("AddSkillPoints", "asp", new Action<string>(this.AddSkillPoints), "*"));
			this.commands.Add(new ConsoleCommand("CreateShip", "cs", new Action<string>(this.CreateShip), "{name}"));
			this.commands.Add(new ConsoleCommand("ListShip", "ls", new Action<string>(this.ListShip), ""));
			this.commands.Add(new ConsoleCommand("UnlockAllGates", "uag", new Action<string>(this.UnlockAllGates), ""));
			this.commands.Add(new ConsoleCommand("RefreshMissionBoard", "rmb", new Action<string>(this.RefreshMissionBoard), ""));
			this.commands.Add(new ConsoleCommand("RefreshBar", "rbar", new Action<string>(this.RefreshBar), ""));
			this.commands.Add(new ConsoleCommand("RefreshRecruitmentCenter", "rrc", new Action<string>(this.RefreshRecruitmentCenter), "{profession}"));
			this.commands.Add(new ConsoleCommand("ToggleAutopilot", "ta", new Action<string>(this.ToggleAutopilot), ""));
			this.commands.Add(new ConsoleCommand("ToggleMapUsage", "mu", new Action<string>(this.ToggleMapUsage), ""));
			this.commands.Add(new ConsoleCommand("AddWarpFuel", "wf", new Action<string>(this.AddWarpFuel), ""));
			this.commands.Add(new ConsoleCommand("AddWarpFuel", "swf", new Action<string>(this.AddSuperWarpFuel), ""));
			this.commands.Add(new ConsoleCommand("SkipTutorial", "st", new Action<string>(this.SkipTutorial), ""));
			this.commands.Add(new ConsoleCommand("AddItem", "ai", new Action<string>(this.AddItem), "{item}"));
			this.commands.Add(new ConsoleCommand("CompleteMission", "cm", new Action<string>(this.CompleteMission), ""));
			this.commands.Add(new ConsoleCommand("RefundSkills", "rs", new Action<string>(this.RefundSkills), ""));
			this.commands.Add(new ConsoleCommand("AddSkillTree", "ast", new Action<string>(this.UnlockSkilltree), "{name}"));
			this.commands.Add(new ConsoleCommand("SetHP", "hp", new Action<string>(this.SetHP), ""));
			this.commands.Add(new ConsoleCommand("SetShield", "sp", new Action<string>(this.SetShieldsHp), ""));
			this.commands.Add(new ConsoleCommand("SetArmor", "ap", new Action<string>(this.SetArmorHp), ""));
			this.commands.Add(new ConsoleCommand("RefreshGeneralShop", "rgs", new Action<string>(this.RefreshGeneralShop), ""));
			this.commands.Add(new ConsoleCommand("RefreshMiningShop", "rms", new Action<string>(this.RefreshMiningShop), ""));
			this.commands.Add(new ConsoleCommand("RefreshSalvageShop", "rss", new Action<string>(this.RefreshSalvageShop), ""));
			this.commands.Add(new ConsoleCommand("RefreshBountyShop", "rbs", new Action<string>(this.RefreshBountyShop), ""));
			this.commands.Add(new ConsoleCommand("RefreshPatrolShop", "rps", new Action<string>(this.RefreshPatrolShop), ""));
			this.commands.Add(new ConsoleCommand("RefreshIndustryShop", "ris", new Action<string>(this.RefreshIndustryShop), ""));
			this.commands.Add(new ConsoleCommand("RefreshConquestShop", "rcs", new Action<string>(this.RefreshConquestShop), ""));
			this.commands.Add(new ConsoleCommand("RefreshUmbralShop", "rus", new Action<string>(this.RefreshUmbralShop), ""));
			this.commands.Add(new ConsoleCommand("RecreateSandboxWorld", "rsw", new Action<string>(this.RecreateSandboxWorld), ""));
			this.commands.Add(new ConsoleCommand("AddMasteryXp", "amm", new Action<string>(this.AddMasteryXp), "{name}"));
			this.commands.Add(new ConsoleCommand("AddXp", "axp", new Action<string>(this.AddXp), ""));
			this.commands.Add(new ConsoleCommand("AddXpToCrew", "acxp", new Action<string>(this.AddXpToCrew), ""));
			this.commands.Add(new ConsoleCommand("AddDamageInRange", "adir", new Action<string>(this.AddDamageInRange), ""));
			this.commands.Add(new ConsoleCommand("FindLocation", "whereis", new Action<string>(this.FindLocation), "{identifier}"));
			this.commands.Add(new ConsoleCommand("Teleport", "goto", new Action<string>(ConsoleScreen.Teleport), "{guid}"));
			this.commands.Add(new ConsoleCommand("FindTeleport", "gotofirst", new Action<string>(this.FindTeleport), "{identifier}"));
			this.commands.Add(new ConsoleCommand("SetReputation", "setrep", new Action<string>(this.SetReputation), "*"));
			this.commands.Add(new ConsoleCommand("AddReputation", "addrep", new Action<string>(this.ChangeReputation), "*"));
			this.commands.Add(new ConsoleCommand("CancelTravel", "ct", new Action<string>(this.CancelTravel), "*"));
			this.commands.Add(new ConsoleCommand("FillRefinery", "fr", new Action<string>(this.FillRefinery), "*"));
			this.commands.Add(new ConsoleCommand("Pause", "pause", new Action<string>(ConsoleScreen.Pause), ""));
			this.commands.Add(new ConsoleCommand("SpawnMercenary", "sm", new Action<string>(ConsoleScreen.SpawnMercenary), ""));
			this.commands.Add(new ConsoleCommand("RemoveMercenary", "rm", new Action<string>(this.RemoveMercenary), ""));
			this.commands.Add(new ConsoleCommand("SpawnEnemy", "se", new Action<string>(this.SpawnEnemy), "{shipname} | *"));
			this.commands.Add(new ConsoleCommand("SpawnFriendly", "sf", new Action<string>(this.SpawnFriendly), "{shipname} | *"));
			this.commands.Add(new ConsoleCommand("SpawnLootBox", "slb", new Action<string>(ConsoleScreen.SpawnLootBox), ""));
			this.commands.Add(new ConsoleCommand("FpsCounter", "fps", new Action<string>(this.ToggleFpsCounter), ""));
			this.commands.Add(new ConsoleCommand("TimeScale", "ts", new Action<string>(this.SetTimeScale), "*"));
			this.commands.Add(new ConsoleCommand("DeleteMission", "dm", new Action<string>(this.DeleteMission), "{storyId}"));
			this.commands.Add(new ConsoleCommand("AddMission", "am", new Action<string>(this.AddMission), "{storyId}"));
			this.commands.Add(new ConsoleCommand("CrewPod", "cp", new Action<string>(this.SpawnCrewPod), ""));
			this.commands.Add(new ConsoleCommand("OfficerPod", "op", new Action<string>(this.SpawnOfficerPod), ""));
			this.commands.Add(new ConsoleCommand("JettisonCrew", "jc", new Action<string>(this.JettisonAllCrew), ""));
			this.commands.Add(new ConsoleCommand("RegenerateConquestSector", "rcs", new Action<string>(this.RegenerateConquestSector), ""));
			this.commands.Add(new ConsoleCommand("ConquestTick", "ctick", new Action<string>(this.ConquestTick), ""));
			this.commands.Add(new ConsoleCommand("UnlockFastLaneTravel", "uft", new Action<string>(this.UnlockFastLaneTravel), ""));
			this.commands.Add(new ConsoleCommand("SpawnAsteroid", "sa", new Action<string>(this.SpawnAsteroid), ""));
			this.commands.Add(new ConsoleCommand("FindFactionOnMap", "ff", new Action<string>(this.FindFactionOnMap), ""));
			this.commands.Add(new ConsoleCommand("ConquestFactionZero", "cfz", new Action<string>(this.ConquestFactionZero), ""));
			this.commands.Add(new ConsoleCommand("EconomyTick", "etick", new Action<string>(this.EconomyTick), ""));
			this.commands.Add(new ConsoleCommand("HideLoadingScreen", "hls", new Action<string>(this.HideLoadingScreen), ""));
			this.commands.Add(new ConsoleCommand("UnlockSkilltreeTier2", "ut2", new Action<string>(this.UnlockSkilltreeTier2), ""));
			this.commands.Add(new ConsoleCommand("AddCharacter", "adc", new Action<string>(this.AddCharacter), ""));
			this.commands.Add(new ConsoleCommand("UnlockSalvageWorkshopStuff", "usw", new Action<string>(this.UnlockSalWorkshopTradein), ""));
			this.commands.Add(new ConsoleCommand("RefreshUmbralBoard", "rub", new Action<string>(this.RefreshUmbralMission), ""));
		}

		// Token: 0x06001847 RID: 6215 RVA: 0x0009897F File Offset: 0x00096B7F
		private void Update()
		{
			if (this.restartInput)
			{
				this.restartInput = false;
				this.debugInput.ActivateInputField();
				this.debugInput.onSelect.Invoke("");
			}
		}

		// Token: 0x06001848 RID: 6216 RVA: 0x000989B0 File Offset: 0x00096BB0
		public void AutoScrollToggleChange()
		{
			this.autoScroll = this.autoScrollToggle.isOn;
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x000989C3 File Offset: 0x00096BC3
		public void Toggle()
		{
			if (base.isActiveAndEnabled)
			{
				base.gameObject.SetActive(false);
				return;
			}
			if (ConsoleScreen.ConsoleAvailable())
			{
				base.gameObject.SetActive(true);
				this.debugInput.ActivateInputField();
			}
		}

		// Token: 0x0600184A RID: 6218 RVA: 0x000989F8 File Offset: 0x00096BF8
		public void AddTextToConsole(string text)
		{
			if (!this.debugOutput)
			{
				return;
			}
			TextMeshProUGUI textMeshProUGUI = this.debugOutput;
			textMeshProUGUI.text = textMeshProUGUI.text + "\n" + text;
			if (this.scrollRect && this.autoScroll)
			{
				this.scrollRect.normalizedPosition = Vector2.zero;
			}
			if (this.debugOutput.text.Length > 10000)
			{
				this.debugOutput.text = "";
			}
		}

		// Token: 0x0600184B RID: 6219 RVA: 0x00098A7C File Offset: 0x00096C7C
		public void OnDebugInputChanged()
		{
			if (!Input.GetKeyDown(KeyCode.Return))
			{
				return;
			}
			string text = this.debugInput.text;
			string text2 = null;
			bool flag = false;
			foreach (ConsoleCommand consoleCommand in this.commands)
			{
				if (text == consoleCommand.command || text.StartsWith(consoleCommand.command + " "))
				{
					flag = true;
					text2 = text.Substring(consoleCommand.command.Length);
				}
				else if (text == consoleCommand.shortCommand || text.StartsWith(consoleCommand.shortCommand + " "))
				{
					flag = true;
					text2 = text.Substring(consoleCommand.shortCommand.Length);
				}
				if (flag)
				{
					consoleCommand.action(text2.Trim());
					break;
				}
			}
			if (!flag)
			{
				this.AddTextToConsole("Command not recognized, available commands:");
				foreach (ConsoleCommand consoleCommand2 in this.commands)
				{
					this.AddTextToConsole(string.Concat(new string[]
					{
						consoleCommand2.command,
						" ",
						consoleCommand2.description,
						" | ",
						consoleCommand2.shortCommand
					}));
				}
			}
			this.debugInput.text = "";
			this.restartInput = true;
		}

		// Token: 0x0600184C RID: 6220 RVA: 0x00098C1C File Offset: 0x00096E1C
		public bool SkipLevelCheck()
		{
			return this.createShipUsed;
		}

		// Token: 0x0600184D RID: 6221 RVA: 0x00098C24 File Offset: 0x00096E24
		public static bool ConsoleAvailable()
		{
			return File.Exists("console.enabled");
		}

		// Token: 0x0600184E RID: 6222 RVA: 0x00098C30 File Offset: 0x00096E30
		public static bool DebugModifier()
		{
			return ConsoleScreen.ConsoleAvailable() && (Keyboard.current.altKey.isPressed && Keyboard.current.shiftKey.isPressed) && Keyboard.current.ctrlKey.isPressed;
		}

		// Token: 0x0600184F RID: 6223 RVA: 0x00098C70 File Offset: 0x00096E70
		private void AddCredits(string input)
		{
			int? num = this.ParseInt(input, "Credits");
			if (num != null)
			{
				GamePlayer.current.credits += (long)num.Value;
				string str = "Added ";
				int? num2 = num;
				this.AddTextToConsole(str + num2.ToString() + " credits");
			}
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x00098CD0 File Offset: 0x00096ED0
		private void RemoveCredits(string input)
		{
			int? num = this.ParseInt(input, "Credits");
			if (num != null)
			{
				GamePlayer.current.credits = Math.Max(0L, GamePlayer.current.credits - (long)num.Value);
				string str = "Removed ";
				int? num2 = num;
				this.AddTextToConsole(str + num2.ToString() + " credits");
			}
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x00098D3C File Offset: 0x00096F3C
		private void AddSkillPoints(string input)
		{
			int? num = this.ParseInt(input, "Skillpoints");
			if (num != null)
			{
				GamePlayer.current.commander.TryGiveBonusSkillPoints(num.Value, true);
				string str = "Added ";
				int? num2 = num;
				this.AddTextToConsole(str + num2.ToString() + " skillpoints");
			}
		}

		// Token: 0x06001852 RID: 6226 RVA: 0x00098D9B File Offset: 0x00096F9B
		private void UnlockSkilltree(string input)
		{
			GamePlayer.current.commander.UnlockSkilltree(input.Trim());
			this.AddTextToConsole("Maybe you unlocked skilltree " + input.Trim());
		}

		// Token: 0x06001853 RID: 6227 RVA: 0x00098DC8 File Offset: 0x00096FC8
		private void SetHP(string input)
		{
			int num;
			if (!int.TryParse(input, out num) || num < 0)
			{
				Debug.LogWarning("Invalid HP input");
				return;
			}
			GamePlayer current = GamePlayer.current;
			SpaceShipData spaceShipData = (current != null) ? current.currentSpaceShip : null;
			if (spaceShipData == null)
			{
				Debug.LogWarning("No spaceship found");
				return;
			}
			GamePlayer.current.emergencyJump = false;
			float num2 = (float)num - spaceShipData.currentHullHP;
			if (num2 > 0f)
			{
				spaceShipData.RepairHullHp(num2);
			}
			else
			{
				spaceShipData.currentHullHP = (float)num;
			}
			this.AddTextToConsole("HP set to " + Mathf.Clamp((float)num, 0f, spaceShipData.maxHullHP).ToString());
		}

		// Token: 0x06001854 RID: 6228 RVA: 0x00098E68 File Offset: 0x00097068
		private void SetShieldsHp(string input)
		{
			int num;
			if (!int.TryParse(input, out num) || num < 0)
			{
				Debug.LogWarning("Invalid HP input");
				return;
			}
			GamePlayer current = GamePlayer.current;
			SpaceShipData spaceShipData = (current != null) ? current.currentSpaceShip : null;
			if (spaceShipData == null)
			{
				Debug.LogWarning("No spaceship found");
				return;
			}
			float num2 = (float)num - spaceShipData.currentShieldHP;
			if (num2 > 0f)
			{
				spaceShipData.RepairShieldHp(num2);
			}
			else
			{
				spaceShipData.currentShieldHP = (float)num;
			}
			this.AddTextToConsole("HP set to " + Mathf.Clamp((float)num, 0f, spaceShipData.maxShieldHP).ToString());
		}

		// Token: 0x06001855 RID: 6229 RVA: 0x00098EFC File Offset: 0x000970FC
		private void SetArmorHp(string input)
		{
			int num;
			if (!int.TryParse(input, out num) || num < 0)
			{
				Debug.LogWarning("Invalid HP input");
				return;
			}
			GamePlayer current = GamePlayer.current;
			SpaceShipData spaceShipData = (current != null) ? current.currentSpaceShip : null;
			if (spaceShipData == null)
			{
				Debug.LogWarning("No spaceship found");
				return;
			}
			float num2 = (float)num - spaceShipData.currentArmorHP;
			if (num2 > 0f)
			{
				spaceShipData.RepairArmorHP(num2);
			}
			else
			{
				spaceShipData.currentArmorHP = (float)num;
			}
			this.AddTextToConsole("HP set to " + Mathf.Clamp((float)num, 0f, spaceShipData.maxArmorHP).ToString());
		}

		// Token: 0x06001856 RID: 6230 RVA: 0x00098F90 File Offset: 0x00097190
		private void RefreshGeneralShop(string input)
		{
			SpaceStation spaceStation = MapPointOfInterest.current as SpaceStation;
			if (spaceStation != null && spaceStation.HasFacility(SpaceStationFacility.GeneralShop))
			{
				spaceStation.GenerateShopInventory();
				SpaceStationInterior instance = SpaceStationInterior.instance;
				if (instance != null && instance.currentTab == SpaceStationFacility.GeneralShop)
				{
					SpaceStationInterior.instance.GoToLocation(SpaceStationInterior.instance.currentTab, true);
				}
			}
			this.AddTextToConsole("Refreshed General Shop");
		}

		// Token: 0x06001857 RID: 6231 RVA: 0x00098FF0 File Offset: 0x000971F0
		private void RefreshMiningShop(string input)
		{
			SpaceStation spaceStation = MapPointOfInterest.current as SpaceStation;
			if (spaceStation != null && spaceStation.HasFacility(SpaceStationFacility.MiningShop))
			{
				spaceStation.GenerateMiningShopInventory();
				SpaceStationInterior instance = SpaceStationInterior.instance;
				if (instance != null && instance.currentTab == SpaceStationFacility.MiningShop)
				{
					SpaceStationInterior.instance.GoToLocation(SpaceStationInterior.instance.currentTab, true);
				}
			}
			this.AddTextToConsole("Refreshed Mining Shop");
		}

		// Token: 0x06001858 RID: 6232 RVA: 0x00099050 File Offset: 0x00097250
		private void RefreshSalvageShop(string input)
		{
			SpaceStation spaceStation = MapPointOfInterest.current as SpaceStation;
			if (spaceStation != null && spaceStation.HasFacility(SpaceStationFacility.SalvageShop))
			{
				spaceStation.GenerateSalvageShopInventory();
				SpaceStationInterior instance = SpaceStationInterior.instance;
				if (instance != null && instance.currentTab == SpaceStationFacility.SalvageShop)
				{
					SpaceStationInterior.instance.GoToLocation(SpaceStationInterior.instance.currentTab, true);
				}
			}
			this.AddTextToConsole("Refreshed Salvage Shop");
		}

		// Token: 0x06001859 RID: 6233 RVA: 0x000990B4 File Offset: 0x000972B4
		private void RefreshBountyShop(string input)
		{
			SpaceStation spaceStation = MapPointOfInterest.current as SpaceStation;
			if (spaceStation != null && spaceStation.HasFacility(SpaceStationFacility.BountyShop))
			{
				spaceStation.GenerateBountyShopInventory();
				SpaceStationInterior instance = SpaceStationInterior.instance;
				if (instance != null && instance.currentTab == SpaceStationFacility.BountyShop)
				{
					SpaceStationInterior.instance.GoToLocation(SpaceStationInterior.instance.currentTab, true);
				}
			}
			this.AddTextToConsole("Refreshed Bounty Shop");
		}

		// Token: 0x0600185A RID: 6234 RVA: 0x00099118 File Offset: 0x00097318
		private void RefreshPatrolShop(string input)
		{
			SpaceStation spaceStation = MapPointOfInterest.current as SpaceStation;
			if (spaceStation != null && spaceStation.HasFacility(SpaceStationFacility.PatrolShop))
			{
				spaceStation.GeneratePatrolShopInventory();
				SpaceStationInterior instance = SpaceStationInterior.instance;
				if (instance != null && instance.currentTab == SpaceStationFacility.PatrolShop)
				{
					SpaceStationInterior.instance.GoToLocation(SpaceStationInterior.instance.currentTab, true);
				}
			}
			this.AddTextToConsole("Refreshed Patrol Shop");
		}

		// Token: 0x0600185B RID: 6235 RVA: 0x0009917C File Offset: 0x0009737C
		private void RefreshIndustryShop(string input)
		{
			SpaceStation spaceStation = MapPointOfInterest.current as SpaceStation;
			if (spaceStation != null && spaceStation.HasFacility(SpaceStationFacility.IndustryShop))
			{
				spaceStation.GenerateIndustryShopInventory();
				SpaceStationInterior instance = SpaceStationInterior.instance;
				if (instance != null && instance.currentTab == SpaceStationFacility.IndustryShop)
				{
					SpaceStationInterior.instance.GoToLocation(SpaceStationInterior.instance.currentTab, true);
				}
			}
			this.AddTextToConsole("Refreshed Industry Shop");
		}

		// Token: 0x0600185C RID: 6236 RVA: 0x000991E0 File Offset: 0x000973E0
		private void RefreshConquestShop(string input)
		{
			SpaceStation spaceStation = MapPointOfInterest.current as SpaceStation;
			if (spaceStation != null && spaceStation.HasFacility(SpaceStationFacility.ConquestShop))
			{
				spaceStation.GenerateConquestShopInventory();
				SpaceStationInterior instance = SpaceStationInterior.instance;
				if (instance != null && instance.currentTab == SpaceStationFacility.ConquestShop)
				{
					SpaceStationInterior.instance.GoToLocation(SpaceStationInterior.instance.currentTab, true);
				}
			}
			this.AddTextToConsole("Refreshed Conquest Shop");
		}

		// Token: 0x0600185D RID: 6237 RVA: 0x00099244 File Offset: 0x00097444
		private void RefreshUmbralShop(string input)
		{
			SpaceStation spaceStation = MapPointOfInterest.current as SpaceStation;
			if (spaceStation != null && spaceStation.umbralShopInventory != null)
			{
				spaceStation.GenerateUmbralShopInventory();
				SpaceStationInterior.instance.GoToLocation(SpaceStationInterior.instance.currentTab, true);
			}
			this.AddTextToConsole("Refreshed Umbral Shop");
		}

		// Token: 0x0600185E RID: 6238 RVA: 0x0009928D File Offset: 0x0009748D
		private void RefreshUmbralMission(string input)
		{
			if (SpaceStation.current != null)
			{
				SpaceStation.current.missionBoard.GetUmbralMission(true);
				SpaceStationInterior.instance.GoToLocation(SpaceStationInterior.instance.currentTab, true);
				this.AddTextToConsole("New Umbral Mission Generated");
			}
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x000992C8 File Offset: 0x000974C8
		private int? ParseInt(string input, string name)
		{
			int? result;
			try
			{
				int num = int.Parse(input);
				if (num <= 0)
				{
					this.AddTextToConsole(name + " must be greater than 0");
				}
				result = new int?(num);
			}
			catch (FormatException ex)
			{
				this.AddTextToConsole("Invalid " + name + ": " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x06001860 RID: 6240 RVA: 0x00099334 File Offset: 0x00097534
		private void CreateShip(string input)
		{
			if (Singleton<TravelManager>.Instance.TravelActive())
			{
				this.AddTextToConsole("Wait for travel to end");
				return;
			}
			string text = input.Trim();
			if (SpaceShip.SpaceShipExists(text))
			{
				this.createShipUsed = true;
				Vector2 vector = GameplayManager.Instance.spaceShip.transform.position;
				GameplayManager.Instance.GeneratePlayerSpaceship(vector, 100, text, true);
				GamePlayer.current.spaceShips.Add(GamePlayer.current.currentSpaceShip);
				string str = "Adding ";
				string str2 = text;
				string str3 = ", position: ";
				Vector2 vector2 = vector;
				this.AddTextToConsole(str + str2 + str3 + vector2.ToString());
				this.createShipUsed = false;
				return;
			}
			this.AddTextToConsole("Cannot find ship with name: " + text);
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x000993F0 File Offset: 0x000975F0
		private void ListShip(string input)
		{
			this.AddTextToConsole("Shipnames:");
			foreach (string text in SpaceShip.allShips.Keys)
			{
				this.AddTextToConsole(text);
			}
		}

		// Token: 0x06001862 RID: 6242 RVA: 0x00099454 File Offset: 0x00097654
		private void UnlockAllGates(string input)
		{
			foreach (SystemMapData systemMapData in GamePlayer.current.map.allSystems)
			{
				foreach (MapPointOfInterest mapPointOfInterest in systemMapData.pointsOfInterest)
				{
					JumpGate jumpGate = mapPointOfInterest as JumpGate;
					if (jumpGate != null)
					{
						jumpGate.UnlockJumpgate();
					}
				}
			}
			this.AddTextToConsole("All gates unlocked");
		}

		// Token: 0x06001863 RID: 6243 RVA: 0x000994F8 File Offset: 0x000976F8
		private void RefreshMissionBoard(string input)
		{
			SpaceStation spaceStation = MapPointOfInterest.current as SpaceStation;
			if (spaceStation != null && spaceStation.HasFacility(SpaceStationFacility.MissionBoard))
			{
				SpaceStationInterior instance = SpaceStationInterior.instance;
				MissionBoard missionBoard = (instance != null) ? instance.GetComponentInChildren<MissionBoard>() : null;
				if (missionBoard)
				{
					missionBoard.GenerateNewMissions();
				}
				else
				{
					spaceStation.missionBoard.RegenerateMissions(6);
				}
				this.AddTextToConsole("Local mission board refreshed");
				return;
			}
			this.AddTextToConsole("Not at space station / mission board!");
		}

		// Token: 0x06001864 RID: 6244 RVA: 0x00099564 File Offset: 0x00097764
		private void RefreshBar(string input)
		{
			SpaceStation spaceStation = MapPointOfInterest.current as SpaceStation;
			if (spaceStation != null && spaceStation.HasFacility(SpaceStationFacility.Bar))
			{
				SpaceStationInterior instance = SpaceStationInterior.instance;
				BarUI barUI = (instance != null) ? instance.GetComponentInChildren<BarUI>() : null;
				spaceStation.bar.CheckUpdatePatrons(true);
				if (barUI)
				{
					barUI.RefreshPatrons();
				}
				this.AddTextToConsole("Bar refreshed");
				return;
			}
			this.AddTextToConsole("Not at space station / bar!");
		}

		// Token: 0x06001865 RID: 6245 RVA: 0x000995CC File Offset: 0x000977CC
		private void RefreshRecruitmentCenter(string input)
		{
			SpaceStation spaceStation = MapPointOfInterest.current as SpaceStation;
			if (spaceStation != null && spaceStation.HasFacility(SpaceStationFacility.RecruitmentCenter))
			{
				Profession profession = Enum.Parse<Profession>(input);
				if (profession == Profession.None)
				{
					profession = Profession.Combat;
				}
				SpaceStation.current.recruitmentCenter.CreateRecruits(profession, true);
				this.AddTextToConsole("Recruits refreshed");
				return;
			}
			this.AddTextToConsole("Not at space station with recruitment center");
		}

		// Token: 0x06001866 RID: 6246 RVA: 0x00099628 File Offset: 0x00097828
		private void ToggleAutopilot(string input)
		{
			GamePlayer.current.SetAutoPlayUsage(!GamePlayer.current.autoPlayUnlocked);
			string str = "enabled";
			if (!GamePlayer.current.autoPlayUnlocked)
			{
				str = "disabled";
			}
			this.AddTextToConsole("Autopilot " + str);
		}

		// Token: 0x06001867 RID: 6247 RVA: 0x00099678 File Offset: 0x00097878
		private void ToggleMapUsage(string input)
		{
			GamePlayer.current.SetMapUsage(!GamePlayer.current.mapUnlocked);
			string str = "enabled";
			if (!GamePlayer.current.mapUnlocked)
			{
				str = "disabled";
			}
			this.AddTextToConsole("Map " + str);
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x000996C5 File Offset: 0x000978C5
		private void AddWarpFuel(string input)
		{
			GamePlayer.current.currentSpaceShip.AddCargo(ItemBuilder.Get("WarpFuel").CreateWarpFuel(WarpFuelItem.WarpFuelType.PlasmaCell, 1f), 1, true);
			this.AddTextToConsole("Plasma Cell added");
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x000996F9 File Offset: 0x000978F9
		private void AddSuperWarpFuel(string input)
		{
			GamePlayer.current.currentSpaceShip.AddCargo(ItemBuilder.Get("WarpFuel").CreateWarpFuel(WarpFuelItem.WarpFuelType.HyperCell, 1f), 1, true);
			this.AddTextToConsole("Hyper Cell added");
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x00099730 File Offset: 0x00097930
		private void AddItem(string input)
		{
			string text = input.Trim();
			int count = 1;
			Match match = new Regex("(.*?) ([0-9]+)$").Match(text);
			if (match.Success)
			{
				text = match.Groups[1].Value.Trim();
				count = int.Parse(match.Groups[2].Value.Trim());
			}
			InventoryItemType item;
			if (InventoryItemType.TryGet(text, out item))
			{
				GamePlayer.current.currentSpaceShip.AddCargo(item, count, true);
				this.AddTextToConsole(text + " added");
				return;
			}
			this.AddTextToConsole("No item named " + text);
		}

		// Token: 0x0600186B RID: 6251 RVA: 0x000997D4 File Offset: 0x000979D4
		private void SkipTutorial(string input)
		{
			GamePlayer.current.currentSpaceShip.AddCargo(ItemBuilder.Get("WarpFuel").CreateWarpFuel(WarpFuelItem.WarpFuelType.HyperCell, 1f), 1, true);
			GamePlayer.current.SetMapUsage(true);
			GamePlayer.current.SetAutoPlayUsage(true);
			for (int i = 0; i < GamePlayer.current.missions.Count; i++)
			{
				Mission mission = GamePlayer.current.missions[i];
				string storyId = mission.storyId;
				if (storyId != null && storyId.StartsWith("tutorial"))
				{
					GamePlayer.current.RemoveMission(mission, false);
					i--;
				}
			}
			foreach (StoryMission storyMission in StoryMission.all)
			{
				if (storyMission.identifier.StartsWith("tutorial"))
				{
					GamePlayer.current.ArchiveMission(storyMission.identifier, false);
				}
			}
			SystemMapData systemMapData;
			SystemMapData systemMapData2;
			TutorialMissions.CreateJumpGateLinkToSandbox(out systemMapData, out systemMapData2);
			this.AddTextToConsole("Skipped Tutorial");
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x000998EC File Offset: 0x00097AEC
		private void CompleteMission(string input)
		{
			Mission mission = null;
			foreach (Mission mission2 in GamePlayer.current.missions)
			{
				if (mission2.trackedOnHud)
				{
					mission = mission2;
					break;
				}
			}
			if (mission != null)
			{
				GamePlayer.current.CompleteMission(mission, true);
				return;
			}
			this.AddTextToConsole("No active mission!");
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x00099968 File Offset: 0x00097B68
		private void RefundSkills(string input)
		{
			GamePlayer.current.commander.RefundAllSkills(true);
			this.AddTextToConsole("All skills refunded.");
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x00099988 File Offset: 0x00097B88
		private void RecreateSandboxWorld(string input)
		{
			GamePlayer.current.missions.Clear();
			GamePlayer.current.map.ClearSectors();
			SandboxWorld.SetupWorld(GamePlayer.current);
			using (IEnumerator<MapPointOfInterest> enumerator = GalaxyMapData.current.allPointsOfInterest.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					ConsoleScreen.Teleport(enumerator.Current.guid);
				}
			}
		}

		// Token: 0x0600186F RID: 6255 RVA: 0x00099A08 File Offset: 0x00097C08
		private void AddMasteryXp(string input)
		{
			string b = input.Trim();
			foreach (SkillTreeData skillTreeData in GamePlayer.current.commander.skillTrees)
			{
				if (skillTreeData.skilltree.identifier == b)
				{
					skillTreeData.AddMasteryXp(1500f);
					GameplayManager.Instance.spaceShip.UpdateCommanderSkills();
				}
			}
		}

		// Token: 0x06001870 RID: 6256 RVA: 0x00099A94 File Offset: 0x00097C94
		private void AddXp(string input)
		{
			int? num = this.ParseInt(input, "XP");
			if (num != null)
			{
				GamePlayer.current.commander.GiveExperience((float)num.Value);
				string str = "Added ";
				int? num2 = num;
				this.AddTextToConsole(str + num2.ToString() + " XP to commander");
			}
		}

		// Token: 0x06001871 RID: 6257 RVA: 0x00099AF4 File Offset: 0x00097CF4
		private void AddXpToCrew(string input)
		{
			int? num = this.ParseInt(input, "XP");
			if (num != null)
			{
				foreach (CrewMemberData crewMemberData in GamePlayer.current.crewMembers)
				{
					crewMemberData.GiveExperience((float)num.Value);
				}
				string str = "Added ";
				int? num2 = num;
				this.AddTextToConsole(str + num2.ToString() + " XP to crew");
			}
		}

		// Token: 0x06001872 RID: 6258 RVA: 0x00099B8C File Offset: 0x00097D8C
		private void AddDamageInRange(string input)
		{
			object obj;
			if (Enum.TryParse(typeof(DamageType), input, true, out obj))
			{
				BasePoiManager.current.AddDamageInRadiusToNearbyAsteroids((DamageType)obj);
				return;
			}
			this.AddTextToConsole("Invalid damageType " + input + ", try one of: Cold, Corrosion, Heat or Radiation");
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x00099BD8 File Offset: 0x00097DD8
		private void FindLocation(string type)
		{
			foreach (MapPointOfInterest mapPointOfInterest in GalaxyMapData.current.allPointsOfInterest)
			{
				if (mapPointOfInterest.GetType().Name == type)
				{
					string text = string.Concat(new string[]
					{
						"POI ",
						mapPointOfInterest.name,
						" (",
						mapPointOfInterest.level.ToString(),
						"): ",
						mapPointOfInterest.guid
					});
					this.AddTextToConsole(text);
					Debug.Log(text);
				}
			}
		}

		// Token: 0x06001874 RID: 6260 RVA: 0x00099C88 File Offset: 0x00097E88
		private void FindTeleport(string type)
		{
			foreach (MapPointOfInterest mapPointOfInterest in GalaxyMapData.current.allPointsOfInterest)
			{
				if (mapPointOfInterest.GetType().Name == type)
				{
					ConsoleScreen.Teleport(mapPointOfInterest.guid);
					break;
				}
			}
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x00099CF4 File Offset: 0x00097EF4
		private void SetReputation(string input)
		{
			this.HandleReputation(input, false);
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x00099CFE File Offset: 0x00097EFE
		private void ChangeReputation(string input)
		{
			this.HandleReputation(input, true);
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x00099D08 File Offset: 0x00097F08
		private void CancelTravel(string input)
		{
			if (Singleton<TravelManager>.Current.CancelTravel(null))
			{
				this.AddTextToConsole("Travel canceled.");
				return;
			}
			this.AddTextToConsole("Travel NOT canceled.");
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x00099D44 File Offset: 0x00097F44
		private void FillRefinery(string input)
		{
			int? num = this.ParseInt(input, "fr");
			if (num != null)
			{
				GamePlayer.current.FillRefinery((float)num.Value);
			}
			this.AddTextToConsole(string.Format("Added {0} to refined materials in refinery.", num));
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x00099D90 File Offset: 0x00097F90
		private void HandleReputation(string input, bool isChange)
		{
			string[] array = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			if (array.Length < 2)
			{
				this.AddTextToConsole("Invalid reputation input: " + input);
				return;
			}
			string text = array[0];
			string text2 = array[1];
			int num;
			if (!int.TryParse(text2, out num))
			{
				this.AddTextToConsole("Invalid reputation value: " + text2);
				return;
			}
			Faction faction = Faction.Get(text);
			if (faction == null)
			{
				this.AddTextToConsole("Faction not found: " + text);
				return;
			}
			if (isChange)
			{
				Faction.player.ChangeFactionReputation(faction, num);
				this.AddTextToConsole("Faction Reputation " + Translation.Translate(faction.name, Array.Empty<object>()) + " changed by: " + text2);
				return;
			}
			Faction.player.SetReputation(faction, num);
			this.AddTextToConsole("Faction Reputation " + Translation.Translate(faction.name, Array.Empty<object>()) + " set to: " + text2);
		}

		// Token: 0x0600187A RID: 6266 RVA: 0x00099E70 File Offset: 0x00098070
		public static void Teleport(string guid)
		{
			MapPointOfInterest pointOfInterest = GalaxyMapData.current.GetPointOfInterest(guid);
			GamePlayer.current.currentSystem = pointOfInterest.system;
			GamePlayer.current.currentPointOfInterest = pointOfInterest;
			GamePlayer.current.mapPosition = pointOfInterest.GetWorldPosition();
			GamePlayer.current.UpdateFleetPosition(pointOfInterest.GetWorldPosition());
			GameplayManager.Instance.SetFleetPosition(pointOfInterest.GetWorldPosition());
			PersistentSingleton<GameManager>.Instance.Reset();
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x00099EDE File Offset: 0x000980DE
		public static void Pause(string bah)
		{
			if (GameManager.isPaused)
			{
				GameManager.Unpause();
				return;
			}
			GameManager.Pause();
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x00099EF4 File Offset: 0x000980F4
		public static void SpawnMercenary(string input)
		{
			MapPointOfInterest current = MapPointOfInterest.current;
			MapPointOfInterest current2 = MapPointOfInterest.current;
			MapPointOfInterest mapPointOfInterest = current;
			float pointsScale = 1f;
			Faction policeGuild = Faction.policeGuild;
			current2.AddTriggeredSpawn(mapPointOfInterest.CreateUnitPayload(pointsScale, new GameplayType?(GameplayType.Combat), policeGuild, 0, 0, 1, 5, null), 0f, 0, false, true);
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x00099F3F File Offset: 0x0009813F
		public void RemoveMercenary(string input)
		{
			if (GamePlayer.current.hiredMercenary != null)
			{
				GamePlayer.current.RemoveHiredMercenary(false);
				this.AddTextToConsole("Removed mercenary.");
				return;
			}
			this.AddTextToConsole("No mercenary found.");
		}

		// Token: 0x0600187E RID: 6270 RVA: 0x00099F70 File Offset: 0x00098170
		private void SpawnShip(string input, bool hostile)
		{
			string text = input.Trim();
			int unitCount;
			bool flag = int.TryParse(text, out unitCount);
			if (!flag)
			{
				unitCount = 1;
			}
			string text2 = null;
			if (text != "" && SpaceShip.SpaceShipExists(text))
			{
				text2 = text;
				this.AddTextToConsole("Found shipname, trying to create: " + text2);
			}
			else if (!flag && text != "")
			{
				this.AddTextToConsole("Cannot find ship with name: " + text);
				return;
			}
			Faction f = hostile ? (Faction.player.IsEnemy(Faction.marauders) ? Faction.marauders : Faction.fanatics) : Faction.player;
			MapPointOfInterest current = MapPointOfInterest.current;
			current.AddTriggeredSpawn(current.CreateFixedPayload(text2, unitCount, f, null, UnitRank.Rookie), 0f, 0, false, true);
		}

		// Token: 0x0600187F RID: 6271 RVA: 0x0009A031 File Offset: 0x00098231
		public void SpawnEnemy(string input)
		{
			this.SpawnShip(input, true);
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x0009A03B File Offset: 0x0009823B
		public void SpawnFriendly(string input)
		{
			this.SpawnShip(input, false);
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x0009A045 File Offset: 0x00098245
		public static void SpawnLootBox(string input)
		{
			Singleton<LootManager>.Instance.CreateLootBox(MapPointOfInterest.current.level, GameplayManager.Instance.spaceShip.transform.position);
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x0009A074 File Offset: 0x00098274
		public void SpawnAsteroid(string input)
		{
			int amount;
			if (int.TryParse(input.Trim(), out amount))
			{
				this.AddTextToConsole("Found count, trying to create: " + amount.ToString() + " asteroids");
			}
			else
			{
				amount = 1;
			}
			if (!MapPointOfInterest.current.hasAsteroids)
			{
				this.AddTextToConsole("No asteroidField data found");
				return;
			}
			AsteroidFieldData asteroidFieldData = MapPointOfInterest.current.asteroidFieldData;
			int amount2 = asteroidFieldData.amount;
			asteroidFieldData.SetAmount(amount);
			MiningPoiHelper.CreateNewAsteroids(MapPointOfInterest.current, asteroidFieldData, false, true);
			asteroidFieldData.SetAmount(amount2);
			this.AddTextToConsole(amount.ToString() + " asteroids added");
		}

		// Token: 0x06001883 RID: 6275 RVA: 0x0009A10C File Offset: 0x0009830C
		public void FindFactionOnMap(string input)
		{
			if (AbstractGalaxyMapManager.current == null)
			{
				this.AddTextToConsole("Map needs to be openend");
				return;
			}
			string text = input.Trim();
			Faction faction = Faction.Get(text);
			if (faction == null)
			{
				this.AddTextToConsole("Faction not found: " + text);
				return;
			}
			AbstractGalaxyMapManager.current.FilterByFaction(faction);
			this.AddTextToConsole("Test showing faction on map: " + Translation.Translate(faction.name, Array.Empty<object>()));
		}

		// Token: 0x06001884 RID: 6276 RVA: 0x0009A180 File Offset: 0x00098380
		private void ToggleFpsCounter(string input)
		{
			this.AddTextToConsole("Toggling fps counter");
			HudManager.Instance.ToggleFpsCounter();
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x0009A198 File Offset: 0x00098398
		private void SetTimeScale(string input)
		{
			float timeScale;
			if (float.TryParse(input.Trim(), out timeScale))
			{
				Time.timeScale = timeScale;
				return;
			}
			this.AddTextToConsole("Don't recognize input: " + timeScale.ToString());
		}

		// Token: 0x06001886 RID: 6278 RVA: 0x0009A1D4 File Offset: 0x000983D4
		private void DeleteMission(string input)
		{
			string b = input.Trim();
			Mission mission = null;
			foreach (Mission mission2 in GamePlayer.current.missions)
			{
				if (mission2.storyId == b)
				{
					mission = mission2;
				}
			}
			if (mission != null)
			{
				GamePlayer.current.missions.Remove(mission);
				string str = "Mission : ";
				Mission mission3 = mission;
				this.AddTextToConsole(str + ((mission3 != null) ? mission3.ToString() : null) + " deleted");
				return;
			}
			string str2 = "Mission : ";
			Mission mission4 = mission;
			this.AddTextToConsole(str2 + ((mission4 != null) ? mission4.ToString() : null) + " not found");
		}

		// Token: 0x06001887 RID: 6279 RVA: 0x0009A298 File Offset: 0x00098498
		private void SpawnCrewPod(string input)
		{
			string random = Behaviour.Crew.Crew.GetRandom(MapPointOfInterest.current.level, true);
			Singleton<LootManager>.Instance.CreateCrewPod(random, SeededRandom.Global.RandomRange(1, 4), GameplayManager.Instance.spaceShip.transform, false);
		}

		// Token: 0x06001888 RID: 6280 RVA: 0x0009A2DD File Offset: 0x000984DD
		private void SpawnOfficerPod(string input)
		{
			Singleton<LootManager>.Instance.CreateOfficerPod(MapPointOfInterest.current.level, GameplayManager.Instance.spaceShip.transform.position, null);
		}

		// Token: 0x06001889 RID: 6281 RVA: 0x0009A30D File Offset: 0x0009850D
		private void JettisonAllCrew(string input)
		{
			GameplayManager.Instance.spaceShip.spaceShipData.JettisonAllCrew();
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x0009A324 File Offset: 0x00098524
		private void RegenerateConquestSector(string input)
		{
			GamePlayer current = GamePlayer.current;
			if (current.GetStoryteller<Conquest>() != null)
			{
				current.RemoveStoryteller<Conquest>();
				List<SectorMapData> list = new List<SectorMapData>();
				foreach (SectorMapData sectorMapData in GalaxyMapData.current.allSectors)
				{
					if (sectorMapData.quadrant == 2)
					{
						list.Add(sectorMapData);
					}
				}
				foreach (SystemMapData systemMapData in GalaxyMapData.current.allSystems)
				{
					JumpGate jumpGate = null;
					foreach (JumpGate jumpGate2 in systemMapData.GetPointsOfInterest<JumpGate>())
					{
						SystemMapData targetSystem = jumpGate2.targetSystem;
						if (targetSystem != null && list.Contains(targetSystem.sector))
						{
							jumpGate = jumpGate2;
							break;
						}
					}
					if (jumpGate != null)
					{
						systemMapData.RemovePointOfInterest(jumpGate);
					}
				}
				foreach (SectorMapData smd in list)
				{
					GalaxyMapData.current.RemoveSector(smd);
				}
			}
			current.AddStoryteller(new Conquest(current), true);
		}

		// Token: 0x0600188B RID: 6283 RVA: 0x0009A49C File Offset: 0x0009869C
		private void ConquestTick(string input)
		{
			Conquest storyteller = GamePlayer.current.GetStoryteller<Conquest>();
			if (storyteller != null)
			{
				storyteller.CleanupEvents();
				storyteller.DoConquestTick(false);
			}
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x0009A4C4 File Offset: 0x000986C4
		private void EconomyTick(string input)
		{
			Economy storyteller = GamePlayer.current.GetStoryteller<Economy>();
			if (storyteller != null)
			{
				storyteller.EconomyTick();
			}
		}

		// Token: 0x0600188D RID: 6285 RVA: 0x0009A4E5 File Offset: 0x000986E5
		private void HideLoadingScreen(string input)
		{
			LoadingScreen.Hide(false);
		}

		// Token: 0x0600188E RID: 6286 RVA: 0x0009A4F0 File Offset: 0x000986F0
		private void ConquestFactionZero(string input)
		{
			Faction faction = Faction.Get(input);
			if (faction == null)
			{
				this.AddTextToConsole("Faction does not exist: " + input);
				return;
			}
			foreach (ConquestSystem conquestSystem in GamePlayer.current.GetStoryteller<Conquest>().systems)
			{
				if (conquestSystem.faction == faction)
				{
					conquestSystem.controlLevel = 0;
					conquestSystem.playerControlLevel = 0;
					conquestSystem.combatStrength = 0f;
				}
			}
			this.AddTextToConsole("Conquest holdings for " + input + " reset to zero.");
		}

		// Token: 0x0600188F RID: 6287 RVA: 0x0009A59C File Offset: 0x0009879C
		private void UnlockFastLaneTravel(string input)
		{
			if (!GamePlayer.current.fastLaneTravelUnlocked)
			{
				GamePlayer.current.SetFastLaneTravelUnlocked(true);
			}
			this.AddTextToConsole("Fast lane travel unlocked");
		}

		// Token: 0x06001890 RID: 6288 RVA: 0x0009A5C0 File Offset: 0x000987C0
		private void AddMission(string input)
		{
			string text = input.Trim();
			Mission mission = StoryMission.Get(GamePlayer.current, text);
			if (mission == null)
			{
				this.AddTextToConsole("Mission : " + text + " not found");
				return;
			}
			GamePlayer.current.AddMissionWithLog(mission);
			this.AddTextToConsole("Mission : " + text + " started");
		}

		// Token: 0x06001891 RID: 6289 RVA: 0x0009A61B File Offset: 0x0009881B
		private void UnlockSkilltreeTier2(string input)
		{
			if (!GamePlayer.current.fastLaneTravelUnlocked)
			{
				GamePlayer.current.SetSkilltreeTier2Unlocked(true);
			}
			this.AddTextToConsole("Skill tree Tier 2 unlocked");
		}

		// Token: 0x06001892 RID: 6290 RVA: 0x0009A63F File Offset: 0x0009883F
		private void UnlockSalWorkshopTradein(string input)
		{
			if (!GamePlayer.current.salvageWorkshopTradeInMaterialsUnlocked)
			{
				GamePlayer.current.SetSalvageWorkshopTradeinUnlocked(true);
			}
			this.AddTextToConsole("Salvage Workshop Trade In Unlocked");
		}

		// Token: 0x06001893 RID: 6291 RVA: 0x0009A664 File Offset: 0x00098864
		private void AddCharacter(string input)
		{
			SpaceStation spaceStation = (SpaceStation)GamePlayer.current.currentPointOfInterest;
			if (spaceStation == null)
			{
				this.AddTextToConsole("Not at a spacestation");
				return;
			}
			spaceStation.characters.Add(input);
			this.AddTextToConsole("Added character");
		}

		// Token: 0x04000F3E RID: 3902
		[SerializeField]
		private TextMeshProUGUI debugOutput;

		// Token: 0x04000F3F RID: 3903
		[SerializeField]
		private TMP_InputField debugInput;

		// Token: 0x04000F40 RID: 3904
		[SerializeField]
		private ScrollRect scrollRect;

		// Token: 0x04000F41 RID: 3905
		[SerializeField]
		private Toggle autoScrollToggle;

		// Token: 0x04000F42 RID: 3906
		private List<ConsoleCommand> commands = new List<ConsoleCommand>();

		// Token: 0x04000F43 RID: 3907
		private bool autoScroll = true;

		// Token: 0x04000F44 RID: 3908
		private bool restartInput;

		// Token: 0x04000F45 RID: 3909
		public bool createShipUsed;
	}
}
