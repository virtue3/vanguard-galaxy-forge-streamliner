using System;

namespace Source.MissionSystem
{
	// Token: 0x020000A9 RID: 169
	public enum MissionTrigger
	{
		// Token: 0x040003CB RID: 971
		None,
		// Token: 0x040003CC RID: 972
		ItemCollected,
		// Token: 0x040003CD RID: 973
		UnitDestroyed,
		// Token: 0x040003CE RID: 974
		UnitProtected,
		// Token: 0x040003CF RID: 975
		MinedOre,
		// Token: 0x040003D0 RID: 976
		SalvagedItem,
		// Token: 0x040003D1 RID: 977
		TakeDamage,
		// Token: 0x040003D2 RID: 978
		TargetAsteroid,
		// Token: 0x040003D3 RID: 979
		TargetWreckage,
		// Token: 0x040003D4 RID: 980
		ArrivedAtSpaceStation,
		// Token: 0x040003D5 RID: 981
		DockedWithSpaceStation,
		// Token: 0x040003D6 RID: 982
		PersonalHangarRepair,
		// Token: 0x040003D7 RID: 983
		InstallMiningLaser,
		// Token: 0x040003D8 RID: 984
		InstallCombatTurret,
		// Token: 0x040003D9 RID: 985
		InstallSalvageLaser,
		// Token: 0x040003DA RID: 986
		MoveCamera,
		// Token: 0x040003DB RID: 987
		MoveToArea,
		// Token: 0x040003DC RID: 988
		LootContainerOpened,
		// Token: 0x040003DD RID: 989
		BountyTargetKilled,
		// Token: 0x040003DE RID: 990
		CompleteDynamicMission,
		// Token: 0x040003DF RID: 991
		VisitUniqueSystem,
		// Token: 0x040003E0 RID: 992
		CombatStationDestroyed,
		// Token: 0x040003E1 RID: 993
		PatrolWaveFinished,
		// Token: 0x040003E2 RID: 994
		PocketSystemSkirmishVictory,
		// Token: 0x040003E3 RID: 995
		EscortUnitCargoUnloaded,
		// Token: 0x040003E4 RID: 996
		CraftItem,
		// Token: 0x040003E5 RID: 997
		SalvagedModule,
		// Token: 0x040003E6 RID: 998
		EquipDroneShip,
		// Token: 0x040003E7 RID: 999
		FriendlyRepaired,
		// Token: 0x040003E8 RID: 1000
		CompletePatrol,
		// Token: 0x040003E9 RID: 1001
		IndustryBoardCraft,
		// Token: 0x040003EA RID: 1002
		FindCargoWithScanner,
		// Token: 0x040003EB RID: 1003
		PlaceTracker,
		// Token: 0x040003EC RID: 1004
		DecoyTransponderUsed,
		// Token: 0x040003ED RID: 1005
		Tutorial2Welcome,
		// Token: 0x040003EE RID: 1006
		Tutorial3Complete,
		// Token: 0x040003EF RID: 1007
		Tutorial4Complete,
		// Token: 0x040003F0 RID: 1008
		UnlockJumpgateOrbitan,
		// Token: 0x040003F1 RID: 1009
		Tutorial5Welcome,
		// Token: 0x040003F2 RID: 1010
		UnlockJumpgateBalam,
		// Token: 0x040003F3 RID: 1011
		Tutorial6Welcome,
		// Token: 0x040003F4 RID: 1012
		SalvageAICore,
		// Token: 0x040003F5 RID: 1013
		Tutorial7Complete,
		// Token: 0x040003F6 RID: 1014
		CraftAICore,
		// Token: 0x040003F7 RID: 1015
		Tutorial8Complete,
		// Token: 0x040003F8 RID: 1016
		Tutorial9Complete,
		// Token: 0x040003F9 RID: 1017
		Tutorial10CombatComplete,
		// Token: 0x040003FA RID: 1018
		TutorialJumpgateStructure,
		// Token: 0x040003FB RID: 1019
		TutorialJumpgatePlates,
		// Token: 0x040003FC RID: 1020
		TutorialJumpgateConduit,
		// Token: 0x040003FD RID: 1021
		TutorialJumpgateBeacon,
		// Token: 0x040003FE RID: 1022
		Tutorial10Complete,
		// Token: 0x040003FF RID: 1023
		TutorialLastJump,
		// Token: 0x04000400 RID: 1024
		MinerChasedOff,
		// Token: 0x04000401 RID: 1025
		SalvagerChasedOff,
		// Token: 0x04000402 RID: 1026
		InteractWithUmbralBeacon,
		// Token: 0x04000403 RID: 1027
		UmbralLuminatePrisoner4,
		// Token: 0x04000404 RID: 1028
		Umbral4LuminatePrisonerRelease,
		// Token: 0x04000405 RID: 1029
		Umbral5KolyatovWelcome,
		// Token: 0x04000406 RID: 1030
		Umbral5KolyatovAttack,
		// Token: 0x04000407 RID: 1031
		UmbralSteelVultureComputer,
		// Token: 0x04000408 RID: 1032
		Umbral7StellarWelcome,
		// Token: 0x04000409 RID: 1033
		Umbral7StellarSkirmish,
		// Token: 0x0400040A RID: 1034
		Umbral7StellarComplete,
		// Token: 0x0400040B RID: 1035
		Umbral10Smuggler,
		// Token: 0x0400040C RID: 1036
		Umbral11Smuggler,
		// Token: 0x0400040D RID: 1037
		Umbral12Stellar,
		// Token: 0x0400040E RID: 1038
		Umbral13Smuggler,
		// Token: 0x0400040F RID: 1039
		Umbral14SmugglerComplete,
		// Token: 0x04000410 RID: 1040
		Umbral14SmugglerFailed,
		// Token: 0x04000411 RID: 1041
		Umbral15Darkspacers,
		// Token: 0x04000412 RID: 1042
		Umbral16Smuggler,
		// Token: 0x04000413 RID: 1043
		Umbral18Stellar,
		// Token: 0x04000414 RID: 1044
		Umbral20Stellar,
		// Token: 0x04000415 RID: 1045
		Umbral21Umbral,
		// Token: 0x04000416 RID: 1046
		Umbral22,
		// Token: 0x04000417 RID: 1047
		Umbral22Failed,
		// Token: 0x04000418 RID: 1048
		FastTravelTalkToNPC,
		// Token: 0x04000419 RID: 1049
		SkillTier2TalkToNPC,
		// Token: 0x0400041A RID: 1050
		MercIntroEmbassyTalkToNPC,
		// Token: 0x0400041B RID: 1051
		ConquestCanisec1,
		// Token: 0x0400041C RID: 1052
		ConquestStellarEmbassy,
		// Token: 0x0400041D RID: 1053
		CSHQIntro,
		// Token: 0x0400041E RID: 1054
		CS2,
		// Token: 0x0400041F RID: 1055
		CS3,
		// Token: 0x04000420 RID: 1056
		ConquestLuminateEmbassy,
		// Token: 0x04000421 RID: 1057
		CLHQIntro,
		// Token: 0x04000422 RID: 1058
		CL2,
		// Token: 0x04000423 RID: 1059
		CL3,
		// Token: 0x04000424 RID: 1060
		ConquestKolyatovEmbassy,
		// Token: 0x04000425 RID: 1061
		CKHQIntro,
		// Token: 0x04000426 RID: 1062
		CK2,
		// Token: 0x04000427 RID: 1063
		CK3,
		// Token: 0x04000428 RID: 1064
		EarnCombatStrengthForFaction,
		// Token: 0x04000429 RID: 1065
		UmbralStationInfected,
		// Token: 0x0400042A RID: 1066
		MissionBoardOpenedWithUmbral,
		// Token: 0x0400042B RID: 1067
		CU1TalktoNPC,
		// Token: 0x0400042C RID: 1068
		CU3TalktoNPC,
		// Token: 0x0400042D RID: 1069
		CU4TalktoNPC,
		// Token: 0x0400042E RID: 1070
		CU5TalktoNPC,
		// Token: 0x0400042F RID: 1071
		CU6TalktoNPC,
		// Token: 0x04000430 RID: 1072
		CU8TalktoNPC,
		// Token: 0x04000431 RID: 1073
		CU9TalktoNPC,
		// Token: 0x04000432 RID: 1074
		CU10TalktoNPC,
		// Token: 0x04000433 RID: 1075
		TradeTerminalProfit,
		// Token: 0x04000434 RID: 1076
		CD1TalktoNPC,
		// Token: 0x04000435 RID: 1077
		CD2TalktoNPC,
		// Token: 0x04000436 RID: 1078
		CD3TalktoNPC
	}
}
