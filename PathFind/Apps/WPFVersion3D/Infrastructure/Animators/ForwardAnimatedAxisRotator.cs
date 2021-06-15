﻿using Common.Extensions;
using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using WPFVersion3D.Infrastructure.Animators.Interface;

using static WPFVersion3D.Constants;

namespace WPFVersion3D.Infrastructure.Animators
{
    internal sealed class ForwardAnimatedAxisRotator : BaseAnimatedAxisRotator, IAnimator
    {
        public ForwardAnimatedAxisRotator(AxisAngleRotation3D axis) : base(axis)
        {

        }

        protected override AnimationTimeline CreateAnimation()
        {
            var duration = CalculateAnimationDuration();
            return new DoubleAnimation(axis.Angle, AngleValueRange.UpperValueOfRange,
                duration, FillBehavior.HoldEnd);
        }

        protected override Duration CalculateAnimationDuration()
        {
            var angleAmplitude = AngleValueRange.Amplitude();
            var duration = InitialRotationAnimationDuration * (angleAmplitude - axis.Angle) / angleAmplitude;
            return new Duration(TimeSpan.FromMilliseconds(duration));
        }
    }
}
