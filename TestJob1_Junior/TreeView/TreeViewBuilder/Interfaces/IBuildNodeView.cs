using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TestJob1_Junior.TreeView.TreeViewModel;

namespace TestJob1_Junior.Tools.TreeViewBuilder.Interfaces
{
    public interface IBuildNodeView<TEntity> where TEntity : class
    {
        //void BuildNode(MultiSelectTreeViewItem item, List<TEntity> entities);
        void BuildNode(TreeItemViewModel item, List<TEntity> entities);
    }
}
