using Newtonsoft.Json.Linq;
using Sakuno.KanColle.Amatsukaze.Game.Models.Raw.Battle;

namespace Sakuno.KanColle.Amatsukaze.Game.Models.Battle.Phases
{
    class ShellingPhase : BattlePhase<RawShellingPhase>
    {
        bool r_IsFriendEscortFleet;
        bool r_IsEnemyEscortFleet;

        public int[] ParticipantingFleet { get; set; }

        internal ShellingPhase(BattleStage rpStage, RawShellingPhase rpRawData, bool rpIsFriendEscortFleet = false, bool rpIsEnemyEscortFleet = false) : base(rpStage, rpRawData)
        {
            r_IsFriendEscortFleet = rpIsFriendEscortFleet;
            r_IsEnemyEscortFleet = rpIsEnemyEscortFleet;
        }

        int GetIndex(int rpPosition)
        {
            if ((r_IsFriendEscortFleet && rpPosition < 6) || (r_IsEnemyEscortFleet && rpPosition >= 6))
                return rpPosition + 12;

            if (ParticipantingFleet != null && ((ParticipantingFleet[0] == 2 && rpPosition < 6) || (ParticipantingFleet[1] == 2 && rpPosition >= 6)))
                return rpPosition + 12;

            return rpPosition;
        }

        internal protected override void Process()
        {
            if (RawData == null)
                return;

            var rParticipants = Stage.FriendAndEnemy;
            var rAttackers = RawData.Attackers;
            var rIsEnemyAttacker = RawData.IsEnemyAttacker;
            for (var i = 0; i < rAttackers.Length; i++)
            {
                var rDefenders = ((JArray)RawData.Defenders[i]).ToObject<int[]>();
                var rDamages = ((JArray)RawData.Damages[i]).ToObject<int[]>();

                if (rIsEnemyAttacker == null)
                    for (var j = 0; j < rDefenders.Length; j++)
                    {
                        if (rDefenders[j] == -1)
                            continue;

                        var rDamage = rDamages[j];
                        rParticipants[GetIndex(rDefenders[j])].Current -= rDamage;
                        rParticipants[GetIndex(rAttackers[i])].DamageGivenToOpponent += rDamage;
                    }
                else
                {
                    var rIsEnemy = rIsEnemyAttacker[i] == 1;

                    for (var j = 0; j < rDefenders.Length; j++)
                    {
                        if (rDefenders[j] == -1)
                            continue;

                        var rDamage = rDamages[j];

                        var rDefenderIndex = rDefenders[j];
                        var rAttackerIndex = rAttackers[i];

                        BattleParticipantSnapshot rDefender, rAttacker;

                        var enemyMainCount = Stage.EnemyMain.Count;
                        var friendMainCount = Stage.FriendMain.Count;

                        if (!rIsEnemy)
                        {
                            if (rDefenderIndex <= enemyMainCount)
                                rDefender = Stage.EnemyMain[rDefenderIndex];
                            else
                                rDefender = Stage.EnemyEscort[rDefenderIndex - enemyMainCount];

                            if (rAttackerIndex <= friendMainCount)
                                rAttacker = Stage.FriendMain[rAttackerIndex];
                            else
                                rAttacker = Stage.FriendEscort[rAttackerIndex - friendMainCount];
                        }
                        else
                        {
                            if (rDefenderIndex <= friendMainCount)
                                rDefender = Stage.FriendMain[rDefenderIndex];
                            else
                                rDefender = Stage.FriendEscort[rDefenderIndex - friendMainCount];

                            if (rAttackerIndex <= enemyMainCount)
                                rAttacker = Stage.EnemyMain[rAttackerIndex];
                            else
                                rAttacker = Stage.EnemyEscort[rAttackerIndex - enemyMainCount];
                        }

                        rDefender.Current -= rDamage;
                        rAttacker.DamageGivenToOpponent += rDamage;
                    }
                }
            }
        }
    }
}
