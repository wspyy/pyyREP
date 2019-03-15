/// <reference path="../MoniData/Moni-js/ChinaAir.js" />
//程序代码 
document.write(" <script language=\"javascript\" src=\"../MoniData/Moni-js/ChinaAir.js\" > <\/script>");
//外网
//var geometryServerUrl = "http://211.99.132.166:8080/ArcGIS/rest/services/Geometry/GeometryServer";//几何服务地址
//var mapServerUrl = "http://211.99.132.166:8080/ArcGIS/rest/services/FuLing/FLBaseMap/MapServer";//地图地址
//内网
var geometryServerUrl = "http://server.arcgisonline.com/arcgis/rest/services/ESRI_Imagery_World_2D/MapServer";//几何服务地址
var mapServerUrl = "http://59.108.92.220:8080/ArcGIS/rest/services/horseMap/chinaMap_terrain/MapServer";//地图地址
//var mapServerUrl = "http://server.arcgisonline.com/ArcGIS/rest/services/ESRI_Imagery_World_2D/MapServer";//地图地址
//var mapServerUrl = "http://map.geoq.cn/ArcGIS/rest/services/ChinaOnlineCommunity/MapServer";//地图地址

//页面加载时创建地图
$(function () {
    creatHMap();
});

var hmap;//地图对象
var baseLayer;
var imgLayer;
var data;
//创建地图对象
function creatHMap() {
    var host = window.location.hostname;
    var license = "";
    if (host == "localhost") license = "2pZ9e1qhM2Q1LjE1MDo4NTg1LDIxMS45OS4xMzIuMTYyOjg1ODUsbG9jYWxob3N0LDIwMTQtMi0yNix0aW1lMg==";
    else if (host == "10.126.73.237") license = "DjuNlkMH9YAzLjIzNyxpcDIsaXAzLDIwMTQtMS0xLHRpbWUy";
    else if (host == "10.126.72.237") license = "DjuNlkMH9YAyLjIzNyxpcDIsaXAzLDIwMTQtMS0xLHRpbWUy";
    else if (host == "10.1.5.51") license = "iZDfxs6cGzgxLGlwMixpcDMsMjAxNC0xLTEsdGltZTI=";
    else if (host == "172.18.98.192") license = "8RxOOAy5hWw4LjE5MixpcDIsaXAzLDIwMTQtMS0xLHRpbWUy";
    else if (host == "10.0.8.2") license = "qyyf+Y3kYVcsaXAyLGlwMywyMDE0LTEtMSx0aW1lMg==";
    else if (host == "192.168.4.69") license = "2pZ9e1qhM2Q0LjY5Ojg1MjYsbG9jYWxob3N0LHl1dHUyMjcsMjAxNC0xLTEsdGltZTI=";
    else if (host == "192.168.4.67") license = "2pZ9e1qhM2Q0LjY3LHl1dHUxODMsaXAzLDIwMTQtMS0xLHRpbWUy";
    else if (host == "172.16.0.21") license = "7+mgC5fTGTguMjEsaXAyLGlwMywyMDE0LTEtMSx0aW1lMg==";
    //初始化地图
    hmap = new HMap("flashContent", license);
    baseLayer = new HMap.TiledLayer(mapServerUrl);
    //imgLayer = new HMap.TiledLayer(ImgmapServerUrl);
    //添加底图
    hmap.addBaseMap([baseLayer]);//切圖
    //hmap.addBaseMap([new HMap.DynamicLayer(mapServerUrl)]);//矢量圖
    //设置地图显示范围
    hmap.extentTo(71.3589943496297, 16.7574780802653, 138.169212649724, 57.9474199191781);
    //增加工具条
    //addToolBar();
    hmap.setMapOption({ ZoomSliderVisible: false });
    hmap.addControl(new HMap.Navigation(71.3589943496297, 16.7574780802653, 138.169212649724, 57.9474199191781, { visible: true, alpha: 0.8, left: 10, top: 10, isShowButtonBar: false, backgroudColor: "0xEBEBEB", mouseOverColor: "0x608D52", selectionColor: "0xfbd860", innerBtnColor: "0x3A8EBE" }));
}

function switchBaseMap() {
    hmap.switchBaseMap([baseLayer]);
}
function switchImgMap() {
    hmap.switchBaseMap([imgLayer]);
}

//添加工具条
function addToolBar() {

    //放大
    var zoomIn = new HMap.ToolIcon("../Img/MapImg/zoomin.png", "放大", "zoomIn", null);
    //缩小
    var zoomOut = new HMap.ToolIcon("../Img/MapImg/zoomout.png", "缩小", "zoomOut", null);
    //平移
    var pan = new HMap.ToolIcon("../Img/MapImg/i_pan.png", "平移", "pan", null);
    //测量
    var rule = new HMap.ToolIcon("../Img/MapImg/测量.png", "测量", "MeasureWidget", null);
    //标绘
    var remark = new HMap.ToolIcon("../Img/MapImg/动态标绘.png", "标绘", "PlotWidget", null);
    //清空
    var clear = new HMap.ToolIcon("../Img/MapImg/i_clear.png", "清空", "ClearMap", null);

    var toolBarWidget = new HMap.ToolBarWidget([zoomIn, zoomOut, pan, rule, remark, clear], toolCallBack, { isShowFullScreen: false, iconSize: 30, top: 2, right: 5, backgroundColor: 0x1497D6, widgetAlpha: 0.7 });

   hmap.addControl(toolBarWidget);

}

//点击图标时相应函数
function toolCallBack(e) {

    if (e.name == "MeasureWidget") {

        var widgetParam = new HMap.WidgetParam({ top: 100, right: 100, backgroundColor: 0x1497D6, widgetAlpha: 0.7 });

        var measureWidget = new HMap.MeasureWidget({ widgetParam: widgetParam });

        hmap.addControl(measureWidget);

    } else if (e.name == "PlotWidget") {

        var widgetParam = new HMap.WidgetParam({ top: 100, right: 100, backgroundColor: 0x1497D6, widgetAlpha: 0.7 });

        var plotWidget = new HMap.PlotWidget({ widgetParam: widgetParam });

        hmap.addControl(plotWidget);

    } else if (e.name == "ClearMap") {

        ClearMap();

    } else if (e.name == "zoomIn") {

        mapZoom("zoomIn");

    } else if (e.name == "zoomOut") {

        mapZoom("zoomOut");
    } else if (e.name == "pan") {
        mapZoom("pan");
    }
}

//清空图层
function ClearMap() {

    hmap.clear();
}

//放大、缩小或平移zoomIn:放大 zoomOut：缩小 pan：平移 
function mapZoom(zoomType) {
    hmap.setMapOperation(zoomType);
}
function hideInfoWindow() {
    hmap.hideInfoWindow();
}
//添加简单点位图形
function addSimpGraphics(objData) {
    ClearMap();
    var simpGraArr = new Array();

    var glowFilter = new HMap.GlowFilter({ color: 0x000000, blurX: 5, blurY: 5 });
    var dropShadowFilter = new HMap.DropShadowFilter({ color: 0x000000, distance: 5, alpha: 0.8 });
    var textFormat = new HMap.TextFormat({ color: 0xffffff, italic: false, font: "宋体", bold: true, size: 12 });

    var infoGraArr = new Array();
    if (objData && objData.length > 0) {
        for (var i = 0; i < objData.length; i++) {
            var china_text = objData[i].CityName + "：" + AirLevelAQI(objData[i].AQI) +"("+  objData[i].AQI + ")";
            var infoGra = new HMap.InfoGraphic(objData[i].lon, objData[i].lat, china_text, {
                displayLevel: 7 ,
                placement: "top",
                textFromat: textFormat,
                borderColor: PointAQIDisplay(objData[i].AQI).toString(),
                backgroundColor: PointAQIDisplay(objData[i].AQI).toString(),
                isMouseOver: true,
                mouseOverBorderColor: 0xffffff,
                mouseOverBackgroundColor: PointAQIDisplay(objData[i].AQI).toString(),
                filters: [glowFilter, dropShadowFilter]
            });
           
            //infoGra.addEventListener("mouseClick", callbackFun);//去掉点击事件
            infoGraArr.push(infoGra);
            var simpGra = new HMap.SimpleGraphic(objData[i].lon, objData[i].lat, { data: { ID: objData[i].ID }, style: "disc", size: 12, color: PointAQIDisplay(objData[i].AQI) });
            //simpGra.addEventListener("mouseClick", callbackFun);//去掉点击事件
            //hmap.hideInfoWindow();
            simpGra.addEventListener("mouseOver", onPicPointGraMouseOver, infoGra);
            simpGra.addEventListener("mouseOut", onPicPointGraMouseOut, infoGra);
            simpGraArr.push(simpGra);
        }
    }
    hmap.addGraphic(simpGraArr, { isAutoZoom: true });
    hmap.addGraphic(infoGraArr, { isAutoZoom: true });
}



//添加图片符号点位图形
function addPicGraphics(objData, flag) {
    ClearMap();
    var glowFilter = new HMap.GlowFilter({ color: 0x000000, blurX: 5, blurY: 5 });

    var dropShadowFilter = new HMap.DropShadowFilter({ color: 0x000000, distance: 5, alpha: 0.8 });

    var textFormat = new HMap.TextFormat({ color: 0xffffff, italic: false, font: "宋体", bold: true, size: 12 });

    var infoGraArr = new Array();

    var picGraArr = new Array();
    if (objData && objData.length > 0) {
        data = objData;
        for (var i = 0; i < objData.length; i++) {
            var infoGra = new HMap.InfoGraphic(objData[i].lon, objData[i].lat, objData[i].stationName, { data: { F: flag, ID: objData[i].StationCode }, displayLevel: 1, placement: "top", borderColor: 0x2F80E3, backgroundColor: 0x2F80E3, textFromat: textFormat, filters: [glowFilter, dropShadowFilter], mouseOverBorderColor: 0xF97F1C, mouseOverBackgroundColor: 0xF97F1C, isMouseOver: true });
            //infoGra.addEventListener("mouseClick", callbackFun);
            infoGraArr.push(infoGra);
            var picGra = new HMap.PictureGraphic(objData[i].lon, objData[i].lat, "../img/MapImg/emergency.gif", { data: { F: flag, ID: objData[i].StationCode } });
            //picGra.addEventListener("mouseClick", callbackFun);
            picGra.addEventListener("mouseOver", onPicPointGraMouseOver, infoGra);
            picGra.addEventListener("mouseOut", onPicPointGraMouseOut, infoGra);
            picGraArr.push(picGra);
        }
    }
    hmap.addGraphic(picGraArr,{ isAutoZoom: true });
    //hmap.addGraphic(infoGraArr, { isAutoZoom: true });
}

//添加天气符号图形
var WeatherGraArr = new Array();
function addWeatherGraphics(objData, flag) {
    ClearMap();
    var glowFilter = new HMap.GlowFilter({ color: 0x000000, blurX: 5, blurY: 5 });
    var dropShadowFilter = new HMap.DropShadowFilter({ color: 0x000000, distance: 5, alpha: 0.8 });
    
    if (objData && objData.length > 0) {
        for (var i = 0; i < objData.length; i++) {
            var picGra = new HMap.PictureGraphic(objData[i].lon, objData[i].lat,
                "../img/Weather/day/" + objData[i].WeatherCode + ".png",
                { data: { F: flag, ID: objData[i].StationCode } });
            picGra.width = 50;
            picGra.height = 50;
            //picGra.addEventListener("mouseClick", callbackFun);
            //picGra.addEventListener("mouseOver", onPicPointGraMouseOver, infoGra);
            //picGra.addEventListener("mouseOut", onPicPointGraMouseOut, infoGra);
            WeatherGraArr.push(picGra);
        }
    }
    //hmap.addGraphic(infoGraArr, { isAutoZoom: true });
    debugger;
    var layerMapServerUrl = "http://192.168.4.87/ArcGIS/rest/services/shuozhou/ShuoZhouBaseMap/MapServer/213"    
    toolSearch = new HMap.Search(layerMapServerUrl, "code like '1406%'", onResult, { onFault: onFault });
    hmap.useTool(toolSearch);

}

function onResult(e) {
    debugger;
    drawResult = null;
    var result = e.result;
    var graphicArr = new Array();
    for (var i = 0; i < result.length; i++) {
        var graphic;
        if (result[i].Geometry.type == "geometry_mapPoint") {
            graphic = new HMap.SimpleGraphic(result[i].Geometry.x, result[i].Geometry.y, { data: result[i].Attributes });
        }
        if (result[i].Geometry.type == "geometry_polyline") {
            graphic = new HMap.PolylineGraphic(result[i].Geometry.paths, { data: result[i].Attributes });
        }
        if (result[i].Geometry.type == "geometry_polygon") {
            graphic = new HMap.PolygonGraphic(result[i].Geometry.rings, {
                data: result[i].Attributes,
                fillColor: 0x0A689F,
                outLineColor: 0xffffff
            });
        }
        if (result[i].Geometry.type == "geometry_extent") {
            graphic = new HMap.ExtentGraphic(result[i].Geometry.xmin, result[i].Geometry.ymin, result[i].Geometry.xmax, result[i].Geometry.ymax, { data: result[i].Attributes });
        }
        graphicArr.push(graphic);
    }
    var graLayerId = hmap.addGraphic(graphicArr);
    hmap.addGraphic(WeatherGraArr, {
        isAutoZoom: true,
        isHighLight: true
    });
}

function onFault(e) {

    alert(e.error);

}
//添加图片符号点位图形
function addMeteorGraphics(objData, flag) {
    ClearMap();
    var infoGraArr = new Array();
    if (objData && objData.length > 0) {
        for (var i = 0; i < objData.length; i++) {
            //var infoGra = new HMap.InfoGraphic(objData[i].lon, objData[i].lat, objData[i].stationName, { data: { F: flag, ID: objData[i].StationCode }, displayLevel: 1, placement: "top", borderColor: 0x2F80E3, backgroundColor: 0x2F80E3, textFromat: textFormat, filters: [glowFilter, dropShadowFilter], mouseOverBorderColor: 0xF97F1C, mouseOverBackgroundColor: 0xF97F1C, isMouseOver: true });
            var infoGra = new HMap.MeteorGraphic(objData[i].lon, objData[i].lat, "", {
                data: {
                    F: flag,
                    staName: objData[i].StationName,
                    //AirTemperature: parseInt(objData[i].TimeMinTemp),
                    AirTemperature: objData[i].temNow,
                    //Humidity: objData[i].RelHumidity,
                    Humidity: objData[i].humidity,
                    AirPress: "--",
                    //Rainfall: objData[i].PrecipitationAmount,
                    //WindDirection: objData[i].MaxWindD,
                    WindDirection: objData[i].WindDirectionCenter,
                    WindSpeed: windSpeed(objData[i].windPower),
                   // WindSpeed: objData[i].WindVelocity10 / 3.6,
                    imgurl: sysUrl + "/",
                    //displayLevel: 2,
                    backgroundAlpha: 0.4,
                    borderAlpha:0.5
                },
            });

            //infoGra.addEventListener("mouseClick", callbackFun);
            infoGraArr.push(infoGra);
        }
    }
    hmap.addGraphic(infoGraArr, { isAutoZoom: true });
}

//添加图片符号点位图形
function addAirGraphics(objData, flag, callbackedFun) {
    ClearMap();
    var infoGraArr = new Array();

    if (objData && objData.length > 0) {
        for (var i = 0; i < objData.length; i++) {
            //var infoGra = new HMap.InfoGraphic(objData[i].lon, objData[i].lat, objData[i].stationName, { data: { F: flag, ID: objData[i].StationCode }, displayLevel: 1, placement: "top", borderColor: 0x2F80E3, backgroundColor: 0x2F80E3, textFromat: textFormat, filters: [glowFilter, dropShadowFilter], mouseOverBorderColor: 0xF97F1C, mouseOverBackgroundColor: 0xF97F1C, isMouseOver: true });
            var infoGra = new HMap.AirGraphic(objData[i].lon, objData[i].lat, "", {
                data: {
                    F: flag,
                    name: objData[i].StationName,
                    code: objData[i].StationCode,
                    value: objData[i].AQI,
                    borcol: 0x00ff00,
                    backcol: 0xffffff
                },
            });

            infoGra.addEventListener("mouseClick", callbackedFun, objData[i].StationCode);
            infoGraArr.push(infoGra);
        }
    }
    hmap.addGraphic(infoGraArr, { isAutoZoom: true });
}
//添加图片符号点位图形
function addPollGraphics(objData, flag, callbackedFun,high) {
    var displaylevel = 7;
    if (!high) {
        ClearMap();
        displaylevel = 15;
    }
    var glowFilter = new HMap.GlowFilter({ color: 0x000000, blurX: 5, blurY: 5 });
    var dropShadowFilter = new HMap.DropShadowFilter({ color: 0x000000, distance: 5, alpha: 0.8 });
    var textFormat = new HMap.TextFormat({ color: 0xffffff, size: 16, font: "楷体", italic: false, bold: false });
    var infoGraArr = new Array();
    var picGraArr = new Array();
    if (objData && objData.length > 0) {
        for (var i = 0; i < objData.length; i++) {
            var infoGra = new HMap.InfoGraphic(objData[i].lon, objData[i].lat, objData[i].EnterpriseName,
             {
                 displayLevel: displaylevel,
                 data: "hello!",
                 placement: "top",
                 textFromat: textFormat,
                 borderColor: 0x2793E6,
                 backgroundColor: 0x2793E6,
                 isMouseOver: true,
                 mouseOverBorderColor: 0xF97F1C,
                 mouseOverBackgroundColor: 0xF97F1C,
                 filters: [glowFilter]
             });
            infoGraArr.push(infoGra);
            if (objData[i].DelFlag == "Main") {
                var picGra = new HMap.PictureGraphic(objData[i].lon, objData[i].lat,
              "../img/MapImg/redPoint.png",
              { data: { F: flag, ID: objData[i].EnterpriseCode } });
                picGra.addEventListener("mouseClick", callbackFun);
                picGra.addEventListener("mouseOver", onPicPointGraMouseOver, infoGra);
                picGra.addEventListener("mouseOut", onPicPointGraMouseOut, infoGra);
                picGraArr.push(picGra);
            } else {
                var picGra = new HMap.PictureGraphic(objData[i].lon, objData[i].lat,
              "../img/MapImg/greenPoint.png",
              { data: { F: flag, ID: objData[i].EnterpriseCode } });
                picGra.addEventListener("mouseClick", callbackFun);
                picGra.addEventListener("mouseOver", onPicPointGraMouseOver, infoGra);
                picGra.addEventListener("mouseOut", onPicPointGraMouseOut, infoGra);
                picGraArr.push(picGra);
            }                  
           
        }
    }
    hmap.addGraphic(picGraArr, { isAutoZoom: true, isHighLight: high });
    hmap.addGraphic(infoGraArr);
}
//鼠标移入事件装题图
function onPicPointGraMouseOver(e) {
    var currentGraphic = e.param;
    if (currentGraphic.displayLevel > hmap.getLevel()) {
        currentGraphic.visible = true;
        hmap.editGraphic(currentGraphic); //编辑图形
    }
}

//鼠标移出事件装题图
function onPicPointGraMouseOut(e) {
    var currentGraphic = e.param;
    if (currentGraphic.displayLevel > hmap.getLevel()) {
        currentGraphic.visible = false;
        hmap.editGraphic(currentGraphic); //编辑图形
    }
}

////链接回调函数
//function callbackFun(e) {
//    onInfoPointGraClick(e);
//    infoWindowClosed(e);
//    onPicPointGraMouseOut(e);
//    if (locationPointId) {
//        alert(locationPointId);
//        hmap.removeGraphic(locationPointId);
//        locationPointId = undefined;
//    }
    //if (e.data.f == 1) {//基础信息
    //    $.mapuni.dialog({ width: 800, title: e.data.epname + "基础信息", url: '../soams_radioactive/radioactivesource/enterdetialframe?pagetype=edit&epid=' + e.data.id });
    //} else {
    //    $.mapuni.dialog({ modal: false, width: 1000, height: 600, title: e.data.epname + "监测数据", url: '../soams_radioactive/videopage/monitdataview?menuid=adb04eb8-c361-4dd1-9f08-a89a0622e779&epid=' + e.data.id });
    //}
    //}
//}

//根据经纬度定位
function centerPoint(x, y) {

    hmap.centerAt(x, y);
}
//风力和风速的转换
function windSpeed(windPower) {
    var power = windPower.substr(0, (windPower.length - 1));
    var speed;
    switch (power) {
        case "0":
            speed = 0.1;
            break;
        case "1":
            speed = 0.9;
            break;
        case "2":
            speed = 2.5;
            break;
        case "3":
            speed = 4.4;
            break;
        case "4":
            speed = 6.7;
            break;
        case "5":
            speed = 9.4;
            break;
        case "6":
            speed = 12.3;
            break;
        case "7":
            speed = 15.5;
            break;
        case "8":
            speed = 19;
            break;
        case "9":
            speed = 22.6;
            break;
        case "10":
            speed = 26.5;
            break;
        case "11":
            speed = 35.5;
            break;
        case "12":
            speed = 38.8;
            break;
    }
    return speed;
}

