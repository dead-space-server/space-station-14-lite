using System.Linq;
using Content.Server.Fax;
using Content.Server.Station.Systems;
using Content.Shared.DeadSpace.StationGoal;
using Content.Shared.Fax.Components;
using Content.Shared.GameTicking;
using Content.Shared.Paper;
using Robust.Shared.ContentPack;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server.DeadSpace.StationGoal
{
    /// <summary>
    ///     System to spawn paper with station goal
    /// </summary>
    public sealed class StationGoalPaperSystem : EntitySystem
    {
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
        [Dependency] private readonly IRobustRandom _random = default!;
        [Dependency] private readonly FaxSystem _faxSystem = default!;
        [Dependency] private readonly IResourceManager _resourceManager = default!;
        [Dependency] private readonly StationSystem _station = default!;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<RoundStartedEvent>(OnRoundStarted);
        }

        private void OnRoundStarted(RoundStartedEvent ev)
        {
            SendRandomGoal();
        }

        public bool SendRandomGoal()
        {
            var availableGoals = _prototypeManager.EnumeratePrototypes<StationGoalPrototype>().ToList();
            var goal = _random.Pick(availableGoals);
            return SendStationGoal(goal);
        }

        /// <summary>
        ///     Send a station goal to all faxes which are authorized to receive it
        /// </summary>
        /// <returns>True if at least one fax received paper</returns>
        public bool SendStationGoal(StationGoalPrototype goal)
        {
            var faxes = EntityManager.EntityQuery<FaxMachineComponent>();
            var wasSent = false;

            string text = _resourceManager.ContentFileReadText(goal.Text).ReadToEnd();

            foreach (var fax in faxes)
            {
                if (!fax.ReceiveStationGoal) continue;

                if (_station.GetOwningStation(fax.Owner) is { } station)
                {
                    text = text.Replace("STATION XX-00", Name(station));
                }

                var printout = new FaxPrintout(text, Loc.GetString(goal.Name), null, "PaperPrintedCentcomm", "paper_stamp-centcom",
                    new List<StampDisplayInfo>
                    {
                        new() { StampedName = Loc.GetString("stamp-component-stamped-name-centcom"), StampedColor = Color.FromHex("#006600") },
                    });

                _faxSystem.Receive(fax.Owner, printout, null, fax);

                wasSent = true;
            }

            return wasSent;
        }
    }
}
