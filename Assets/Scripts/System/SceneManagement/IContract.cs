using Cysharp.Threading.Tasks;

public interface IBeforeSceneLoad
{
    UniTask OnBeforeSceneLoad();
}

public interface IAfterSceneLoad
{
    UniTask OnAfterSceneLoad();
}
