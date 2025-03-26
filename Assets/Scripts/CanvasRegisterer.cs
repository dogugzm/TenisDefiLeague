using Assets.Scripts.PanelService;
using UnityEngine;
using VContainer;

public class CanvasRegisterer : MonoBehaviour
{
    private CanvasHandler _canvasHandler;
    [SerializeField] private CanvasId canvasId;

    [Inject]
    private void Injection(CanvasHandler canvasHandler)
    {
        _canvasHandler = canvasHandler;
    }

    private void Start()
    {
        _canvasHandler.RegisterCanvas(canvasId, transform);
    }
}