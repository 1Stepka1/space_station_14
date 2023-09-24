using Content.Server.Actions;
using Content.Server.Atmos.EntitySystems;
using Content.Server.Nutrition.Components;
using Content.Server.Popups;
using Content.Shared.Actions;
using Content.Shared.Atmos;
using Content.Shared.Nutrition.Components;
using Content.Shared.Nutrition.EntitySystems;
using Robust.Server.GameObjects;
using Robust.Shared.Player;

namespace Content.Server.Capibara
{
    public sealed partial class CapibaraSystem : EntitySystem
    {
        [Dependency] private readonly PopupSystem _popup = default!;
        [Dependency] private readonly ActionsSystem _action = default!;
        [Dependency] private readonly AtmosphereSystem _atmos = default!;
        [Dependency] private readonly TransformSystem _xform = default!;
        [Dependency] private readonly HungerSystem _hunger = default!;

        public override void Initialize()
        {
            base.Initialize();
            //TODO xTray капибара уже бесит, одни проблемы с ней
            // SubscribeLocalEvent<CapibaraComponent, ComponentStartup>(OnStartup);
            // SubscribeLocalEvent<CapibaraComponent, CapibaraRaiseArmyActionEvent>(OnRaiseArmy);
        }

        private void OnStartup(EntityUid uid, CapibaraComponent component, ComponentStartup args)
        {
            _action.AddAction(uid, ref component.ActionRaiseArmy, component.ArmyMobSpawnId);
        }

        /// <summary>
        /// Summons an allied rat servant at the King, costing a small amount of hunger
        /// </summary>
        private void OnRaiseArmy(EntityUid uid, CapibaraComponent component, CapibaraRaiseArmyActionEvent args)
        {
            if (args.Handled)
                return;

            if (!TryComp<HungerComponent>(uid, out var hunger))
                return;

            //make sure the hunger doesn't go into the negatives
            if (hunger.CurrentHunger < component.HungerPerArmyUse)
            {
                _popup.PopupEntity(Loc.GetString("rat-king-too-hungry"), uid, uid);
                return;
            }
            args.Handled = true;
            _hunger.ModifyHunger(uid, -component.HungerPerArmyUse, hunger);
            Spawn(component.ArmyMobSpawnId, Transform(uid).Coordinates); //spawn the little mouse boi
        }

    }

    public sealed partial class CapibaraRaiseArmyActionEvent : InstantActionEvent { };
    public sealed partial class CapibaraDomainActionEvent : InstantActionEvent { };
};
