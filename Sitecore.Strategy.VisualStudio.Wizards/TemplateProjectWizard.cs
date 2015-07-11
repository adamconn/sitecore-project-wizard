using System;
using System.IO;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.VisualStudio.Commands.Commands;
using Sitecore.VisualStudio.Sites;
using VSLangProj;

namespace Sitecore.Strategy.VisualStudio.Wizards
{
    public class TemplateProjectWizard : IWizard
    {
        private Site Site { get; set; }
        protected virtual Site PromptForSite()
        {
            var command = new SwitchSite();
            var context = new MySiteContext();
            command.Execute(context);
            var selectedSite = context.Site;
            if (selectedSite == null || selectedSite.DataService == null || !selectedSite.DataService.CheckDataService())
            {
                return null;
            }
            return selectedSite;
        }
        protected virtual void AddReplacementsForSite(Site site, Dictionary<string, string> replacementsDictionary)
        {
            replacementsDictionary.Add("$rocks_Site_DataServiceName$", site.DataServiceName);
            replacementsDictionary.Add("$rocks_Site_HostName$", site.HostName);
            replacementsDictionary.Add("$rocks_Site_Name$", site.Name);
            var safeSiteName = string.Join("_", site.Name.Split(System.IO.Path.GetInvalidFileNameChars()));
            replacementsDictionary.Add("$rocks_Site_Name_safe$", safeSiteName);
            replacementsDictionary.Add("$rocks_Site_UserName$", site.UserName);
            replacementsDictionary.Add("$rocks_Site_WebRootPath$", site.WebRootPath);
            //TODO: set values using version...
        }

        protected string SafeProjectName { get; set; }
        public virtual void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            this.SafeProjectName = replacementsDictionary["$safeprojectname$"];
            var site = PromptForSite();
            if (site == null)
            {
                DeleteSolution(replacementsDictionary);
                throw new WizardBackoutException("No site selected or error. Cancel project creation.");
            }
            this.Site = site;
            AddReplacementsForSite(site, replacementsDictionary);
        }

        protected virtual void DeleteSolution(Dictionary<string, string> replacementsDictionary)
        {
            var directory = replacementsDictionary["$solutiondirectory$"];
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
        }

        public virtual void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            return;
        }

        public virtual bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        public virtual void RunFinished()
        {
            return;
        }

        public virtual void BeforeOpeningFile(ProjectItem projectItem)
        {
            return;
        }

        protected virtual IEnumerable<string> GetSelectedAssemblies(Project project)
        {
            var properties = project.DTE.Properties["Sitecore Projects", "References"];
            if (properties == null)
            {
                return null;
            }
            var s = (string)properties.Item("ReferencedAssemblies").Value;
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }
            var names = s.Split(new char[] { ',' });
            var prj = (VSProject)project.Object;
            var path = Path.Combine(this.Site.WebRootPath, "bin");
            var assemblies = new List<string>();
            foreach (var name in names)
            {
                assemblies.Add(Path.Combine(path, name));
            }
            return assemblies;
        }

        protected virtual void AddReferences(Project project)
        {
            var assemblies = GetSelectedAssemblies(project);
            if (assemblies == null)
            {
                return;
            }
            var prj = (VSProject)project.Object;
            foreach (var assembly in assemblies)
            {
                var reference = prj.References.Add(assembly);
                reference.CopyLocal = false;
            }
        }

        protected virtual void RemoveRocksConfigFileFromProject(Project project)
        {
            var rocksConfigFileName = string.Format("{0}.csproj.sitecore", project.Name);
            foreach (ProjectItem item in project.ProjectItems)
            {
                if (item.Name == rocksConfigFileName)
                {
                    item.Remove();
                }
            }
        }
        public virtual void ProjectFinishedGenerating(Project project)
        {
            AddReferences(project);
            RemoveRocksConfigFileFromProject(project);
        }
    }
}
