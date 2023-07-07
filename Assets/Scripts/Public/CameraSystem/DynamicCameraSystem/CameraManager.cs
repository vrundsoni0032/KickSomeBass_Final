using Core.EventSystem;
using CinematicSystem;
using Events;
using System.Collections.Generic;

namespace Core.CameraSystem
{
public class CameraManager
{
    private Dictionary<string,BaseCamera> m_sceneCameras;
    private BaseCamera m_activeCamera;
    private BaseCamera m_cameraToSwitchTo;
    private CutsceneManager m_cutSceneManager;

    bool m_cutsceneInProgress=false;
    public CameraManager()
    {
        //YCLogger.Info("CameraManager", "Initialized.");
        m_sceneCameras = new Dictionary<string, BaseCamera>();
        m_cutSceneManager=new CutsceneManager();

        GameUtil.EventManager.RegisterSubscriber(HandleCutSceneEvent, PlayCutSceneEvent.EventID);
    }

    public void Update()
    {
        if(m_cutsceneInProgress)
        {
            UpdateCutsceneManager();
        }
    }

    void UpdateCutsceneManager()
    {
        if (m_cutsceneInProgress)
        {
            bool stillRunning = m_cutSceneManager.Update();

            if (!stillRunning)
            {
                if (m_cameraToSwitchTo != null)
                {
                    SwitchCamera(m_cameraToSwitchTo);
                    m_cameraToSwitchTo = null;
                }
                m_cutsceneInProgress = false;
            }
        }
    }

    public void SwitchCamera(BaseCamera aNewActiveCamera)
    {
        if(m_activeCamera)
        m_activeCamera.Deactivate();

        aNewActiveCamera.Activate();

        m_activeCamera = aNewActiveCamera;
    }

    public void SwitchCamera(string aNewActiveCameraName)
    {
        BaseCamera newCamera;
        m_sceneCameras.TryGetValue(aNewActiveCameraName, out newCamera);
        if (newCamera)
        {
            newCamera.Activate();
            if(m_activeCamera)
            m_activeCamera.Deactivate();
            return;
        }
        YCLogger.Warn(aNewActiveCameraName + " camera not found");
    }

    public void RegisterCamera(BaseCamera aCamera)
    {
        if (!aCamera) return;

        if (aCamera.GetName() == "") {YCLogger.Warn(aCamera.name + " Assign a unique m_name in Camera ");}

        if(m_sceneCameras.ContainsKey(aCamera.GetName()))
        {
            YCLogger.Warn(aCamera.name + " Already registered");
            return;
        }
        
        m_sceneCameras.Add(aCamera.GetName(),aCamera);

        //YCLogger.Debug(aCamera.name + " has been registered");
    }

    public BaseCamera GetActiveCamera() { return m_activeCamera;}

    void HandleCutSceneEvent(KSB_IEvent aEvent)
    {
       PlayCutSceneEvent cutSceneEvent = (PlayCutSceneEvent)aEvent;

       CinematicClip clip = cutSceneEvent.GetCinematicClip();

       bool switchBackToCurrentActiveCamera= cutSceneEvent.SwitchCamereaBack();

       m_cameraToSwitchTo = m_activeCamera;

       if (clip)
       {
           SwitchCamera("CinematicCamera");
       
           clip.SetTarget(cutSceneEvent.GetGameObject());
           clip.Initialize();
           m_cutSceneManager.StartCutsceneClip(clip);
           m_cutsceneInProgress = true;
       }
    }
}
}