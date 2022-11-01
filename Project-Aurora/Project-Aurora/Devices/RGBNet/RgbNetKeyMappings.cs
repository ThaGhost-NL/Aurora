﻿using System.Collections.Generic;
using RGB.NET.Core;

namespace Aurora.Devices.RGBNet;

public static class RgbNetKeyMappings
{
    public static readonly Dictionary<DeviceKeys, LedId> KeyNames = new()
    {
        {DeviceKeys.ESC, LedId.Keyboard_Escape},
        {DeviceKeys.F1 , LedId.Keyboard_F1},
        {DeviceKeys.F2 , LedId.Keyboard_F2},
        {DeviceKeys.F3 , LedId.Keyboard_F3},
        {DeviceKeys.F4 , LedId.Keyboard_F4},
        {DeviceKeys.F5 , LedId.Keyboard_F5},
        {DeviceKeys.F6 , LedId.Keyboard_F6},
        {DeviceKeys.F7 , LedId.Keyboard_F7},
        {DeviceKeys.F8 , LedId.Keyboard_F8},
        {DeviceKeys.F9 , LedId.Keyboard_F9},
        {DeviceKeys.F10 , LedId.Keyboard_F10},
        {DeviceKeys.F11 , LedId.Keyboard_F11},
        {DeviceKeys.F12 , LedId.Keyboard_F12},
        {DeviceKeys.TILDE , LedId.Keyboard_GraveAccentAndTilde},
        {DeviceKeys.ONE , LedId.Keyboard_1},
        {DeviceKeys.TWO , LedId.Keyboard_2},
        {DeviceKeys.THREE , LedId.Keyboard_3},
        {DeviceKeys.FOUR , LedId.Keyboard_4},
        {DeviceKeys.FIVE , LedId.Keyboard_5},
        {DeviceKeys.SIX , LedId.Keyboard_6},
        {DeviceKeys.SEVEN , LedId.Keyboard_7},
        {DeviceKeys.EIGHT , LedId.Keyboard_8},
        {DeviceKeys.NINE , LedId.Keyboard_9},
        {DeviceKeys.ZERO , LedId.Keyboard_0},
        {DeviceKeys.MINUS , LedId.Keyboard_MinusAndUnderscore},
        {DeviceKeys.TAB , LedId.Keyboard_Tab},
        {DeviceKeys.Q , LedId.Keyboard_Q},
        {DeviceKeys.W , LedId.Keyboard_W},
        {DeviceKeys.E , LedId.Keyboard_E},
        {DeviceKeys.R , LedId.Keyboard_R},
        {DeviceKeys.T , LedId.Keyboard_T},
        {DeviceKeys.Y , LedId.Keyboard_Y},
        {DeviceKeys.U , LedId.Keyboard_U},
        {DeviceKeys.I , LedId.Keyboard_I},
        {DeviceKeys.O , LedId.Keyboard_O},
        {DeviceKeys.P , LedId.Keyboard_P},
        {DeviceKeys.OPEN_BRACKET , LedId.Keyboard_BracketLeft},
        {DeviceKeys.CLOSE_BRACKET , LedId.Keyboard_BracketRight},
        {DeviceKeys.CAPS_LOCK , LedId.Keyboard_CapsLock},
        {DeviceKeys.A , LedId.Keyboard_A},
        {DeviceKeys.S , LedId.Keyboard_S},
        {DeviceKeys.D , LedId.Keyboard_D},
        {DeviceKeys.F , LedId.Keyboard_F},
        {DeviceKeys.G , LedId.Keyboard_G},
        {DeviceKeys.H , LedId.Keyboard_H},
        {DeviceKeys.J , LedId.Keyboard_J},
        {DeviceKeys.K , LedId.Keyboard_K},
        {DeviceKeys.L , LedId.Keyboard_L},
        {DeviceKeys.SEMICOLON , LedId.Keyboard_SemicolonAndColon},
        {DeviceKeys.APOSTROPHE , LedId.Keyboard_ApostropheAndDoubleQuote},
        {DeviceKeys.LEFT_SHIFT , LedId.Keyboard_LeftShift},
        {DeviceKeys.Z , LedId.Keyboard_Z},
        {DeviceKeys.X , LedId.Keyboard_X},
        {DeviceKeys.C , LedId.Keyboard_C},
        {DeviceKeys.V , LedId.Keyboard_V},
        {DeviceKeys.B , LedId.Keyboard_B},
        {DeviceKeys.N , LedId.Keyboard_N},
        {DeviceKeys.M , LedId.Keyboard_M},
        {DeviceKeys.COMMA , LedId.Keyboard_CommaAndLessThan},
        {DeviceKeys.PERIOD , LedId.Keyboard_PeriodAndBiggerThan},
        {DeviceKeys.FORWARD_SLASH , LedId.Keyboard_SlashAndQuestionMark},
        {DeviceKeys.LEFT_CONTROL , LedId.Keyboard_LeftCtrl},
        {DeviceKeys.LEFT_WINDOWS , LedId.Keyboard_LeftGui},
        {DeviceKeys.LEFT_ALT , LedId.Keyboard_LeftAlt},
        {DeviceKeys.SPACE , LedId.Keyboard_Space},
        {DeviceKeys.RIGHT_ALT , LedId.Keyboard_RightAlt},
        {DeviceKeys.RIGHT_WINDOWS , LedId.Keyboard_RightGui},
        {DeviceKeys.APPLICATION_SELECT , LedId.Keyboard_Application},
        {DeviceKeys.PRINT_SCREEN , LedId.Keyboard_PrintScreen},
        {DeviceKeys.SCROLL_LOCK , LedId.Keyboard_ScrollLock},
        {DeviceKeys.PAUSE_BREAK , LedId.Keyboard_PauseBreak},
        {DeviceKeys.INSERT , LedId.Keyboard_Insert},
        {DeviceKeys.HOME , LedId.Keyboard_Home},
        {DeviceKeys.PAGE_UP , LedId.Keyboard_PageUp},
        {DeviceKeys.BACKSLASH , LedId.Keyboard_Backslash},
        {DeviceKeys.OEMTilde , LedId.Keyboard_NonUsTilde},
        {DeviceKeys.ENTER , LedId.Keyboard_Enter},
        {DeviceKeys.EQUALS , LedId.Keyboard_EqualsAndPlus},
        {DeviceKeys.BACKSPACE , LedId.Keyboard_Backspace},
        {DeviceKeys.DELETE , LedId.Keyboard_Delete},
        {DeviceKeys.END , LedId.Keyboard_End},
        {DeviceKeys.PAGE_DOWN , LedId.Keyboard_PageDown},
        {DeviceKeys.RIGHT_SHIFT , LedId.Keyboard_RightShift},
        {DeviceKeys.RIGHT_CONTROL , LedId.Keyboard_RightCtrl},
        {DeviceKeys.ARROW_UP , LedId.Keyboard_ArrowUp},
        {DeviceKeys.ARROW_LEFT , LedId.Keyboard_ArrowLeft},
        {DeviceKeys.ARROW_DOWN , LedId.Keyboard_ArrowDown},
        {DeviceKeys.ARROW_RIGHT , LedId.Keyboard_ArrowRight},
        {DeviceKeys.NUM_LOCK , LedId.Keyboard_NumLock},
        {DeviceKeys.NUM_SLASH , LedId.Keyboard_NumSlash},
        {DeviceKeys.NUM_ASTERISK , LedId.Keyboard_NumAsterisk},
        {DeviceKeys.NUM_MINUS , LedId.Keyboard_NumMinus},
        {DeviceKeys.NUM_PLUS , LedId.Keyboard_NumPlus},
        {DeviceKeys.NUM_ENTER , LedId.Keyboard_NumEnter},
        {DeviceKeys.NUM_SEVEN , LedId.Keyboard_Num7},
        {DeviceKeys.NUM_EIGHT , LedId.Keyboard_Num8},
        {DeviceKeys.NUM_NINE , LedId.Keyboard_Num9},
        {DeviceKeys.NUM_FOUR , LedId.Keyboard_Num4},
        {DeviceKeys.NUM_FIVE , LedId.Keyboard_Num5},
        {DeviceKeys.NUM_SIX , LedId.Keyboard_Num6},
        {DeviceKeys.NUM_ONE , LedId.Keyboard_Num1},
        {DeviceKeys.NUM_TWO , LedId.Keyboard_Num2},
        {DeviceKeys.NUM_THREE , LedId.Keyboard_Num3},
        {DeviceKeys.NUM_ZERO , LedId.Keyboard_Num0},
        {DeviceKeys.NUM_PERIOD , LedId.Keyboard_NumPeriodAndDelete},
        {DeviceKeys.G1 , LedId.Keyboard_Programmable1},
        {DeviceKeys.G2 , LedId.Keyboard_Programmable2},
        {DeviceKeys.G3 , LedId.Keyboard_Programmable3},
        {DeviceKeys.G4 , LedId.Keyboard_Programmable4},
        {DeviceKeys.G5 , LedId.Keyboard_Programmable5},
        {DeviceKeys.G6 , LedId.Keyboard_Programmable6},
        {DeviceKeys.G7 , LedId.Keyboard_Programmable7},
        {DeviceKeys.G8 , LedId.Keyboard_Programmable8},
        {DeviceKeys.G9 , LedId.Keyboard_Programmable9},
        {DeviceKeys.LOGO , LedId.Logo},
        {DeviceKeys.Peripheral_Logo , LedId.Custom1},
    };
}