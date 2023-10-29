using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TestJob1_Junior.Serialize.Models.Output;
using TestJob1_Junior.Serialize.Models;
using TestJob1_Junior.Tools.TreeViewBuilder.Interfaces;
using TestJob1_Junior.TreeView.TreeViewModel;

namespace TestJob1_Junior.Tools.TreeViewBuilder.Classes
{
    public class ObjectRealtyBuildTreeView: IBuildNodeView<BaseDataTypeBuildRecordsBuildRecord>, IBuildNodeView<BaseDataTypeConstructionRecordsConstructionRecord> 
    {
        public void BuildTree(MultiSelectTreeView treeView, BuildingsDTO buildingDTO, ConstructionsDTO constructionDTO)
        {
            TreeItemViewModel treeNode = new TreeItemViewModel (null, false)
            {
                DisplayName = "ObjectRealty",
                IsExpanded = true
            };
            treeView.Items.Add(treeNode);
            BuildNode(treeNode, buildingDTO.buildings);
            BuildNode(treeNode, constructionDTO.constructions);
        }

        public void BuildNode(TreeItemViewModel parentNode, List<BaseDataTypeConstructionRecordsConstructionRecord> entities)
        {
            foreach (var building in entities)
            {
                TreeItemViewModel childTreeNode = new TreeItemViewModel (parentNode, false)
                {
                    DisplayName = building.Object.CommonData.CadNumber,
                    IsExpanded = false
                };
                parentNode.Children.Add(childTreeNode);
            }
        }

        public void BuildNode(TreeItemViewModel parentNode, List<BaseDataTypeBuildRecordsBuildRecord> entities)
        {
            foreach (var construction in entities)
            {
                TreeItemViewModel childTreeNode = new TreeItemViewModel(parentNode, false)
                {
                    DisplayName = construction.Object.CommonData.CadNumber,
                    IsExpanded = false
                };
                parentNode.Children.Add(childTreeNode);
            }
        }

        public ObjectRealtyBuildTreeView(MultiSelectTreeView treeView, BuildingsDTO buildingsDTO, ConstructionsDTO constructionsDTO)
        {
            this.BuildTree(treeView, buildingsDTO, constructionsDTO);
        }
    }
}
