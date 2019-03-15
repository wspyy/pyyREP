
/***************************************************************
    
HMap 中的方法

***************************************************************/
var systemColorStyle;
//地图配置
function setMapOptionFun(mapOption) {
    if (mapOption.colorStyle == "Blue") {
        systemColorStyle = "Blue";
        //字体颜色、背景色（深）、背景色（浅）、鼠标移入颜色、鼠标选中颜色、鼠标移入字体颜色、鼠标选中字体颜色
        mapOption.colors = [0x000000, 0x1497D6, 0x1497D6, 0x2E7B64, 0x188713, 0xffffff, 0xffffff];
    }
    else if (mapOption.colorStyle == "Green") {
        systemColorStyle = "Green";
        mapOption.colors = [0x000000, 0x24953C, 0x24953C, 0x87F773, 0x039003, 0xffffff, 0xF4D919];
    }
    else if (mapOption.colorStyle == "Blue_S") {
        systemColorStyle = "Blue_S";
        //字体颜色、背景色（深）、背景色（浅）、鼠标移入颜色、鼠标选中颜色、鼠标移入字体颜色、鼠标选中字体颜色
        mapOption.colors = [0x000000, 0x1497D6, 0x85D9F0, 0x2E7B64, 0x188713, 0xffffff, 0xffffff];
    }
    else if (mapOption.colorStyle == "Green_S") {
        systemColorStyle = "Green_S";
        mapOption.colors = [0x000000, 0x24953C, 0x87F773, 0x75B907, 0x039003, 0xffffff, 0xF4D919];
    }
    else {
        systemColorStyle = "Blue";
        mapOption.colors = [0x000000, 0x1497D6, 0x85D9F0, 0x2E7B64, 0x188713, 0xffffff, 0xffffff];
    }
    proQueue.push({ type: "setMapOption", param: { option: mapOption} });
    doProQueue();
}

//添加底图
function addBaseMapFun(layers, baseMapOption) {
    proQueue.push({ type: "addBaseMap", param: { layers: layers, option: baseMapOption} });
    doProQueue();
}

//切换底图
function switchBaseMapFun(layers) {
    var layerIds = new Array();
    for (var i = 0; i < layers.length; i++) {
        layerIds.push(layers[i].id);
    }
    proQueue.push({ type: "switchBaseMap", param: { layerIds: layerIds} });
    doProQueue();
}

//添加控件
function addControlFun(control) {
    if (control.isWidget) {
        if (!widgetIsLoad[control.widgetId]) {            //判断模块是否加载
            proQueue.push({ type: "addWidget", param: control.widgetId });
            allWidgetQueue[control.widgetId] = new Array();     //初始化该模块的程序执行列队
            allWidgetQueue[control.widgetId].push({ type: control.loadInterface, param: control }); //模块加载完成
            doWidgetQueue(control.widgetId);
        }
        else {
            allWidgetQueue[control.widgetId].push({ type: control.loadInterface, param: control }); //模块加载完成
            doWidgetQueue(control.widgetId);
        }
    }
    else {
        proQueue.push({ type: "addControl", param: control });
    }
    doProQueue();
    return control;
}

//移除控件
function removeControlFun(control) {
    proQueue.push({ type: "removeControl", param: control });
    doProQueue();
}

//添加专题图
function addSpecialLayerFun(layer) {
    var speLayerId = "specialLayer" + getIdNum();
    proQueue.push({ type: "addSpecialLayer", param: { speLayerId: speLayerId, speLayer: layer} });
    doProQueue();
    return speLayerId;
}

//移除专题图
function removeSpecialLayerFun(layerId) {
    proQueue.push({ type: "removeLayer", param: layerId });
    doProQueue();
}

//添加图形
function addGraphicFun(graphics,option) {
    var graLayerId = "graphicsLayer" + getIdNum();
    for (var i = 0; i < graphics.length; i++) {
        graphics[i] = getGraphicObj(graphics[i], graLayerId);
    }
    proQueue.push({ type: "addGraphic", param: { layerId: graLayerId, graphicList: graphics, addGraParam: option} });
    doProQueue();
    return graLayerId;
}

//编辑图形
function editGraphicFun(graphic) {
    graphic.isEdit = true;
    graphic = getGraphicObj(graphic, graphic.graLayerId);
    proQueue.push({ type: "editGraphic", param: { graParam: graphic} });
    doProQueue();
}

//移除地图图形
function removeGraphicFun(graphicsId) {
    proQueue.push({ type: "removeLayer", param: graphicsId });
    doProQueue();
}

//地图平移居中
function centerAtFun(x, y) {
    proQueue.push({ type: "centerAt", param: { x: x, y: y} });
    doProQueue();
}

//按等级缩放居中
function centerAndLevelFun(x, y, level) {
    proQueue.push({ type: "centerAndLevel", param: { x: x, y: y, level: level} });
    doProQueue();
}

//按比例缩放居中
function centerAndScaleFun(x, y, scale) {
    proQueue.push({ type: "centerAndScale", param: { x: x, y: y, scale: scale} });
    doProQueue();
}

//地图指定范围显示
function extentToFun(xmin, ymin, xmax, ymax) {
    proQueue.push({ type: "extentTo", param: { xmin: xmin, ymin: ymin, xmax: xmax, ymax: ymax} });
    doProQueue();
}

//设置地图操作
function setMapOperationFun(type) {
    proQueue.push({ type: "setMapOperation", param: type });
    doProQueue();
}

//地图清空
function clearFun(unClearIDs) {
    proQueue.push({ type: "clear", param: unClearIDs });
    doProQueue();
}

//获取地图中心点
function getCenterFun() {
    if (isMapLoad) {
        return webMap.getCenter();
    }
}

//获取地图当前的缩放等级
function getLevelFun() {
    if (isMapLoad) {
        return webMap.getLevel();
    }
}

//获取地图缩放等级的最大值
function getMaxLevelFun() {
    if (isMapLoad) {
        return webMap.getMaxLevel();
    }
}

//获取地图的当前比例尺
function getScaleFun() {
    if (isMapLoad) {
        return webMap.getScale();
    }
}

//获取地图当前显示范围
function getExtentFun() {
    if (isMapLoad) {
        return webMap.getExtent();
    }
}

//HMap添加监听事件 
function addEventListenerFun(type, listener) {
    listenerList.Load.push({ id: "HMap", type: type, callback: listener });     //向监听列队中添加值
}

//添加气泡窗口
function showInfoWindowFun(x, y, infoWindow) {
    proQueue.push({ type: "showInfoWindow", param: { x: x, y: y, infoWindow: infoWindow} });
    doProQueue();
}
//影藏气泡窗口
function hideInfoWindowFun() {
    proQueue.push({ type: "hideInfoWindow" });
    doProQueue();
}

//添加工具
function useToolFun(toolParam) {
    proQueue.push({ type: "tool", param: toolParam });
    doProQueue();
}


//绘制图形
function addDrawFun(drawParam) {
    var drawId = "draw" + getIdNum();
    proQueue.push({ type: "draw", param: { id: drawId, drawParam: drawParam} });
    doProQueue();
    return drawId;
}

//移除绘制图形
function removeDrawResultFun(resultId) {
    proQueue.push({ type: "removeLayer", param: resultId });
    doProQueue();
}


//添加图表
function addMapChartFun(mapCharts) {
    var widgetId = "MapChartWidget";
    if (!widgetIsLoad[widgetId]) {            //判断模块是否加载
        proQueue.push({ type: "addWidget", param: widgetId });
        allWidgetQueue[widgetId] = new Array();     //初始化该模块的程序执行列队
        doProQueue();
    }
    //添加图表方法
    var chartsLayerId = "chartsLayer" + getIdNum();
    allWidgetQueue[widgetId].push({ type: "AddMapChart", param: { layerId: chartsLayerId, mapChartList: mapCharts} });
    doWidgetQueue(widgetId);

    return chartsLayerId;
}

//移除图表
function removeMapChartFun(mapChartId) {
    proQueue.push({ type: "removeLayer", param: mapChartId });
    doProQueue();
}


/////////////////////////////////////////////////

//对定义的Graphic重新整理
function getGraphicObj(graphicParam, graLayerId) {

    var graphicObj = new Object();

    if (graphicParam.type == "graphic_picture") {
        graphicObj.type = "graphic_picture";
        graphicObj.id = graphicParam.id;
        graphicObj.mapPoint = new HMap.MapPoint(graphicParam.x, graphicParam.y);
        graphicObj.symbol = new HMap.PictureMarkerSymbol(graphicParam.pictureUrl, {
            width: graphicParam.width,
            height: graphicParam.height,
            xoffset: graphicParam.xoffset,
            yoffset: graphicParam.yoffset,
            angle: graphicParam.angle
        });
        graphicObj.visible = graphicParam.visible;
        graphicObj.alpha = graphicParam.alpha;
        graphicObj.data = graphicParam.data;
        graphicObj.mouseClickEvent = graphicParam.mouseClickEvent;
        graphicObj.mouseOverEvent = graphicParam.mouseOverEvent;
        graphicObj.mouseOutEvent = graphicParam.mouseOutEvent;
    }
    else if (graphicParam.type == "graphic_info") {
        graphicObj.type = "graphic_info";
        graphicObj.id = graphicParam.id;
        graphicObj.mapPoint = new HMap.MapPoint(graphicParam.x, graphicParam.y);
        graphicObj.symbol = new HMap.InfoSymbol(graphicParam.text, {
            textFormat: graphicParam.textFormat,
            backgroundColor: graphicParam.backgroundColor,
            borderColor: graphicParam.borderColor,
            placement: graphicParam.placement
        });
        graphicObj.visible = graphicParam.visible;
        graphicObj.alpha = graphicParam.alpha;
        graphicObj.filters = graphicParam.filters;
        graphicObj.displayLevel = graphicParam.displayLevel;
        graphicObj.displayScale = graphicParam.displayScale;
        graphicObj.isMouseOver = graphicParam.isMouseOver;
        graphicObj.mouseOverAlpha = graphicParam.mouseOverAlpha;
        graphicObj.mouseOverFilters = graphicParam.mouseOverFilters;
        graphicObj.mouseOverTextFormat = graphicParam.mouseOverTextFormat;
        graphicObj.mouseOverBorderColor = graphicParam.mouseOverBorderColor;
        graphicObj.mouseOverBackgroundColor = graphicParam.mouseOverBackgroundColor;
        graphicObj.data = graphicParam.data;
        graphicObj.mouseClickEvent = graphicParam.mouseClickEvent;
        graphicObj.mouseOverEvent = graphicParam.mouseOverEvent;
        graphicObj.mouseOutEvent = graphicParam.mouseOutEvent;
    }
    else if (graphicParam.type == "graphic_simple") {
        graphicObj.type = "graphic_simple";
        graphicObj.id = graphicParam.id;
        graphicObj.mapPoint = new HMap.MapPoint(graphicParam.x, graphicParam.y);
        graphicObj.symbol = new HMap.SimpleMarkerSymbol({
            style: graphicParam.style,
            size: graphicParam.size,
            color: graphicParam.color,
            xoffset: graphicParam.xoffset,
            yoffset: graphicParam.yoffset,
            angle: graphicParam.angle,
            outLineStyle: graphicParam.outLineStyle,
            outLineColor: graphicParam.outLineColor,
            outLineAlpha: graphicParam.outLineAlpha,
            outLinerWidth: graphicParam.outLinerWidth
        });
        graphicObj.visible = graphicParam.visible;
        graphicObj.alpha = graphicParam.alpha;
        graphicObj.data = graphicParam.data;
        graphicObj.mouseClickEvent = graphicParam.mouseClickEvent;
        graphicObj.mouseOverEvent = graphicParam.mouseOverEvent;
        graphicObj.mouseOutEvent = graphicParam.mouseOutEvent;
    }
    else if (graphicParam.type == "graphic_text") {
        graphicObj.type = "graphic_text";
        graphicObj.id = graphicParam.id;
        graphicObj.mapPoint = new HMap.MapPoint(graphicParam.x, graphicParam.y);
        graphicObj.symbol = new HMap.TextSymbol(graphicParam.text, {
            color: graphicParam.color,
            border: graphicParam.border,
            borderColor: graphicParam.borderColor,
            background: graphicParam.background,
            placement: graphicParam.placement,
            xoffset: graphicParam.xoffset,
            yoffset: graphicParam.yoffset,
            angle: graphicParam.angle,
            textFormatSize: graphicParam.textFormatSize,
            textFormatFont: graphicParam.textFormatFont,
            textFormatBold: graphicParam.textFormatBold,
            textFormatItalic: graphicParam.textFormatItalic,
            textFormatUnderline: graphicParam.textFormatUnderline,
            textFormatAlign: graphicParam.textFormatAlign
        });
        graphicObj.visible = graphicParam.visible;
        graphicObj.alpha = graphicParam.alpha;
        graphicObj.data = graphicParam.data;
        graphicObj.mouseClickEvent = graphicParam.mouseClickEvent;
        graphicObj.mouseOverEvent = graphicParam.mouseOverEvent;
        graphicObj.mouseOutEvent = graphicParam.mouseOutEvent;
    }
    else if (graphicParam.type == "graphic_polyline") {
        graphicObj.type = "graphic_polyline";
        graphicObj.id = graphicParam.id;
        graphicObj.polyline = new HMap.Polyline(graphicParam.mapPoints);
        graphicObj.symbol = new HMap.SimpleLineSymbol({
            style: graphicParam.style,
            color: graphicParam.color,
            width: graphicParam.width,
            alpha: graphicParam.alpha
        });
        graphicObj.visible = graphicParam.visible;
        graphicObj.data = graphicParam.data;
        graphicObj.mouseClickEvent = graphicParam.mouseClickEvent;
        graphicObj.mouseOverEvent = graphicParam.mouseOverEvent;
        graphicObj.mouseOutEvent = graphicParam.mouseOutEvent;
    }
    else if (graphicParam.type == "graphic_polygon") {
        graphicObj.type = "graphic_polygon";
        graphicObj.id = graphicParam.id;
        graphicObj.polygon = new HMap.Polygon(graphicParam.mapPoints);
        graphicObj.symbol = new HMap.SimpleFillSymbol({
            color: graphicParam.fillColor,
            alpha: graphicParam.fillAlpha,
            outLineStyle: graphicParam.outLineStyle,
            outLineColor: graphicParam.outLineColor,
            outLineAlpha: graphicParam.outLineAlpha,
            outLinerWidth: graphicParam.outLinerWidth
        });
        graphicObj.visible = graphicParam.visible;
        graphicObj.alpha = graphicParam.alpha;
        graphicObj.data = graphicParam.data;
        graphicObj.mouseClickEvent = graphicParam.mouseClickEvent;
        graphicObj.mouseOverEvent = graphicParam.mouseOverEvent;
        graphicObj.mouseOutEvent = graphicParam.mouseOutEvent;
    }
    else if (graphicParam.type == "graphic_extent") {
        graphicObj.type = "graphic_extent";
        graphicObj.id = graphicParam.id;
        graphicObj.extent = new HMap.Extent(graphicParam.xmin, graphicParam.ymin, graphicParam.xmax, graphicParam.ymax);
        graphicObj.symbol = new HMap.SimpleFillSymbol({
            color: graphicParam.fillColor,
            alpha: graphicParam.fillAlpha,
            outLineStyle: graphicParam.outLineStyle,
            outLineColor: graphicParam.outLineColor,
            outLineAlpha: graphicParam.outLineAlpha,
            outLinerWidth: graphicParam.outLinerWidth
        });
        graphicObj.visible = graphicParam.visible;
        graphicObj.alpha = graphicParam.alpha;
        graphicObj.data = graphicParam.data;
        graphicObj.mouseClickEvent = graphicParam.mouseClickEvent;
        graphicObj.mouseOverEvent = graphicParam.mouseOverEvent;
        graphicObj.mouseOutEvent = graphicParam.mouseOutEvent;
    }
    else if (graphicParam.type == "graphic_meteor") {
        graphicObj.type = "graphic_meteor";
        graphicObj.id = graphicParam.id;
        graphicObj.mapPoint = new HMap.MapPoint(graphicParam.x, graphicParam.y);
        graphicObj.symbol = new HMap.PictureMarkerSymbol(graphicParam.pictureUrl, {
            width: graphicParam.width,
            height: graphicParam.height,
            xoffset: graphicParam.xoffset,
            yoffset: graphicParam.yoffset,
            angle: graphicParam.angle
        });
        graphicObj.visible = graphicParam.visible;
        graphicObj.alpha = graphicParam.alpha;
        graphicObj.data = graphicParam.data;
        graphicObj.mouseClickEvent = graphicParam.mouseClickEvent;
        graphicObj.mouseOverEvent = graphicParam.mouseOverEvent;
        graphicObj.mouseOutEvent = graphicParam.mouseOutEvent;
    }
    else if (graphicParam.type == "graphic_api") {
        graphicObj.type = "graphic_api";
        graphicObj.id = graphicParam.id;
        graphicObj.mapPoint = new HMap.MapPoint(graphicParam.x, graphicParam.y);
        graphicObj.symbol = new HMap.PictureMarkerSymbol(graphicParam.pictureUrl, {
            width: graphicParam.width,
            height: graphicParam.height,
            xoffset: graphicParam.xoffset,
            yoffset: graphicParam.yoffset,
            angle: graphicParam.angle
        });
        graphicObj.visible = graphicParam.visible;
        graphicObj.alpha = graphicParam.alpha;
        graphicObj.data = graphicParam.data;
        graphicObj.mouseClickEvent = graphicParam.mouseClickEvent;
        graphicObj.mouseOverEvent = graphicParam.mouseOverEvent;
        graphicObj.mouseOutEvent = graphicParam.mouseOutEvent;
    }
    graphicObj.graLayerId = graLayerId;
    graphicParam.graLayerId = graLayerId;

    //对Graphic添加所属图层Id并添加到监听
    if (graphicParam.listenerObjList) {
        for (var i = 0; i < graphicParam.listenerObjList.length; i++) {
            var listenerType = graphicParam.listenerObjList[i].listenerType;
            var listenerFun = graphicParam.listenerObjList[i].listenerFun;
            if (listenerType == "mouseClick") {
                graphicObj.mouseClickEvent = true;
//                graphicObj.graLayerId = graLayerId;
//                graphicParam.listenerParam.target.graLayerId = graLayerId;

                if (graphicParam.isEdit) {
                    for (var i = 0; i < listenerList.mouseClick.length; i++) {
                        if (listenerList.mouseClick[i].id == graphicObj.id) {
                            listenerList.mouseClick[i] = { id: graphicObj.id, type: listenerType, callback: listenerFun, param: graphicParam.listenerParam }; //更新监听
                        }
                    }
                }
                else {
                    listenerList.mouseClick.push({ id: graphicObj.id, type: listenerType, callback: listenerFun, param: graphicParam.listenerParam });      //向监听列队中添加值
                }
            }
            if (listenerType == "mouseOver") {
                graphicObj.mouseOverEvent = true;
//                graphicObj.graLayerId = graLayerId;
//                graphicParam.listenerParam.target.graLayerId = graLayerId;

                if (graphicParam.isEdit) {
                    for (var i = 0; i < listenerList.mouseOver.length; i++) {
                        if (listenerList.mouseOver[i].id == graphicObj.id) {
                            listenerList.mouseOver[i] = { id: graphicObj.id, type: listenerType, callback: listenerFun, param: graphicParam.listenerParam }; //更新监听
                        }
                    }
                }
                else {
                    listenerList.mouseOver.push({ id: graphicObj.id, type: listenerType, callback: listenerFun, param: graphicParam.listenerParam });      //向监听列队中添加值
                }
            }
            if (listenerType == "mouseOut") {
                graphicObj.mouseOutEvent = true;
//                graphicObj.graLayerId = graLayerId;
//                graphicParam.listenerParam.target.graLayerId = graLayerId;

                if (graphicParam.isEdit) {
                    for (var i = 0; i < listenerList.mouseOut.length; i++) {
                        if (listenerList.mouseOut[i].id == graphicObj.id) {
                            listenerList.mouseOut[i] = { id: graphicObj.id, type: listenerType, callback: listenerFun, param: graphicParam.listenerParam }; //更新监听
                        }
                    }
                }
                else {
                    listenerList.mouseOut.push({ id: graphicObj.id, type: listenerType, callback: listenerFun, param: graphicParam.listenerParam });      //向监听列队中添加值
                }
            }
        }
    }
    
    return graphicObj;
}

