namespace QuadTreeDS.BinarySearchTree;

public class BinarySearchTree<V> where V : IComparable<V>
{
    public class TreeNode
    {
        public V Value;
        public TreeNode Left;
        public TreeNode Right;

        public TreeNode(V value)
        {
            Value = value;
            Left = null;
            Right = null;
        }
    }

    public TreeNode Root { get; private set; }

    public void Insert(V value)
    {
        TreeNode newNode = new TreeNode(value);

        if (Root is null)
        {
            Root = newNode;
            return;
        }

        TreeNode current = Root;
        TreeNode parent = null;

        while (current is not null)
        {
            parent = current;
            if (value.CompareTo(current.Value) < 0)
            {
                current = current.Left;
                if (current is null)
                {
                    parent.Left = newNode;
                    return;
                }
            }
            else
            {
                current = current.Right;
                if (current is null)
                {
                    parent.Right = newNode;
                    return;
                }
            }
        }
    }

    public bool TryInsert(V value)
    {
        if (Root == null)
        {
            Root = new TreeNode(value);
            return true;
        }

        TreeNode current = Root;
        TreeNode parent = null;

        while (current is not null)
        {
            int comparison = value.CompareTo(current.Value);
            if (comparison == 0)
            {
                return false; //duplication
            }

            parent = current;
            if (comparison < 0)
            {
                current = current.Left;
                if (current is null)
                {
                    parent.Left = new TreeNode(value);
                    return true;
                }
            }
            else
            {
                current = current.Right;
                if (current is null)
                {
                    parent.Right = new TreeNode(value);
                    return true;
                }
            }
        }

        return false; 
    }

    public bool Search(V value)
    {
        TreeNode current = Root;

        while (current is not null)
        {
            int result = value.CompareTo(current.Value);

            if (result == 0)
                return true;

            if (result < 0)
                current = current.Left;
            else
                current = current.Right;
        }

        return false;
    }


    public bool Delete(V value)
    {
        bool isDeleted = false;
        Root = Delete(Root, value, ref isDeleted);
        return isDeleted;
    }

    private TreeNode Delete(TreeNode root, V value, ref bool isDeleted)
    {
        isDeleted = false;
        TreeNode current = root;
        TreeNode parent = null;

        while (current is not null && !current.Value.Equals(value))
        {
            parent = current;
            current = value.CompareTo(current.Value) < 0 ? current.Left : current.Right;
        }

        if (current is null)
            return root; // not found

        isDeleted = true;

        // one or no child
        if (current.Left is null || current.Right is null)
        {
            TreeNode newChild = current.Left ?? current.Right;

            if (parent is null)
            {
                return newChild; //delete root
            }

            if (current == parent.Left)
                parent.Left = newChild;
            else
                parent.Right = newChild;
        }
        else // two children
        {
            TreeNode successorParent = current;
            TreeNode successor = current.Right;

            // find leftmost node in right subtree (in-order successor)
            while (successor.Left != null)
            {
                successorParent = successor;
                successor = successor.Left;
            }

            // Right rotation
            if (successorParent != current)
            {
                successorParent.Left = successor.Right;
            }
            else //left rotation
            {
                successorParent.Right = successor.Right;
            }

            // Replace current's value with successor's value
            current.Value = successor.Value;
        }

        return root;
    }


}
