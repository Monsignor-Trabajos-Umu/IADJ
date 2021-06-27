using UnityEngine;

namespace Assets.Scrips.Steering.Pathfinding.A
{
    // Es igual que un nodo pero con heap
    public class NodeHeaped : Node, IHeapItem<NodeHeaped>
    {

        public int HeapIndex { get; set; }

        public NodeHeaped(bool pared, Vector3 worldPosition, int gridX, int gridZ) : base(
            pared, worldPosition, gridX, gridZ)
        {
        }



        //Heap extras
        public int CompareTo(NodeHeaped nodeToCompare)
        {
            var compare = fCost.CompareTo(nodeToCompare.fCost);
            if (compare == 0) compare = hCost.CompareTo(nodeToCompare.hCost);
            return -compare;
        }
    }
}