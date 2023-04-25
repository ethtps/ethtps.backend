namespace ETHTPS.Daemon.Infra
{
    public interface IDaemonAction<T>
    {
        public Task RunAsync();
    }
}
