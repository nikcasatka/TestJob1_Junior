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
        // Создаем переменную для хранения десериализованной КПТ.
        private ExtractCadastralPlanTerritory _kpt;

        // Создаем списки объектов.
        private List<BaseDataTypeLandRecordsLandRecord> _parcels;
        private List<BaseDataTypeBuildRecordsBuildRecord> _buildings;
        private List<BaseDataTypeConstructionRecordsConstructionRecord> _constructions;
        private EntitySpatialBound _spatialData;
        private List<MunicipalBoundariesTypeMunicipalBoundaryRecord> _bounds;
        private List<ZonesAndTerritoriesBoundariesTypeZonesAndTerritoriesRecord> _zones;

        // Создаем переменную для объединения всех перечней объектов из десериализованной КПТ для кастомной сериализации.
        private KptDTO _kptDTO;

        // Создаем списки для выбранных объектов.
        private List<BaseDataTypeLandRecordsLandRecord> _selectedParcels;
        private List<BaseDataTypeBuildRecordsBuildRecord> _selectedBuildings;
        private List<BaseDataTypeConstructionRecordsConstructionRecord> _selectedConstructions;
        private EntitySpatialBound _selectedSpatialData;
        private List<MunicipalBoundariesTypeMunicipalBoundaryRecord> _selectedBounds;
        private List<ZonesAndTerritoriesBoundariesTypeZonesAndTerritoriesRecord> _selectedZones;

        // Создаем переменную для объединения всех перечней выбранных объектов.
        private KptDTO _allSelectedObject;

        public MainWindow()
        {
            InitializeComponent();
        }

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
                            _kpt = xmlSerializerKPT.Deserialize(fs) as ExtractCadastralPlanTerritory;
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show($"Выбранный XML-файл не соответствует XSD-схеме кадастрового плана территории (extract_cadastral_plan_territory).\nEx: {ex.Message}");
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show($"Файл занят или не может быть прочитан: {ex.Message}");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"необработанное исключение: {ex.Message}");
                        }

                        // Инициализируем или очищаем списки объектов.
                        _parcels = new List<BaseDataTypeLandRecordsLandRecord>();
                        _buildings = new List<BaseDataTypeBuildRecordsBuildRecord>();
                        _constructions = new List<BaseDataTypeConstructionRecordsConstructionRecord>();
                        _bounds = new List<MunicipalBoundariesTypeMunicipalBoundaryRecord>();
                        _zones = new List<ZonesAndTerritoriesBoundariesTypeZonesAndTerritoriesRecord>();

                        // Заполненяем списки объектов.
                        foreach (CadastralBlock cb in _kpt.CadastralBlocks)
                        {
                            _parcels.AddRange(cb.RecordData.BaseData.LandRecords);
                            _buildings.AddRange(cb.RecordData.BaseData.BuildRecords);
                            _constructions.AddRange(cb.RecordData.BaseData.ConstructionRecords);
                            _spatialData = cb.SpatialData.EntitySpatial;
                            _bounds.AddRange(cb.MunicipalBoundaries);
                            _zones.AddRange(cb.ZonesAndTerritoriesBoundaries);
                        }

                        // Сливаем списки объектов в объединяющий класс для последующей кастомной сериализации.
                        _kptDTO = new KptDTO(
                        new ParcelsDTO(_parcels),
                        new BuildingsDTO(_buildings),
                        new ConstructionsDTO(_constructions),
                        new SpatialDataDTO(_spatialData),
                        new BoundsDTO(_bounds),
                        new ZonesDTO(_zones)
                        );

                        // Отрисовываем древо объектов по сформированным спискам объектов.
                        ParcelBuildTreeView parcelBuildTreeView = new ParcelBuildTreeView(treeView, _kptDTO.parcels);
                        ObjectRealtyBuildTreeView objectRealtyBuildTreeView = new ObjectRealtyBuildTreeView(treeView, _kptDTO.buildings, _kptDTO.constructions);
                        SpatialDataBuildTreeView spatialDataBuildTreeView = new SpatialDataBuildTreeView(treeView, _kptDTO.spatialData);
                        BoundsBuildTreeView boundsBuildTreeView = new BoundsBuildTreeView(treeView, _kptDTO.bounds);
                        ZonesBuildTreeView zonesBuildTreeView = new ZonesBuildTreeView(treeView, _kptDTO.zones);
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
                    XmlSerializer xmlSerializerDTO = new XmlSerializer(typeof(KptDTO));

                    xmlSerializerDTO.Serialize(fs, _allSelectedObject);

                    MessageBox.Show("Сериализация выполнена успешно.");
                }
            }
        }

        private void treeView_SelectionChanged(object sender, EventArgs e)
        {
            // Инициализируем или очищаем списки объектов.
            _selectedParcels = new List<BaseDataTypeLandRecordsLandRecord>();
            _selectedBuildings = new List<BaseDataTypeBuildRecordsBuildRecord>();
            _selectedConstructions = new List<BaseDataTypeConstructionRecordsConstructionRecord>();
            _selectedSpatialData = new EntitySpatialBound();
            _selectedBounds = new List<MunicipalBoundariesTypeMunicipalBoundaryRecord>();
            _selectedZones = new List<ZonesAndTerritoriesBoundariesTypeZonesAndTerritoriesRecord>();

            foreach (TreeItemViewModel selected in treeView.SelectedItems)
            {
                if (selected is TreeItemViewModel selectedItem)
                {
                    if (selectedItem.Parent is TreeItemViewModel selectedParent)
                    {
                        switch (selectedParent.DisplayName)
                        {
                            case "Parcel":
                                {
                                    BaseDataTypeLandRecordsLandRecord parcel = _kptDTO.parcels.parcels.Where(x => x.Object.CommonData.CadNumber == selectedItem.DisplayName.ToString()).FirstOrDefault();
                                    _selectedParcels.Add(parcel);
                                    break;
                                }

                            case "ObjectRealty":
                                {
                                    if (_kptDTO.buildings.buildings.Where(x => x.Object.CommonData.CadNumber == selectedItem.DisplayName.ToString()).Any())
                                    {
                                        BaseDataTypeBuildRecordsBuildRecord building = _kptDTO.buildings.buildings.Where(x => x.Object.CommonData.CadNumber == selectedItem.DisplayName.ToString()).FirstOrDefault();
                                        _selectedBuildings.Add(building);
                                    }
                                    else
                                    {
                                        BaseDataTypeConstructionRecordsConstructionRecord construction = _kptDTO.constructions.constructions.Where(x => x.Object.CommonData.CadNumber == selectedItem.DisplayName.ToString()).FirstOrDefault();
                                        _selectedConstructions.Add(construction);
                                    }
                                    break;
                                }

                            case "SpatialData":
                                {
                                    _selectedSpatialData = _kptDTO.spatialData.spatialData;
                                    break;
                                }

                            case "Bound":
                                {
                                    MunicipalBoundariesTypeMunicipalBoundaryRecord bound = _kptDTO.bounds.bounds.Where(x => x.BObjectMunicipalBoundary.BObject.RegNumbBorder == selectedItem.DisplayName.ToString()).FirstOrDefault();
                                    _selectedBounds.Add(bound);
                                    break;
                                }

                            case "Zone":
                                {
                                    ZonesAndTerritoriesBoundariesTypeZonesAndTerritoriesRecord zone = _kptDTO.zones.zones.Where(x => x.BObjectZonesAndTerritories.BObject.RegNumbBorder == selectedItem.DisplayName.ToString()).FirstOrDefault();
                                    _selectedZones.Add(zone);
                                    break;
                                }
                        }
                    }
                }
            }

            _allSelectedObject = new KptDTO(
                new ParcelsDTO(_selectedParcels),
                new BuildingsDTO(_selectedBuildings),
                new ConstructionsDTO(_selectedConstructions),
                new SpatialDataDTO(_selectedSpatialData),
                new BoundsDTO(_selectedBounds),
                new ZonesDTO(_selectedZones));

            richTextBoxObjectsContent.Document.Blocks.Clear();

            using (StringWriter writer = new StringWriter())
            {
                XmlSerializer xmlSerializerDTO = new XmlSerializer(typeof(KptDTO));
                xmlSerializerDTO.Serialize(writer, _allSelectedObject);
                richTextBoxObjectsContent.Document.Blocks.Add(new Paragraph(new Run(writer.ToString())));
            }
        }
    }
}
