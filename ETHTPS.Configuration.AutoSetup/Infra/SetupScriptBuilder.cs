namespace ETHTPS.Configuration.AutoSetup.Infra
{
    internal class SetupScriptBuilder
    {
        public string Environment { get; }

        private readonly List<SetupScript> _scripts = new();
        private Action? _pre;
        private Action? _post;
        private Action? _clean;

        public SetupScriptBuilder(string environment)
        {
            Environment = environment;
        }

        public SetupScriptBuilder Add<T>(T script)
            where T : SetupScript
        {
            _scripts.Add(script);
            return this;
        }

        public SetupScriptBuilder AddAction(Action action)
        {
            _scripts.Add(new ActionScript(action));
            return this;
        }

        public SetupScriptBuilder AddPre(Action action)
        {
            _pre = action;
            return this;
        }

        public SetupScriptBuilder AddPost(Action action)
        {
            _post = action;
            return this;
        }

        public SetupScriptBuilder AddClean(Action action)
        {
            _clean = action;
            return this;
        }

        public SetupScript Build()
        {
            return new ActionScript(() =>
            {
                foreach (var script in _scripts)
                {
                    script.Pre();
                    script.Run();
                    script.Post();
                    script.Clean();
                }
            }, _pre, _post, _clean);
        }
    }
}
