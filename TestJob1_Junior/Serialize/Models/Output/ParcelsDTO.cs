using System.Collections.Generic;

namespace TestJob1_Junior.Serialize.Models.Output
{
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    public class ParcelsDTO
    {
        [System.Xml.Serialization.XmlElementAttribute("land_record")]
        public List<BaseDataTypeLandRecordsLandRecord> parcels { get; set; }

        public ParcelsDTO(List<BaseDataTypeLandRecordsLandRecord> parcels)
        {
            this.parcels = parcels;
        }
        public ParcelsDTO() { }
    }
}
