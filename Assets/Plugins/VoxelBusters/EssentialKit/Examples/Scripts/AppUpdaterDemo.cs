// internal namespace
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;

namespace VoxelBusters.EssentialKit.Demo
{
	public class AppUpdaterDemo : DemoActionPanelBase<AppUpdaterDemoAction, AppUpdaterDemoActionType>
	{
		#region Base methods

        protected override void OnActionSelectInternal(AppUpdaterDemoAction selectedAction)
        {
            switch (selectedAction.ActionType)
            {
                case AppUpdaterDemoActionType.RequestUpdateInfo:
					Log("Requesting update info."); 
                    AppUpdater.RequestUpdateInfo((result, error) => {
                        if(error == null)
                        {
                            Log("Received updated info: " + result);
                        }
                        else
                        {
                            Log("Failed to receive updated info: " + error);
                        }
                    });
                    break;

                case AppUpdaterDemoActionType.PromptUpdate:
					Log("Prompting an update."); 
                    PromptUpdateOptions options = new PromptUpdateOptions.Builder()
                                                    .SetIsForceUpdate(true)
                                                    .SetPromptTitle("Update Available")
                                                    .SetPromptMessage("There is an update available. Do you want to update?")
                                                    .Build();

                    AppUpdater.PromptUpdate(options, (isSuccess, error) => {
                        if(error == null)
                        {
                            Log("User opted to update.");
                        }
                        else
                        {
                            Log("Failed to update: " + error);
                        }
                    });
                    break;
                case AppUpdaterDemoActionType.ResourcePage:
                    ProductResources.OpenResourcePage(NativeFeatureType.kAppUpdater);
                    break;

                default:
                    break;
            }
        }
        
        #endregion
	}
}