using Content.Shared.Actions;
using Content.Shared.Buckle;
using Content.Shared.Buckle.Components;
using Content.Shared.Cuffs.Components;
using Content.Shared.Movement.Systems;
using Content.Shared.Physics;
using Content.Shared.Standing;
using Content.Shared.Stunnable;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Systems;

namespace Content.Shared.DeadSpace.Falldown;

public sealed class SharedFalldownSystem : EntitySystem
{
    [Dependency] private readonly StandingStateSystem _standing = default!;
    [Dependency] private readonly SharedActionsSystem _actions = default!;
    [Dependency] private readonly SharedStunSystem _stunSystem = default!;
    [Dependency] private readonly MovementSpeedModifierSystem _movementSpeedModifier = default!;
    [Dependency] private readonly SharedPhysicsSystem _physics = default!;
    [Dependency] private readonly SharedBuckleSystem _buckleSystem = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<FalldownComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<FalldownComponent, ComponentShutdown>(OnCompShutdown);
        SubscribeLocalEvent<FalldownComponent, FalldownActionEvent>(OnFalldownAction);
        SubscribeLocalEvent<FalldownComponent, StandAttemptEvent>(OnStandAttemptEvent);
        SubscribeLocalEvent<FalldownComponent, RefreshMovementSpeedModifiersEvent>(OnRefreshMovespeed);

        SubscribeLocalEvent<FalldownComponent, BuckledEvent>((ent, comp, _) => Standup(ent, comp));
        SubscribeLocalEvent<FalldownComponent, CuffedStateChangeEvent>((ent, comp, _) =>
        {
            if (!TryComp<CuffableComponent>(ent, out var cuffableComponent))
                return;

            if (!cuffableComponent.CanStillInteract)
                Standup(ent, comp);
        });
    }

    private void OnMapInit(EntityUid ent, FalldownComponent comp, MapInitEvent ev)
    {
        _actions.AddAction(ent, ref comp.FalldownActionEntity, comp.FalldownAction);
        Dirty(ent, comp);
    }

    private void OnCompShutdown(EntityUid ent, FalldownComponent comp, ComponentShutdown ev)
    {
        _actions.RemoveAction(ent, comp.FalldownActionEntity);
        Dirty(ent, comp);
    }

    private void Falldown(EntityUid ent, FalldownComponent comp)
    {
        if (comp.IsDown || !TryComp<StandingStateComponent>(ent, out var standingState) || !standingState.Standing)
            return;

        if (_buckleSystem.IsBuckled(ent))
            _buckleSystem.TryUnbuckle(ent, ent);

        if (!IsClientSide(ent))
            comp.IsDown = true;

        _standing.Down(ent);
        _stunSystem.TryStun(ent, comp.FallDelay, true);

        if (TryComp(ent, out FixturesComponent? fixtureComponent))
        {
            foreach (var key in standingState.ChangedFixtures)
            {
                if (fixtureComponent.Fixtures.TryGetValue(key, out var fixture))
                    _physics.SetCollisionMask(ent, key, fixture, fixture.CollisionMask | (int) CollisionGroup.MidImpassable, fixtureComponent);
            }
        }
        standingState.ChangedFixtures.Clear();
        DoAfterAction(ent, comp);
    }

    private void Standup(EntityUid ent, FalldownComponent comp)
    {
        if (comp.IsDown == false)
            return;

        comp.IsDown = false;
        _standing.Stand(ent);
        _stunSystem.TryStun(ent, comp.StandDelay, true);
        DoAfterAction(ent, comp);
    }

    /// <summary>
    /// Оно падает. И встаёт.
    /// </summary>
    private void OnFalldownAction(EntityUid ent, FalldownComponent comp, FalldownActionEvent ev)
    {
        if (comp.FalldownActionEntity is null)
            return;

        if (comp.IsDown)
        {
            Standup(ent, comp);
        }
        else
        {
            Falldown(ent, comp);
        }
    }

    /// <summary>
    /// Если объект упал сам - не позволяем ему встать от других ивентов (к примеру - прошел стаминакрит).
    /// </summary>
    private void OnStandAttemptEvent(EntityUid ent, FalldownComponent comp, StandAttemptEvent ev)
    {
        if (comp.IsDown)
            ev.Cancel();
    }

    /// <summary>
    /// Обновляем иконку действия и модификатор скорости.
    /// </summary>
    private void DoAfterAction(EntityUid ent, FalldownComponent comp)
    {
        _actions.SetToggled(comp.FalldownActionEntity, comp.IsDown);
        _actions.SetUseDelay(comp.FalldownActionEntity, comp.IsDown ? comp.FallDelay : comp.StandDelay);
        _actions.StartUseDelay(comp.FalldownActionEntity);

        _movementSpeedModifier.RefreshMovementSpeedModifiers(ent);
        Dirty(ent, comp);
    }

    /// <summary>
    /// Замедляем движение если объект упал.
    /// </summary>
    private void OnRefreshMovespeed(EntityUid uid, FalldownComponent comp, RefreshMovementSpeedModifiersEvent args)
    {
        if (!comp.IsDown)
            return;

        args.ModifySpeed(0.2f, 0.2f);
    }
}
