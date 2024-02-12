using Dalamud.Data;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace MouseTargetTooltip
{
    public class MouseTooltipPlugin : IDalamudPlugin
    {
        public static string Name => "Mouse Target Tooltip";

        private const string CommandName = "/mtar";

        public MouseTooltipPlugin(
            [RequiredVersion("1.0")] DalamudPluginInterface? pluginInterface,
            [RequiredVersion("1.0")] ICommandManager? commandManager,
            [RequiredVersion("1.0")] IClientState? clientState,
            [RequiredVersion("1.0")] ITargetManager? targetManager)
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
        public static ICommandManager? Cm { get; private set; }
        public static IClientState? Cs { get; private set; }
        public static PluginUi? Ui { get; private set; }
        public static ITargetManager? Tm { get; private set; }
    }
}
