using System.Collections.Generic;
using System.Windows.Controls;
using TestJob1_Junior.Serialize.Models;
using TestJob1_Junior.Serialize.Models.Output;
using TestJob1_Junior.Tools.TreeViewBuilder.Interfaces;
using TestJob1_Junior.TreeView.TreeViewModel;

namespace TestJob1_Junior.Tools.TreeViewBuilder.Classes
{
    public class ParcelBuildTreeView : IBuildTreeView<ParcelsDTO>, IBuildNodeView<BaseDataTypeLandRecordsLandRecord>
    {

        public void BuildNode(TreeItemViewModel parentNode, List<BaseDataTypeLandRecordsLandRecord> entities)
        {
            foreach (var parcel in entities)
            {
                TreeItemViewModel childTreeNode = new TreeItemViewModel (parentNode, false)
                {
                    DisplayName = parcel.Object.CommonData.CadNumber,
                    IsExpanded = false
                };
                parentNode.Children.Add(childTreeNode);
            }
        }

        public void BuildTree(MultiSelectTreeView treeView, ParcelsDTO entitiesDTO)
        {
            TreeItemViewModel treeNode = new TreeItemViewModel (null, false)
            {
                DisplayName = "Parcel",
                IsExpanded = true
            };
            treeView.Items.Add(treeNode);
            BuildNode(treeNode, entitiesDTO.parcels);
        }

        public ParcelBuildTreeView(MultiSelectTreeView treeView, ParcelsDTO entitiesDTO)
        {
            this.BuildTree(treeView, entitiesDTO);
        }
    }
}
