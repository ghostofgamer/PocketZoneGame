namespace SaveDataContent
{
    [System.Serializable]
    public class ItemData 
    {
        public int id;
        public int count;
        public int idPosition;
        
        public ItemData(int id, int count, int idPosition)
        {
            this.id = id;
            this.count = count;
            this.idPosition = idPosition;
        }
    }
}