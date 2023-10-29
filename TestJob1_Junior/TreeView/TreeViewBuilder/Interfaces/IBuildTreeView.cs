using System.Collections.Generic;
using System.Windows.Controls;
using TestJob1_Junior.TreeView.TreeViewModel;

namespace TestJob1_Junior.Tools.TreeViewBuilder.Interfaces
{
    public interface IBuildTreeView<TEntityDTO> where TEntityDTO : class 
    {
        //void BuildTree(TreeView treeView, TEntityDTO entitiesDTO);

        void BuildTree(MultiSelectTreeView treeView, TEntityDTO entitiesDTO);
    }
}
