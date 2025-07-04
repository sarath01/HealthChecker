// Copyright (c) 2025 Sarath Reddy Konda
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using UIKit;
using CoreGraphics;

namespace Animatix.iOS
{
    public static class AnimationHelper
    {
        public static void FadeIn(this UIView view, double duration = 0.5, double delay = 0.0, Action? onComplete = null)
        {
            view.Alpha = 0;
            UIView.Animate(duration, delay, UIViewAnimationOptions.CurveEaseInOut, () =>
            {
                view.Alpha = 1;
            }, onComplete);
        }

        public static void ScaleIn(this UIView view, double duration = 0.5, double delay = 0.0, float fromScale = 0.5f, Action? onComplete = null)
        {
            view.Transform = CGAffineTransform.MakeScale(fromScale, fromScale);
            UIView.Animate(duration, delay, UIViewAnimationOptions.CurveEaseInOut, () =>
            {
                view.Transform = CGAffineTransform.MakeIdentity();
            }, onComplete);
        }

        public static void SlideInFromLeft(this UIView view, CGRect finalFrame, double duration = 0.5, double delay = 0.0, Action? onComplete = null)
        {
            var startFrame = finalFrame;
            startFrame.X = -finalFrame.Width;
            view.Frame = startFrame;

            UIView.Animate(duration, delay, UIViewAnimationOptions.CurveEaseInOut, () =>
            {
                view.Frame = finalFrame;
            }, onComplete);
        }

        public static void Rotate360(this UIView view, double duration = 1.0)
        {
            UIView.Animate(duration, () =>
            {
                view.Transform = CGAffineTransform.MakeRotation((float)Math.PI);
            }, () =>
            {
                UIView.Animate(duration, () =>
                {
                    view.Transform = CGAffineTransform.MakeRotation((float)(Math.PI * 2));
                }, null);
            });
        }
    }
}