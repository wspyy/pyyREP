
/***************************************************************
    
可用于 HMap.addSpecialLayer() 方法加载的图层
     
***************************************************************/

//切图地图服务图层
function TiledLayerFun(url, tiledLayerOption) {
    this.type = "layer_tiled"
    this.id = this.type + "_" + getIdNum();
    this.url = url;
    if (tiledLayerOption) {
        this.name = tiledLayerOption.name;
        this.alpha = tiledLayerOption.alpha;
        this.visible = tiledLayerOption.visible;
        this.displayLevels = tiledLayerOption.displayLevels;
        this.token = tiledLayerOption.token;
    }
}

//动态地图服务图层
function DynamicLayerFun(url, dynamicLayerOption) {
    this.type = "layer_dynamic"
    this.id = this.type + "_" + getIdNum();
    this.url = url;
    if (dynamicLayerOption) {
        this.name = dynamicLayerOption.name;
        this.alpha = dynamicLayerOption.alpha;
        this.visible = dynamicLayerOption.visible;
        this.visibleLayers = dynamicLayerOption.visibleLayers;
        this.imageFormat = dynamicLayerOption.imageFormat;
        this.token = dynamicLayerOption.token;
    }
}

//要素图层
function FeatureLayerFun(url, flayerOption) {
    this.type = "layer_feature"
    this.id = this.type + "_" + getIdNum();
    this.url = url;
    if (flayerOption) {
        this.alpha = featureLayerOption.alpha;
        this.visible = featureLayerOption.visible;
        this.token = featureLayerOption.token;
    }
}

//名称要素图层
function FeatureLayerByNameFun(dynamicLayerUrl, layerName, featureLayerOption) {
    this.type = "layer_featureByName";
    this.id = this.type + "_" + getIdNum();
    this.dynamicLayerUrl = dynamicLayerUrl;
    this.layerName = layerName;
    if (featureLayerOption) {
        this.alpha = featureLayerOption.alpha;
        this.visible = featureLayerOption.visible;
        this.token = featureLayerOption.token;
    }
}

//天地图图层
function TDTLayerFun(mapType, option) {
    this.type = "layer_TDT";
    this.id = this.type + "_" + getIdNum();
    this.mapType = mapType;
    if (option) {
        this.alpha = option.alpha;
        this.visible = option.visible;
        this.name = option.name;
        this.isShowNote = option.isShowNote;
        this.displayLODs = option.displayLODs;
        this.token = option.token;
    }
}

/***************************************************************
        
可用于 HMap.addGraphic方法加载的图形
                        
***************************************************************/

//图片符号点位图形
function PictureGraphicFun(x, y, pictureUrl, option) {
    this.type = "graphic_picture";
    this.id = this.type + "_" + getIdNum();
    this.x = x;
    this.y = y;
    this.pictureUrl = pictureUrl;
    if (option) {
        this.visible = option.visible;
        this.alpha = option.alpha;
        this.width = option.width;
        this.height = option.height;
        this.xoffset = option.xoffset;
        this.yoffset = option.yoffset;
        this.angle = option.angle;
        this.data = option.data;
    }

    this.listenerObjList = new Array();
    this.listenerObj = new Object();
    this.addEventListener = function (type, listener, param) {
        if (type == "mouseClick") {
            this.listenerObj = { listenerType: type, listenerFun: listener };
            this.listenerObjList.push(this.listenerObj);
            this.listenerParam = { x: x, y: y, data: this.data, target: this, param: param };
        }
        if (type == "mouseOver") {
            this.listenerObj = { listenerType: type, listenerFun: listener };
            this.listenerObjList.push(this.listenerObj);
            this.listenerParam = { x: x, y: y, data: this.data, target: this, param: param };
        }
        if (type == "mouseOut") {
            this.listenerObj = { listenerType: type, listenerFun: listener };
            this.listenerObjList.push(this.listenerObj);
            this.listenerParam = { x: x, y: y, data: this.data, target: this, param: param };
        }
    }
}

//气泡信息符号点位图形
function InfoGraphicFun(x, y, text, option) {
    this.type = "graphic_info";
    this.id = this.type + "_" + getIdNum();
    this.x = x;
    this.y = y;
    this.text = text;
    if (option) {
        this.visible = option.visible;
        this.alpha = option.alpha;
        this.filters = option.filters;
        this.displayLevel = option.displayLevel;
        this.displayScale = option.displayScale;
        this.textFormat = option.textFromat;
        this.borderColor = option.borderColor;
        this.backgroundColor = option.backgroundColor;
        this.placement = option.placement;
        this.isMouseOver = option.isMouseOver;
        this.mouseOverAlpha = option.mouseOverAlpha;
        this.mouseOverFilters = option.mouseOverFilters;
        this.mouseOverTextFormat = option.mouseOverTextFormat;
        this.mouseOverBorderColor = option.mouseOverBorderColor;
        this.mouseOverBackgroundColor = option.mouseOverBackgroundColor;
        this.data = option.data;

    }
    this.listenerObjList = new Array();
    this.listenerObj = new Object();
    this.addEventListener = function (type, listener, param) {
        if (type == "mouseClick") {
            this.listenerObj = { listenerType: type, listenerFun: listener };
            this.listenerObjList.push(this.listenerObj);
            this.listenerParam = { x: x, y: y, data: this.data, target: this, param: param };
        }
        if (type == "mouseOver") {
            this.listenerObj = { listenerType: type, listenerFun: listener };
            this.listenerObjList.push(this.listenerObj);
            this.listenerParam = { x: x, y: y, data: this.data, target: this, param: param };
        }
        if (type == "mouseOut") {
            this.listenerObj = { listenerType: type, listenerFun: listener };
            this.listenerObjList.push(this.listenerObj);
            this.listenerParam = { x: x, y: y, data: this.data, target: this, param: param };
        }
    }
}

//简单点位图形
function SimpleGraphicFun(x, y, option) {
    this.type = "graphic_simple";
    this.id = this.type + "_" + getIdNum();
    this.x = x;
    this.y = y;
    if (option) {
        this.visible = option.visible;
        this.alpha = option.alpha;
        this.data = option.data;

        this.style = option.style;
        this.size = option.size;
        this.color = option.color;
        this.xoffset = option.xoffset;
        this.yoffset = option.yoffset;
        this.angle = option.angle;
        this.outLineStyle = option.outLineStyle;
        this.outLineColor = option.outLineColor;
        this.outLineAlpha = option.outLineAlpha;
        this.outLinerWidth = option.outLinerWidt;
    }
    this.listenerObjList = new Array();
    this.listenerObj = new Object();
    this.addEventListener = function (type, listener, param) {
        if (type == "mouseClick") {
            this.listenerObj = { listenerType: type, listenerFun: listener };
            this.listenerObjList.push(this.listenerObj);
            this.listenerParam = { x: x, y: y, data: this.data, target: this, param: param };
        }
        if (type == "mouseOver") {
            this.listenerObj = { listenerType: type, listenerFun: listener };
            this.listenerObjList.push(this.listenerObj);
            this.listenerParam = { x: x, y: y, data: this.data, target: this, param: param };
        }
        if (type == "mouseOut") {
            this.listenerObj = { listenerType: type, listenerFun: listener };
            this.listenerObjList.push(this.listenerObj);
            this.listenerParam = { x: x, y: y, data: this.data, target: this, param: param };
        }
    }
}

// 文本点位图形
function TextGraphicFun(x, y, text, option) {
    this.type = "graphic_text";
    this.id = this.type + "_" + getIdNum();
    this.x = x;
    this.y = y;
    this.text = text;
    if (option) {
        this.visible = option.visible;
        this.alpha = option.alpha;
        this.data = option.data;
        this.color = option.color;
        this.border = option.border;
        this.borderColor = option.borderColor;
        this.background = option.background;
        this.placement = option.placement;
        this.xoffset = option.xoffset;
        this.yoffset = option.yoffset;
        this.angle = option.angle;
        this.textFormatSize = option.textFormatSize;
        this.textFormatFont = option.textFormatFont;
        this.textFormatBold = option.textFormatBold;
        this.textFormatItalic = option.textFormatItalic;
        this.textFormatUnderline = option.textFormatUnderline;
        this.textFormatAlig = option.textFormatAlig;
    }
    this.listenerObjList = new Array();
    this.listenerObj = new Object();
    this.addEventListener = function (type, listener, param) {
        if (type == "mouseClick") {
            this.listenerObj = { listenerType: type, listenerFun: listener };
            this.listenerObjList.push(this.listenerObj);
            this.listenerParam = { x: x, y: y, data: this.data, target: this, param: param };
        }
        if (type == "mouseOver") {
            this.listenerObj = { listenerType: type, listenerFun: listener };
            this.listenerObjList.push(this.listenerObj);
            this.listenerParam = { x: x, y: y, data: this.data, target: this, param: param };
        }
        if (type == "mouseOut") {
            this.listenerObj = { listenerType: type, listenerFun: listener };
            this.listenerObjList.push(this.listenerObj);
            this.listenerParam = { x: x, y: y, data: this.data, target: this, param: param };
        }
    }
}

//线状图形形
function PolylineGraphicFun(mapPoints, option) {
    this.type = "graphic_polyline";
    this.id = this.type + "_" + getIdNum();
    this.mapPoints = mapPoints;
    if (option) {
        this.visible = option.visible;
        this.data = option.data;
        this.style = option.style;
        this.color = option.color;
        this.width = option.width;
        this.alpha = option.alpha;
    }
    this.listenerObjList = new Array();
    this.listenerObj = new Object();
    this.addEventListener = function (type, listener, param) {
        if (type == "mouseClick") {
            this.listenerObj = { listenerType: type, listenerFun: listener };
            this.listenerObjList.push(this.listenerObj);
            this.listenerParam = { paths: mapPoints, data: this.data, target: this, param: param };
        }
        if (type == "mouseOver") {
            this.listenerObj = { listenerType: type, listenerFun: listener };
            this.listenerObjList.push(this.listenerObj);
            this.listenerParam = { x: x, y: y, data: this.data, target: this, param: param };
        }
        if (type == "mouseOut") {
            this.listenerObj = { listenerType: type, listenerFun: listener };
            this.listenerObjList.push(this.listenerObj);
            this.listenerParam = { x: x, y: y, data: this.data, target: this, param: param };
        }
    }
}

//面状多边形
function PolygonGraphicFun(mapPoints, option) {
    this.type = "graphic_polygon";
    this.id = this.type + "_" + getIdNum();
    this.mapPoints = mapPoints;
    if (option) {
        this.visible = option.visible;
        this.alpha = option.alpha;
        this.data = option.data;
        this.fillAlpha = option.fillAlpha;
        this.fillColor = option.fillColor;
        this.outLineStyle = option.outLineStyle;
        this.outLineColor = option.outLineColor;
        this.outLineAlpha = option.outLineAlpha;
        this.outLinerWidth = option.outLinerWidth;
    }
    this.listenerObjList = new Array();
    this.listenerObj = new Object();
    this.addEventListener = function (type, listener, param) {
        if (type == "mouseClick") {
            this.listenerObj = { listenerType: type, listenerFun: listener };
            this.listenerObjList.push(this.listenerObj);
            this.listenerParam = { rings: mapPoints, data: this.data, target: this, param: param };
        }
        if (type == "mouseOver") {
            this.listenerObj = { listenerType: type, listenerFun: listener };
            this.listenerObjList.push(this.listenerObj);
            this.listenerParam = { x: x, y: y, data: this.data, target: this, param: param };
        }
        if (type == "mouseOut") {
            this.listenerObj = { listenerType: type, listenerFun: listener };
            this.listenerObjList.push(this.listenerObj);
            this.listenerParam = { x: x, y: y, data: this.data, target: this, param: param };
        }
    }
}

//范围图形
function ExtentGraphicFun(xmin, ymin, xmax, ymax, option) {
    this.type = "graphic_extent";
    this.id = this.type + "_" + getIdNum();
    this.xmin = xmin;
    this.ymin = ymin;
    this.xmax = xmax;
    this.ymax = ymax;
//    this.symbol = new HMap.SimpleFillSymbol(option);
    if (option) {
        this.visible = option.visible;
        this.alpha = option.alpha;
        this.data = option.data;
        this.fillAlpha = option.fillAlpha;
        this.fillColor = option.fillColor;
        this.outLineStyle = option.outLineStyle;
        this.outLineColor = option.outLineColor;
        this.outLineAlpha = option.outLineAlpha;
        this.outLinerWidth = option.outLinerWidth;
    }
    this.listenerObjList = new Array();
    this.listenerObj = new Object();
    this.addEventListener = function (type, listener, param) {
        if (type == "mouseClick") {
            this.listenerObj = { listenerType: type, listenerFun: listener };
            this.listenerObjList.push(this.listenerObj);
            this.listenerParam = { xmin: xmin, ymin: ymin, xmax: xmax, ymax: ymax, data: this.data, target: this, param: param };
        }
        if (type == "mouseOver") {
            this.listenerObj = { listenerType: type, listenerFun: listener };
            this.listenerObjList.push(this.listenerObj);
            this.listenerParam = { x: x, y: y, data: this.data, target: this, param: param };
        }
        if (type == "mouseOut") {
            this.listenerObj = { listenerType: type, listenerFun: listener };
            this.listenerObjList.push(this.listenerObj);
            this.listenerParam = { x: x, y: y, data: this.data, target: this, param: param };
        }
    }
}


//图片符号点位图形
function MeteorGraphicFun(x, y, pictureUrl, option) {
    this.type = "graphic_meteor";
    this.id = this.type + "_" + getIdNum();
    this.x = x;
    this.y = y;
    this.pictureUrl = pictureUrl;
    if (option) {
        this.visible = option.visible;
        this.alpha = option.alpha;
        this.width = option.width;
        this.height = option.height;
        this.xoffset = option.xoffset;
        this.yoffset = option.yoffset;
        this.angle = option.angle;
        this.data = option.data;
    }

    this.listenerObjList = new Array();
    this.listenerObj = new Object();
    this.addEventListener = function (type, listener, param) {
        if (type == "mouseClick") {
            this.listenerObj = { listenerType: type, listenerFun: listener };
            this.listenerObjList.push(this.listenerObj);
            this.listenerParam = { x: x, y: y, data: this.data, target: this, param: param };
        }
        if (type == "mouseOver") {
            this.listenerObj = { listenerType: type, listenerFun: listener };
            this.listenerObjList.push(this.listenerObj);
            this.listenerParam = { x: x, y: y, data: this.data, target: this, param: param };
        }
        if (type == "mouseOut") {
            this.listenerObj = { listenerType: type, listenerFun: listener };
            this.listenerObjList.push(this.listenerObj);
            this.listenerParam = { x: x, y: y, data: this.data, target: this, param: param };
        }
    }
}


//图片符号点位图形
function AirGraphicFun(x, y, pictureUrl, option) {
    this.type = "graphic_api";
    this.id = this.type + "_" + getIdNum();
    this.x = x;
    this.y = y;
    this.pictureUrl = pictureUrl;
    if (option) {
        this.visible = option.visible;
        this.alpha = option.alpha;
        this.width = option.width;
        this.height = option.height;
        this.xoffset = option.xoffset;
        this.yoffset = option.yoffset;
        this.angle = option.angle;
        this.data = option.data;
    }

    this.listenerObjList = new Array();
    this.listenerObj = new Object();
    this.addEventListener = function (type, listener, param) {
        if (type == "mouseClick") {
            this.listenerObj = { listenerType: type, listenerFun: listener };
            this.listenerObjList.push(this.listenerObj);
            this.listenerParam = { x: x, y: y, data: this.data, target: this, param: param };
        }
        if (type == "mouseOver") {
            this.listenerObj = { listenerType: type, listenerFun: listener };
            this.listenerObjList.push(this.listenerObj);
            this.listenerParam = { x: x, y: y, data: this.data, target: this, param: param };
        }
        if (type == "mouseOut") {
            this.listenerObj = { listenerType: type, listenerFun: listener };
            this.listenerObjList.push(this.listenerObj);
            this.listenerParam = { x: x, y: y, data: this.data, target: this, param: param };
        }
    }
}


/***************************************************************
        
基础实体类
                        
***************************************************************/

//地图点位
function MapPointFun(x, y) {
    this.type = "geometry_mapPoint";
    this.id = this.type + "_" + getIdNum();
    this.x = x;
    this.y = y;
}

//线状几何图形
function PolylineFun(paths) {
    this.type = "geometry_polyline";
    this.id = this.type + "_" + getIdNum();
    this.paths = paths;
}

//面状几何图形
function PolygonFun(rings) {
    this.type = "geometry_polygon";
    this.id = this.type + "_" + getIdNum();
    this.rings = rings;
}

//几何范围
function ExtentFun(xmin, ymin, xmax, ymax) {
    this.type = "geometry_extent";
    this.id = this.type + "_" + getIdNum();
    this.xmin = xmin;
    this.ymin = ymin;
    this.xmax = xmax;
    this.ymax = ymax;
}

//图片点位符号
function PictureMarkerSymbolFun(pictureUrl, option) {
    this.type = "symbol_pictureMarker";
    this.id = this.type + "_" + getIdNum();
    this.pictureUrl = pictureUrl;
    if (option) {
        this.width = option.width;
        this.height = option.height;
        this.xoffset = option.xoffset;
        this.yoffset = option.yoffset;
        this.angle = option.angle;
    }
}

//简单点位符号
function SimpleMarkerSymbolFun(option) {
    this.type = "symbol_simpleMarker";
    this.id = this.type + "_" + getIdNum();

    if (option) {
        this.style = option.style;
        this.size = option.size;
        this.color = option.color;
        this.xoffset = option.xoffset;
        this.yoffset = option.yoffset;
        this.angle = option.angle;
        this.outline = new HMap.SimpleLineSymbol({
            style: option.outLineStyle,
            color: option.outLineColor,
            alpha: option.outLineAlpha,
            width: option.outLinerWidth
        });
    }
}

//气泡符号
function InfoSymbolFun(text, option) {
    this.type = "symbol_info";
    this.id = this.type + "_" + getIdNum();
    this.text = text;
    if (option) {
        this.textFormat = option.textFormat;
        this.borderColor = option.borderColor;
        this.backgroundColor = option.backgroundColor;
        this.placement = option.placement;
    }
}

//简单线状符号
function SimpleLineSymbolFun(option) {
    this.type = "symbol_simpleLine";
    this.id = this.type + "_" + getIdNum();
    if (option) {
        this.style = option.style;
        this.color = option.color;
        this.width = option.width;
        this.alpha = option.alpha;
    }
}

//简单面状符号
function SimpleFillSymbolFun(option) {
    this.type = "symbol_simpleFill";
    this.id = this.type + "_" + getIdNum();
    if (option) {
        this.color = option.color;
        this.alpha = option.alpha;
        this.outline = new HMap.SimpleLineSymbol({
            style: option.outLineStyle,
            color: option.outLineColor,
            alpha: option.outLineAlpha,
            width: option.outLinerWidth
        });
    }
}

//文本符号
function TextSymbolFun(text, option) {
    this.type = "symbol_text";
    this.id = this.type + "_" + getIdNum();
    this.text = text;
    if (option) {
        this.color = option.color;
        this.border = option.border;
        this.borderColor = option.borderColor;
        this.background = option.background;
        this.placement = option.placement;
        this.xoffset = option.xoffset;
        this.yoffset = option.yoffset;
        this.angle = option.angle;
        this.textFormat = new HMap.TextFormat({
            size: option.textFormatSize,
            font: option.textFormatFont,
            bold: option.textFormatBold,
            italic: option.textFormatItalic,
            underline: option.textFormatUnderline,
            align: option.textFormatAlign
        });
    }
}

//文本样式
function TextFormatFun(option) {
    this.type = "textFormat";
    if (option) {
        this.size = option.size;
        this.font = option.font;
        this.color = option.color;
        this.bold = option.bold;
        this.italic = option.italic;
        this.underline = option.underline;
        this.align = option.align;
    }
}

//发光滤镜
function GlowFilterFun(option) {
    this.type = "filter_glow";
    this.id = this.type + "_" + getIdNum();
    if (option) {
        this.color = option.color;
        this.alpha = option.alpha;
        this.blurX = option.blurX;
        this.blurY = option.blurY;
        this.strength = option.strength;
        this.quality = option.quality;
        this.inner = option.inner;
        this.knockout = option.knockout;
    }
}

//投影滤镜
function DropShadowFilterFun(option) {
    this.type = "filter_dropShadow";
    this.id = this.type + "_" + getIdNum();
    if (option) {
        this.distance = option.distance;
        this.angle = option.angle;
        this.color = option.color;
        this.alpha = option.alpha;
        this.blurX = option.blurX;
        this.blurY = option.blurY;
        this.strength = option.strength;
        this.quality = option.quality;
        this.inner = option.inner;
        this.knockout = option.knockout;
    }
}

/***************************************************************
        
可用于 HMap.addControl方法
                        
***************************************************************/

//图片
function ImageFun(imgUrl, ImageOption) {
    this.type = "control_image";
    this.id = this.type + "_" + getIdNum();
    this.imgUrl = imgUrl;
    if (ImageOption) {
        this.visible = ImageOption.visible;
        this.alpha = ImageOption.alpha;
        this.left = ImageOption.left;
        this.right = ImageOption.right;
        this.top = ImageOption.top;
        this.bottom = ImageOption.bottom;
        this.width = ImageOption.width;
        this.height = ImageOption.height;
    }
}


/***************************************************************
        
可用于 InfoWindow 相关方法
                        
***************************************************************/

//属性气泡窗口
function PopInfoWindowFun(title, option) {
    this.type = "infoWindow_pop";
    this.id = this.type + "_" + getIdNum();
    this.title = title;
    if (option) {
        this.fields = option.fields;
        this.links = option.links;
        this.titleTextColor = option.titleTextColor;
        if (systemColorStyle == "Blue" || systemColorStyle == "Blue_S") {
            this.titleBgColor = 0x5CACE1;
            this.titleBottomColor = 0xE7E7E7;
            this.bgUpColor = 0xE7E7E7;
            this.bgBottomColor = 0xABF2F4;
            this.borderColor = 0x0388C4;
        }
        if (systemColorStyle == "Green" || systemColorStyle == "Green_S") {
            this.titleBgColor = 0x24953C;
            this.titleBottomColor = 0xE7E7E7;
            this.bgUpColor = 0xE7E7E7;
            this.bgBottomColor = 0x87F773;
            this.borderColor = 0x24953C;
        }
        if (option.titleBgColor) {
            this.titleBgColor = option.titleBgColor;
        }
        if (option.titleBottomColor) {
            this.titleBottomColor = option.titleBottomColor;
        }
        if (option.bgUpColor) {
            this.bgUpColor = option.bgUpColor;
        }
        if (option.bgBottomColor) {
            this.bgBottomColor = option.bgBottomColor;
        }
        if (option.borderColor) {
            this.borderColor = option.borderColor;
        }
        this.width = option.width;
        this.height = option.height;
        this.infoPlacement = option.infoPlacement;
        this.infoPlacementMode = option.infoPlacementMode;
    }
    this.addEventListener = function (type, listener, param) {
        if (type == "infoWindowClosed") {
            listenerList.infoWindowClosed.push({ id: this.id, type: "infoWindowClosed", callback: listener, param: { param: param} });     //向监听列队中添加值
        }
    }
}
function PopFieldOptionFun(name, value, unit, visible) {
    this.id = "popField_" + getIdNum();
    this.name = name;
    this.value = value;
    this.unit = unit;
    this.visible = visible;
}
function PopLinkOptionFun(name, callBack, data) {
    this.id = "popLink_" + getIdNum();
    this.name = name;
    this.callBack = callBack;
    this.data = data;
    listenerList.callBack.push({ id: this.id, type: "callBack", callback: callBack });     //向监听列队中添加值
}

//页面气泡窗口
function IframeInfoWindowFun(url, width, height) {
    this.type = "infoWindow_iframe";
    this.id = this.type + "_" + getIdNum();
    this.url = url;
    this.width = width;
    this.height = height;
    this.addEventListener = function (type, listener, param) {
        if (type == "infoWindowClosed") {
            listenerList.infoWindowClosed.push({ id: this.id, type: "infoWindowClosed", callback: listener, param: { param: param} });     //向监听列队中添加值
        }
    }
}

/***************************************************************
        
工具中的方法
                        
***************************************************************/

//获取图层地址
function GetLayerUrlFun(dynamicLayerUrl, layerName, onResult, option) {
    this.type = "tool_getLayerUrl";
    this.id = this.type + "_" + getIdNum();
    this.dynamicLayerUrl = dynamicLayerUrl;
    this.layerName = layerName;
    this.onResult = onResult;
    if (option) {
        this.onFault = option.onFault;
    }
    listenerList.onResult.push({ id: this.id, type: "onResult", callback: onResult });     //向监听列队中添加值
    listenerList.onFault.push({ id: this.id, type: "onFault", callback: onFault });     //向监听列队中添加值
}

//查询
function SearchFun(url, where, onResult, option) {
    this.type = "tool_search";
    this.id = this.type + "_" + getIdNum();
    this.url = url;
    this.where = where;
    this.onResult = onResult;
    if (option) {
        this.onFault = option.onFault;
        if (option.geometry) {
            if (option.geometry.type.split("_")[0] == "graphic") {
                this.geometry = getGraphicObj(option.geometry, "");
            }
            else {
                this.geometry = option.geometry;
            }
        }
        this.outFields = option.outFields;
        this.useAMF = option.useAMF;
        this.returnGeometry = option.returnGeometry;
        this.spatialRelationship = option.spatialRelationship;
    }
    listenerList.onResult.push({ id: this.id, type: "onResult", callback: onResult });     //向监听列队中添加值
    listenerList.onFault.push({ id: this.id, type: "onFault", callback: onFault });     //向监听列队中添加值
}

//按图层名称查询
function SearchByNameFun(dynamicLayerUrl, layerName, where, onResult, option) {
    this.type = "tool_searchByName";
    this.id = this.type + "_" + getIdNum();
    this.dynamicLayerUrl = dynamicLayerUrl;
    this.layerName = layerName;
    this.where = where;
    this.onResult = onResult;
    if (option) {
        this.onFault = option.onFault;
        if (option.geometry) {
            if (option.geometry.type.split("_")[0] == "graphic") {
                this.geometry = getGraphicObj(option.geometry, "");
            }
            else {
                this.geometry = option.geometry;
            }
        }
        this.outFields = option.outFields;
        this.useAMF = option.useAMF;
        this.returnGeometry = option.returnGeometry;
        this.spatialRelationship = option.spatialRelationship;
    }
    listenerList.onResult.push({ id: this.id, type: "onResult", callback: onResult });     //向监听列队中添加值
    listenerList.onFault.push({ id: this.id, type: "onFault", callback: onFault });     //向监听列队中添加值
}

//缓冲
function BufferFun(geomtryServerUrl, geometrys, distances, onResult, option) {
    this.type = "tool_buffer";
    this.id = this.type + "_" + getIdNum();
    this.geomtryServerUrl = geomtryServerUrl;
    var geometryArr = new Array();
    for (var i = 0; i < geometrys.length;i++ ) {
        if (geometrys[i].type.split("_")[0] == "graphic") {
            geometryArr.push(getGraphicObj(geometrys[i], ""));
        }
        else {
            geometryArr.push(geometrys[i]);
        }
    }
    this.geometrys = geometryArr;
    this.distances = distances;
    this.onResult = onResult;
    if (option) {
        this.onFault = option.onFault;
        this.unit = option.unit;
    }
    listenerList.onResult.push({ id: this.id, type: "onResult", callback: onResult });     //向监听列队中添加值
    listenerList.onFault.push({ id: this.id, type: "onFault", callback: onFault });     //向监听列队中添加值
}

//叠加对比
function RelationFun(geomtryServerUrl, searchGeometrys, geometryList, onResult, option) {
    this.type = "tool_relation";
    this.id = this.type + "_" + getIdNum();
    this.geomtryServerUrl = geomtryServerUrl;
//    this.searchGeometrys = searchGeometrys;
//    this.geometryList = geometryList;

    var searchgeometryArr = new Array();
    for (var i = 0; i < searchGeometrys.length; i++) {
        if (searchGeometrys[i].type.split("_")[0] == "graphic") {
            searchgeometryArr.push(getGraphicObj(searchGeometrys[i], ""));
        }
        else {
            searchgeometryArr.push(searchGeometrys[i]);
        }
    }
    this.searchGeometrys = searchgeometryArr;

    var geometryArr = new Array();
    for (var j = 0; j < geometryList.length; j++) {
        if (geometryList[j].type.split("_")[0] == "graphic") {
            geometryArr.push(getGraphicObj(geometryList[j], ""));
        }
        else {
            geometryArr.push(geometryList[j]);
        }
    }
    this.geometryList = geometryArr;
    this.onResult = onResult;
    if (option) {
        this.onFault = option.onFault;
        this.spatialRelationship = option.spatialRelationship;
    }
    listenerList.onResult.push({ id: this.id, type: "onResult", callback: onResult });     //向监听列队中添加值
    listenerList.onFault.push({ id: this.id, type: "onFault", callback: onFault });     //向监听列队中添加值
}

//距离测量
function MeasureDistanceFun(pointsArr, onResult, option) {
    this.type = "measureDistance";
    this.id = this.type + "_" + getIdNum();
    this.pointsArr = pointsArr;
    this.onResult = onResult;
    if (option) {
        this.geometryServerUrl = option.geometryServerUrl;
        this.lengthUnit = option.lengthUnit;
        this.wkid = option.wkid;
        this.onFault = option.onFault;
    }
    listenerList.onResult.push({ id: this.id, type: "onResult", callback: this.onResult });     //向监听列队中添加值
    listenerList.onFault.push({ id: this.id, type: "onFault", callback: this.onFault });     //向监听列队中添加值
}

//长度测量
function MeasureLengthFun(lines, onResult, option) {
    this.type = "measureLength";
    this.id = this.type + "_" + getIdNum();
    this.lines = lines;
    this.onResult = onResult;
    if (option) {
        this.geometryServerUrl = option.geometryServerUrl;
        this.lengthUnit = option.lengthUnit;
        this.wkid = option.wkid;
        this.onFault = option.onFault;
    }
    listenerList.onResult.push({ id: this.id, type: "onResult", callback: this.onResult });     //向监听列队中添加值
    listenerList.onFault.push({ id: this.id, type: "onFault", callback: this.onFault });     //向监听列队中添加值
}

//面积测量
function MeasureAreaFun(polygons, onResult, option) {
    this.type = "measureArea";
    this.id = this.type + "_" + getIdNum();
    this.polygons = polygons;
    this.onResult = onResult;
    if (option) {
        this.geometryServerUrl = option.geometryServerUrl;
        this.areaUnit = option.areaUnit;
        this.lengthUnit = option.lengthUnit;
        this.wkid = option.wkid;
        this.onFault = option.onFault;
    }
    listenerList.onResult.push({ id: this.id, type: "onResult", callback: this.onResult });     //向监听列队中添加值
    listenerList.onFault.push({ id: this.id, type: "onFault", callback: this.onFault });     //向监听列队中添加值
}

/***************************************************************
        
绘制图形中的方法
                        
***************************************************************/

//绘制图片符号点状图形
function DrawPicturePointFun(pictureUrl, number, drawCompleted, option) {
    this.type = "draw_PicturePoint";
    this.id = this.type + "_" + getIdNum();
    this.symbol = new HMap.PictureMarkerSymbol(pictureUrl, option);
    this.number = number;
    this.drawCompleted = drawCompleted;
    if (option) {
        this.showTime = option.showTime;
    }
    listenerList.drawCompleted.push({ id: this.id, type: "drawCompleted", callback: drawCompleted });     //向监听列队中添加值
}

//绘制简单符号点状图形
function DrawSimplePointFun(number, drawCompleted, option) {
    this.type = "draw_SimplePoint";
    this.id = this.type + "_" + getIdNum();
    this.number = number;
    this.drawCompleted = drawCompleted;
    this.symbol = new HMap.SimpleMarkerSymbol(option);
    if (option) {
        this.showTime = option.showTime;
    }
    listenerList.drawCompleted.push({ id: this.id, type: "drawCompleted", callback: drawCompleted });     //向监听列队中添加值
}

//绘制线状图形
function DrawPolylineFun(number, drawCompleted, option) {
    this.type = "draw_Polyline";
    this.id = this.type + "_" + getIdNum();
    this.number = number;
    this.drawCompleted = drawCompleted;
    this.symbol = new HMap.SimpleLineSymbol(option);
    if (option) {
        this.showTime = option.showTime;
    }
    listenerList.drawCompleted.push({ id: this.id, type: "drawCompleted", callback: drawCompleted });     //向监听列队中添加值
}

//绘制面状图形
function DrawPolygonFun(number, drawCompleted, option) {
    this.type = "draw_Polygon";
    this.id = this.type + "_" + getIdNum();
    this.number = number;
    this.drawCompleted = drawCompleted;
    this.symbol = new HMap.SimpleFillSymbol(option);
    if (option) {
        this.showTime = option.showTime;
    }
    listenerList.drawCompleted.push({ id: this.id, type: "drawCompleted", callback: drawCompleted });     //向监听列队中添加值
}

//绘制范围图形
function DrawExtentFun(number, drawCompleted, option) {
    this.type = "draw_Extent";
    this.id = this.type + "_" + getIdNum();
    this.number = number;
    this.drawCompleted = drawCompleted;
    this.symbol = new HMap.SimpleFillSymbol(option);
    if (option) {
        this.showTime = option.showTime;
    }
    listenerList.drawCompleted.push({ id: this.id, type: "drawCompleted", callback: drawCompleted });     //向监听列队中添加值
}

//绘制气象符号点状图形
function DrawMeteorPointFun(pictureUrl, number, drawCompleted, option) {
    this.type = "draw_MeteorPoint";
    this.id = this.type + "_" + getIdNum();
    this.symbol = new HMap.PictureMarkerSymbol(pictureUrl, option);
    this.number = number;
    this.drawCompleted = drawCompleted;
    if (option) {
        this.showTime = option.showTime;
        this.data = option.data;
    }
    listenerList.drawCompleted.push({ id: this.id, type: "drawCompleted", callback: drawCompleted });     //向监听列队中添加值
}

/***************************************************************
        
添加移除图表的方法
                        
***************************************************************/

//地图图表
function MapChartFun(x, y, dataSource, chartSeries, option) {
    this.type = "MapChart";
    this.id = this.type + "_" + getIdNum();
    this.dataSource = dataSource;
    this.mapPoint = new HMap.MapPoint(x, y);
    this.chartSeries = chartSeries;
    if (option) {
        this.visible = option.visible;
        this.alpha = option.alpha;
        this.title = option.title;
        this.nameField = option.nameField;
        this.initialWidth = option.initialWidth;
        this.initialHeight = option.initialHeight;
        this.maxScale = option.maxScale;
        this.minScale = option.minScale;
        this.isShowBg = option.isShowBg;
        this.V_xAxisVisible = option.V_xAxisVisible;
        this.V_yAxisVisible = option.V_yAxisVisible;
        this.V_lineAxisVisible = option.V_lineAxisVisible;
    }

    this.addEventListener = function (type, listener, param) {
        if (type == "mouseClick") {
            this.listenerParam = { id: this.id, param: param };
            listenerList.mouseClick.push({ id: this.id, type: type, callback: listener, param: this.listenerParam });      //向监听列队中添加值
        }
    }

}

//柱状图
function ColumnChartSeriesFun(valueField, option) {
    this.type = "columnchart";
    this.id = this.type + "_" + getIdNum();
    this.valueField = valueField;
    if (option) {
        this.name = option.name;
        this.color = option.color;
    }
}

//线状图
function LineChartSeriesFun(valueField, option) {
    this.type = "linechart";
    this.id = this.type + "_" + getIdNum();
    this.valueField = valueField;
    if (option) {
        this.name = option.name;
        this.color = option.color;
    }
}

//饼状图
function PieChartSeriesFun(valueField, option) {
    this.type = "piechart";
    this.id = this.type + "_" + getIdNum();
    this.valueField = valueField;
    if (option) {
        this.name = option.name;
        this.colors = option.colors;
    }
}

/***************************************************************
        
模块中的方法
                        
***************************************************************/
//模块样式设置
function WidgetParamFun(option) {
    if (option) {
        this.widgetIcon = option.widgetIcon;
        this.widgetTitle = option.widgetTitle;
        this.width = option.width;
        this.height = option.height;

        this.top = option.top;
        this.bottom = option.bottom;
        this.left = option.left;
        this.right = option.right;

        this.backgroundColor = option.backgroundColor;
        this.widgetAlpha = option.widgetAlpha;
    }
}

//测量
function MeasureWidgetFun(option) {
    this.type = "MeasureWidget";
    this.id = this.type + "_" + getIdNum();
    this.isWidget = true;
    this.widgetId = "MeasureWidget";
    this.loadInterface = "MeasureWidget_Loaded";
    var widgetParam = new Object();
    this.widgetParam = widgetParam;
    if (option) {
        widgetParam.widgetParam = option.widgetParam;
        this.widgetClosed = option.widgetClosed;
    }
    listenerList.widgetClosed.push({ id: this.widgetId, type: "widgetClosed", callback: this.widgetClosed });     //监听模块关闭事件
}

//编辑
function EditWidgetFun(option) {
    this.type = "EditWidget";
    this.id = this.type + "_" + getIdNum();
    this.isWidget = true;
    this.widgetId = "EditWidget";
    this.loadInterface = "EditWidget_Loaded";
    var widgetParam = new Object();
    this.widgetParam = widgetParam;
    if (option) {
        widgetParam.widgetParam = option.widgetParam;
        this.widgetClosed = option.widgetClosed;
    }
    listenerList.widgetClosed.push({ id: this.widgetId, type: "widgetClosed", callback: this.widgetClosed });     //监听模块关闭事件
    //模块测试方法1
    this.widgetfunc1 = function (paramStr) {
        allWidgetQueue[this.widgetId].push({ type: "widget_t1", param: paramStr });
        doWidgetQueue(this.widgetId);
    }
}

//导航模块
function NavigationFun(xmin, ymin, xmax, ymax, option) {
    this.type = "NavigationWidget";
    this.id = this.type + "_" + getIdNum();
    this.isWidget = true;
    this.widgetId = "NavigationWidget";
    this.loadInterface = "AddNavigation";
    var navigation = new Object();
    this.widgetParam = navigation;
    navigation.type = "control_navigation";
    navigation.id = this.id
    navigation.fullexent = new HMap.Extent(xmin, ymin, xmax, ymax);
    if (option) {
        navigation.visible = option.visible;
        navigation.alpha = option.alpha;
        navigation.left = option.left;
        navigation.right = option.right;
        navigation.top = option.top;
        navigation.bottom = option.bottom;
        navigation.backgroudColor = option.backgroudColor;
        navigation.innerBtnColor = option.innerBtnColor;
        navigation.mouseOverColor = option.mouseOverColor;
        navigation.selectionColor = option.selectionColor;
        navigation.isShowButtonBar = option.isShowButtonBar;
    }
}

//鹰眼
function OverViewFun(xmin, ymin, xmax, ymax, option) {
    this.type = "OverViewWidget";
    this.id = this.type + "_" + getIdNum();
    this.isWidget = true;
    this.widgetId = "OverViewWidget";
    this.loadInterface = "AddOverView";
    var overivew = new Object();
    this.widgetParam = overivew;
    overivew.type = "control_overview";
    overivew.id = this.id
    overivew.fullexent = new HMap.Extent(xmin, ymin, xmax, ymax);
    if (option) {
        overivew.maptype = option.maptype;
        overivew.mapurl = option.mapurl;
        overivew.visible = option.visible;
        overivew.alpha = option.alpha;
        overivew.left = option.left;
        overivew.right = option.right;
        overivew.top = option.top;
        overivew.bottom = option.bottom;
        overivew.backgroudColor = option.backgroudColor;
        overivew.innerBtnColor = option.innerBtnColor;
        overivew.mouseOverColor = option.mouseOverColor;
        overivew.selectionColor = option.selectionColor;
        overivew.isShowButtonBar = option.isShowButtonBar;
    }
}

//图例模块
function LegendWidgetFun(option) {
    this.type = "LegendWidget";
    this.id = this.type + "_" + getIdNum();
    this.isWidget = true;
    this.widgetId = "LegendWidget";
    this.loadInterface = "LegendWidget_Loaded";
    var widgetParam = new Object();
    this.widgetParam = widgetParam;
    if (option) {
        widgetParam.visible = option.visible;
        widgetParam.alpha = option.alpha;
        widgetParam.left = option.left;
        widgetParam.right = option.right;
        widgetParam.top = option.top;
        widgetParam.bottom = option.bottom;
        widgetParam.minHeight = option.minHeight;
        widgetParam.maxHeight = option.maxHeight;
        widgetParam.minWidth = option.minWidth;
        widgetParam.maxWidth = option.maxWidth;
        widgetParam.backgroundColor = option.backgroundColor;
    }
    allWidgetQueue[this.widgetId] = new Array();     //初始化该模块的程序执行列队
    //添加
    this.addLegend = function (legend) {
        allWidgetQueue[this.widgetId].push({ type: "addLegend", param: legend });
        doWidgetQueue(this.widgetId);
        return legend;
    }
    //移除
    this.removeLegend = function (legend) {
        allWidgetQueue[this.widgetId].push({ type: "removeLegend", param: legend });
        doWidgetQueue(this.widgetId);
    }
    //清空
    this.clearLegend = function () {
        allWidgetQueue[this.widgetId].push({ type: "clearLegend" });
        doWidgetQueue(this.widgetId);
    }
}

//简单图例
function SimpleLegendFun(name, option) {
    this.type = "control_simplelegend";
    this.id = this.type + "_" + getIdNum();
    this.name = name;
    if (option) {
        this.layerUrl = option.layerUrl;
        this.legendSymbols = option.legendSymbols;
        this.legendLabels = option.legendLabels;
    }
}

//对应图层的图例
function LayerLegendFun(name, layerUrl, option) {
    this.type = "control_layerLegend";
    this.id = this.type + "_" + getIdNum();
    this.name = name;
    this.layerUrl = layerUrl;
    if (option) {
        this.legendSymbols = option.legendSymbols;
        this.legendLabels = option.legendLabels;
    }
}

//对应图层名称图例
function LayerByNameLegendFun(name, dynamicLayerUrl, layerName, option) {
    this.type = "control_layerByNameLegend";
    this.id = this.type + "_" + getIdNum();
    this.name = name;
    this.dynamicLayerUrl = dynamicLayerUrl;
    this.layerName = layerName;
    if (option) {
        this.legendSymbols = option.legendSymbols;
        this.legendLabels = option.legendLabels;
    }
}

//动态标绘
function PlotWidgetFun(option) {
    this.type = "PlotWidget";
    this.id = this.type + "_" + getIdNum();
    this.isWidget = true;
    this.widgetId = "PlotWidget";
    this.loadInterface = "PlotWidget_Loaded";
    var widgetParam = new Object();
    this.widgetParam = widgetParam;
    if (option) {
        widgetParam.widgetParam = option.widgetParam;
        this.widgetClosed = option.widgetClosed;
    }
    listenerList.widgetClosed.push({ id: this.widgetId, type: "widgetClosed", callback: this.widgetClosed });     //监听模块关闭事件
}

//工具条
function ToolBarWidgetFun(toolIcons, toolCallBack, option) {
    this.type = "ToolBarWidget";
    this.id = this.type + "_" + getIdNum();
    this.isWidget = true;
    this.widgetId = "ToolBarWidget";
    this.loadInterface = "ToolBarWidget_Loaded";
    var widgetParam = new Object();
    this.widgetParam = widgetParam;
    widgetParam.toolIcons = toolIcons;
    widgetParam.toolCallBack = toolCallBack;
    if (option) {
        widgetParam.isShowFullScreen = option.isShowFullScreen;
        widgetParam.width = option.width;
        widgetParam.height = option.height;
        widgetParam.iconSize = option.iconSize;

        widgetParam.top = option.top;
        widgetParam.bottom = option.bottom;
        widgetParam.left = option.left;
        widgetParam.right = option.right;

        widgetParam.backgroundColor = option.backgroundColor;
        widgetParam.widgetAlpha = option.widgetAlpha;
    }
    listenerList.toolCallBack.push({ id: this.widgetId, type: "toolCallBack", callback: toolCallBack });     //向监听列队中添加值
}
function ToolIconFun(url, label, name, data) {
    this.type = "ToolIcon";
    this.id = this.type + "_" + getIdNum();
    this.url = url;
    this.label = label;
    this.name = name;
    this.data = data;
}

//路径分析
function RutingAnalysisWidgetFun(GPUrl, option) {
    this.type = "RoutingAnalysisWidget";
    this.id = this.type + "_" + getIdNum();
    this.isWidget = true;
    this.widgetId = "RoutingAnalysisWidget";
    this.loadInterface = "RoutingAnaylsisWidget_Loaded";
    var widgetParam = new Object();
    widgetParam.url = GPUrl;
    this.widgetParam = widgetParam;
    if (option) {
        widgetParam.targetPoint = option.targetPoint;
        widgetParam.startPoints = option.startPoints;
        widgetParam.barrierPoints = option.barrierPoints;
        widgetParam.targetPointLabel = option.targetPointLabel;
        widgetParam.targetPointIconUrl = option.targetPointIconUrl;
        widgetParam.targetIconUrl = option.targetIconUrl;
        widgetParam.startPointLabel = option.startPointLabel;
        widgetParam.startPointIconUrl = option.startPointIconUrl;
        widgetParam.startedPointIconUrl = option.startedPointIconUrl;
        widgetParam.barrierPointLabel = option.barrierPointLabel;
        widgetParam.barrierPointIconUrl = option.barrierPointIconUrl;
        widgetParam.speed = option.speed;
        widgetParam.isAnalyst = option.isAnalyst;
        widgetParam.widgetParam = option.widgetParam;
        this.widgetClosed = option.widgetClosed;
    }
    listenerList.widgetClosed.push({ id: this.widgetId, type: "widgetClosed", callback: this.widgetClosed });     //监听模块关闭事件
}
function RAPointFun(name, x, y) {
    this.name = name;
    this.x = x;
    this.y = y;
}

//气模型
function AirModelWidgetFun(gpServiceUrl, geometryServiceUrl, airlayers, option) {
    this.type = "AirModelWidget";
    this.id = this.type + "_" + getIdNum();
    this.isWidget = true;
    this.widgetId = "AirModelWidget";
    this.loadInterface = "AirModelWidget_Loaded";
    var widgetParam = new Object();
    this.widgetParam = widgetParam;
    widgetParam.gpServiceUrl = gpServiceUrl;
    widgetParam.geometryServiceUrl = geometryServiceUrl;
    widgetParam.airlayers = airlayers;

    if (option) {
        widgetParam.matters = option.matters;
        widgetParam.gpParaName = option.gpParaName;
        widgetParam.vectorResult = option.vectorResult;
        widgetParam.RasterAlpha = option.RasterAlpha;
        widgetParam.infoWindowTitleBgColor = option.infoWindowTitleBgColor;
        widgetParam.airModelInit = option.airModelInit;
        widgetParam.widgetParam = option.widgetParam;
        widgetParam.infoTitleTextColor = option.infoTitleTextColor;
        if (systemColorStyle == "Blue" || systemColorStyle == "Blue_S") {
            widgetParam.infoTitleBgColor = 0x5CACE1;
            widgetParam.infoTitleBottomColor = 0xE7E7E7;
            widgetParam.infoBgUpColor = 0xE7E7E7;
            widgetParam.bgBottomColor = 0xABF2F4;
        }
        if (systemColorStyle == "Green" || systemColorStyle == "Green_S") {
            widgetParam.infoTitleBgColor = 0x24953C;
            widgetParam.infoTitleBottomColor = 0xE7E7E7;
            widgetParam.infoBgUpColor = 0xE7E7E7;
            widgetParam.infoBgBottomColor = 0x87F773;
        }
        if (option.infoTitleBgColor) {
            widgetParam.infoTitleBgColor = option.infoTitleBgColor;
        }
        if (option.infoTitleBottomColor) {
            widgetParam.infoTitleBottomColor = option.infoTitleBottomColor;
        }
        if (option.infoBgUpColor) {
            widgetParam.infoBgUpColor = option.infoBgUpColor;
        }
        if (option.infoBgBottomColor) {
            this.infoBgBottomColor = option.infoBgBottomColor;
        }
        if (option.infoBorderColor) {
            widgetParam.infoBorderColor = option.infoBorderColor;
        }
        this.widgetClosed = option.widgetClosed;
    }
    listenerList.widgetClosed.push({ id: this.widgetId, type: "widgetClosed", callback: this.widgetClosed });     //监听模块关闭事件
}
function AirLayerFun(layerUrl, layerName, symbolURL, titlefield, fields) {
    this.layerUrl = layerUrl;
    this.layerName = layerName;
    this.symbolURL = symbolURL;
    this.titlefield = titlefield;
    this.fields = fields;
}
function AirLayerFieldFun(name, alias) {
    this.name = name;
    this.alias = alias;
}
function AirMatterFun(name, lowLevel, mediumLevel, highLevel) {
    this.name = name;
    this.lowLevel = lowLevel;
    this.mediumLevel = mediumLevel;
    this.highLevel = highLevel;
}
function AirModelInitFun(option) {
    if (option) {
        this.longitude = option.longitude;
        this.latitude = option.latitude;
        this.height = option.height;
        this.fengsu = option.fengsu;
        this.windDirection = option.windDirection;
        this.qiya = option.qiya;
        this.wendu = option.wendu;
        this.polluteType = option.polluteType;
        this.gasStable = option.gasStable;
        this.amountPerSecond = option.amountPerSecond;
        this.minutes = option.minutes;
        this.isAnalyst = option.isAnalyst;
    }
}

//轨迹回放
function TrackPlayWidgetFun(timeIntervar, trackInfos, option) {
    this.type = "TrackPlayWidget";
    this.id = this.type + "_" + getIdNum();
    this.isWidget = true;
    this.widgetId = "TrackPlayWidget";
    this.loadInterface = "TrackPlayWidget_Loaded";
    var widgetParam = new Object();
    this.widgetParam = widgetParam;
    widgetParam.trackInfos = trackInfos;
    widgetParam.timeIntervar = timeIntervar;
    if (option) {
        widgetParam.widgetParam = option.widgetParam;
        this.widgetClosed = option.widgetClosed;
    }
    listenerList.widgetClosed.push({ id: this.widgetId, type: "widgetClosed", callback: this.widgetClosed });     //监听模块关闭事件
}
function TrackInfoFun(option) {
    this.carID = option.carID;
    this.carName = option.carName;
    this.trackPointInfos = option.trackPointInfos;
    this.trackLineSymbol = option.trackLineSymbol;
    this.playLineSymbol = option.playLineSymbol;
    this.trackStartImg = option.trackStartImg;
    this.trackEndImg = option.trackEndImg;
    this.trackMoveImg = option.trackMoveImg;
    this.trackMoveInfo = option.trackMoveInfo;
    this.infoWindow = option.infoWindow;
    this.isShowInfoWindow = option.isShowInfoWindow;
}
function TrackPointInfoFun(longitude,latitude,time) {
    this.longitude = longitude;
    this.latitude = latitude;
    this.time = time;
}

//实时轨迹
function RealTrackWidgetFun(realTrackInfos) {
    this.type = "RealTrackWidget";
    this.id = this.type + "_" + getIdNum();
    this.isWidget = true;
    this.widgetId = "RealTrackWidget";
    this.loadInterface = "RealTrackWidget_Loaded";
    var widgetParam = new Object();
    this.widgetParam = widgetParam;
    widgetParam.realTrackInfos = realTrackInfos;
    
    allWidgetQueue[this.widgetId] = new Array();     //初始化该模块的程序执行列队
    //获取实时点位
    this.getLocation = function (realPointInfo) {
        allWidgetQueue[this.widgetId].push({ type: "getTrackLocation", param: realPointInfo });
        doWidgetQueue(this.widgetId);
    }
    //清除轨迹结果
    this.clearTrackResult = function (carID) {
        allWidgetQueue[this.widgetId].push({ type: "clearTrackResult", param: carID });
        doWidgetQueue(this.widgetId);
    }
}
function RealTrackInfoFun(option) {
    this.carID = option.carID;
    this.carName = option.carName;
    this.trackLineSymbol = option.trackLineSymbol;
    this.trackMoveImg = option.trackMoveImg;
    this.trackMoveInfo = option.trackMoveInfo;
    this.isShowTrackMoveInfo = option.isShowTrackMoveInfo;
    this.isTrackDisplay = option.isTrackDisplay;
}
function RealPointInfoFun(carID, longitude, latitude, time, speed, data) {
    this.carID = carID;
    this.longitude = longitude;
    this.latitude = latitude;
    this.time = time;
    this.speed = speed;
    this.data = data;
}

//综合查询
function QueryWidgetFun(searchLayers, geomtryServerUrl,option) {
    this.type = "QueryWidget";
    this.id = this.type + "_" + getIdNum();
    this.isWidget = true;
    this.widgetId = "QueryWidget";
    this.loadInterface = "QueryWidget_Loaded";
    var widgetParam = new Object();
    this.widgetParam = widgetParam;
    widgetParam.searchLayers = searchLayers;
    widgetParam.geomtryServerUrl = geomtryServerUrl;

    if (option) {
        widgetParam.widgetParam = option.widgetParam;
        widgetParam.bufferRadius = option.bufferRadius;
        widgetParam.queryGraAlpha = option.queryGraAlpha;
        this.widgetClosed = option.widgetClosed;
    }
    listenerList.widgetClosed.push({ id: this.widgetId, type: "widgetClosed", callback: this.widgetClosed });     //监听模块关闭事件
}
function SearchLayerFun(layerUrl, layerLabel, layerIcon, nameField, queryInfos) {
    this.layerUrl = layerUrl;
    this.layerLabel = layerLabel;
    this.layerIcon = layerIcon;
    this.nameField = nameField;
    this.queryInfos = queryInfos;
}
function QueryInfoFun(label, fieldName, fieldUnit, visible) {
    this.label = label;
    this.fieldName = fieldName;
    this.fieldUnit = fieldUnit;
    this.visible = visible;
}

//空气质量区域渲染,IDW模型
function IDWModelWidgetFun(renderDataParam, idwModeParam) {
    this.type = "IDWModelWidget";
    this.id = this.type + "_" + getIdNum();
    this.isWidget = true;
    this.widgetId = "IDWModelWidget";
    this.loadInterface = "IDWModelWidget_Loaded";
    var widgetParam = new Object();
    this.widgetParam = widgetParam;
    widgetParam.renderDataParam = renderDataParam;
    widgetParam.idwModeParam = idwModeParam;

    allWidgetQueue[this.widgetId] = new Array();     //初始化该模块的程序执行列队
    //获取实时点位
    this.IDWModeAnalysis = function (renderDataParam, idwModeParam) {
        allWidgetQueue[this.widgetId].push({ type: "IDWModeAnalysis", param: { renderDataParam: renderDataParam, idwModeParam: renderDataParam} });
        doWidgetQueue(this.widgetId);
    }

}
function RenderDataParamFun(option) {
    this.layerId = option.layerId;
    this.xField = option.xField;
    this.yField = option.yField;
    this.valueField = option.valueField;
    this.alpha = option.alpha;
}
function IDWModeParamFun(option) {
    this.gpUrl = option.gpUrl;
    this.inputPointsName = option.inputPointsName;
    this.zvalue = option.zvalue;
    this.outputDZXName = option.outputDZXName;
    this.outputDZMName = option.outputDZMName;
    this.Contourinterval = option.Contourinterval;
    this.ContourintervalParamName = option.ContourintervalParamName;
}

//图片播放
function PlayPicWidgetFun(pictures, option) {
    this.type = "PlayPicWidget";
    this.id = this.type + "_" + getIdNum();
    this.isWidget = true;
    this.widgetId = "PlayPicWidget";
    this.loadInterface = "PlayPicWidget_Loaded";
    var widgetParam = new Object();
    this.widgetParam = widgetParam;
    widgetParam.pictures = pictures;

    if (option) {
        widgetParam.timeUnit = option.timeUnit;
        widgetParam.isReplay = option.isReplay;
        widgetParam.widgetParam = option.widgetParam;
        this.widgetClosed = option.widgetClosed;
        widgetParam.widgetWisible = option.widgetWisible;
    }
    allWidgetQueue[this.widgetId] = new Array();     //初始化该模块的程序执行列队
    //播放
    this.play = function () {
        allWidgetQueue[this.widgetId].push({ type: "PlayPicWidget_Play" });
        doWidgetQueue(this.widgetId);
    }
    //暂停
    this.pause = function () {
        allWidgetQueue[this.widgetId].push({ type: "PlayPicWidget_Pause" });
        doWidgetQueue(this.widgetId);
    }
    //上一张
    this.previous = function () {
        allWidgetQueue[this.widgetId].push({ type: "PlayPicWidget_Previous" });
        doWidgetQueue(this.widgetId);
    }
    //下一张
    this.next = function () {
        allWidgetQueue[this.widgetId].push({ type: "PlayPicWidget_Next"});
        doWidgetQueue(this.widgetId);
    }
    //停止
    this.stop = function () {
        allWidgetQueue[this.widgetId].push({ type: "PlayPicWidget_Stop" });
        doWidgetQueue(this.widgetId);
    }
    //停止
    this.closeAndclear = function () {
        allWidgetQueue[this.widgetId].push({ type: "PlayPicWidget_Clear" });
        doWidgetQueue(this.widgetId);
    }

    listenerList.widgetClosed.push({ id: this.widgetId, type: "widgetClosed", callback: this.widgetClosed });     //监听模块关闭事件
}
function PlayPicFun(option) {
    this.picId = option.picId;
    this.picUrl = option.picUrl;
    this.picInfo = option.picInfo;
    this.fadeInTime = option.fadeInTime;
    this.fadeOutTime = option.fadeOutTime;
    this.pauseTime = option.pauseTime;
    this.alphaMax = option.alphaMax;
    this.alphaMin = option.alphaMin;
    this.picExtent = option.picExtent;
}
