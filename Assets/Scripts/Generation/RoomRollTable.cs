namespace DLS.LD39.Generation
{
    using System.Collections.Generic;
    using Data;

    public class RoomRollTable
    {
        public List<float> _chances = new List<float>();
        public List<RoomType> _typeTable = new List<RoomType>();

        public RoomRollTable(IEnumerable<RoomProbability> probability)
        {
            
        }
    }
}
