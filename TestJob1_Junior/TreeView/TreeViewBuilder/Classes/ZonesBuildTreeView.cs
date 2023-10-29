using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TestJob1_Junior.Serialize.Models;
using TestJob1_Junior.Serialize.Models.Output;
using TestJob1_Junior.Tools.TreeViewBuilder.Interfaces;
using TestJob1_Junior.TreeView.TreeViewModel;

namespace TestJob1_Junior.Tools.TreeViewBuilder.Classes
{
    public class ZonesBuildTreeView : IBuildTreeView<ZonesDTO>, IBuildNodeView<ZonesAndTerritoriesBoundariesTypeZonesAndTerritoriesRecord>
    {
        public void BuildNode(TreeItemViewModel parentNode, List<ZonesAndTerritoriesBoundariesTypeZonesAndTerritoriesRecord> entities)
        {
            foreach (var zone in entities)
            {
                TreeItemViewModel childTreeNode = new TreeItemViewModel (parentNode, false)
                {
                    DisplayName = zone.BObjectZonesAndTerritories.BObject.RegNumbBorder,
                    IsExpanded = false
                };
                parentNode.Children.Add(childTreeNode);
            }
        }

        public void BuildTree(MultiSelectTreeView treeView, ZonesDTO entitiesDTO)
        {
            TreeItemViewModel treeNode = new TreeItemViewModel (null, false)
            {
                DisplayName = "Zone",
                IsExpanded = true
            };
            treeView.Items.Add(treeNode);
            BuildNode(treeNode, entitiesDTO.zones);
        }

        public ZonesBuildTreeView(MultiSelectTreeView treeView, ZonesDTO entitiesDTO)
        {
            this.BuildTree(treeView, entitiesDTO);
        }
    }
}
