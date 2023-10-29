namespace TestJob1_Junior.Serialize.Models.Output
{
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute("TestJob1", Namespace = "")]
    public class KptDTO
    {
        [System.Xml.Serialization.XmlElementAttribute("land_records")]
        public ParcelsDTO parcels { get; set; }


        [System.Xml.Serialization.XmlElementAttribute("building_records")]
        public BuildingsDTO buildings { get; set; }


        [System.Xml.Serialization.XmlElementAttribute("construction_records")]
        public ConstructionsDTO constructions { get; set; }


        [System.Xml.Serialization.XmlElementAttribute("spatial_data")]
        public SpatialDataDTO spatialData { get; set; }


        [System.Xml.Serialization.XmlElementAttribute("municipal_boundaries")]
        public BoundsDTO bounds { get; set; }


        [System.Xml.Serialization.XmlElementAttribute("zones_and_territories_boundaries")]
        public ZonesDTO zones { get; set; }


        public KptDTO() { }

        public KptDTO(ParcelsDTO parcels, BuildingsDTO buildings, ConstructionsDTO constructions, SpatialDataDTO spatialData, BoundsDTO bounds, ZonesDTO zones)
        {
            this.parcels = parcels;
            this.buildings = buildings;
            this.constructions = constructions;
            this.spatialData = spatialData;
            this.bounds = bounds;
            this.zones = zones;
        }
    }
}
