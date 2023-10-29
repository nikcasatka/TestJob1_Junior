namespace TestJob1_Junior.Serialize.Models.Output
{
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute]
    public class SpatialDataDTO
    {
        [System.Xml.Serialization.XmlElementAttribute("entity_spatial")]
        public EntitySpatialBound spatialData { get; set; }

        public SpatialDataDTO(EntitySpatialBound spatialData)
        {
            this.spatialData = spatialData;
        }
        public SpatialDataDTO() { }
    }
}
