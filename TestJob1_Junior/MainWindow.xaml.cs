using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml.Linq;
using System.Xml.Serialization;
using TestJob1_Junior.Serialize.Models;
using TestJob1_Junior.Tools.TreeViewBuilder;
using TestJob1_Junior.Serialize.Models.Output;
using TestJob1_Junior.Tools.TreeViewBuilder.Classes;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using System.Windows.Documents;
using TestJob1_Junior.TreeView.TreeViewModel;

namespace TestJob1_Junior
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            MultiSelectTreeView treeView = new MultiSelectTreeView();

            InitializeComponent();
        }

        // Создаем переменную для хранения десериализованной КПТ.
        ExtractCadastralPlanTerritory kpt;

        // Создаем списки объектов.
        List<BaseDataTypeLandRecordsLandRecord> parcels;
        List<BaseDataTypeBuildRecordsBuildRecord> buildings;
        List<BaseDataTypeConstructionRecordsConstructionRecord> constructions;
        EntitySpatialBound spatialData;
        List<MunicipalBoundariesTypeMunicipalBoundaryRecord> bounds;
        List<ZonesAndTerritoriesBoundariesTypeZonesAndTerritoriesRecord> zones;

        // Создаем переменную для объединения всех перечней объектов из десериализованной КПТ для кастомной сериализации.
        AllInOneDTO allInOneDTO;

        /// <summary>
        /// Обработчик кнопки выбора файла КПТ на локальном ПК.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFileButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создаем и настраиваем диалоговое окно открытия файла.
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "xml files (*.xml)|*.xml";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                // Открываем диалоговое окно.
                if (openFileDialog.ShowDialog() == true)
                {
                    // Прописываем полный путь к файлу в текстБокс главного окна.
                    opentFileRootTextBox.Text = openFileDialog.FileName;

                    // Десериализуем объект.
                    using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.OpenOrCreate))
                    {

                        // Десериализация КПТ. В случае несоответствия выбранного XML-файла схеме КПТ (extract_cadastral_plan_territory) процесс
                        // прерывается исключением и открывается диалоговое информационное окно с указанием на несоответствие.
                        try
                        {
                            XmlSerializer xmlSerializerKPT = new XmlSerializer(typeof(ExtractCadastralPlanTerritory));
                            kpt = xmlSerializerKPT.Deserialize(fs) as ExtractCadastralPlanTerritory;
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show($"Выбранный XML-файл не соответствует XSD-схеме кадастрового плана территории (extract_cadastral_plan_territory).\nEx: {ex.Message}");
                        }
                        // Добавит катч на занятый файл
                        catch (Exception ex)
                        {
                            MessageBox.Show($"необработанное исключение: {ex.Message}");
                        }

                        // Инициализируем или очищаем списки объектов.
                        parcels = new List<BaseDataTypeLandRecordsLandRecord>();
                        buildings = new List<BaseDataTypeBuildRecordsBuildRecord>();
                        constructions = new List<BaseDataTypeConstructionRecordsConstructionRecord>();
                        bounds = new List<MunicipalBoundariesTypeMunicipalBoundaryRecord>();
                        zones = new List<ZonesAndTerritoriesBoundariesTypeZonesAndTerritoriesRecord>();

                        // Заполненяем списки объектов.
                        foreach (CadastralBlock cb in kpt.CadastralBlocks)
                        {
                            parcels.AddRange(cb.RecordData.BaseData.LandRecords);
                            buildings.AddRange(cb.RecordData.BaseData.BuildRecords);
                            constructions.AddRange(cb.RecordData.BaseData.ConstructionRecords);
                            spatialData = cb.SpatialData.EntitySpatial;
                            bounds.AddRange(cb.MunicipalBoundaries);
                            zones.AddRange(cb.ZonesAndTerritoriesBoundaries);
                        }

                        // Сливаем списки объектов в объединяющий класс для последующей кастомной сериализации.
                        allInOneDTO = new AllInOneDTO(
                        new ParcelsDTO(parcels),
                        new BuildingsDTO(buildings),
                        new ConstructionsDTO(constructions),
                        new SpatialDataDTO(spatialData),
                        new BoundsDTO(bounds),
                        new ZonesDTO(zones)
                        );

                        // Отрисовываем древо объектов по сформированным спискам объектов.
                        ParcelBuildTreeView parcelBuildTreeView = new ParcelBuildTreeView(treeView, allInOneDTO.parcels);
                        ObjectRealtyBuildTreeView objectRealtyBuildTreeView = new ObjectRealtyBuildTreeView(treeView, allInOneDTO.buildings, allInOneDTO.constructions);
                        SpatialDataBuildTreeView spatialDataBuildTreeView = new SpatialDataBuildTreeView(treeView, allInOneDTO.spatialData);
                        BoundsBuildTreeView boundsBuildTreeView = new BoundsBuildTreeView(treeView, allInOneDTO.bounds);
                        ZonesBuildTreeView zonesBuildTreeView = new ZonesBuildTreeView(treeView, allInOneDTO.zones);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Необработанное исключение: {ex.Message}");
            }
        }

        /// <summary>
        /// Обработчик кнопки сериализации выбранных в дереве объектов.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serializeFocusedObjectsButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = "c:\\";
            saveFileDialog.Filter = "xml files (*.xml)|*.xml";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            Nullable<bool> result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                //Сериализуем
                using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.OpenOrCreate))
                {
                    XmlSerializer xmlSerializerDTO = new XmlSerializer(typeof(AllInOneDTO));

                    xmlSerializerDTO.Serialize(fs, allSelectedObject);

                    MessageBox.Show("Сериализация выполнена успешно.");
                }
            }
        }

        // Создаем списки объектов.
        List<BaseDataTypeLandRecordsLandRecord> selectedParcels;
        List<BaseDataTypeBuildRecordsBuildRecord> selectedBuildings;
        List<BaseDataTypeConstructionRecordsConstructionRecord> selectedConstructions;
        EntitySpatialBound selectedSpatialData;
        List<MunicipalBoundariesTypeMunicipalBoundaryRecord> selectedBounds;
        List<ZonesAndTerritoriesBoundariesTypeZonesAndTerritoriesRecord> selectedZones;

        AllInOneDTO allSelectedObject;

        private void treeView_SelectionChanged(object sender, EventArgs e)
        {
            // Инициализируем или очищаем списки объектов.
            selectedParcels = new List<BaseDataTypeLandRecordsLandRecord>();
            selectedBuildings = new List<BaseDataTypeBuildRecordsBuildRecord>();
            selectedConstructions = new List<BaseDataTypeConstructionRecordsConstructionRecord>();
            selectedSpatialData = new EntitySpatialBound();
            selectedBounds = new List<MunicipalBoundariesTypeMunicipalBoundaryRecord>();
            selectedZones = new List<ZonesAndTerritoriesBoundariesTypeZonesAndTerritoriesRecord>();

            foreach (TreeItemViewModel selected in treeView.SelectedItems)
            {
                if (selected is TreeItemViewModel selectedItem)
                {
                    if (selectedItem.Parent is TreeItemViewModel selectedParent)
                    {
                        //XmlSerializer serializer;

                        switch (selectedParent.DisplayName)
                        {
                            case "Parcel":
                                {
                                    BaseDataTypeLandRecordsLandRecord parcel = allInOneDTO.parcels.parcels.Where(x => x.Object.CommonData.CadNumber == selectedItem.DisplayName.ToString()).FirstOrDefault();
                                    selectedParcels.Add(parcel);
                                    //serializer = new XmlSerializer(typeof(BaseDataTypeLandRecordsLandRecord));
                                    //serializer.Serialize(writer, parcel);
                                    break;
                                }

                            case "ObjectRealty":
                                {
                                    if (allInOneDTO.buildings.buildings.Where(x => x.Object.CommonData.CadNumber == selectedItem.DisplayName.ToString()).Any())
                                    {
                                        BaseDataTypeBuildRecordsBuildRecord building = allInOneDTO.buildings.buildings.Where(x => x.Object.CommonData.CadNumber == selectedItem.DisplayName.ToString()).FirstOrDefault();
                                        selectedBuildings.Add(building);
                                        //serializer = new XmlSerializer(typeof(BaseDataTypeBuildRecordsBuildRecord));
                                        //serializer.Serialize(writer, building);
                                    }
                                    else
                                    {
                                        BaseDataTypeConstructionRecordsConstructionRecord construction = allInOneDTO.constructions.constructions.Where(x => x.Object.CommonData.CadNumber == selectedItem.DisplayName.ToString()).FirstOrDefault();
                                        selectedConstructions.Add(construction);
                                        //serializer = new XmlSerializer(typeof(BaseDataTypeConstructionRecordsConstructionRecord));
                                        //serializer.Serialize(writer, construction);
                                    }
                                    break;
                                }

                            case "SpatialData":
                                {
                                    selectedSpatialData = allInOneDTO.spatialData.spatialData;
                                    //serializer = new XmlSerializer(typeof(EntitySpatialBound));
                                    //serializer.Serialize(writer, spatial);
                                    break;
                                }

                            case "Bound":
                                {
                                    MunicipalBoundariesTypeMunicipalBoundaryRecord bound = allInOneDTO.bounds.bounds.Where(x => x.BObjectMunicipalBoundary.BObject.RegNumbBorder == selectedItem.DisplayName.ToString()).FirstOrDefault();
                                    selectedBounds.Add(bound);
                                    //serializer = new XmlSerializer(typeof(MunicipalBoundariesTypeMunicipalBoundaryRecord));
                                    //serializer.Serialize(writer, bound);
                                    break;
                                }

                            case "Zone":
                                {
                                    ZonesAndTerritoriesBoundariesTypeZonesAndTerritoriesRecord zone = allInOneDTO.zones.zones.Where(x => x.BObjectZonesAndTerritories.BObject.RegNumbBorder == selectedItem.DisplayName.ToString()).FirstOrDefault();
                                    selectedZones.Add(zone);
                                    //serializer = new XmlSerializer(typeof(ZonesAndTerritoriesBoundariesTypeZonesAndTerritoriesRecord));
                                    //serializer.Serialize(writer, zone);
                                    break;
                                }
                        }
                    }
                }
            }

            allSelectedObject = new AllInOneDTO(
                new ParcelsDTO(selectedParcels),
                new BuildingsDTO(selectedBuildings),
                new ConstructionsDTO(selectedConstructions),
                new SpatialDataDTO(selectedSpatialData),
                new BoundsDTO(selectedBounds),
                new ZonesDTO(selectedZones));

            richTextBoxObjectsContent.Document.Blocks.Clear();

            using (StringWriter writer = new StringWriter())
            {
                XmlSerializer xmlSerializerDTO = new XmlSerializer(typeof(AllInOneDTO));
                xmlSerializerDTO.Serialize(writer, allSelectedObject);
                richTextBoxObjectsContent.Document.Blocks.Add(new Paragraph(new Run(writer.ToString())));
            }
        }
    }
}
