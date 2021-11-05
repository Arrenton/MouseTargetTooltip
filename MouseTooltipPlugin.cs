using Dalamud.Data;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.IoC;
using Dalamud.Plugin;

namespace MouseTargetTooltip
{
    public class MouseTooltipPlugin : IDalamudPlugin
    {
        public string Name => "Mouse Target Tooltip";

        private const string CommandName = "/mtar";
        

        public MouseTooltipPlugin(
            [RequiredVersion("1.0")] DalamudPluginInterface? pluginInterface,
            [RequiredVersion("1.0")] CommandManager? commandManager,
            [RequiredVersion("1.0")] ClientState? clientState,
            [RequiredVersion("1.0")] TargetManager? targetManager)
        {
            Pi = pluginInterface;
            Cm = commandManager;
            Cs = clientState;
            Tm = targetManager;

            var configuration = Pi?.GetPluginConfig() as Configuration ?? new Configuration();
            configuration.Initialize(Pi);

            Ui = new PluginUi(configuration);

            Cm?.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "Toggle config window for Mouse Target Tooltip"
            });

            if (Pi != null)
            {
                Pi.UiBuilder.Draw += DrawUi;
                Pi.UiBuilder.OpenConfigUi += DrawConfigUi;
            }
        }

        public void Dispose()
        {
            Ui?.Dispose();

            Cm?.RemoveHandler(CommandName);

            if (Pi != null)
            {
                Pi.UiBuilder.Draw -= DrawUi;

                Pi.Dispose();
            }
        }

        private void OnCommand(string command, string args)
        {
            // in response to the slash command, just display our main ui
            if (Ui != null) Ui.SettingsVisible = true;
        }

        private void DrawUi()
        {
            Ui?.Draw(Tm?.MouseOverTarget);
        }

        private void DrawConfigUi()
        {
            if (Ui != null) Ui.SettingsVisible = true;
        }
        public static DalamudPluginInterface? Pi { get; private set; }
        public static CommandManager? Cm { get; private set; }
        public static ClientState? Cs { get; private set; }
        public static PluginUi? Ui { get; private set; }
        public static TargetManager? Tm { get; private set; }
    }
}
