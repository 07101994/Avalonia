﻿using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform;
using Avalonia.Rendering;
using MonoMac.AppKit;

namespace Avalonia.MonoMac
{
    public class MonoMacPlatform : IWindowingPlatform, IPlatformSettings
    {
        internal static MonoMacPlatform Instance { get; private set; }
        readonly MouseDevice _mouseDevice = new MouseDevice();
        readonly KeyboardDevice _keyboardDevice = new KeyboardDevice();
        NSApplication _app;
        void DoInitialize()
        {
            AvaloniaLocator.CurrentMutable
                .Bind<IStandardCursorFactory>().ToTransient<CursorFactoryStub>()
                .Bind<IPlatformIconLoader>().ToSingleton<IconLoader>()
                .Bind<IKeyboardDevice>().ToConstant(_keyboardDevice)
                .Bind<IMouseDevice>().ToConstant(_mouseDevice)
                .Bind<IPlatformSettings>().ToConstant(this)
                .Bind<IRendererFactory>().ToConstant(ImmediateRenderer.Factory)
                .Bind<IWindowingPlatform>().ToConstant(this)
                .Bind<IPlatformThreadingInterface>().ToConstant(PlatformThreadingInterface.Instance);

            InitializeCocoaApp();
        }

        public static void Initialize()
        {
            Instance = new MonoMacPlatform();
            Instance.DoInitialize();

        }

        void InitializeCocoaApp()
        {
            NSApplication.Init();
            _app = NSApplication.SharedApplication;
            _app.ActivationPolicy = NSApplicationActivationPolicy.Regular;

        }


        public Size DoubleClickSize => new Size(4, 4);
        public TimeSpan DoubleClickTime => TimeSpan.FromSeconds(NSEvent.DoubleClickInterval);

        public IWindowImpl CreateWindow() => new WindowImpl();

        public IEmbeddableWindowImpl CreateEmbeddableWindow()
        {
            throw new PlatformNotSupportedException();
        }

        public IPopupImpl CreatePopup()
        {
            return new PopupImpl();
        }
    }
}


namespace Avalonia
{
    public static class MonoMacPlatformExtensions
    {
        public static AppBuilderBase<T> UseMonoMac<T>(this AppBuilderBase<T> builder) where T : AppBuilderBase<T>, new()
        {
            return builder.UseWindowingSubsystem(MonoMac.MonoMacPlatform.Initialize);
        }
    }
}