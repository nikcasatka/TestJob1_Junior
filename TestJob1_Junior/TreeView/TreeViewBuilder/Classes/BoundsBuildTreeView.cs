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
    public class BoundsBuildTreeView : IBuildTreeView<BoundsDTO>, IBuildNodeView<MunicipalBoundariesTypeMunicipalBoundaryRecord>
    {
        public void BuildNode(TreeItemViewModel parentNode, List<MunicipalBoundariesTypeMunicipalBoundaryRecord> entities)
        {
            foreach (var bound in entities)
            {
                TreeItemViewModel childTreeNode = new TreeItemViewModel (parentNode, false)
                {
                    DisplayName = bound.BObjectMunicipalBoundary.BObject.RegNumbBorder,
                    IsExpanded = false
                };
                parentNode.Children.Add(childTreeNode);
            }
        }

        public void BuildTree(MultiSelectTreeView treeView, BoundsDTO entitiesDTO)
        {
            TreeItemViewModel treeNode = new TreeItemViewModel (null, false)
            {
                DisplayName = "Bound",
                IsExpanded = true
            };
            treeView.Items.Add(treeNode);
            BuildNode(treeNode, entitiesDTO.bounds);
        }

        public BoundsBuildTreeView(MultiSelectTreeView treeView, BoundsDTO entitiesDTO)
        {
            this.BuildTree(treeView, entitiesDTO);
        }
    }
}
