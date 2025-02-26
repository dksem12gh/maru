public class UiManager
{
    PopupContoroller _popupCtrl = new PopupContoroller();
    SignalController _signalCtrl = new SignalController();    
    
    public PopupContoroller Popup { get { return _popupCtrl; } }
    public SignalController Signal { get { return _signalCtrl; } }
}
