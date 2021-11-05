using ImGuiNET;
using MouseTargetTooltip.WindowTypes;
using System;
using System.Diagnostics;
using System.Numerics;
using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using MouseTargetTooltip.Enums;
using static MouseTargetTooltip.Constants.Constants;

namespace MouseTargetTooltip
{
    public class PluginUi : IDisposable
    {
        internal readonly Configuration Configuration;

        private GameObject? _mouseTarget;
        private GameObject? _lastKnownActor;
        private float _alpha;
        private Stopwatch _fadeTimer;
        private Stopwatch _uiSpeedTimer;
        private double _uiSpeed;
        
        private bool _visible;
        public bool Visible
        {
            get => _visible;
            set => _visible = value;
        }

        private bool _settingsVisible;
        public bool SettingsVisible
        {
            get => _settingsVisible;
            set => _settingsVisible = value;
        }
        
        public PluginUi(Configuration configuration)
        {
            Configuration = configuration;
            _fadeTimer = new Stopwatch();
            _uiSpeedTimer = Stopwatch.StartNew();
        }

        public void Dispose()
        {
        }

        public void Draw(GameObject? mouseOverTarget)
        {
            _mouseTarget = mouseOverTarget;

            _uiSpeed = _uiSpeedTimer.ElapsedMilliseconds / 1000f;

            if (_mouseTarget != null)
            {
                _lastKnownActor = _mouseTarget;
                _fadeTimer.Restart();
                _alpha = 1;
            }
            else
            {
                if (_fadeTimer.ElapsedMilliseconds > Configuration.FadeDelay * 1000 && _alpha > 0)
                {
                    _alpha -= (float)(1 / Math.Max(Configuration.FadeTime, 0.0001f) * _uiSpeed);
                    if (_alpha < 0)
                    {
                        _alpha = 0;
                        _lastKnownActor = null;
                    }
                }
            }

            DrawTooltipWindow();
            DrawSettingsWindow();

            _uiSpeedTimer.Restart();
        }
        
        public void DrawTooltipWindow()
        {
            if (_lastKnownActor is null)
            {
                if (Configuration.Movable)
                {
                    if (!ImGui.IsMouseDragging(ImGuiMouseButton.Left))
                        ImGui.SetNextWindowPos(new Vector2(Configuration.TooltipX, Configuration.TooltipY), ImGuiCond.Always, PivotAlignment(MouseTooltipPlugin.Ui!.Configuration.TooltipAlignment));

                    _visible = true;
                    DebugWindow.Draw(1, Configuration);
                }

                return;
            }

            ImGui.SetNextWindowPos(new Vector2(Configuration.TooltipX, Configuration.TooltipY), ImGuiCond.Always, PivotAlignment(MouseTooltipPlugin.Ui!.Configuration.TooltipAlignment));

            switch (_lastKnownActor.ObjectKind)
            {
                case ObjectKind.BattleNpc:
                    BattleNpcWindow.Draw(_lastKnownActor as BattleNpc, _alpha, Configuration);
                    break;
                case ObjectKind.Player:
                    PlayerWindow.Draw(_lastKnownActor as PlayerCharacter, _alpha, Configuration);
                    break;
                default:
                    DefaultWindow.Draw(_lastKnownActor, _alpha);
                    break;
            }
        }

        public void DrawSettingsWindow()
        {
            if (!SettingsVisible)
            {
                return;
            }

            ImGui.SetNextWindowSize(new Vector2(300, 300), ImGuiCond.FirstUseEver);
            if (ImGui.Begin("Kingdom Hearts Bars: Settings", ref _settingsVisible,
                ImGuiWindowFlags.NoCollapse))
            {
                ImGui.BeginTabBar("MouseTargetTooltipBar");
                if (ImGui.BeginTabItem("General"))
                {
                    var movable = Configuration.Movable;
                    if (ImGui.Checkbox("Draggable", ref movable))
                    {
                        Configuration.Movable = movable;
                        Configuration.Save();
                    }

                    ImGui.NewLine();

                    if (ImGui.BeginCombo("Tooltip Window Alignment", Enum.GetName(Configuration.TooltipAlignment)))
                    {
                        var alignments = Enum.GetNames(typeof(WindowAlignment));
                        for (int i = 0; i < alignments.Length; i++)
                        {
                            if (ImGui.Selectable(alignments[i]))
                            {
                                Configuration.TooltipAlignment = (WindowAlignment)i;
                            }
                        }
                        ImGui.EndCombo();
                    }

                    ImGui.NewLine();

                    var showName = Configuration.ShowName;
                    if (ImGui.Checkbox("Show Name", ref showName))
                    {
                        Configuration.ShowName = showName;
                        Configuration.Save();
                    }
                    var showLevel = Configuration.ShowLevel;
                    if (ImGui.Checkbox("Show Level", ref showLevel))
                    {
                        Configuration.ShowLevel = showLevel;
                        Configuration.Save();
                    }
                    var showJob = Configuration.ShowJob;
                    if (ImGui.Checkbox("Show Job", ref showJob))
                    {
                        Configuration.ShowJob = showJob;
                        Configuration.Save();
                    }
                    ImGui.NewLine();
                    ImGui.Text("Fading");
                    ImGui.Separator();
                    var fadeDelay = Configuration.FadeDelay;
                    if (ImGui.InputFloat("Fade Delay", ref fadeDelay))
                    {
                        Configuration.FadeDelay = fadeDelay;
                        Configuration.Save();
                    }
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.Begin("MTTS ToolTip", ImGuiWindowFlags.Tooltip | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoTitleBar);
                        ImGui.Text("Number of seconds to start fading");
                        ImGui.End();
                    }
                    var fadeTime = Configuration.FadeTime;
                    if (ImGui.InputFloat("Fade Time", ref fadeTime))
                    {
                        if (fadeTime < 0f) fadeTime = 0f;
                        Configuration.FadeTime = fadeTime;
                        Configuration.Save();
                    }
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.Begin("MTTS ToolTip", ImGuiWindowFlags.Tooltip | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoTitleBar);
                        ImGui.Text("Number of seconds to fade completely");
                        ImGui.End();
                    }
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Parameter Bars"))
                {
                    var showHealth = Configuration.ShowHealthBar;
                    if (ImGui.Checkbox("Show Health Bar", ref showHealth))
                    {
                        Configuration.ShowHealthBar = showHealth;
                        Configuration.Save();
                    }
                    var showHealthValue = Configuration.ShowHpValue;
                    if (ImGui.Checkbox("Show Health Value", ref showHealthValue))
                    {
                        Configuration.ShowHpValue = showHealthValue;
                        Configuration.Save();
                    }
                    var showResource = Configuration.ShowMpBar;
                    if (ImGui.Checkbox("Show Resource Bar", ref showResource))
                    {
                        Configuration.ShowMpBar = showResource;
                        Configuration.Save();
                    }
                    var showResourceValue = Configuration.ShowMpValue;
                    if (ImGui.Checkbox("Show Resource Value", ref showResourceValue))
                    {
                        Configuration.ShowMpValue = showResourceValue;
                        Configuration.Save();
                    }
                    ImGui.EndTabItem();
                }
                ImGui.EndTabBar();
            }
            ImGui.End();
        }
    }
}
