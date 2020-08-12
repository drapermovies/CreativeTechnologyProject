namespace TrafficSimulation
{
    public struct JunctionHeuristicNode
    {
        public int index;

        public int gCost; //Heuristic cost from current location
        public int hCost; //Heuristic cost to end node
        public int fCost; //G & H combined values

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }
    }

    public struct RoadHeuristicNode
    {
        public int index;

        public int gCost; //Heuristic cost from start node
        public int hCost; //Heuristic cost to end node
        public int fCost; //G & H combined values

        public bool canTravelAlong;

        public int previousRoadIndex;

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }
    }
}