using Sitecore.VisualStudio.Sites;

namespace Sitecore.Strategy.VisualStudio.Wizards
{
    public class MySiteContext : ISiteContext
    {
        public void SetSite(Site site)
        {
            this.Site = site;
        }

        public Site Site { get; private set; }
    }
}