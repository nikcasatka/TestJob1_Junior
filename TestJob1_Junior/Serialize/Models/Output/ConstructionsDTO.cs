using System.Collections.Generic;

namespace TestJob1_Junior.Serialize.Models.Output
{
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute]
    public class ConstructionsDTO
    {
        [System.Xml.Serialization.XmlElementAttribute("construction_record")]
        public List<BaseDataTypeConstructionRecordsConstructionRecord> constructions { get; set; }

        public ConstructionsDTO(List<BaseDataTypeConstructionRecordsConstructionRecord> constructions)
        {
            this.constructions = constructions;
        }
        public ConstructionsDTO() { }
    }
}
