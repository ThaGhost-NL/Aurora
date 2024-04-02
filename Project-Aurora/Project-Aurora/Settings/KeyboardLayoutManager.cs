using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using AuroraRgb.EffectsEngine;
using AuroraRgb.Settings.Controls.Keycaps;
using AuroraRgb.Settings.Layouts;
using AuroraRgb.Utils;
using Common;
using Common.Devices;
using RazerSdkReader;
using Application = System.Windows.Application;
using Color = System.Drawing.Color;
using MessageBox = System.Windows.MessageBox;

namespace AuroraRgb.Settings;

[JsonSerializable(typeof(VirtualGroup))]
[JsonSerializable(typeof(VirtualGroupConfiguration))]
[JsonSerializable(typeof(KeyboardLayout))]
internal partial class LayoutsSourceGenerationContext : JsonSerializerContext;

public class KeyboardLayoutManager
{
    private const string CulturesFolder = "kb_layouts";

    public Dictionary<DeviceKeys, DeviceKeys> LayoutKeyConversion { get; private set; } = new();

    private VirtualGroup _virtualKeyboardGroup = new();

    private readonly Dictionary<DeviceKeys, Keycap> _virtualKeyboardMap = new();

    public Task<Grid> VirtualKeyboard { get; }

    public Task<Panel> AbstractVirtualKeyboard => CreateUserControl(_virtualKeyboardGroup, CancellationToken.None, true);

    public delegate void LayoutUpdatedEventHandler(object? sender);

    public event LayoutUpdatedEventHandler? KeyboardLayoutUpdated;

    public PreferredKeyboardLocalization LoadedLocalization { get; private set; } = PreferredKeyboardLocalization.None;

    private readonly string _layoutsPath;

    private readonly Task<ChromaReader?> _rzSdk;

    private CancellationTokenSource _cancellationTokenSource = new();

    public KeyboardLayoutManager(Task<ChromaReader?> rzSdk)
    {
        _rzSdk = rzSdk;
        _layoutsPath = Path.Combine(Global.ExecutingDirectory, CulturesFolder);
        var vkTcs = new TaskCompletionSource<Grid>(TaskCreationOptions.RunContinuationsAsynchronously);
        VirtualKeyboard = vkTcs.Task;
        Application.Current.Dispatcher.BeginInvoke(() =>
        {
            var grid = new Grid { Width = 8, Height = 8, MaxWidth = double.PositiveInfinity, MaxHeight = double.PositiveInfinity };
            vkTcs.SetResult(grid);
        }, DispatcherPriority.Loaded);
    }

    public async Task Initialize()
    {
        await LoadBrandDefault();

        Global.Configuration.PropertyChanged += Configuration_PropertyChanged;
    }

    private async Task LoadBrandDefault()
    {
        var cancellationTokenSource = _cancellationTokenSource;
        await cancellationTokenSource.CancelAsync();

        var newCancelSource = new CancellationTokenSource();
        _cancellationTokenSource = newCancelSource;
        var virtualKeyboardGroup = new VirtualGroup();

        try
        {
            await LoadBrand(
                virtualKeyboardGroup, newCancelSource.Token,
                Global.Configuration.KeyboardBrand,
                Global.Configuration.MousePreference,
                Global.Configuration.MousepadPreference,
                Global.Configuration.MouseOrientation,
                Global.Configuration.HeadsetPreference,
                Global.Configuration.ChromaLedsPreference
            );
            _virtualKeyboardGroup = virtualKeyboardGroup;
        }
        catch (OperationCanceledException)
        {
            // load cancelled, go on
        }

        cancellationTokenSource.Dispose();
    }

    private async Task LoadBrand(VirtualGroup virtualKeyboardGroup,
        CancellationToken cancellationToken,
        PreferredKeyboard keyboardPreference = PreferredKeyboard.None,
        PreferredMouse mousePreference = PreferredMouse.None,
        PreferredMousepad mousepadPreference = PreferredMousepad.None,
        MouseOrientationType mouseOrientation = MouseOrientationType.RightHanded,
        PreferredHeadset headsetPreference = PreferredHeadset.None,
        PreferredChromaLeds chromaLeds = PreferredChromaLeds.Automatic)
    {
#if !DEBUG
        try
        {
#endif
            //Load keyboard layout
            if (!Directory.Exists(_layoutsPath))
            {
                return;
            }

            var layout = Global.Configuration.KeyboardLocalization;

            var culture = layout switch
            {
                PreferredKeyboardLocalization.None => Thread.CurrentThread.CurrentCulture.Name,
                PreferredKeyboardLocalization.intl => "intl",
                PreferredKeyboardLocalization.us => "en-US",
                PreferredKeyboardLocalization.uk => "en-GB",
                PreferredKeyboardLocalization.ru => "ru-RU",
                PreferredKeyboardLocalization.fr => "fr-FR",
                PreferredKeyboardLocalization.de => "de-DE",
                PreferredKeyboardLocalization.jpn => "ja-JP",
                PreferredKeyboardLocalization.nordic => "nordic",
                PreferredKeyboardLocalization.tr => "tr-TR",
                PreferredKeyboardLocalization.swiss => "de-CH",
                PreferredKeyboardLocalization.abnt2 => "pt-BR",
                PreferredKeyboardLocalization.dvorak => "dvorak",
                PreferredKeyboardLocalization.dvorak_int => "dvorak_int",
                PreferredKeyboardLocalization.hu => "hu-HU",
                PreferredKeyboardLocalization.it => "it-IT",
                PreferredKeyboardLocalization.la => "es-AR",
                PreferredKeyboardLocalization.es => "es-ES",
                PreferredKeyboardLocalization.iso => "iso",
                PreferredKeyboardLocalization.ansi => "ansi",
                _ => Thread.CurrentThread.CurrentCulture.Name
            };


            LoadedLocalization = GetLocalization(culture);
            await LoadCulture(GetCulture(culture), virtualKeyboardGroup, cancellationToken);

            if (PeripheralLayoutMap.KeyboardLayoutMap.TryGetValue(keyboardPreference, out var keyboardLayoutFile))
            {
                var layoutConfigPath = Path.Combine(_layoutsPath, keyboardLayoutFile);
                await LoadKeyboard(layoutConfigPath, virtualKeyboardGroup, cancellationToken);
            }

            if (PeripheralLayoutMap.MouseLayoutMap.TryGetValue(mousePreference, out var mouseLayoutJsonFile))
            {
                var mouseFeaturePath = Path.Combine(_layoutsPath, "Extra Features", mouseLayoutJsonFile);

                await LoadMouse(mouseOrientation, mouseFeaturePath, virtualKeyboardGroup, cancellationToken);
            }

            if (PeripheralLayoutMap.MousepadLayoutMap.TryGetValue(mousepadPreference, out var mousepadLayoutJsonFile))
            {
                var mousepadFeaturePath = Path.Combine(_layoutsPath, "Extra Features", mousepadLayoutJsonFile);

                await LoadGenericLayout(mousepadFeaturePath, virtualKeyboardGroup, cancellationToken);
            }

            if (PeripheralLayoutMap.HeadsetLayoutMap.TryGetValue(headsetPreference, out var headsetLayoutJsonFile))
            {
                var headsetFeaturePath = Path.Combine(_layoutsPath, "Extra Features", headsetLayoutJsonFile);

                await LoadGenericLayout(headsetFeaturePath, virtualKeyboardGroup, cancellationToken);
            }

            if (chromaLeds == PreferredChromaLeds.Automatic && await _rzSdk is not null)
            {
                chromaLeds = PreferredChromaLeds.Suggested;
            }

            if (PeripheralLayoutMap.ChromaLayoutMap.TryGetValue(chromaLeds, out var chromaLayoutJsonFile))
            {
                var headsetFeaturePath = Path.Combine(_layoutsPath, "Extra Features", chromaLayoutJsonFile);

                await LoadGenericLayout(headsetFeaturePath, virtualKeyboardGroup, cancellationToken);
            }
#if !DEBUG
        }
        catch (Exception e)
        {
            Global.logger.Error(e, "Error loading layouts");
        }
#endif

        await Application.Current.Dispatcher.InvokeAsync(async () =>
        {
            try
            {
                await CreateUserControl(virtualKeyboardGroup, cancellationToken);
                KeyboardLayoutUpdated?.Invoke(this);
            }
            catch (Exception e)
            {
                Global.logger.Error(e, "Keyboard control generation failed");
            }
        }, DispatcherPriority.Loaded, cancellationToken);
    }

    private PreferredKeyboardLocalization GetLocalization(string culture)
    {
        switch (culture)
        {
            case "de-DE":
            case "hsb-DE":
            case "dsb-DE":
                return PreferredKeyboardLocalization.de;
            case "fr-CH":
            case "de-CH":
                return PreferredKeyboardLocalization.swiss;
            case "fr-FR":
            case "br-FR":
            case "oc-FR":
            case "co-FR":
            case "gsw-FR":
                return PreferredKeyboardLocalization.fr;
            case "cy-GB":
            case "gd-GB":
            case "en-GB":
                return PreferredKeyboardLocalization.uk;
            case "ru-RU":
            case "tt-RU":
            case "ba-RU":
            case "sah-RU":
                return PreferredKeyboardLocalization.ru;
            case "en-US":
                return PreferredKeyboardLocalization.us;
            case "da-DK":
            case "se-SE":
            case "nb-NO":
            case "nn-NO":
            case "nordic":
                return PreferredKeyboardLocalization.nordic;
            case "pt-BR":
                return PreferredKeyboardLocalization.abnt2;
            case "dvorak":
                return PreferredKeyboardLocalization.dvorak;
            case "dvorak_int":
                return PreferredKeyboardLocalization.dvorak_int;
            case "hu-HU":
                return PreferredKeyboardLocalization.hu;
            case "it-IT":
                return PreferredKeyboardLocalization.it;
            case "es-AR":
            case "es-BO":
            case "es-CL":
            case "es-CO":
            case "es-CR":
            case "es-EC":
            case "es-MX":
            case "es-PA":
            case "es-PY":
            case "es-PE":
            case "es-UY":
            case "es-VE":
            case "es-419":
                return PreferredKeyboardLocalization.la;
            case "es-ES":
                return PreferredKeyboardLocalization.es;
            case "iso":
                return PreferredKeyboardLocalization.iso;
            case "ansi":
                return PreferredKeyboardLocalization.ansi;
            default:
                return PreferredKeyboardLocalization.intl;
        }
    }

    private static string GetCulture(string culture)
    {
        switch (culture)
        {
            case "tr-TR":
                return "tr";
            case "ja-JP":
                return "jpn";
            case "de-DE":
            case "hsb-DE":
            case "dsb-DE":
                return "de";
            case "fr-CH":
            case "de-CH":
                return "swiss";
            case "fr-FR":
            case "br-FR":
            case "oc-FR":
            case "co-FR":
            case "gsw-FR":
                return "fr";
            case "cy-GB":
            case "gd-GB":
            case "en-GB":
                return "uk";
            case "ru-RU":
            case "tt-RU":
            case "ba-RU":
            case "sah-RU":
                return "ru";
            case "en-US":
                return "us";
            case "da-DK":
            case "se-SE":
            case "nb-NO":
            case "nn-NO":
            case "nordic":
                return "nordic";
            case "pt-BR":
                return "abnt2";
            case "dvorak":
                return "dvorak";
            case "dvorak_int":
                return "dvorak_int";
            case "hu-HU":
                return "hu";
            case "it-IT":
                return "it";
            case "es-AR":
            case "es-BO":
            case "es-CL":
            case "es-CO":
            case "es-CR":
            case "es-EC":
            case "es-MX":
            case "es-PA":
            case "es-PY":
            case "es-PE":
            case "es-UY":
            case "es-VE":
            case "es-419":
                return "la";
            case "es-ES":
                return "es";
            case "iso":
                return "iso";
            case "ansi":
                return "ansi";
            default:
                return "intl";
        }
    }

    private async Task<VirtualGroup?> LoadLayout(string path, CancellationToken cancellationToken)
    {
        if (!File.Exists(path))
        {
            MessageBox.Show(path + " could not be found", "Layout not found", MessageBoxButton.OK);
            return null;
        }

        var featureContent = await File.ReadAllTextAsync(path, Encoding.UTF8, cancellationToken);
        return JsonSerializer.Deserialize(featureContent, LayoutsSourceGenerationContext.Default.VirtualGroup);
    }

    private async Task LoadKeyboard(string layoutConfigPath, VirtualGroup virtualKeyboardGroup, CancellationToken cancellationToken)
    {
        if (!File.Exists(layoutConfigPath))
        {
            MessageBox.Show(layoutConfigPath + " could not be found", "Layout not found", MessageBoxButton.OK);
            return;
        }

        var content = await File.ReadAllTextAsync(layoutConfigPath, Encoding.UTF8, cancellationToken);
        var layoutConfig = JsonSerializer.Deserialize(content, LayoutsSourceGenerationContext.Default.VirtualGroupConfiguration)!;

        virtualKeyboardGroup.AdjustKeys(layoutConfig.KeyModifications);
        virtualKeyboardGroup.RemoveKeys(layoutConfig.KeysToRemove);

        foreach (var key in layoutConfig.KeyConversion.Where(key => !LayoutKeyConversion.ContainsKey(key.Key)))
        {
            LayoutKeyConversion.Add(key.Key, key.Value);
        }

        foreach (var feature in layoutConfig.IncludedFeatures)
        {
            var featurePath = Path.Combine(_layoutsPath, "Extra Features", feature);

            if (!File.Exists(featurePath)) continue;
            var featureContent = await File.ReadAllTextAsync(featurePath, Encoding.UTF8, cancellationToken);
            var featureConfig = JsonSerializer.Deserialize(featureContent, LayoutsSourceGenerationContext.Default.VirtualGroup)!;

            virtualKeyboardGroup.AddFeature(featureConfig.GroupedKeys.ToArray(), featureConfig.OriginRegion);

            foreach (var key in featureConfig.KeyConversion.Where(key => !LayoutKeyConversion.ContainsKey(key.Key)))
            {
                LayoutKeyConversion.Add(key.Key, key.Value);
            }
        }
    }

    private async Task LoadMouse(MouseOrientationType mouseOrientation, string mouseFeaturePath, VirtualGroup virtualKeyboardGroup,
        CancellationToken cancellationToken)
    {
        var featureConfig = await LoadLayout(mouseFeaturePath, cancellationToken);
        if (featureConfig == null)
        {
            return;
        }

        if (mouseOrientation == MouseOrientationType.LeftHanded)
        {
            if (featureConfig.OriginRegion == KeyboardRegion.TopRight)
                featureConfig.OriginRegion = KeyboardRegion.TopLeft;
            else if (featureConfig.OriginRegion == KeyboardRegion.BottomRight)
                featureConfig.OriginRegion = KeyboardRegion.BottomLeft;

            var outlineWidth = 0.0;

            foreach (var key in featureConfig.GroupedKeys)
            {
                if (outlineWidth == 0.0 && key.Tag == DeviceKeys.NONE)
                {
                    //We found outline (NOTE: Outline has to be first in the grouped keys)
                    outlineWidth = key.Width + 2 * key.MarginLeft;
                }

                key.MarginLeft -= outlineWidth;
            }
        }

        virtualKeyboardGroup.AddFeature(featureConfig.GroupedKeys.ToArray(), featureConfig.OriginRegion);
    }

    private async Task LoadGenericLayout(string headsetFeaturePath, VirtualGroup virtualKeyboardGroup, CancellationToken cancellationToken)
    {
        var featureConfig = await LoadLayout(headsetFeaturePath, cancellationToken);
        if (featureConfig == null)
        {
            return;
        }

        virtualKeyboardGroup.AddFeature(featureConfig.GroupedKeys.ToArray(), featureConfig.OriginRegion);
    }

    private static int PixelToByte(double pixel)
    {
        return (int)Math.Round(pixel / (double)Global.Configuration.BitmapAccuracy);
    }

    private void Configuration_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        IEnumerable<string> relatedProperties =
        [
            nameof(Configuration.BitmapAccuracy),
            nameof(Configuration.VirtualkeyboardKeycapType),
            nameof(Configuration.KeyboardBrand), nameof(Configuration.KeyboardLocalization),
            nameof(Configuration.MousePreference), nameof(Configuration.MouseOrientation),
            nameof(Configuration.MousepadPreference),
            nameof(Configuration.HeadsetPreference),
            nameof(Configuration.ChromaLedsPreference),
        ];
        if (!relatedProperties.Contains(e.PropertyName)) return;

        Global.LightingStateManager.PreUpdate += LightingStateManager_LoadLayout;
    }

    private async void LightingStateManager_LoadLayout(object? sender, EventArgs e)
    {
        Global.LightingStateManager.PreUpdate -= LightingStateManager_LoadLayout;
        await LoadBrandDefault();
    }

    private void CalculateBitmap(KeyboardControlGenerator kcg, VirtualGroup virtualKeyboardGroup)
    {
        double curWidth = 0;
        double curHeight = 0;
        double widthMax = 1;
        double heightMax = 1;
        var bitmapMap =
            new Dictionary<DeviceKeys, BitmapRectangle>(Effects.MaxDeviceId, EnumHashGetter.Instance as IEqualityComparer<DeviceKeys>);

        foreach (var key in virtualKeyboardGroup.GroupedKeys)
        {
            if (key.Tag.Equals(DeviceKeys.NONE))
                continue;

            var width = key.Width;
            var widthBit = PixelToByte(width);
            var height = key.Height;
            var heightBit = PixelToByte(height);
            var xOffset = key.MarginLeft;
            var yOffset = key.MarginTop;
            double brX, brY;

            if (key.AbsoluteLocation)
            {
                bitmapMap[key.Tag] = new BitmapRectangle(PixelToByte(xOffset), PixelToByte(yOffset), widthBit, heightBit);
                brX = xOffset + width;
                brY = yOffset + height;
            }
            else
            {
                var x = xOffset + curWidth;
                var y = yOffset + curHeight;

                bitmapMap[key.Tag] = new BitmapRectangle(PixelToByte(x), PixelToByte(y), widthBit, heightBit);

                brX = x + width;
                brY = y + height;

                if (key.LineBreak)
                {
                    curHeight += 37;
                    curWidth = 0;
                }
                else
                {
                    curWidth = brX;
                    curHeight = Math.Max(curHeight, y);
                }
            }

            widthMax = Math.Max(widthMax, brX);
            heightMax = Math.Max(heightMax, brY);
        }

        //+1 for rounding error, where the bitmap rectangle B(X)+B(Width) > B(X+Width)
        Effects.Canvas = new EffectCanvas(
            PixelToByte(virtualKeyboardGroup.Region.Width),
            PixelToByte(virtualKeyboardGroup.Region.Height),
            bitmapMap,
            (float)kcg.BaselineX, (float)kcg.BaselineY,
            (float)kcg.GridWidth, (float)kcg.GridHeight
        );
    }

    private async Task<Panel> CreateUserControl(VirtualGroup virtualKeyboardGroup, CancellationToken cancellationToken,
        bool abstractKeycaps = false)
    {
        if (!abstractKeycaps)
            _virtualKeyboardMap.Clear();

        var virtualKb = abstractKeycaps ? new Grid() : await VirtualKeyboard;
        var kcg = new KeyboardControlGenerator(abstractKeycaps, _virtualKeyboardMap, virtualKeyboardGroup, _layoutsPath, virtualKb,
            cancellationToken);

        var keyboardControl = await kcg.Generate();
        CalculateBitmap(kcg, virtualKeyboardGroup);
        return keyboardControl;
    }

    private async Task LoadCulture(string culture, VirtualGroup virtualKeyboardGroup, CancellationToken cancellationToken)
    {
        var fileName = "Plain Keyboard\\layout." + culture + ".json";
        var layoutPath = Path.Combine(_layoutsPath, fileName);

        if (!File.Exists(layoutPath))
        {
            return;
        }

        var content = await File.ReadAllTextAsync(layoutPath, Encoding.UTF8, cancellationToken);
        var keyboard = JsonSerializer.Deserialize(content, LayoutsSourceGenerationContext.Default.KeyboardLayout)!;

        virtualKeyboardGroup.Clear(keyboard.Keys);

        LayoutKeyConversion = keyboard.KeyConversion;
    }

    public void SetKeyboardColors(Dictionary<DeviceKeys, SimpleColor> keyLights, CancellationToken cancellationToken)
    {
        foreach (var (key, value) in _virtualKeyboardMap)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            if (!keyLights.TryGetValue(key, out var keyColor)) continue;
            // cancel low priority calls when render stops
            var opaqueColor = ColorUtils.MultiplyColorByScalar(keyColor, keyColor.A / 255.0D);
            var drawingColor = Color.FromArgb(255, opaqueColor.R, opaqueColor.G, opaqueColor.B);
            value.SetColor(ColorUtils.DrawingColorToMediaColor(drawingColor));
        }
    }
}