using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Behaviour.Ability;
using Behaviour.Equipment.Aspect;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Spacestation;
using Behaviour.Util;
using Source.Crew;
using Source.Item;
using Source.Player;
using Source.Simulation.Story;
using Source.SpaceShip;
using Source.Util;
using UnityEngine;

namespace Behaviour.Crew
{
	// Token: 0x020003A7 RID: 935
	public class SkilltreeNode : MonoBehaviour
	{
		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x0600235E RID: 9054 RVA: 0x000CA451 File Offset: 0x000C8651
		// (set) Token: 0x0600235F RID: 9055 RVA: 0x000CA458 File Offset: 0x000C8658
		public static SkilltreeNode combatLoneTurret { get; private set; }

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06002360 RID: 9056 RVA: 0x000CA460 File Offset: 0x000C8660
		// (set) Token: 0x06002361 RID: 9057 RVA: 0x000CA467 File Offset: 0x000C8667
		public static SkilltreeNode miningBasicScanning { get; private set; }

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06002362 RID: 9058 RVA: 0x000CA46F File Offset: 0x000C866F
		// (set) Token: 0x06002363 RID: 9059 RVA: 0x000CA476 File Offset: 0x000C8676
		public static SkilltreeNode miningTreasureChance { get; private set; }

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06002364 RID: 9060 RVA: 0x000CA47E File Offset: 0x000C867E
		// (set) Token: 0x06002365 RID: 9061 RVA: 0x000CA485 File Offset: 0x000C8685
		public static SkilltreeNode miningAdvancedScanning { get; private set; }

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06002366 RID: 9062 RVA: 0x000CA48D File Offset: 0x000C868D
		// (set) Token: 0x06002367 RID: 9063 RVA: 0x000CA494 File Offset: 0x000C8694
		public static SkilltreeNode miningHazardYieldIncrease { get; private set; }

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06002368 RID: 9064 RVA: 0x000CA49C File Offset: 0x000C869C
		// (set) Token: 0x06002369 RID: 9065 RVA: 0x000CA4A3 File Offset: 0x000C86A3
		public static SkilltreeNode miningDualYieldBonus { get; private set; }

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x0600236A RID: 9066 RVA: 0x000CA4AB File Offset: 0x000C86AB
		// (set) Token: 0x0600236B RID: 9067 RVA: 0x000CA4B2 File Offset: 0x000C86B2
		public static SkilltreeNode miningExtraOre { get; private set; }

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x0600236C RID: 9068 RVA: 0x000CA4BA File Offset: 0x000C86BA
		// (set) Token: 0x0600236D RID: 9069 RVA: 0x000CA4C1 File Offset: 0x000C86C1
		public static SkilltreeNode miningMegaYield { get; private set; }

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x0600236E RID: 9070 RVA: 0x000CA4C9 File Offset: 0x000C86C9
		// (set) Token: 0x0600236F RID: 9071 RVA: 0x000CA4D0 File Offset: 0x000C86D0
		public static SkilltreeNode industrialBasicRecipes { get; private set; }

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06002370 RID: 9072 RVA: 0x000CA4D8 File Offset: 0x000C86D8
		// (set) Token: 0x06002371 RID: 9073 RVA: 0x000CA4DF File Offset: 0x000C86DF
		public static SkilltreeNode industrialT1CraftingSpeed { get; private set; }

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06002372 RID: 9074 RVA: 0x000CA4E7 File Offset: 0x000C86E7
		// (set) Token: 0x06002373 RID: 9075 RVA: 0x000CA4EE File Offset: 0x000C86EE
		public static SkilltreeNode industrialMaterialStorage { get; private set; }

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06002374 RID: 9076 RVA: 0x000CA4F6 File Offset: 0x000C86F6
		// (set) Token: 0x06002375 RID: 9077 RVA: 0x000CA4FD File Offset: 0x000C86FD
		public static SkilltreeNode industrialBasicJobs { get; private set; }

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06002376 RID: 9078 RVA: 0x000CA505 File Offset: 0x000C8705
		// (set) Token: 0x06002377 RID: 9079 RVA: 0x000CA50C File Offset: 0x000C870C
		public static SkilltreeNode industrialSellValue { get; private set; }

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06002378 RID: 9080 RVA: 0x000CA514 File Offset: 0x000C8714
		// (set) Token: 0x06002379 RID: 9081 RVA: 0x000CA51B File Offset: 0x000C871B
		public static SkilltreeNode industrialRefBonusCraft1 { get; private set; }

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x0600237A RID: 9082 RVA: 0x000CA523 File Offset: 0x000C8723
		// (set) Token: 0x0600237B RID: 9083 RVA: 0x000CA52A File Offset: 0x000C872A
		public static SkilltreeNode industrialT3ExtraCraftingLevel { get; private set; }

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x0600237C RID: 9084 RVA: 0x000CA532 File Offset: 0x000C8732
		// (set) Token: 0x0600237D RID: 9085 RVA: 0x000CA539 File Offset: 0x000C8739
		public static SkilltreeNode industrialCraftingSpeed2 { get; private set; }

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x0600237E RID: 9086 RVA: 0x000CA541 File Offset: 0x000C8741
		// (set) Token: 0x0600237F RID: 9087 RVA: 0x000CA548 File Offset: 0x000C8748
		public static SkilltreeNode industrialCrystalRefineChance { get; private set; }

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06002380 RID: 9088 RVA: 0x000CA550 File Offset: 0x000C8750
		// (set) Token: 0x06002381 RID: 9089 RVA: 0x000CA557 File Offset: 0x000C8757
		public static SkilltreeNode industrialCraftCreditReduction { get; private set; }

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06002382 RID: 9090 RVA: 0x000CA55F File Offset: 0x000C875F
		// (set) Token: 0x06002383 RID: 9091 RVA: 0x000CA566 File Offset: 0x000C8766
		public static SkilltreeNode industrialForgeBonusCraft { get; private set; }

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06002384 RID: 9092 RVA: 0x000CA56E File Offset: 0x000C876E
		// (set) Token: 0x06002385 RID: 9093 RVA: 0x000CA575 File Offset: 0x000C8775
		public static SkilltreeNode industrialMaterialStorage2 { get; private set; }

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06002386 RID: 9094 RVA: 0x000CA57D File Offset: 0x000C877D
		// (set) Token: 0x06002387 RID: 9095 RVA: 0x000CA584 File Offset: 0x000C8784
		public static SkilltreeNode industrialEnhancedJobs { get; private set; }

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06002388 RID: 9096 RVA: 0x000CA58C File Offset: 0x000C878C
		// (set) Token: 0x06002389 RID: 9097 RVA: 0x000CA593 File Offset: 0x000C8793
		public static SkilltreeNode industrialTravelSpeedBonus { get; private set; }

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x0600238A RID: 9098 RVA: 0x000CA59B File Offset: 0x000C879B
		// (set) Token: 0x0600238B RID: 9099 RVA: 0x000CA5A2 File Offset: 0x000C87A2
		public static SkilltreeNode industrialExtraCraftingLevels2 { get; private set; }

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x0600238C RID: 9100 RVA: 0x000CA5AA File Offset: 0x000C87AA
		// (set) Token: 0x0600238D RID: 9101 RVA: 0x000CA5B1 File Offset: 0x000C87B1
		public static SkilltreeNode promptEngineeringBasicActivityDelay { get; private set; }

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x0600238E RID: 9102 RVA: 0x000CA5B9 File Offset: 0x000C87B9
		// (set) Token: 0x0600238F RID: 9103 RVA: 0x000CA5C0 File Offset: 0x000C87C0
		public static SkilltreeNode promptEngineeringEnhancedActivityDelay { get; private set; }

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06002390 RID: 9104 RVA: 0x000CA5C8 File Offset: 0x000C87C8
		// (set) Token: 0x06002391 RID: 9105 RVA: 0x000CA5CF File Offset: 0x000C87CF
		public static SkilltreeNode promptEngineeringT2UseWarpFuel { get; private set; }

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06002392 RID: 9106 RVA: 0x000CA5D7 File Offset: 0x000C87D7
		// (set) Token: 0x06002393 RID: 9107 RVA: 0x000CA5DE File Offset: 0x000C87DE
		public static SkilltreeNode PromptEngineeringYieldPenaltyReduction { get; private set; }

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06002394 RID: 9108 RVA: 0x000CA5E6 File Offset: 0x000C87E6
		// (set) Token: 0x06002395 RID: 9109 RVA: 0x000CA5ED File Offset: 0x000C87ED
		public static SkilltreeNode PromptEngineeringMiningPowerPenaltyReduction { get; private set; }

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06002396 RID: 9110 RVA: 0x000CA5F5 File Offset: 0x000C87F5
		// (set) Token: 0x06002397 RID: 9111 RVA: 0x000CA5FC File Offset: 0x000C87FC
		public static SkilltreeNode PromptEngineeringExperiencePenaltyReduction { get; private set; }

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06002398 RID: 9112 RVA: 0x000CA604 File Offset: 0x000C8804
		// (set) Token: 0x06002399 RID: 9113 RVA: 0x000CA60B File Offset: 0x000C880B
		public static SkilltreeNode PromptEngineeringBasicMissionRunner { get; private set; }

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x0600239A RID: 9114 RVA: 0x000CA613 File Offset: 0x000C8813
		// (set) Token: 0x0600239B RID: 9115 RVA: 0x000CA61A File Offset: 0x000C881A
		public static SkilltreeNode promptEngineeringEnhancedMissionRunner { get; private set; }

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x0600239C RID: 9116 RVA: 0x000CA622 File Offset: 0x000C8822
		// (set) Token: 0x0600239D RID: 9117 RVA: 0x000CA629 File Offset: 0x000C8829
		public static SkilltreeNode promptEngineeringT5UseWarpFuel { get; private set; }

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x0600239E RID: 9118 RVA: 0x000CA631 File Offset: 0x000C8831
		// (set) Token: 0x0600239F RID: 9119 RVA: 0x000CA638 File Offset: 0x000C8838
		public static SkilltreeNode promptStackableBonus { get; private set; }

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x060023A0 RID: 9120 RVA: 0x000CA640 File Offset: 0x000C8840
		// (set) Token: 0x060023A1 RID: 9121 RVA: 0x000CA647 File Offset: 0x000C8847
		public static SkilltreeNode promptRepBonus { get; private set; }

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x060023A2 RID: 9122 RVA: 0x000CA64F File Offset: 0x000C884F
		// (set) Token: 0x060023A3 RID: 9123 RVA: 0x000CA656 File Offset: 0x000C8856
		public static SkilltreeNode promptRefinerySpeed { get; private set; }

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x060023A4 RID: 9124 RVA: 0x000CA65E File Offset: 0x000C885E
		// (set) Token: 0x060023A5 RID: 9125 RVA: 0x000CA665 File Offset: 0x000C8865
		public static SkilltreeNode promptFleetStrengthBoost { get; private set; }

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x060023A6 RID: 9126 RVA: 0x000CA66D File Offset: 0x000C886D
		// (set) Token: 0x060023A7 RID: 9127 RVA: 0x000CA674 File Offset: 0x000C8874
		public static SkilltreeNode promptItemRewardBonus { get; private set; }

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x060023A8 RID: 9128 RVA: 0x000CA67C File Offset: 0x000C887C
		// (set) Token: 0x060023A9 RID: 9129 RVA: 0x000CA683 File Offset: 0x000C8883
		public static SkilltreeNode PromptEngineeringAutoRepair { get; private set; }

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x060023AA RID: 9130 RVA: 0x000CA68B File Offset: 0x000C888B
		// (set) Token: 0x060023AB RID: 9131 RVA: 0x000CA692 File Offset: 0x000C8892
		public static SkilltreeNode PromptEngineeringAutoRestock { get; private set; }

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x060023AC RID: 9132 RVA: 0x000CA69A File Offset: 0x000C889A
		// (set) Token: 0x060023AD RID: 9133 RVA: 0x000CA6A1 File Offset: 0x000C88A1
		public static SkilltreeNode PromptEngineeringSystemSearchRange { get; private set; }

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x060023AE RID: 9134 RVA: 0x000CA6A9 File Offset: 0x000C88A9
		// (set) Token: 0x060023AF RID: 9135 RVA: 0x000CA6B0 File Offset: 0x000C88B0
		public static SkilltreeNode SalvagingEquipmentAmount { get; private set; }

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x060023B0 RID: 9136 RVA: 0x000CA6B8 File Offset: 0x000C88B8
		// (set) Token: 0x060023B1 RID: 9137 RVA: 0x000CA6BF File Offset: 0x000C88BF
		public static SkilltreeNode SalvagingRefinedMaterialChance { get; private set; }

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x060023B2 RID: 9138 RVA: 0x000CA6C7 File Offset: 0x000C88C7
		// (set) Token: 0x060023B3 RID: 9139 RVA: 0x000CA6CE File Offset: 0x000C88CE
		public static SkilltreeNode salvagingDebrisChance { get; private set; }

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x060023B4 RID: 9140 RVA: 0x000CA6D6 File Offset: 0x000C88D6
		// (set) Token: 0x060023B5 RID: 9141 RVA: 0x000CA6DD File Offset: 0x000C88DD
		public static SkilltreeNode salvagingDualYieldBonus { get; private set; }

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x060023B6 RID: 9142 RVA: 0x000CA6E5 File Offset: 0x000C88E5
		// (set) Token: 0x060023B7 RID: 9143 RVA: 0x000CA6EC File Offset: 0x000C88EC
		public static SkilltreeNode salvagingMaterialChance { get; private set; }

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x060023B8 RID: 9144 RVA: 0x000CA6F4 File Offset: 0x000C88F4
		// (set) Token: 0x060023B9 RID: 9145 RVA: 0x000CA6FB File Offset: 0x000C88FB
		public static SkilltreeNode salvagingWorkshopUnlock { get; private set; }

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x060023BA RID: 9146 RVA: 0x000CA703 File Offset: 0x000C8903
		// (set) Token: 0x060023BB RID: 9147 RVA: 0x000CA70A File Offset: 0x000C890A
		public static SkilltreeNode salvagingSalMegaYield { get; private set; }

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x060023BC RID: 9148 RVA: 0x000CA712 File Offset: 0x000C8912
		// (set) Token: 0x060023BD RID: 9149 RVA: 0x000CA719 File Offset: 0x000C8919
		public static SkilltreeNode dronesSpeed { get; private set; }

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x060023BE RID: 9150 RVA: 0x000CA721 File Offset: 0x000C8921
		// (set) Token: 0x060023BF RID: 9151 RVA: 0x000CA728 File Offset: 0x000C8928
		public static SkilltreeNode dronesDefenses { get; private set; }

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x060023C0 RID: 9152 RVA: 0x000CA730 File Offset: 0x000C8930
		// (set) Token: 0x060023C1 RID: 9153 RVA: 0x000CA737 File Offset: 0x000C8937
		public static SkilltreeNode dronesFasterDeploy { get; private set; }

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x060023C2 RID: 9154 RVA: 0x000CA73F File Offset: 0x000C893F
		// (set) Token: 0x060023C3 RID: 9155 RVA: 0x000CA746 File Offset: 0x000C8946
		public static SkilltreeNode dronesRebuildIncrease { get; private set; }

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x060023C4 RID: 9156 RVA: 0x000CA74E File Offset: 0x000C894E
		// (set) Token: 0x060023C5 RID: 9157 RVA: 0x000CA755 File Offset: 0x000C8955
		public static SkilltreeNode dronesAmount { get; private set; }

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x060023C6 RID: 9158 RVA: 0x000CA75D File Offset: 0x000C895D
		// (set) Token: 0x060023C7 RID: 9159 RVA: 0x000CA764 File Offset: 0x000C8964
		public static SkilltreeNode dronesUnlockDrones1 { get; private set; }

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x060023C8 RID: 9160 RVA: 0x000CA76C File Offset: 0x000C896C
		// (set) Token: 0x060023C9 RID: 9161 RVA: 0x000CA773 File Offset: 0x000C8973
		public static SkilltreeNode dronesChangeDronesInField { get; private set; }

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x060023CA RID: 9162 RVA: 0x000CA77B File Offset: 0x000C897B
		// (set) Token: 0x060023CB RID: 9163 RVA: 0x000CA782 File Offset: 0x000C8982
		public static SkilltreeNode dronesHullRepair { get; private set; }

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x060023CC RID: 9164 RVA: 0x000CA78A File Offset: 0x000C898A
		// (set) Token: 0x060023CD RID: 9165 RVA: 0x000CA791 File Offset: 0x000C8991
		public static SkilltreeNode dronesRebuildIncrease2 { get; private set; }

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x060023CE RID: 9166 RVA: 0x000CA799 File Offset: 0x000C8999
		// (set) Token: 0x060023CF RID: 9167 RVA: 0x000CA7A0 File Offset: 0x000C89A0
		public static SkilltreeNode dronesAmount2 { get; private set; }

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x060023D0 RID: 9168 RVA: 0x000CA7A8 File Offset: 0x000C89A8
		// (set) Token: 0x060023D1 RID: 9169 RVA: 0x000CA7AF File Offset: 0x000C89AF
		public static SkilltreeNode dronesUnlockUtilityDrones { get; private set; }

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x060023D2 RID: 9170 RVA: 0x000CA7B7 File Offset: 0x000C89B7
		// (set) Token: 0x060023D3 RID: 9171 RVA: 0x000CA7BE File Offset: 0x000C89BE
		public static SkilltreeNode combatReactorOutputCP { get; private set; }

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x060023D4 RID: 9172 RVA: 0x000CA7C6 File Offset: 0x000C89C6
		// (set) Token: 0x060023D5 RID: 9173 RVA: 0x000CA7CD File Offset: 0x000C89CD
		public static SkilltreeNode combatInstantReload { get; private set; }

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x060023D6 RID: 9174 RVA: 0x000CA7D5 File Offset: 0x000C89D5
		// (set) Token: 0x060023D7 RID: 9175 RVA: 0x000CA7DC File Offset: 0x000C89DC
		public static SkilltreeNode combatMegaCrit { get; private set; }

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x060023D8 RID: 9176 RVA: 0x000CA7E4 File Offset: 0x000C89E4
		// (set) Token: 0x060023D9 RID: 9177 RVA: 0x000CA7EB File Offset: 0x000C89EB
		public static SkilltreeNode combatFastReloadManual { get; private set; }

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x060023DA RID: 9178 RVA: 0x000CA7F3 File Offset: 0x000C89F3
		// (set) Token: 0x060023DB RID: 9179 RVA: 0x000CA7FA File Offset: 0x000C89FA
		public static SkilltreeNode DefenseSpikeShield { get; private set; }

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x060023DC RID: 9180 RVA: 0x000CA802 File Offset: 0x000C8A02
		// (set) Token: 0x060023DD RID: 9181 RVA: 0x000CA809 File Offset: 0x000C8A09
		public static SkilltreeNode economySellValue { get; private set; }

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x060023DE RID: 9182 RVA: 0x000CA811 File Offset: 0x000C8A11
		// (set) Token: 0x060023DF RID: 9183 RVA: 0x000CA818 File Offset: 0x000C8A18
		public static SkilltreeNode economyEquipmentCost { get; private set; }

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x060023E0 RID: 9184 RVA: 0x000CA820 File Offset: 0x000C8A20
		// (set) Token: 0x060023E1 RID: 9185 RVA: 0x000CA827 File Offset: 0x000C8A27
		public static SkilltreeNode economyMarketVolatility { get; private set; }

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x060023E2 RID: 9186 RVA: 0x000CA82F File Offset: 0x000C8A2F
		// (set) Token: 0x060023E3 RID: 9187 RVA: 0x000CA836 File Offset: 0x000C8A36
		public static SkilltreeNode economyTradeTooltip { get; private set; }

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x060023E4 RID: 9188 RVA: 0x000CA83E File Offset: 0x000C8A3E
		// (set) Token: 0x060023E5 RID: 9189 RVA: 0x000CA845 File Offset: 0x000C8A45
		public static SkilltreeNode economyGlobalValue { get; private set; }

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x060023E6 RID: 9190 RVA: 0x000CA84D File Offset: 0x000C8A4D
		// (set) Token: 0x060023E7 RID: 9191 RVA: 0x000CA854 File Offset: 0x000C8A54
		public static SkilltreeNode economyCargoTravelSpeed { get; private set; }

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x060023E8 RID: 9192 RVA: 0x000CA85C File Offset: 0x000C8A5C
		// (set) Token: 0x060023E9 RID: 9193 RVA: 0x000CA863 File Offset: 0x000C8A63
		public static SkilltreeNode economyCargoDamage { get; private set; }

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x060023EA RID: 9194 RVA: 0x000CA86B File Offset: 0x000C8A6B
		// (set) Token: 0x060023EB RID: 9195 RVA: 0x000CA872 File Offset: 0x000C8A72
		public static SkilltreeNode economyTradeCraftedDeal { get; private set; }

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x060023EC RID: 9196 RVA: 0x000CA87A File Offset: 0x000C8A7A
		public string identifier
		{
			get
			{
				Skilltree parent = this.parent;
				return ((parent != null) ? parent.name : null) + base.name;
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x060023ED RID: 9197 RVA: 0x000CA899 File Offset: 0x000C8A99
		// (set) Token: 0x060023EE RID: 9198 RVA: 0x000CA8A1 File Offset: 0x000C8AA1
		public Skilltree parent { get; private set; }

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x060023EF RID: 9199 RVA: 0x000CA8AA File Offset: 0x000C8AAA
		public string displayName
		{
			get
			{
				return "@" + this.identifier + "Name";
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x060023F0 RID: 9200 RVA: 0x000CA8C1 File Offset: 0x000C8AC1
		public string descriptionText
		{
			get
			{
				return "@" + this.identifier + "Description";
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x060023F1 RID: 9201 RVA: 0x000CA8D8 File Offset: 0x000C8AD8
		public IEnumerable<BoostStat> statBoosts
		{
			get
			{
				return this.boosts;
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x060023F2 RID: 9202 RVA: 0x000CA8E0 File Offset: 0x000C8AE0
		public int investedPoints
		{
			get
			{
				GamePlayer current = GamePlayer.current;
				int? num;
				if (current == null)
				{
					num = null;
				}
				else
				{
					SkillTreeData skillTreeData = current.commander.GetSkillTreeData(this.parent, false);
					num = ((skillTreeData != null) ? new int?(skillTreeData.GetCurrentPoints(this)) : null);
				}
				int? num2 = num;
				return num2.GetValueOrDefault();
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x060023F3 RID: 9203 RVA: 0x000CA934 File Offset: 0x000C8B34
		public int crewLevelRequired
		{
			get
			{
				return (this.tier - 1) * 5;
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x060023F4 RID: 9204 RVA: 0x000CA940 File Offset: 0x000C8B40
		public bool isActive
		{
			get
			{
				return this.currentPoints > 0;
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x060023F5 RID: 9205 RVA: 0x000CA94C File Offset: 0x000C8B4C
		public int currentPoints
		{
			get
			{
				int num = this.investedPoints;
				if (GamePlayer.current != null)
				{
					SpaceShipData currentSpaceShip = GamePlayer.current.currentSpaceShip;
					if (((currentSpaceShip != null) ? currentSpaceShip.crewMembers : null) != null)
					{
						foreach (CrewMemberData crewMemberData in GamePlayer.current.currentSpaceShip.crewMembers)
						{
							if (crewMemberData != null)
							{
								using (IEnumerator<SkilltreeNode> enumerator = crewMemberData.unlockedNodes.GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										if (enumerator.Current == this)
										{
											num++;
										}
									}
								}
							}
						}
					}
				}
				return num;
			}
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x060023F6 RID: 9206 RVA: 0x000CA9F0 File Offset: 0x000C8BF0
		public float currentIncrease
		{
			get
			{
				return (float)this.currentPoints * this.customIncrease;
			}
		}

		// Token: 0x060023F7 RID: 9207 RVA: 0x000CAA00 File Offset: 0x000C8C00
		public int CurrentCommanderPoints()
		{
			return this.investedPoints;
		}

		// Token: 0x060023F8 RID: 9208 RVA: 0x000CAA08 File Offset: 0x000C8C08
		public void Initialize()
		{
			base.GetComponents<BoostStat>(this.boosts);
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x000CAA16 File Offset: 0x000C8C16
		public void SetParent(Skilltree parent)
		{
			this.parent = parent;
		}

		// Token: 0x060023FA RID: 9210 RVA: 0x000CAA1F File Offset: 0x000C8C1F
		public bool IsAbility()
		{
			return this.abilities.Count > 0;
		}

		// Token: 0x060023FB RID: 9211 RVA: 0x000CAA30 File Offset: 0x000C8C30
		public bool IsActivatedAbility()
		{
			if (this.abilities.Count > 0)
			{
				using (List<AbstractAbility>.Enumerator enumerator = this.abilities.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.GetComponent<ActivatedAbility>() != null)
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060023FC RID: 9212 RVA: 0x000CAAA0 File Offset: 0x000C8CA0
		public bool IsTriggeredAbility()
		{
			if (this.abilities.Count > 0)
			{
				using (List<AbstractAbility>.Enumerator enumerator = this.abilities.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.GetComponent<TriggeredAbility>() != null)
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060023FD RID: 9213 RVA: 0x000CAB10 File Offset: 0x000C8D10
		public bool CanInvestSkillPoints()
		{
			if (this.parent == null)
			{
				return false;
			}
			if (this.conquestLocked && GamePlayer.current.GetStoryteller<Conquest>() == null)
			{
				return false;
			}
			if (this.tier > 3 && !GamePlayer.current.skilltreeTier2Unlocked)
			{
				return false;
			}
			if (this.parent.GetInvestedSkillPoints() < this.requiredPointsInTree)
			{
				return false;
			}
			if (GamePlayer.current.commander.GetInvestedSkillPoints() < this.requiredPointsTotal)
			{
				return false;
			}
			if (this.requiredNode && this.requiredNode.investedPoints < this.requiredNode.maxSkillPoints)
			{
				return false;
			}
			using (List<SkilltreeNode>.Enumerator enumerator = this.exclusiveNodes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.investedPoints > 0)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060023FE RID: 9214 RVA: 0x000CABFC File Offset: 0x000C8DFC
		public bool CanRemoveSkillPoint(bool showNotification = false)
		{
			return !(this.parent == null) && !(SpaceStationInterior.instance == null) && this.investedPoints != 0 && this.IsTreeValidAfterChange(this, showNotification);
		}

		// Token: 0x060023FF RID: 9215 RVA: 0x000CAC2F File Offset: 0x000C8E2F
		private bool IsTreeValidAfterChange(SkilltreeNode nodeToRemoveFrom, bool showNotification = false)
		{
			return this.IsTreeValidAfterRemoving(nodeToRemoveFrom, showNotification);
		}

		// Token: 0x06002400 RID: 9216 RVA: 0x000CAC3C File Offset: 0x000C8E3C
		private bool IsTreeValidAfterRemoving(SkilltreeNode nodeToRemoveFrom, bool showNotification = false)
		{
			foreach (SkilltreeNode skilltreeNode in this.parent.allNodes)
			{
				if (!(skilltreeNode.requiredNode == null) && !(skilltreeNode == nodeToRemoveFrom) && skilltreeNode.requiredNode == nodeToRemoveFrom && skilltreeNode.investedPoints > 0)
				{
					if (showNotification)
					{
						Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SkillCantUnassignRequired", new object[]
						{
							Translation.Translate(skilltreeNode.displayName, Array.Empty<object>())
						})).WithColor(ColorHelper.red90).Show();
					}
					return false;
				}
			}
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			foreach (SkilltreeNode skilltreeNode2 in this.parent.allNodes)
			{
				int num = skilltreeNode2.tier;
				int num2 = (skilltreeNode2 == nodeToRemoveFrom) ? (skilltreeNode2.investedPoints - 1) : skilltreeNode2.investedPoints;
				if (num2 < 0)
				{
					num2 = 0;
				}
				if (dictionary.ContainsKey(num))
				{
					Dictionary<int, int> dictionary2 = dictionary;
					int key = num;
					dictionary2[key] += num2;
				}
				else
				{
					dictionary[num] = num2;
				}
			}
			foreach (SkilltreeNode skilltreeNode3 in this.parent.allNodes)
			{
				if (skilltreeNode3.investedPoints != 0 && ((skilltreeNode3 == nodeToRemoveFrom) ? (skilltreeNode3.investedPoints - 1) : skilltreeNode3.investedPoints) > 0)
				{
					int num3 = 0;
					for (int i = 0; i < skilltreeNode3.tier; i++)
					{
						int num4;
						if (dictionary.TryGetValue(i, out num4))
						{
							num3 += num4;
						}
					}
					if (num3 < skilltreeNode3.requiredPointsInTree)
					{
						if (showNotification)
						{
							Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SkillCantUnassign", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
						}
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06002401 RID: 9217 RVA: 0x000CAE78 File Offset: 0x000C9078
		public void InvestSkillPoints(int count)
		{
			GamePlayer.current.commander.GetSkillTreeData(this.parent, true).InvestSkillPoints(this, count);
		}

		// Token: 0x06002402 RID: 9218 RVA: 0x000CAE97 File Offset: 0x000C9097
		public void RemoveSkillPoints(int count)
		{
			GamePlayer.current.commander.GetSkillTreeData(this.parent, true).RemoveSkillPoints(this, count);
		}

		// Token: 0x06002403 RID: 9219 RVA: 0x000CAEB6 File Offset: 0x000C90B6
		public string GetAbilityDescription()
		{
			return this.CreateSkillDescription(this.currentPoints, 0).Item1;
		}

		// Token: 0x06002404 RID: 9220 RVA: 0x000CAECC File Offset: 0x000C90CC
		public ValueTuple<string, string> CreateSkillDescription(int currentPoints, int crewRanks)
		{
			int num = Mathf.Max(1, currentPoints);
			int num2 = this.maxSkillPoints;
			bool flag = this.customIncrease != 0f;
			this.IsAbility();
			float num3 = flag ? (this.customIncrease * (float)num) : 0f;
			float num4 = flag ? (this.customIncrease * (float)(num + 1)) : 0f;
			float triggerChance = 0f;
			float triggerChance2 = 0f;
			List<EquipStatLine> list = new List<EquipStatLine>();
			List<EquipStatLine> list2 = new List<EquipStatLine>();
			if (!flag)
			{
				if (this.IsAbility())
				{
					using (List<AbstractAbility>.Enumerator enumerator = this.abilities.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							AbstractAbility abstractAbility = enumerator.Current;
							TriggeredAbility triggeredAbility = abstractAbility as TriggeredAbility;
							if (triggeredAbility != null)
							{
								triggerChance = triggeredAbility.GetTriggerChance(num);
								triggerChance2 = triggeredAbility.GetTriggerChance(num + 1);
							}
							foreach (BoostStat boostStat in abstractAbility.payload.GetComponentsInChildren<BoostStat>())
							{
								list.AddRange(boostStat.GetStats(num));
								list2.AddRange(boostStat.GetStats(num + 1));
							}
						}
						goto IL_165;
					}
				}
				foreach (BoostStat boostStat2 in this.statBoosts)
				{
					list.AddRange(boostStat2.GetStats(num));
					list2.AddRange(boostStat2.GetStats(num + 1));
				}
			}
			IL_165:
			string custom = null;
			if (flag)
			{
				float percentage = (currentPoints - crewRanks == num2) ? num3 : num4;
				custom = (this.isCustomPercentage ? GameMath.FormatPercentage(percentage, FormatPercentageMode.Default, 1) : percentage.ToString());
			}
			return new ValueTuple<string, string>(SkilltreeNode.BuildDescriptionLine(this.descriptionText, triggerChance, list, flag ? (this.isCustomPercentage ? GameMath.FormatPercentage(num3, FormatPercentageMode.Default, 1) : num3.ToString()) : null), SkilltreeNode.BuildDescriptionLine(this.descriptionText, triggerChance2, list2, custom));
		}

		// Token: 0x06002405 RID: 9221 RVA: 0x000CB0D0 File Offset: 0x000C92D0
		private static string BuildDescriptionLine(string desc, float triggerChance, List<EquipStatLine> stats, string custom = null)
		{
			if (custom != null)
			{
				return Translation.Translate(desc, new object[]
				{
					custom
				});
			}
			List<object> list = new List<object>();
			if (triggerChance > 0f)
			{
				list.Add(GameMath.FormatPercentage(triggerChance, FormatPercentageMode.Default, 1));
			}
			list.AddRange(from stat in stats
			select stat.ToString(false));
			return Translation.Translate(desc, list.ToArray());
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x000CB144 File Offset: 0x000C9344
		public static SkilltreeNode GetCrewNode(string name)
		{
			if (name == null)
			{
				return null;
			}
			SkilltreeNode result;
			if (SkilltreeNode.crewNodes.TryGetValue(name, out result))
			{
				return result;
			}
			foreach (Skilltree skilltree in Skilltree.all)
			{
				SkilltreeNode node = skilltree.GetNode(name);
				if (node != null)
				{
					return node;
				}
			}
			Debug.Log("Node bestaat niet: " + name);
			return null;
		}

		// Token: 0x06002407 RID: 9223 RVA: 0x000CB1C8 File Offset: 0x000C93C8
		public static void LoadCrewNodes()
		{
			SkilltreeNode.crewNodes.Clear();
			SkilltreeNode[] array = Resources.LoadAll<SkilltreeNode>("Crew/CrewSkills");
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].name.StartsWith("_"))
				{
					SkilltreeNode.crewNodes[array[i].identifier] = array[i];
					array[i].Initialize();
				}
			}
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x000CB22C File Offset: 0x000C942C
		public static void LoadStaticNodes()
		{
			SkilltreeNode.combatLoneTurret = SkilltreeNode.GetCrewNode("Combat - OffensiveLoneTurret");
			SkilltreeNode.miningBasicScanning = SkilltreeNode.GetCrewNode("MiningT1BasicScanning");
			SkilltreeNode.miningTreasureChance = SkilltreeNode.GetCrewNode("MiningT2TreasureChance");
			SkilltreeNode.miningAdvancedScanning = SkilltreeNode.GetCrewNode("MiningT4AdvancedScanning");
			SkilltreeNode.miningHazardYieldIncrease = SkilltreeNode.GetCrewNode("MiningT4HazardYieldIncrease");
			SkilltreeNode.miningDualYieldBonus = SkilltreeNode.GetCrewNode("MiningT4DualYieldBonus");
			SkilltreeNode.miningExtraOre = SkilltreeNode.GetCrewNode("MiningT5ExtraOre");
			SkilltreeNode.miningMegaYield = SkilltreeNode.GetCrewNode("MiningT5MegaYield");
			SkilltreeNode.industrialBasicRecipes = SkilltreeNode.GetCrewNode("IndustrialT1BasicRecipes");
			SkilltreeNode.industrialT1CraftingSpeed = SkilltreeNode.GetCrewNode("IndustrialT1CraftingSpeed");
			SkilltreeNode.industrialMaterialStorage = SkilltreeNode.GetCrewNode("IndustrialT2MaterialStorage");
			SkilltreeNode.industrialSellValue = SkilltreeNode.GetCrewNode("IndustrialT2CraftedValue");
			SkilltreeNode.industrialRefBonusCraft1 = SkilltreeNode.GetCrewNode("IndustrialT2RefBonusCraft");
			SkilltreeNode.industrialT3ExtraCraftingLevel = SkilltreeNode.GetCrewNode("IndustrialT3ExtraCraftingLevel");
			SkilltreeNode.industrialBasicJobs = SkilltreeNode.GetCrewNode("IndustrialT3BasicJobs");
			SkilltreeNode.industrialCraftingSpeed2 = SkilltreeNode.GetCrewNode("IndustrialCraftingSpeed");
			SkilltreeNode.industrialCrystalRefineChance = SkilltreeNode.GetCrewNode("IndustrialCrystalRefineChance");
			SkilltreeNode.industrialCraftCreditReduction = SkilltreeNode.GetCrewNode("IndustrialCraftCreditReduction");
			SkilltreeNode.industrialForgeBonusCraft = SkilltreeNode.GetCrewNode("IndustrialForgeBonusCraft");
			SkilltreeNode.industrialMaterialStorage2 = SkilltreeNode.GetCrewNode("IndustrialMaterialStorage2");
			SkilltreeNode.industrialTravelSpeedBonus = SkilltreeNode.GetCrewNode("IndustrialTravelSpeedBonus");
			SkilltreeNode.industrialEnhancedJobs = SkilltreeNode.GetCrewNode("IndustrialEnhancedJobs");
			SkilltreeNode.industrialExtraCraftingLevels2 = SkilltreeNode.GetCrewNode("IndustrialExtraCraftingLevels2");
			SkilltreeNode.promptEngineeringBasicActivityDelay = SkilltreeNode.GetCrewNode("PromptEngineeringBasicActivityDelay");
			SkilltreeNode.promptEngineeringT2UseWarpFuel = SkilltreeNode.GetCrewNode("PromptEngineeringT2UseWarpFuel");
			SkilltreeNode.PromptEngineeringYieldPenaltyReduction = SkilltreeNode.GetCrewNode("PromptEngineeringT3YieldBonus");
			SkilltreeNode.PromptEngineeringMiningPowerPenaltyReduction = SkilltreeNode.GetCrewNode("PromptEngineeringT1MiningPowerPenaltyReduction");
			SkilltreeNode.PromptEngineeringExperiencePenaltyReduction = SkilltreeNode.GetCrewNode("PromptEngineeringExperiencePenaltyReduction");
			SkilltreeNode.PromptEngineeringBasicMissionRunner = SkilltreeNode.GetCrewNode("PromptEngineeringBasicMissionRunner");
			SkilltreeNode.promptEngineeringEnhancedMissionRunner = SkilltreeNode.GetCrewNode("PromptEngineeringEnhancedMissionRunner");
			SkilltreeNode.promptEngineeringT5UseWarpFuel = SkilltreeNode.GetCrewNode("PromptEngineeringT5UseWarpFuel");
			SkilltreeNode.promptStackableBonus = SkilltreeNode.GetCrewNode("PromptEngineeringStackableBonus");
			SkilltreeNode.promptRepBonus = SkilltreeNode.GetCrewNode("PromptEngineeringRepGainIncrease");
			SkilltreeNode.promptRefinerySpeed = SkilltreeNode.GetCrewNode("PromptEngineeringRefinerySpeedBoost");
			SkilltreeNode.promptFleetStrengthBoost = SkilltreeNode.GetCrewNode("PromptEngineeringFleetStrengthBoost");
			SkilltreeNode.promptEngineeringEnhancedActivityDelay = SkilltreeNode.GetCrewNode("PromptEngineeringEnhancedActivityDelay");
			SkilltreeNode.promptItemRewardBonus = SkilltreeNode.GetCrewNode("PromptEngineeringItemRewardBonus");
			SkilltreeNode.SalvagingEquipmentAmount = SkilltreeNode.GetCrewNode("SalvagingEquipmentAmount");
			SkilltreeNode.SalvagingRefinedMaterialChance = SkilltreeNode.GetCrewNode("SalvagingRefinedMaterialChance");
			SkilltreeNode.salvagingDebrisChance = SkilltreeNode.GetCrewNode("SalvagingDebrisChance");
			SkilltreeNode.salvagingDualYieldBonus = SkilltreeNode.GetCrewNode("SalvagingDualYieldSalBonus");
			SkilltreeNode.salvagingMaterialChance = SkilltreeNode.GetCrewNode("SalvagingMaterialChance");
			SkilltreeNode.salvagingWorkshopUnlock = SkilltreeNode.GetCrewNode("SalvagingWorkshopUnlock");
			SkilltreeNode.salvagingSalMegaYield = SkilltreeNode.GetCrewNode("SalvagingSalMegaYield");
			SkilltreeNode.dronesSpeed = SkilltreeNode.GetCrewNode("DronesSpeed");
			SkilltreeNode.dronesDefenses = SkilltreeNode.GetCrewNode("DronesDefenses");
			SkilltreeNode.dronesFasterDeploy = SkilltreeNode.GetCrewNode("DronesFasterDeploy");
			SkilltreeNode.dronesRebuildIncrease = SkilltreeNode.GetCrewNode("DronesRebuildIncrease");
			SkilltreeNode.dronesAmount = SkilltreeNode.GetCrewNode("DronesAmount");
			SkilltreeNode.dronesUnlockDrones1 = SkilltreeNode.GetCrewNode("DronesUnlockDrones1");
			SkilltreeNode.dronesChangeDronesInField = SkilltreeNode.GetCrewNode("DronesChangeDronesInField");
			SkilltreeNode.dronesHullRepair = SkilltreeNode.GetCrewNode("DronesHullRepair");
			SkilltreeNode.dronesRebuildIncrease2 = SkilltreeNode.GetCrewNode("DronesRebuildIncrease2");
			SkilltreeNode.dronesAmount2 = SkilltreeNode.GetCrewNode("DronesAmount2");
			SkilltreeNode.dronesUnlockUtilityDrones = SkilltreeNode.GetCrewNode("DronesUnlockUtilityDrones");
			SkilltreeNode.combatReactorOutputCP = SkilltreeNode.GetCrewNode("Combat - OffensiveReactorOutputCP");
			SkilltreeNode.combatInstantReload = SkilltreeNode.GetCrewNode("Combat - OffensiveInstantReload");
			SkilltreeNode.combatMegaCrit = SkilltreeNode.GetCrewNode("Combat - OffensiveMegaCrit");
			SkilltreeNode.combatFastReloadManual = SkilltreeNode.GetCrewNode("Combat - OffensiveFastReloadManual");
			SkilltreeNode.DefenseSpikeShield = SkilltreeNode.GetCrewNode("DefenseHordeDefense");
			SkilltreeNode.economySellValue = SkilltreeNode.GetCrewNode("EconomySellValue");
			SkilltreeNode.economyEquipmentCost = SkilltreeNode.GetCrewNode("EconomyEquipmentCost");
			SkilltreeNode.economyMarketVolatility = SkilltreeNode.GetCrewNode("EconomyMarketVolatility");
			SkilltreeNode.economyTradeTooltip = SkilltreeNode.GetCrewNode("EconomyTradeTooltip");
			SkilltreeNode.economyGlobalValue = SkilltreeNode.GetCrewNode("EconomyGlobalValue");
			SkilltreeNode.economyCargoTravelSpeed = SkilltreeNode.GetCrewNode("EconomyCargoTravelSpeed");
			SkilltreeNode.economyCargoDamage = SkilltreeNode.GetCrewNode("EconomyCargoDamage");
			SkilltreeNode.economyTradeCraftedDeal = SkilltreeNode.GetCrewNode("EconomyTradeCraftedDeal");
		}

		// Token: 0x06002409 RID: 9225 RVA: 0x000CB635 File Offset: 0x000C9835
		public static IEnumerable<SkilltreeNode> GetMajorNodes(Profession profession, int tier)
		{
			foreach (SkilltreeNode skilltreeNode in SkilltreeNode.crewNodes.Values)
			{
				if (skilltreeNode.tier == tier && skilltreeNode.availableForCrew.HasFlag(profession))
				{
					yield return skilltreeNode;
				}
			}
			Dictionary<string, SkilltreeNode>.ValueCollection.Enumerator enumerator = default(Dictionary<string, SkilltreeNode>.ValueCollection.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0400156D RID: 5485
		private static Dictionary<string, SkilltreeNode> crewNodes = new Dictionary<string, SkilltreeNode>();

		// Token: 0x0400156F RID: 5487
		public Sprite icon;

		// Token: 0x04001570 RID: 5488
		public SkillNodeLevel skillNodeLevel;

		// Token: 0x04001571 RID: 5489
		public Profession availableForCrew;

		// Token: 0x04001572 RID: 5490
		public int tier;

		// Token: 0x04001573 RID: 5491
		public int row;

		// Token: 0x04001574 RID: 5492
		public int maxSkillPoints;

		// Token: 0x04001575 RID: 5493
		public int requiredPointsInTree;

		// Token: 0x04001576 RID: 5494
		public int requiredPointsTotal;

		// Token: 0x04001577 RID: 5495
		public float customIncrease;

		// Token: 0x04001578 RID: 5496
		public bool isCustomPercentage = true;

		// Token: 0x04001579 RID: 5497
		public SkilltreeNode requiredNode;

		// Token: 0x0400157A RID: 5498
		public List<SkilltreeNode> exclusiveNodes;

		// Token: 0x0400157B RID: 5499
		public List<AbstractAbility> abilities = new List<AbstractAbility>();

		// Token: 0x0400157C RID: 5500
		private List<BoostStat> boosts = new List<BoostStat>();

		// Token: 0x0400157D RID: 5501
		public bool conquestLocked;
	}
}
