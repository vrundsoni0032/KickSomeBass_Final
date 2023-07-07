using Core.EventSystem;

namespace Core.Controller
{
public interface KSB_IController
{
    public void HandleInputEvent(KSB_IEvent pEvent);
    public void UnRegisterToEventSystem();
}
}