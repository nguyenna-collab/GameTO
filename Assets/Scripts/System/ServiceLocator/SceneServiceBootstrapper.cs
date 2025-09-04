namespace Service_Locator
{
    public class SceneServiceBootstrapper : Bootstrapper
    {
        public override void Bootstrap()
        {
            Container.ConfigureForScene();
        }
    }
}