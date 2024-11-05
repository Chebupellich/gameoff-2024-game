using UnityEngine;

public class ShowPanelButton : MonoBehaviour
{
    public string PanelId;

    public PanelShowBehaviour Behaviour;
    
    private PanelManager _panelManager;

    private void Start()
    {
        _panelManager = PanelManager.Instance;
    }

    public void DoShowPanel()
    {
        _panelManager.ShowPanel(PanelId, Behaviour);
    }
}
