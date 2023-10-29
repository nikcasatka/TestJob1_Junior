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
    public class SpatialDataBuildTreeView : IBuildTreeView<SpatialDataDTO>
    {
        public void BuildNode(TreeItemViewModel parentNode, EntitySpatialBound entity)
        {
            TreeItemViewModel childTreeNode = new TreeItemViewModel (parentNode, false)
            {
                DisplayName = entity.SkId,
                IsExpanded = false
            };
            parentNode.Children.Add(childTreeNode);
        }

        public void BuildTree(MultiSelectTreeView treeView, SpatialDataDTO entitiesDTO)
        {
            TreeItemViewModel treeNode = new TreeItemViewModel (null, false)
            {
                DisplayName = "SpatialData",
                IsExpanded = true
            };
            treeView.Items.Add(treeNode);
            BuildNode(treeNode, entitiesDTO.spatialData);
        }

        public SpatialDataBuildTreeView(MultiSelectTreeView treeView, SpatialDataDTO entityDTO)
        {
            this.BuildTree(treeView, entityDTO);
        }
    }
}
