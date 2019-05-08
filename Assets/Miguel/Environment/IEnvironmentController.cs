public interface IEnvironmentController
{
    bool Initialize();
    bool Paint();
    bool Release();
    bool PaintRandom();
    bool ReleaseRandom();
}