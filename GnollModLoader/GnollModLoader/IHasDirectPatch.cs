namespace GnollModLoader
{
    // This mod has a direct harmony patch
    public interface IHasDirectPatch
    {

        //Apply harmony patch
        void ApplyPatch(Patcher patcher);
    }
}
