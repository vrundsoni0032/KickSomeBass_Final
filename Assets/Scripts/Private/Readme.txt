The KSB_MainCore is the first object to instantiate in the game.
It is responsible to instantiate all the other game cores.
It is also responsible to load and unload levels in the scene.
Later, it will also be responsible to show the main menu and showing the loading screen between asynchronous level load calls.

For code,
To notify the level is ended to the main core, just call NotifyEndScene() on your respective game core, passing the name for the next scene to load.

KSB_GameHelper.GetFishCore().NotifyForEndScene(SceneToLoadName, KSB_GameHelper.GetRespectiveCore());