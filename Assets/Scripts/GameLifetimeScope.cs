using Assets.Scripts.PanelService;
using Managers;
using MockDataSystem;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private PanelSettings _panelSettings;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_panelSettings);
        builder.Register<FirebaseService.FirebaseService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        builder.Register<Assets.Scripts.LoadingService.LoadingService>(Lifetime.Singleton).AsImplementedInterfaces()
            .AsSelf();
        builder.Register<PanelService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        builder.Register<PanelFactory>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        builder.Register<PanelPool>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        builder.Register<CanvasHandler>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        builder.Register<LeagueService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        builder.Register<AuthenticationService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        builder.Register<UserManager>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        builder.Register<MatchService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        builder.Register<MockService>(Lifetime.Singleton).AsImplementedInterfaces()
            .AsSelf();
    }
}