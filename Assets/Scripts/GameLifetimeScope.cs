using Managers;
using MockDataSystem;
using PanelService;
using PanelsViews;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private PanelSettings _panelSettings;
    [SerializeField] private HeaderPanelViewTitle titleHeaderPrefab;
    [SerializeField] private HeaderPanelViewUser titleHeaderUserPrefab;


    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_panelSettings);
        builder.RegisterInstance(titleHeaderPrefab).AsSelf();
        builder.RegisterInstance(titleHeaderUserPrefab).AsSelf();
        builder.Register<FirebaseService.FirebaseService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        builder.Register<Assets.Scripts.LoadingService.LoadingService>(Lifetime.Singleton).AsImplementedInterfaces()
            .AsSelf();


        builder.Register<PanelService.PanelService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        builder.Register<PanelFactory>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        builder.Register<PanelPool>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        builder.Register<HeaderService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        builder.Register<CanvasHandler>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        builder.Register<LeagueService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        builder.Register<AuthenticationService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        builder.Register<UserManager>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        builder.Register<MatchService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        builder.Register<MockService>(Lifetime.Singleton).AsImplementedInterfaces()
            .AsSelf();
    }
}