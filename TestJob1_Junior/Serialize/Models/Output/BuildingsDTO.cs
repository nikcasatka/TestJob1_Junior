using System.Collections.Generic;

namespace TestJob1_Junior.Serialize.Models.Output
{
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute]
    public class BuildingsDTO
    {
        [System.Xml.Serialization.XmlElementAttribute("building_record")]
        public List<BaseDataTypeBuildRecordsBuildRecord> buildings { get; set; }

        public BuildingsDTO(List<BaseDataTypeBuildRecordsBuildRecord> buildings)
        {
            this.buildings = buildings;
        }
        public BuildingsDTO() { }
    }
}
