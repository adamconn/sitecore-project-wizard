// Guids.cs
// MUST match guids.h
using System;

namespace Sitecore.Strategy.VisualStudio
{
    static class GuidList
    {
        public const string guidSitecore_Strategy_VisualStudioPkgString = "730daa56-d622-4a3e-96d5-fbc1152b7955";
        public const string guidSitecore_Strategy_VisualStudioCmdSetString = "10ee27ff-4951-46f3-a6d1-bea3d03e35d0";

        public static readonly Guid guidSitecore_Strategy_VisualStudioCmdSet = new Guid(guidSitecore_Strategy_VisualStudioCmdSetString);
    };
}