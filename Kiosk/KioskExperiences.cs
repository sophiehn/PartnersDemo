﻿// 
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
// 
// Microsoft Cognitive Services: http://www.microsoft.com/cognitive
// 
// Microsoft Cognitive Services Github:
// https://github.com/Microsoft/Cognitive
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace IntelligentKioskSample
{
    public static class KioskExperiences
    {
        private static readonly Type[] expTypes;

        static KioskExperiences()
        {
            expTypes = typeof(KioskExperiences).GetTypeInfo().Assembly.GetTypes().Where(t =>
                t.Namespace == "IntelligentKioskSample.Views"
                && t.GetTypeInfo().GetCustomAttribute<KioskExperienceAttribute>() != null)
                .ToArray();
        }

        public static IEnumerable<KioskExperience> Experiences
        {
            get
            {
                return expTypes.Select(t => new KioskExperience()
                {
                    PageType = t,
                    Attributes = t.GetTypeInfo().GetCustomAttribute<KioskExperienceAttribute>()
                }).OrderBy(e => e.Attributes.Title);
            }
        }
    }

    public class KioskExperience
    {
        public Type PageType { get; set; }
        public KioskExperienceAttribute Attributes { get; set; }
    }

    public enum ExperienceType
    {
        Kiosk,
        Other
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class KioskExperienceAttribute : Attribute
    {
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public ExperienceType ExperienceType { get; set; }
    }
}
