using System.Collections.Generic;

namespace TestJob1_Junior.Serialize.Models.Output
{
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute]
    public class ZonesDTO
    {
        [System.Xml.Serialization.XmlElementAttribute("zones_and_territories_record")]
        public List<ZonesAndTerritoriesBoundariesTypeZonesAndTerritoriesRecord> zones { get; set; }

        public ZonesDTO(List<ZonesAndTerritoriesBoundariesTypeZonesAndTerritoriesRecord> zones)
        {
            this.zones = zones;
        }
        public ZonesDTO() { }
    }
}
