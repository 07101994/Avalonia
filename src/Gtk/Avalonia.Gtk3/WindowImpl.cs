﻿using System;
using Avalonia.Controls;
using Avalonia.Gtk3.Interop;
using Avalonia.Platform;

namespace Avalonia.Gtk3
{
    class WindowImpl : TopLevelImpl, IWindowImpl
    {
        public WindowState WindowState { get; set; } //STUB
        public void SetTitle(string title)
        {
            using (var t = new Utf8Buffer(title))
                Native.GtkWindowSetTitle(GtkWidget, t);
        }

        public IDisposable ShowDialog()
        {
            return null;
            //STUB
        }

        public void SetSystemDecorations(bool enabled) => Native.GtkWindowSetDecorated(GtkWidget, enabled);

        public void SetIcon(IWindowIconImpl icon)
        {
            //STUB
        }

        public WindowImpl() : base(Native.GtkWindowNew(GtkWindowType.TopLevel))
        {
        }

        public override Size ClientSize
        {
            get
            {
                int w, h;
                Native.GtkWindowGetSize(GtkWidget, out w, out h);
                return new Size(w, h);
            }
            set { Native.GtkWindowResize(GtkWidget, (int) value.Width, (int) value.Height); }
        }

        public void SetCoverTaskbarWhenMaximized(bool enable)
        {
            //Why do we even have that?
        }
    }
}
