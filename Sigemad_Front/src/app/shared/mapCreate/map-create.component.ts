import { Component, EventEmitter, inject, Input, Output, signal, SimpleChanges, OnChanges, OnInit, AfterViewInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';

import Feature from 'ol/Feature';
import Point from 'ol/geom/Point';
import { Draw, Snap, Select } from 'ol/interaction';
import { DrawEvent } from 'ol/interaction/Draw';
import { Tile as TileLayer, Vector as VectorLayer } from 'ol/layer';
import Map from 'ol/Map';
import { OSM, Vector as VectorSource, WMTS, XYZ } from 'ol/source';
import View from 'ol/View';
import LayerGroup from 'ol/layer/Group';
import { defaults as defaultControls, FullScreen, ScaleLine } from 'ol/control';
import LayerSwitcher from 'ol-ext/control/LayerSwitcher';
import TileWMS from 'ol/source/TileWMS';
import Icon from 'ol/style/Icon';
import Style from 'ol/style/Style';
import Stroke from 'ol/style/Stroke';
import { Geometry, Polygon } from 'ol/geom';
import WMTSTileGrid from 'ol/tilegrid/WMTS';
import { get as getProjection } from 'ol/proj';
import { getTopLeft } from 'ol/extent';
import Overlay from 'ol/Overlay';
import GeoJSON from 'ol/format/GeoJSON';
import KML from 'ol/format/KML';
import proj4 from 'proj4';
import { fromLonLat, toLonLat } from 'ol/proj';
import { ConfigService } from 'src/app/app.config.service';

import Bar from 'ol-ext/control/Bar';
import Toggle from 'ol-ext/control/Toggle';
import SearchNominatim from 'ol-ext/control/SearchNominatim';

import { MunicipalityService } from '../../services/municipality.service';
import { Municipality } from '../../types/municipality.type';

import 'ol/ol.css';
import 'ol-ext/dist/ol-ext.css';
import { DragDropModule } from '@angular/cdk/drag-drop';

// Define the projection for UTM zone 30N (EPSG:25830)
const utm30n = '+proj=utm +zone=30 +ellps=GRS80 +units=m +no_defs';

@Component({
  selector: 'app-map-create',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule, FlexLayoutModule, DragDropModule],
  templateUrl: './map-create.component.html',
  styleUrl: './map-create.component.css',
})
export class MapCreateComponent implements OnInit, OnChanges, AfterViewInit {
  @Input() municipio: any;
  @Input() listaMunicipios: any;
  @Input() onlyView: any = null;
  @Input() centroideMunicipio: boolean = false;
  @Input() polygon: any;
  @Input() close: boolean = true;
  @Input() fileContent: string | null = null;
  @Input() showSearchCoordinates: boolean = false;

  @Output() save = new EventEmitter<Feature<Geometry>[]>();

  public source!: VectorSource;
  public map!: Map;
  public view!: View;
  public drawPoint!: Draw;
  public drawPolygon!: Draw;
  public snap!: Snap;
  public layerEdition!: VectorLayer;
  public coords: any;
  public select!: Select;
  //public layerCentroideMunicipio!: VectorLayer<VectorSource<Feature<Point>>>;
  public layerLimitesMunicipio: any;
  public highLightMunicipio!: VectorLayer<VectorSource<Feature<Polygon>>>;
  public length!: number;
  public latitude!: number;
  public section: string = '';
  public popup!: Overlay;

  public coordinates: string = '';
  public cursorPosition = { x: 0, y: 0 };

  public data = inject(MAT_DIALOG_DATA);
  public matDialogRef = inject(MatDialogRef);
  public matDialog = inject(MatDialog);
  public urlGeoserver = inject(ConfigService).urlGeoserver;


  public municipalityService = inject(MunicipalityService);

  public municipalities = signal<Municipality[]>([]);
  public municipioSelected = signal(this.data?.municipio || {});


  private styleEdicion = new Style({
    // Estilo para puntos
    image: new Icon({
      anchor: [0.5, 0.5],
      src: '/assets/img/centroide.png',
      scale: 0.1,
    }),
    // Estilo para polígonos
    stroke: new Stroke({
      color: 'rgb(255, 128, 0)',
      width: 5,
    }),
  });

  async ngOnInit() {

    const { municipio, listaMunicipios, defaultPolygon, onlyView, centroideMunicipio, showSearchCoordinates } = this.data;

    if (municipio != null) this.municipio = municipio;

    if (defaultPolygon == null && this.polygon == null) {
      this.polygon = [];
    }

    if (defaultPolygon != null) {
      this.polygon = defaultPolygon;
    }

    if (onlyView != null) this.onlyView = onlyView;

    if (centroideMunicipio != null) this.centroideMunicipio = centroideMunicipio;

    if (showSearchCoordinates != null) this.showSearchCoordinates = showSearchCoordinates;

    this.configureMap(this.municipio, this.polygon, this.onlyView, this.centroideMunicipio);
    this.highlightSelectedMunicipio(this.municipio.descripcion);
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['polygon'] && changes['polygon'].currentValue && this.source) {
      this.updateMapWithPolygon(changes['polygon'].currentValue);
    }
    if (changes['municipio'] && changes['municipio'].currentValue && this.source) {
      //this.updateMapWithMunicipio(changes['municipio'].currentValue);
      this.highlightSelectedMunicipio(changes['municipio'].currentValue.descripcion);
    }
    if (changes['fileContent'] && changes['fileContent'].currentValue) {
      this.loadFileContent(changes['fileContent'].currentValue);
    }
  }

  ngAfterViewInit() {
    if (this.data.onlyView) {
      setTimeout(() => {
        const closeButton = document.getElementById('btnCerrar') as HTMLElement;
        if (closeButton) {
          closeButton.focus();
        }
      }, 500);
    }
  }

  private updateMapWithPolygon(newPolygon: any) {
    if (newPolygon) {
      this.layerEdition.getSource()?.clear();

      if (newPolygon) {
        const defaultPolygonMercator = newPolygon.map((coord: any) => fromLonLat(coord));
        const polygonFeature = new Feature({
          geometry: new Polygon([defaultPolygonMercator]),
        });
        this.layerEdition.getSource()?.addFeature(polygonFeature);
      }
    }
  }

  private updateMapWithMunicipio(newMunicipio: any) {
    if (newMunicipio && newMunicipio.geoPosicion) {
      const coordinates = fromLonLat(newMunicipio.geoPosicion.coordinates);
      this.map.getView().setCenter(coordinates);
      const pointFeature = new Feature(new Point(coordinates));
    }
  }

  configureMap(municipio: any, defaultPolygon: any = null, onlyView: any = null, centroideMunicipio: boolean) {
    if (!municipio) {
      return;
    }

    const baseLayers = this.getBaseLayers();

    const layersGroupAdmin = this.getAdminLayers();

    const layersGroupIncendio = this.getFireLayers(municipio, defaultPolygon, centroideMunicipio);

    // Crear el popup
    const container = document.getElementById('popup');
    this.popup = new Overlay({
      element: container!,
      autoPan: true,
      positioning: 'bottom-center',
    });

    this.view = new View({
      center: fromLonLat(municipio.geoPosicion.coordinates),
      zoom: 12,
      projection: 'EPSG:3857',
    });

    // Crear el mapa
    this.map = new Map({
      controls: defaultControls({
        zoom: true,
        zoomOptions: {
          zoomInTipLabel: 'Acercar',
          zoomOutTipLabel: 'Alejar',
        },
      }).extend([new FullScreen({ tipLabel: 'Pantalla completa' })]),
      target: 'map',
      layers: [baseLayers, layersGroupAdmin, layersGroupIncendio],
      view: this.view,
      overlays: [this.popup],
    });

    const layersSwitcher = new LayerSwitcher({
      mouseover: false,
      show_progress: true,
      trash: true,
      extent: true,
    });

    layersSwitcher.tip = {
      up: 'Arriba/Abajo',
      down: 'Arriba/Abajo',
      info: 'Información',
      extent: 'Extensión',
      trash: 'Eliminar',
      plus: 'Expandir/Contraer',
    };

    this.map.addControl(layersSwitcher);

    const layerSwitcherElement = document.querySelector('.ol-layerswitcher');
    if (layerSwitcherElement) {
      layerSwitcherElement.setAttribute('title', 'Capas');
    }

    this.map.addControl(new ScaleLine());

    this.map.addControl(
      new SearchNominatim({
        placeholder: 'Buscar ubicación...',
        title: 'Buscar en el mapa',
        onselect: (event: any) => {
          const coordenadas = event.coordinate;
          this.map.getView().setCenter(coordenadas);
          this.map.getView().setZoom(15);
        },
      })
    );

    if (!onlyView) {
      this.addInteractions();
    }

    this.map.on('pointermove', (event) => {
      const coordinate = event.coordinate;
      const [lon, lat] = toLonLat(coordinate);
      const [x, y] = proj4('EPSG:4326', utm30n, [lon, lat]);

      this.coordinates = `X: ${x.toFixed(2)}, Y: ${y.toFixed(2)}`;
    });

    this.infoPopup(layersGroupAdmin);
  }

  getBaseLayers() {
    const baseLayers = new LayerGroup({
      properties: { title: 'Capas base', openInLayerSwitcher: true },
      layers: [
        new TileLayer({
          source: new WMTS({
            url: 'https://www.ign.es/wmts/ign-base?',
            layer: 'IGNBaseTodo',
            matrixSet: 'GoogleMapsCompatible',
            format: 'image/png',
            style: 'default',
            tileGrid: new WMTSTileGrid({
              origin: getTopLeft(getProjection('EPSG:3857')?.getExtent() || [0, 0, 0, 0]),
              resolutions: [
                156543.03392804097, 78271.51696402048, 39135.75848201024, 19567.87924100512, 9783.93962050256, 4891.96981025128, 2445.98490512564,
                1222.99245256282, 611.49622628141, 305.748113140705, 152.8740565703525, 76.43702828517625, 38.21851414258813, 19.109257071294063,
                9.554628535647032, 4.777314267823516, 2.388657133911758, 1.194328566955879, 0.5971642834779395, 0.29858214173896974,
                0.14929107086948487,
              ],
              matrixIds: ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14', '15', '16', '17', '18', '19', '20'],
            }),
            attributions: '© Instituto Geográfico Nacional de España (IGN)',
          }),
          properties: { title: 'IGN callejero', baseLayer: true },
          visible: true,
        }),
        new TileLayer({
          source: new TileWMS({
            url: 'https://www.ign.es/wms-inspire/pnoa-ma?',
            params: { LAYERS: 'OI.OrthoimageCoverage', FORMAT: 'image/jpeg' },
            attributions: '© PNOA - IGN España',
          }),
          properties: { title: 'IGN satélite', baseLayer: true },
          visible: false,
        }),
        new TileLayer({
          source: new XYZ({
            url: 'https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}',
            attributions: 'Tiles © <a href="https://www.esri.com/">Esri</a> - Source: Esri, Maxar, Earthstar Geographics',
          }),
          properties: { title: 'Satélite', baseLayer: true },
          visible: false,
        }),
        new TileLayer({
          source: new OSM(),
          properties: { title: 'OpenStreetMap', baseLayer: true },
          visible: false,
        }),
      ],
    });
    // Evitar que se oculten todas las capas base
    function preventGroupLayerToggle(event: any) {
      if (event.target === baseLayers) {
        baseLayers.setVisible(true);
      }
    }
    baseLayers.on('change:visible', preventGroupLayerToggle);
    return baseLayers;
  }

  getAdminLayers() {
    this.layerLimitesMunicipio = new TileLayer({
      source: new TileWMS({
        url: this.urlGeoserver + 'wms?version=1.1.0',
        params: {
          LAYERS: 'limites_municipio',
          TILED: true,
        },
        serverType: 'geoserver',
        transition: 0,
      }),
      properties: { title: 'Límites municipio' },
    });

    const wmsLayersGroup = new LayerGroup({
      properties: { title: 'Límites administrativos', openInLayerSwitcher: true },
      layers: [
        new TileLayer({
          source: new TileWMS({
            url: this.urlGeoserver + 'wms?version=1.1.0',
            params: {
              LAYERS: 'limites_autonomia',
              TILED: true,
            },
            serverType: 'geoserver',
            transition: 0,
          }),
          properties: { title: 'Límites autonómicos' },
        }),
        new TileLayer({
          source: new TileWMS({
            url: this.urlGeoserver + 'wms?version=1.1.0',
            params: {
              LAYERS: 'limites_provincia',
              TILED: true,
            },
            serverType: 'geoserver',
            transition: 0,
          }),
          properties: { title: 'Límites provinciales' },
        }),
        this.layerLimitesMunicipio,
        new TileLayer({
          source: new TileWMS({
            url: this.urlGeoserver + 'wms?version=1.1.0',
            params: {
              LAYERS: 'nucleos_poblacion',
              TILED: true,
            },
            serverType: 'geoserver',
            transition: 0,
          }),
          properties: { id: 'nucleos_poblacion', title: 'Núcleos de población' },
        }),
      ],
    });
    return wmsLayersGroup;
  }

  getFireLayers(municipio: any, defaultPolygon: any, centroideMunicipio: boolean) {
    let defaultPolygonMercator;

    //si defaultPolygon es un array de una dimension, se convierte en un array de dos dimensiones
    if (defaultPolygon && defaultPolygon.length > 0 && !Array.isArray(defaultPolygon[0])) {
      defaultPolygon = [defaultPolygon];
    }

    if (defaultPolygon && defaultPolygon.length > 0) {
      defaultPolygonMercator = defaultPolygon.map((coord: any) => fromLonLat(coord));
    }
    this.source = new VectorSource();

    this.layerEdition = new VectorLayer({
      source: this.source,
      style: this.styleEdicion,
      properties: { title: 'Área afectada' },
    });

    const point = new Point(fromLonLat(municipio.geoPosicion.coordinates));
    const pointFeature = new Feature({
      geometry: point,
    });

    if (defaultPolygonMercator) {
      let geometryFeature;
      if (defaultPolygonMercator.length > 1) {
        geometryFeature = new Feature({
          geometry: new Polygon([defaultPolygonMercator]),
        });
      } else {
        geometryFeature = new Feature({
          geometry: new Point(defaultPolygonMercator[0]),
        });
      }
      this.source.addFeature(geometryFeature);
    }

    if (this.source.getFeatures().length == 0 && centroideMunicipio) {
      this.source.addFeature(pointFeature);
    }

    /*
    this.layerCentroideMunicipio = new VectorLayer({
      source: new VectorSource({
        features: [pointFeature],
      }),
      properties: { title: 'Municipio' },
    });

    this.layerCentroideMunicipio.setStyle(
      new Style({
        image: new Icon({
          anchor: [1, 1],
          src: '/assets/img/centroide.png',
          scale: 0.07,
        }),
      })
    );
    */

    this.highLightMunicipio = new VectorLayer({
      source: new VectorSource({
        format: new GeoJSON(),
      }),
      properties: { title: 'Límites municipio' },
    });

    return new LayerGroup({
      properties: { title: 'Incendios', openInLayerSwitcher: true },
      // layers: [this.layerCentroideMunicipio, this.layerEdicion, this.highLightMunicipio],
      layers: [this.highLightMunicipio, this.layerEdition],
    });
  }

  infoPopup(layersGroupAdmin: LayerGroup) {
    this.map.on('singleclick', (evt) => {
      const coordinate = evt.coordinate;

      // Hacer la consulta WMS GetFeatureInfo
      const viewResolution = this.view.getResolution();

      const nucleosPoblacionLayer = layersGroupAdmin
        .getLayers()
        .getArray()
        .find((layer) => layer.get('id') === 'nucleos_poblacion') as TileLayer;

      const url = (nucleosPoblacionLayer?.getSource() as TileWMS)?.getFeatureInfoUrl(coordinate, viewResolution!, 'EPSG:3857', {
        INFO_FORMAT: 'application/json',
      });

      if (url) {
        fetch(url)
          .then((response) => response.json())
          .then((data) => {
            if (data.features.length > 0) {
              const content = document.getElementById('popup-content');
              content!.innerHTML = `
                <h4>Núcleo de población</h4>
                <p>Núcleo: ${data.features[0].properties.nombre}</p>
                <p>Población: ${data.features[0].properties.poblacion || 'N/A'}</p>
              `;
              this.popup.setPosition(coordinate);
            } else {
              this.popup.setPosition(undefined);
            }
          });
      }
    });
  }

  changeMunicipio(event: any) {
    //console.info('event', event);
  }

  addInteractions() {
    let mainBar = new Bar({});
    this.map.addControl(mainBar);

    let drawBar = new Bar({
      group: true,
      toggleOne: true,
    });

    mainBar.addControl(drawBar);

    this.drawPoint = new Draw({
      type: 'Point',
      source: this.source,
    });

    let tgPoint = new Toggle({
      title: 'Dibujar punto',
      html: '<img src="/assets/img/location-dot-solid.svg" alt="Toggle Icon" style="width: 24px; height: 24px;">',
      interaction: this.drawPoint,
    });
    tgPoint.setActive(true);

    this.drawPoint.on('drawstart', () => {
      this.source.clear();
    });

    this.drawPoint.on('drawend', (drawEvent: DrawEvent) => {
      const geometry = drawEvent.feature.getGeometry();
      if (geometry instanceof Point) {
        const coords = [];
        coords.push(toLonLat(geometry.getCoordinates()));
        this.coords = coords;
        this.save.emit(this.coords);
      }
    });

    drawBar.addControl(tgPoint);

    /*    
    this.drawPolygon = new Draw({
      type: 'Polygon',
      source: this.source,
    });

    let tgPolygon = new Toggle({
      title: 'Dibujar polígono',
      html: '<img src="/assets/img/draw-polygon.svg" alt="Toggle Icon" style="width: 24px; height: 24px;">',
      interaction: this.drawPolygon,
    });
    tgPolygon.setActive(true);

    this.drawPolygon.on('drawstart', (drawEvent: DrawEvent) => {
      this.coords = null;
      const features = this.source.getFeatures();
      const last = features[features.length - 1];
      this.source.removeFeature(last);
    });

    this.drawPolygon.on('drawend', (drawEvent: DrawEvent) => {
      const coords = [];

      for (let coord of drawEvent.target.sketchCoords_[0]) {
        coords.push(toLonLat(coord));
      }

      coords.push(coords[0]);

      this.coords = coords;
      this.save.emit(this.coords);
    });

    drawBar.addControl(tgPolygon);

    const tgSelect = new Toggle({
      html: '<img src="/assets/img/hand-pointer.svg" alt="Toggle Icon" style="width: 24px; height: 24px;">',
      title: 'Seleccionar',
      interaction: new Select({ hitTolerance: 2 }),
    });
    drawBar.addControl(tgSelect);

    const tgDelete = new Toggle({
      html: '<img src="/assets/img/trash.svg" alt="Toggle Icon" style="width: 24px; height: 24px;">',
      title: 'Borrar',
      onToggle: () => {
        if (this.select) {
          const selectedFeatures = this.select.getFeatures();
          selectedFeatures.forEach((feature) => {
            this.source.removeFeature(feature);
          });
          selectedFeatures.clear();
        }
      },
    });
    drawBar.addControl(tgDelete);

    this.select = new Select();
    this.map.addInteraction(this.select);
*/
    // this.map.addInteraction(this.drawPolygon);
    // this.snap = new Snap({ source: this.source });
    // this.map.addInteraction(this.snap);
  }

  savePolygon() {
    if (this.coords) {
      localStorage.setItem('coordinates' + this.section, this.coords);
      localStorage.setItem('polygon' + this.section, '1');
      this.save.emit(this.coords);
      if (this.close) this.closeModal();
    }
  }

  closeModal() {
    if (this.close) this.matDialogRef.close();
  }

  searchCoordinates() {
    const utmX = (document.getElementById('utm-x') as HTMLInputElement).value;
    const utmY = (document.getElementById('utm-y') as HTMLInputElement).value;

    if (utmX && utmY) {
      const [lon, lat] = proj4(utm30n, 'EPSG:4326', [parseFloat(utmX), parseFloat(utmY)]);
      const coordinate = fromLonLat([lon, lat]);
      this.view.setCenter(coordinate);
      this.view.setZoom(13);
    }
  }

  private highlightSelectedMunicipio(municipio: string) {
    this.highLightMunicipio.getSource()?.clear();

    if (this.layerLimitesMunicipio) {
      const source = this.layerLimitesMunicipio.getSource() as TileWMS;
      const center = this.map.getView().getCenter();
      const resolution = this.map.getView().getResolution();
      if (center && resolution) {
        const radius = 1000; // Radio en metros

        const newExtent = [center[0] - radius, center[1] - radius, center[0] + radius, center[1] + radius];

        const url = source.getFeatureInfoUrl(newExtent, resolution, 'EPSG:3857', { INFO_FORMAT: 'application/json', FEATURE_COUNT: 10, BUFFER: 20 });
        if (url) {
          fetch(url)
            .then((response) => response.json())
            .then((data) => {
              // Función para normalizar y limpiar el nombre del municipio
              const normalizeString = (str: string) => {
                return str
                  .toLowerCase()
                  .normalize('NFD') // Normaliza la cadena
                  .replace(/[\u0300-\u036f]/g, '') // Elimina acentos
                  .replace(/\b(el|la|los|las|as|os|de|del|y|a|o|l|els|les)\b/g, '') // Elimina artículos y preposiciones
                  .replace(/\b(general)\b/g, '') // casos especiales
                  .replace(/guipuzkoa/g, 'guipuzcoa') // casos especiales
                  .replace(/araba/g, 'alava') // casos especiales
                  .replace(/[^a-z0-9\s]/g, '') // Elimina caracteres especiales
                  .replace(/\s+/g, ' ') // Elimina espacios en blanco duplicados
                  .trim(); // Elimina espacios en blanco al inicio y al final
              };

              const normalizedMunicipio = normalizeString(municipio);

              const feature = data.features.find((f: any) => {
                // console.info('f.properties.NAMEUNIT normalizado: ', normalizeString(f.properties.NAMEUNIT));
                // console.info('normalizedMunicipio: ', normalizedMunicipio);
                return normalizeString(f.properties.NAMEUNIT).includes(normalizedMunicipio);
              });

              if (feature) {
                const highlightStyle = new Style({
                  stroke: new Stroke({
                    color: 'rgb(255, 128, 0)',
                    width: 5,
                  }),
                });

                const coordinates = feature.geometry.coordinates;

                coordinates.forEach((polygonCoords: any) => {
                  const olFeature = new Feature({
                    geometry: new Polygon(polygonCoords), // Crear un nuevo polígono para cada conjunto de coordenadas
                  });

                  olFeature.setStyle(
                    new Style({
                      stroke: new Stroke({
                        color: 'rgb(255, 128, 0)', // Color del borde
                        width: 5,
                      }),
                    })
                  );

                  this.highLightMunicipio.getSource()?.addFeature(olFeature);
                });

                // Ajustar el zoom para ver el municipio completo
                const extent = this.highLightMunicipio.getSource()?.getExtent();
                if (extent) {
                  this.map.getView().fit(extent, { duration: 1000, padding: [50, 50, 50, 50] });
                }
              } else {
                console.warn('No se encontró el feature para el municipio:', municipio);
              }
            })
            .catch((error) => console.error('Error al obtener FeatureInfo:', error));
        }
      }
    }
  }

  async loadFileContent(fileContent: string) {
    if (fileContent) {
      let features;
      if (fileContent.includes('FeatureCollection')) {
        // Es un GeoJSON
        const geojsonFormat = new GeoJSON();
        features = geojsonFormat.readFeatures(fileContent, {
          featureProjection: 'EPSG:3857',
        });
      } else if (fileContent.includes('<kml')) {
        // Es un KML
        const kmlFormat = new KML();
        features = kmlFormat.readFeatures(fileContent, {
          featureProjection: 'EPSG:3857',
        });
      }

      if (features) {
        this.source.clear();
        this.source.addFeatures(features);

        const coordinates = features.map((feature: any) => {
          const geometry = feature.getGeometry();
          if (geometry) {
            let coords = geometry.getCoordinates();
            // Si el array de coordenadas tiene más de 2 niveles, se guarda solo el primer nivel
            if (Array.isArray(coords[0]) && Array.isArray(coords[0][0])) {
              coords = coords[0];
            }
            const coordsTrans = coords.map((coord: number[]) => toLonLat(coord));
            //console.info('coordsTrans: ', coordsTrans);
            return coordsTrans;
          }
          return [];
        });

        coordinates[0].forEach((subArray: any) => {
          if (subArray.length === 3) {
            subArray.splice(2, 1); // Eliminar el tercer valor si existe
          }
        });

        this.coords = coordinates[0]; // solo se guarda el primer array de coordenadas
        this.save.emit(this.coords);
      }
    }
  }
}
