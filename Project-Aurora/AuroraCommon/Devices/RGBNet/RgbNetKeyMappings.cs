﻿using System.Collections.ObjectModel;
using RGB.NET.Core;

namespace Common.Devices.RGBNet;

public static class RgbNetKeyMappings
{
    private static readonly Dictionary<LedId, DeviceKeys> KeyNameMap = new()
    {
        { LedId.Keyboard_Escape,										DeviceKeys.ESC },
        { LedId.Keyboard_F1,											DeviceKeys.F1 },
        { LedId.Keyboard_F2,											DeviceKeys.F2 },
        { LedId.Keyboard_F3,											DeviceKeys.F3 },
        { LedId.Keyboard_F4,											DeviceKeys.F4 },
        { LedId.Keyboard_F5,											DeviceKeys.F5 },
        { LedId.Keyboard_F6,											DeviceKeys.F6 },
        { LedId.Keyboard_F7,											DeviceKeys.F7 },
        { LedId.Keyboard_F8,											DeviceKeys.F8 },
        { LedId.Keyboard_F9,											DeviceKeys.F9 },
        { LedId.Keyboard_F10,											DeviceKeys.F10 },
        { LedId.Keyboard_F11,											DeviceKeys.F11 },
        { LedId.Keyboard_F12,											DeviceKeys.F12 },
        { LedId.Keyboard_GraveAccentAndTilde,							DeviceKeys.TILDE },
        { LedId.Keyboard_1,												DeviceKeys.ONE },
        { LedId.Keyboard_2,												DeviceKeys.TWO },
        { LedId.Keyboard_3,												DeviceKeys.THREE },
        { LedId.Keyboard_4,												DeviceKeys.FOUR },
        { LedId.Keyboard_5,												DeviceKeys.FIVE },
        { LedId.Keyboard_6,												DeviceKeys.SIX },
        { LedId.Keyboard_7,												DeviceKeys.SEVEN },
        { LedId.Keyboard_8,												DeviceKeys.EIGHT },
        { LedId.Keyboard_9,												DeviceKeys.NINE },
        { LedId.Keyboard_0,												DeviceKeys.ZERO },
        { LedId.Keyboard_MinusAndUnderscore,							DeviceKeys.MINUS },
        { LedId.Keyboard_Tab,											DeviceKeys.TAB },
        { LedId.Keyboard_Q,												DeviceKeys.Q },
        { LedId.Keyboard_W,												DeviceKeys.W },
        { LedId.Keyboard_E,												DeviceKeys.E },
        { LedId.Keyboard_R,												DeviceKeys.R },
        { LedId.Keyboard_T,												DeviceKeys.T },
        { LedId.Keyboard_Y,												DeviceKeys.Y },
        { LedId.Keyboard_U,												DeviceKeys.U },
        { LedId.Keyboard_I,												DeviceKeys.I },
        { LedId.Keyboard_O,												DeviceKeys.O },
        { LedId.Keyboard_P,												DeviceKeys.P },
        { LedId.Keyboard_BracketLeft,									DeviceKeys.OPEN_BRACKET },
        { LedId.Keyboard_BracketRight,									DeviceKeys.CLOSE_BRACKET },
        { LedId.Keyboard_CapsLock,										DeviceKeys.CAPS_LOCK },
        { LedId.Keyboard_A,												DeviceKeys.A },
        { LedId.Keyboard_S,												DeviceKeys.S },
        { LedId.Keyboard_D,												DeviceKeys.D },
        { LedId.Keyboard_F,												DeviceKeys.F },
        { LedId.Keyboard_G,												DeviceKeys.G },
        { LedId.Keyboard_H,												DeviceKeys.H },
        { LedId.Keyboard_J,												DeviceKeys.J },
        { LedId.Keyboard_K,												DeviceKeys.K },
        { LedId.Keyboard_L,												DeviceKeys.L },
        { LedId.Keyboard_SemicolonAndColon,								DeviceKeys.SEMICOLON },
        { LedId.Keyboard_NonUsTilde,									DeviceKeys.HASHTAG },
        { LedId.Keyboard_ApostropheAndDoubleQuote,						DeviceKeys.APOSTROPHE },
        { LedId.Keyboard_LeftShift,										DeviceKeys.LEFT_SHIFT },
        { LedId.Keyboard_Z,												DeviceKeys.Z },
        { LedId.Keyboard_X,												DeviceKeys.X },
        { LedId.Keyboard_C,												DeviceKeys.C },
        { LedId.Keyboard_V,												DeviceKeys.V },
        { LedId.Keyboard_B,												DeviceKeys.B },
        { LedId.Keyboard_N,												DeviceKeys.N },
        { LedId.Keyboard_M,												DeviceKeys.M },
        { LedId.Keyboard_CommaAndLessThan,								DeviceKeys.COMMA },
        { LedId.Keyboard_PeriodAndBiggerThan,							DeviceKeys.PERIOD },
        { LedId.Keyboard_SlashAndQuestionMark,							DeviceKeys.FORWARD_SLASH },
        { LedId.Keyboard_NonUsBackslash,								DeviceKeys.BACKSLASH_UK },
        { LedId.Keyboard_LeftCtrl,										DeviceKeys.LEFT_CONTROL },
        { LedId.Keyboard_LeftGui,										DeviceKeys.LEFT_WINDOWS },
        { LedId.Keyboard_LeftAlt,										DeviceKeys.LEFT_ALT },
        { LedId.Keyboard_Space,											DeviceKeys.SPACE },
        { LedId.Keyboard_RightAlt,										DeviceKeys.RIGHT_ALT },
        { LedId.Keyboard_RightGui,										DeviceKeys.RIGHT_WINDOWS },
        { LedId.Keyboard_Function,										DeviceKeys.FN_Key },
        { LedId.Keyboard_Application,									DeviceKeys.APPLICATION_SELECT },
        { LedId.Keyboard_PrintScreen,									DeviceKeys.PRINT_SCREEN },
        { LedId.Keyboard_ScrollLock,									DeviceKeys.SCROLL_LOCK },
        { LedId.Keyboard_PauseBreak,									DeviceKeys.PAUSE_BREAK },
        { LedId.Keyboard_Insert,										DeviceKeys.INSERT },
        { LedId.Keyboard_Home,											DeviceKeys.HOME },
        { LedId.Keyboard_PageUp,										DeviceKeys.PAGE_UP },
        { LedId.Keyboard_Backslash,										DeviceKeys.BACKSLASH },
        { LedId.Keyboard_Enter,											DeviceKeys.ENTER },
        { LedId.Keyboard_EqualsAndPlus,									DeviceKeys.EQUALS },
        { LedId.Keyboard_Backspace,										DeviceKeys.BACKSPACE },
        { LedId.Keyboard_Delete,										DeviceKeys.DELETE },
        { LedId.Keyboard_End,											DeviceKeys.END },
        { LedId.Keyboard_PageDown,										DeviceKeys.PAGE_DOWN },
        { LedId.Keyboard_RightShift,									DeviceKeys.RIGHT_SHIFT },
        { LedId.Keyboard_RightCtrl,										DeviceKeys.RIGHT_CONTROL },
        { LedId.Keyboard_ArrowUp,										DeviceKeys.ARROW_UP },
        { LedId.Keyboard_ArrowLeft,										DeviceKeys.ARROW_LEFT },
        { LedId.Keyboard_ArrowDown,										DeviceKeys.ARROW_DOWN },
        { LedId.Keyboard_ArrowRight,									DeviceKeys.ARROW_RIGHT },
        { LedId.Keyboard_NumLock,										DeviceKeys.NUM_LOCK },
        { LedId.Keyboard_NumSlash,										DeviceKeys.NUM_SLASH },
        { LedId.Keyboard_NumAsterisk,									DeviceKeys.NUM_ASTERISK },
        { LedId.Keyboard_NumMinus,										DeviceKeys.NUM_MINUS },
        { LedId.Keyboard_NumPlus,										DeviceKeys.NUM_PLUS },
        { LedId.Keyboard_NumEnter,										DeviceKeys.NUM_ENTER },
        { LedId.Keyboard_Num7,											DeviceKeys.NUM_SEVEN },
        { LedId.Keyboard_Num8,											DeviceKeys.NUM_EIGHT },
        { LedId.Keyboard_Num9,											DeviceKeys.NUM_NINE },
        { LedId.Keyboard_Num4,											DeviceKeys.NUM_FOUR },
        { LedId.Keyboard_Num5,											DeviceKeys.NUM_FIVE },
        { LedId.Keyboard_Num6,											DeviceKeys.NUM_SIX },
        { LedId.Keyboard_Num1,											DeviceKeys.NUM_ONE },
        { LedId.Keyboard_Num2,											DeviceKeys.NUM_TWO },
        { LedId.Keyboard_Num3,											DeviceKeys.NUM_THREE },
        { LedId.Keyboard_Num0,											DeviceKeys.NUM_ZERO },
        { LedId.Keyboard_NumPeriodAndDelete,							DeviceKeys.NUM_PERIOD },
        { LedId.Keyboard_Programmable1,									DeviceKeys.G1 },
        { LedId.Keyboard_Programmable2,									DeviceKeys.G2 },
        { LedId.Keyboard_Programmable3,									DeviceKeys.G3 },
        { LedId.Keyboard_Programmable4,									DeviceKeys.G4 },
        { LedId.Keyboard_Programmable5,									DeviceKeys.G5 },
        { LedId.Keyboard_Programmable6,									DeviceKeys.G6 },
        { LedId.Keyboard_Programmable7,									DeviceKeys.G7 },
        { LedId.Keyboard_Programmable8,									DeviceKeys.G8 },
        { LedId.Keyboard_Programmable9,									DeviceKeys.G9 },
        { LedId.Keyboard_Programmable10,								DeviceKeys.G10 },
        { LedId.Keyboard_Programmable11,								DeviceKeys.G11 },
        { LedId.Keyboard_Programmable12,								DeviceKeys.G12 },
        { LedId.Keyboard_Programmable13,								DeviceKeys.G13 },
        { LedId.Keyboard_Programmable14,								DeviceKeys.G14 },
        { LedId.Keyboard_Programmable15,								DeviceKeys.G15 },
        { LedId.Keyboard_Programmable16,								DeviceKeys.G16 },
        { LedId.Keyboard_Programmable17,								DeviceKeys.G17 },
        { LedId.Keyboard_Programmable18,								DeviceKeys.G18 },
        { LedId.Keyboard_Programmable19,								DeviceKeys.G19 },
        { LedId.Logo,													DeviceKeys.LOGO },
        { LedId.Keyboard_MediaMute,										DeviceKeys.VOLUME_MUTE },
        { LedId.Keyboard_MediaPlay,										DeviceKeys.MEDIA_PLAY_PAUSE },
        { LedId.Keyboard_MediaNextTrack,								DeviceKeys.MEDIA_NEXT },
        { LedId.Keyboard_MediaPreviousTrack,							DeviceKeys.MEDIA_PREVIOUS },
        { LedId.Keyboard_MediaStop,										DeviceKeys.MEDIA_STOP },
        { LedId.Keyboard_Profile,										DeviceKeys.PROFILESWITCH },
        { LedId.Keyboard_Brightness,									DeviceKeys.BRIGHTNESS_SWITCH },
        { LedId.Keyboard_WinLock,										DeviceKeys.LOCK_SWITCH },
        { LedId.Keyboard_IndicatorNumLock,								DeviceKeys.NUM_LOCK_LED },
        { LedId.Keyboard_IndicatorCapsLock,								DeviceKeys.CAPS_LOCK_LED },
        { LedId.Keyboard_IndicatorScrollLock,							DeviceKeys.SCROLL_LOCK_LED },
        { LedId.Keyboard_IndicatorMacro,								DeviceKeys.MACRO_INDICATOR },
        { LedId.Keyboard_IndicatorWinLock,								DeviceKeys.WIN_LOCK_INDICATOR },

        { LedId.Custom1,												DeviceKeys.CL1 },
        { LedId.Custom2,												DeviceKeys.CL2 },
        { LedId.Custom3,												DeviceKeys.CL3 },
        { LedId.Custom4,												DeviceKeys.CL4 },
        { LedId.Custom5,												DeviceKeys.CL5 },
	
        { LedId.Mouse1,													DeviceKeys.PERIPHERAL_LIGHT1 },
        { LedId.Mouse2,													DeviceKeys.PERIPHERAL_LIGHT2 },
        { LedId.Mouse3,													DeviceKeys.PERIPHERAL_LIGHT3 },
        { LedId.Mouse4,													DeviceKeys.PERIPHERAL_LIGHT4 },
        { LedId.Mouse5,													DeviceKeys.PERIPHERAL_LIGHT5 },
        { LedId.Mouse6,													DeviceKeys.PERIPHERAL_LIGHT6 },
        { LedId.Mouse7,													DeviceKeys.PERIPHERAL_LIGHT7 },
        { LedId.Mouse8,													DeviceKeys.PERIPHERAL_LIGHT8 },
        { LedId.Mouse9,													DeviceKeys.PERIPHERAL_LIGHT9 },
        { LedId.Mouse10,												DeviceKeys.PERIPHERAL_LIGHT10 },
        { LedId.Mouse11,												DeviceKeys.PERIPHERAL_LIGHT11 },
        { LedId.Mouse12,												DeviceKeys.PERIPHERAL_LIGHT12 },
        { LedId.Mouse13,												DeviceKeys.PERIPHERAL_LIGHT13 },
        { LedId.Mouse14,												DeviceKeys.PERIPHERAL_LIGHT14 },
        { LedId.Mouse15,												DeviceKeys.PERIPHERAL_LIGHT15 },
        { LedId.Mouse16,												DeviceKeys.PERIPHERAL_LIGHT16 },
        { LedId.Mouse17,												DeviceKeys.PERIPHERAL_LIGHT17 },
        { LedId.Mouse18,												DeviceKeys.PERIPHERAL_LIGHT18 },
        { LedId.Mouse19,												DeviceKeys.PERIPHERAL_LIGHT19 },
        { LedId.Mouse20,												DeviceKeys.PERIPHERAL_LIGHT20 },

        { LedId.Mousepad1,												DeviceKeys.MOUSEPADLIGHT1 },
        { LedId.Mousepad2,												DeviceKeys.MOUSEPADLIGHT2 },
        { LedId.Mousepad3,												DeviceKeys.MOUSEPADLIGHT3 },
        { LedId.Mousepad4,												DeviceKeys.MOUSEPADLIGHT4 },
        { LedId.Mousepad5,												DeviceKeys.MOUSEPADLIGHT5 },
        { LedId.Mousepad6,												DeviceKeys.MOUSEPADLIGHT6 },
        { LedId.Mousepad7,												DeviceKeys.MOUSEPADLIGHT7 },
        { LedId.Mousepad8,												DeviceKeys.MOUSEPADLIGHT8 },
        { LedId.Mousepad9,												DeviceKeys.MOUSEPADLIGHT9 },
        { LedId.Mousepad10,												DeviceKeys.MOUSEPADLIGHT10 },
        { LedId.Mousepad11,												DeviceKeys.MOUSEPADLIGHT11 },
        { LedId.Mousepad12,												DeviceKeys.MOUSEPADLIGHT12 },
        { LedId.Mousepad13,												DeviceKeys.MOUSEPADLIGHT13 },
        { LedId.Mousepad14,												DeviceKeys.MOUSEPADLIGHT14 },
        { LedId.Mousepad15,												DeviceKeys.MOUSEPADLIGHT15 },
        { LedId.Mousepad16,												DeviceKeys.MOUSEPADLIGHT16 },
        { LedId.Mousepad17,												DeviceKeys.MOUSEPADLIGHT17 },
        { LedId.Mousepad18,												DeviceKeys.MOUSEPADLIGHT18 },
        { LedId.Mousepad19,												DeviceKeys.MOUSEPADLIGHT19 },
        { LedId.Mousepad20,												DeviceKeys.MOUSEPADLIGHT20 },
        { LedId.Mousepad21,												DeviceKeys.MOUSEPADLIGHT21 },

        { LedId.Headset1,												DeviceKeys.HEADSET1 },
        { LedId.Headset2,												DeviceKeys.HEADSET2 },
        { LedId.Headset3,												DeviceKeys.HEADSET3 },
        { LedId.Headset4,												DeviceKeys.HEADSET4 },
        { LedId.Headset5,												DeviceKeys.HEADSET5 },
    };

    public static readonly ReadOnlyDictionary<LedId, DeviceKeys> KeyNames = new(KeyNameMap);

    public static readonly ReadOnlyDictionary<DeviceKeys, LedId> AuroraToRgbNet = new(
        KeyNameMap.ToDictionary(kv => kv.Value, kv => kv.Key)
    );
}