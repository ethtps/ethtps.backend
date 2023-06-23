namespace ETHTPS.API.BIL.Security.Humanity
{
    public interface IHumanityCheckService
    {
        Task<bool> CheckHumanityAsync(string humanityProof);
        bool IsHumanityCheckRequired { get; }
    }
}
