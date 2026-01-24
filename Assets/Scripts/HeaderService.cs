using PanelService;
using PanelsViews;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;
using VContainer.Unity;

public interface ITitleHeader : IHeader<HeaderPanelViewTitle, HeaderPanelViewTitle.Data>
{
}

public interface IUserHeader : IHeader<HeaderPanelViewUser, HeaderPanelViewUser.Data>
{
}


public class HeaderService : IInitializable
{
    private readonly IPanelService _panelService;
    private readonly IObjectResolver _objectResolver;
    private HeaderPanelViewTitle headerTitlePrefab;
    private HeaderPanelViewUser headerUserPrefab;

    public HeaderService(IPanelService panelService, HeaderPanelViewTitle headerTitlePrefab,
        HeaderPanelViewUser headerUserPrefab, IObjectResolver objectResolver)
    {
        _panelService = panelService;
        this.headerTitlePrefab = headerTitlePrefab;
        this.headerUserPrefab = headerUserPrefab;
        _objectResolver = objectResolver;
    }
    
    public void Initialize()
    {
        _panelService.OnPanelShow += OnPanelShow;
    }

    private void OnPanelShow(PanelBase panel)
    {
        if (panel is ITitleHeader titleHeader)
        {
            if (titleHeader.HeaderView == null)
            {
                var headerObject = _objectResolver.Instantiate(headerTitlePrefab, titleHeader.GetHeaderParent());
                titleHeader.HeaderView = headerObject;
                headerObject.Init(titleHeader.HeaderData);
            }
            else
            {
                titleHeader.HeaderView.Init(titleHeader.HeaderData);
            }
        }
        else if (panel is IUserHeader userHeader)
        {
            if (userHeader.HeaderView == null)
            {
                var headerObject = _objectResolver.Instantiate(headerUserPrefab, userHeader.GetHeaderParent());
                userHeader.HeaderView = headerUserPrefab;
                headerObject.Init(userHeader.HeaderData);
            }
            else
            {
                userHeader.HeaderView.Init(userHeader.HeaderData);
            }
        }
    }
}