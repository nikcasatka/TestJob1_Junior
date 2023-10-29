using System.Collections.Generic;

namespace TestJob1_Junior.Serialize.Models.Output
{
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute]
    public class BoundsDTO
    {
        [System.Xml.Serialization.XmlElementAttribute("municipal_boundary_record")]
        public List<MunicipalBoundariesTypeMunicipalBoundaryRecord> bounds { get; set; }

        public BoundsDTO(List<MunicipalBoundariesTypeMunicipalBoundaryRecord> bounds)
        {
            this.bounds = bounds;
        }
        public BoundsDTO() { }
    }
}
