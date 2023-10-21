using QuadTree.SpatialItems;

namespace QuadTree;

public class QuadTree<K,V> where K : IComparable<K>
{
    public QuadTreeNode<K,V> Root { get; private set; }

    public QuadTree(Rectangle boundary)
    {
        Root = new QuadTreeNode<K, V>(boundary,null);
        //Root.Subdivide();
    }

    public void Insert(QuadTreeObject<K, V> quadTreeObject)
    {
        if(quadTreeObject.Item is Point || quadTreeObject.Item is Rectangle)
        {
            Root.Insert(quadTreeObject);
        } 
        else
        {
            throw new ArgumentException("Item in object must be either a Point or a Rectangle");
        }
    }

    public List<QuadTreeObject<K,V>>? Find(QuadTreeObject<K,V> quadTreeObject)
    {
        if (quadTreeObject.Item is Point || quadTreeObject.Item is Rectangle)
        {
            return Root.Find(quadTreeObject);
        }
        else
        {
            throw new ArgumentException("Item in object must be either a Point or a Rectangle");
        }
    }

    //delete point
    public bool Delete(QuadTreeObject<K, V> quadTreeObject)
    {
        if (quadTreeObject.Item is Point)
        {
            DeletePoint(quadTreeObject);
            return true;
        }
        else if (quadTreeObject.Item is Rectangle)
        {
            //return DeleteRectangle((Rectangle)quadTreeObject.Item);
            return false;
        }
        else
        {
            throw new ArgumentException("Item in object must be either a Point or a Rectangle");
        }
    }

    public SpatialItem DeletePoint(QuadTreeObject<K, V> quadTreeObject)
    {
        QuadTreeNode<K, V>? foundNode = null;
        QuadTreeNode<K, V> currentNode = Root;

        while (true)
        {

            if (currentNode.Data.Count != 0)
            {

                foreach (var kvp in currentNode.Data)
                {
                    if (kvp.Key.Equals(quadTreeObject.Key))
                    {
                        foundNode = currentNode;
                        currentNode.Data.Remove(kvp); //Delete the point
                        break;

                    }
                }

                if (foundNode is not null)
                    break;

            }

            if (currentNode.Children == null)
                break;

            Quadrant? choosenQuadrant = null;

            //FIXME: What if it's rectangle?
            if (quadTreeObject.Item is Rectangle)
            {

                foreach (Quadrant quadrant in Enum.GetValues(typeof(Quadrant)))
                {
                    if (currentNode.Children[(int)quadrant].Boundary.Contains((Rectangle)quadTreeObject.Item))
                    {
                        choosenQuadrant = quadrant;
                        break;
                    }
                }
            }
            
            if(quadTreeObject.Item is Point)
                choosenQuadrant = currentNode.DetermineQuadrant((Point)quadTreeObject.Item);

            //If the point is not in any quadrant, return null
            if (choosenQuadrant == null)
            {
                break;
            }

            currentNode = currentNode.Children[(int)choosenQuadrant];
        }

        if (foundNode is null)
            return null;

        //If the node is empty, delete it
        //if (foundNode.isLeaf() && foundNode.Data.Count == 0)
        //{
            currentNode = foundNode.Parent;
        //}

        while (currentNode is not null) //Neviem ci dobra podmienka
        {

            int sumEmptyQuadrants = 0;
            foreach (var child in currentNode.Children)
            {
                if (child.Data.Count == 0)
                    sumEmptyQuadrants++;
            }

            //Ak som prazdny a list
            if (sumEmptyQuadrants == 4)
            {
                currentNode.MakeLeaf();
                currentNode = currentNode.Parent;
            } else
            {
                break;
            }

            
        }
        return (SpatialItem)quadTreeObject.Item;
    
    }
}