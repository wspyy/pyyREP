
var webMap; //flex地图主体
var isSystemLoad = false;   //系统是否加载完成
var isLicenseTrue = false;  //许可是否验证完成
var isMapLoad = false;  //地图是否加载完成
var proQueue = new Array();     //存放程序执行列队
var allWidgetQueue = {};   //存放模块程序执行列队      
var listenerArray = new Array();    //存放所有的监听事件
var listenerList = new Object();
var idNum = 0;      //记录最新id编码

//记录模块是否已经加载
var widgetIsLoad = {
    widgetInterface1: false,
    MeasureWidget: false,
    EditWidget: false,
    MapChartWidget: false,
    NavigationWidget: false
}
Array.prototype.indexOf = function (val) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == val) return i;
    }
    return -1;
};
Array.prototype.remove = function (val) {
    var index = this.indexOf(val);
    if (index > -1) {
        this.splice(index, 1);
    }
};

function HMap(flashContent, license) {

    HMap.BUFFER_METER = 9001; //米
    HMap.BUFFER_KILOMETER = 9036; //千米
    HMap.METER = "esriMeters";
    HMap.KILOMETER = "esriKilometers";
    HMap.SQUARE_METERS = "esriSquareMeters";
    HMap.SQUARE_KILOMETERS = "esriSquareKilometers";


    loadSystem(flashContent); //加载flash文件
    proQueue.push({ type: "verifyLicense", param: license });   //将许可验证加入程序列队

    listenerList.mouseClick = new Array();
    listenerList.mouseOver = new Array();
    listenerList.mouseOut = new Array();
    listenerList.onResult = new Array();
    listenerList.onFault = new Array();
    listenerList.drawCompleted = new Array();
    listenerList.callBack = new Array();
    listenerList.toolCallBack = new Array();
    listenerList.infoWindowClosed = new Array();
    listenerList.widgetClosed = new Array();
    listenerList.Load = new Array();
    

    /***************************************************************
    
    HMap 中的方法

    ***************************************************************/

    //地图配置
    this.setMapOption = setMapOptionFun;

    //添加底图
    this.addBaseMap = addBaseMapFun;

    //切换底图
    this.switchBaseMap = switchBaseMapFun;

    //添加控件
    this.addControl = addControlFun;

    //移除控件
    this.removeControl = removeControlFun

    //添加专题图
    this.addSpecialLayer = addSpecialLayerFun;

    //移除专题图
    this.removeSpecialLayer = removeSpecialLayerFun;

    //添加图形
    this.addGraphic = addGraphicFun;

    //编辑图形
    this.editGraphic = editGraphicFun;

    //移除地图图形
    this.removeGraphic = removeGraphicFun;

    //添加工具
    this.useTool = useToolFun;

    //地图平移居中
    this.centerAt = centerAtFun;

    //按等级缩放居中
    this.centerAndLevel = centerAndLevelFun;

    //按比例缩放居中
    this.centerAndScale = centerAndScaleFun;

    //地图指定范围显示
    this.extentTo = extentToFun;

    //设置地图操作
    this.setMapOperation = setMapOperationFun;

    //地图清空
    this.clear = clearFun;

    //获取地图中心点
    this.getCenter = getCenterFun;

    //获取地图当前的缩放等级
    this.getLevel = getLevelFun;

    //获取地图缩放等级的最大值
    this.getMaxLevel = getMaxLevelFun;

    //获取地图的当前比例尺
    this.getScale = getScaleFun;

    //获取地图当前显示范围
    this.getExtent = getExtentFun;

    //HMap添加监听事件
    this.addEventListener = addEventListenerFun;

    //添加气泡窗口
    this.showInfoWindow = showInfoWindowFun;

    //影藏气泡窗口
    this.hideInfoWindow = hideInfoWindowFun;

    //绘制图形
    this.addDraw = addDrawFun;

    //移除绘制图形
    this.removeDrawResult = removeDrawResultFun;

    //添加图表
    this.addMapChart = addMapChartFun;

    //移除图表
    this.removeMapChart = removeMapChartFun;

    /***************************************************************
    
    可用于 HMap.addSpecialLayer() 方法加载的图层
     
    ***************************************************************/

    //切图地图服务图层
    HMap.TiledLayer = TiledLayerFun;

    //动态地图服务图层
    HMap.DynamicLayer = DynamicLayerFun;

    //要素图层
    HMap.FeatureLayer = FeatureLayerFun;

    //名称要素图层
    HMap.FeatureLayerByName = FeatureLayerByNameFun;

    //天地图图层
    HMap.TDTLayer = TDTLayerFun;


    /***************************************************************
        
    可用于 HMap.addGraphic方法加载的图形
                        
    ***************************************************************/

    //图片符号点位图形
    HMap.PictureGraphic = PictureGraphicFun;

    //气泡信息符号点位图形
    HMap.InfoGraphic = InfoGraphicFun;

    //简单点位图形
    HMap.SimpleGraphic = SimpleGraphicFun

    // 文本点位图形
    HMap.TextGraphic = TextGraphicFun;

    //线状图形形
    HMap.PolylineGraphic = PolylineGraphicFun;

    //面状多边形
    HMap.PolygonGraphic = PolygonGraphicFun;

    //范围图形
    HMap.ExtentGraphic = ExtentGraphicFun;

    /***************************************************************
        
    基础实体类
                        
    ***************************************************************/

    //地图点位
    HMap.MapPoint = MapPointFun;

    //线状几何图形
    HMap.Polyline = PolylineFun;

    //面状几何图形
    HMap.Polygon = PolygonFun;

    //几何范围
    HMap.Extent = ExtentFun;

    //图片点位符号
    HMap.PictureMarkerSymbol = PictureMarkerSymbolFun;

    //简单点位符号
    HMap.SimpleMarkerSymbol = SimpleMarkerSymbolFun;

    //气泡符号
    HMap.InfoSymbol = InfoSymbolFun;

    //简单线状符号
    HMap.SimpleLineSymbol = SimpleLineSymbolFun;

    //简单面状符号
    HMap.SimpleFillSymbol = SimpleFillSymbolFun;

    //文本符号
    HMap.TextSymbol = TextSymbolFun;

    //文本样式
    HMap.TextFormat = TextFormatFun;

    //发光滤镜
    HMap.GlowFilter = GlowFilterFun;

    //投影滤镜
    HMap.DropShadowFilter = DropShadowFilterFun;

    /***************************************************************
        
    可用于 HMap.addControl方法
                        
    ***************************************************************/

    //图片
    HMap.Image = ImageFun;

   
    /***************************************************************
        
    可用于 InfoWindow 相关方法
                        
    ***************************************************************/

    //属性气泡窗口
    HMap.PopInfoWindow = PopInfoWindowFun;
    HMap.PopFieldOption = PopFieldOptionFun;
    HMap.PopLinkOption = PopLinkOptionFun;

    //页面气泡窗口
    HMap.IframeInfoWindow = IframeInfoWindowFun;

    /***************************************************************
        
    工具中的方法
                        
    ***************************************************************/

    //获取图层地址
    HMap.GetLayerUrl = GetLayerUrlFun;

    //查询
    HMap.Search = SearchFun;

    //按图层名称查询
    HMap.SearchByName = SearchByNameFun;

    //缓冲
    HMap.Buffer = BufferFun;

    //叠加对比
    HMap.Relation = RelationFun;

    //距离测量
    HMap.MeasureDistance = MeasureDistanceFun;

    //长度测量
    HMap.MeasureLength = MeasureLengthFun;

    //面积测量
    HMap.MeasureArea = MeasureAreaFun;

    /***************************************************************
        
    绘制图形中的方法
                        
    ***************************************************************/

    //绘制图片符号点状图形
    HMap.DrawPicturePoint = DrawPicturePointFun;

    //绘制简单符号点状图形
    HMap.DrawSimplePoint = DrawSimplePointFun;

    //绘制线状图形
    HMap.DrawPolyline = DrawPolylineFun;

    //绘制面状图形
    HMap.DrawPolygon = DrawPolygonFun;

    //绘制范围图形
    HMap.DrawExtent = DrawExtentFun;

    /***************************************************************
        
    添加移除图表的方法
                        
    ***************************************************************/

    //地图图表
    HMap.MapChart = MapChartFun;

    //柱状图
    HMap.ColumnChartSeries = ColumnChartSeriesFun;

    //线状图
    HMap.LineChartSeries = LineChartSeriesFun;

    //饼状图
    HMap.PieChartSeries = PieChartSeriesFun;


    /***************************************************************
        
    模块中的方法
                        
    ***************************************************************/

    //测量
    HMap.MeasureWidget = MeasureWidgetFun;

    //编辑
    HMap.EditWidget = EditWidgetFun;

    //导航模块
    HMap.Navigation = NavigationFun;

    //图例模块
    HMap.LegendWidget = LegendWidgetFun;
    //简单图例
    HMap.SimpleLegend = SimpleLegendFun;
    //对应图层的图例
    HMap.LayerLegend = LayerLegendFun;
    //对应图层名称图例
    HMap.LayerByNameLegend = LayerByNameLegendFun;

    //动态标绘
    HMap.PlotWidget = PlotWidgetFun;

    //工具条
    HMap.ToolBarWidget = ToolBarWidgetFun;
    HMap.ToolIcon = ToolIconFun;

    //路径分析
    HMap.RutingAnalysisWidget = RutingAnalysisWidgetFun;
    HMap.RAPoint = RAPointFun;
    HMap.WidgetParam = WidgetParamFun;

    //气模型
    HMap.AirModelWidget = AirModelWidgetFun;
    HMap.AirLayer = AirLayerFun;
    HMap.AirLayerField = AirLayerFieldFun;
    HMap.AirMatter = AirMatterFun;
    HMap.AirModelInit = AirModelInitFun;

    //轨迹回放
    HMap.TrackPlayWidget = TrackPlayWidgetFun;
    HMap.TrackInfo = TrackInfoFun;
    HMap.TrackPointInfo = TrackPointInfoFun;

    //实时轨迹
    HMap.RealTrackWidget = RealTrackWidgetFun;
    HMap.RealTrackInfo = RealTrackInfoFun;
    HMap.RealPointInfo = RealPointInfoFun;

    //综合查询
    HMap.QueryWidget = QueryWidgetFun;
    HMap.SearchLayer = SearchLayerFun;
    HMap.QueryInfo = QueryInfoFun;

    //空气质量区域渲染,IDW模型
    HMap.IDWModelWidget = IDWModelWidgetFun;
    HMap.RenderDataParam = RenderDataParamFun;
    HMap.IDWModeParam = IDWModeParamFun;

    //图片播放
    HMap.PlayPicWidget = PlayPicWidgetFun;
    HMap.PlayPic = PlayPicFun;

    //点缓冲查询
    HMap.PointBufferWidget = PointBufferWidgetFun

    //气象符号点位图形
    HMap.MeteorGraphic = MeteorGraphicFun;

    //空气质量符号点位图形
    HMap.AirGraphic = AirGraphicFun;;
   

    //一维水模型
    HMap.YWSModeWidget = YWSModeWidgetFun;
    HMap.RiverInfoParam = RiverInfoParamFun;
    HMap.EventInfoParam = EventInfoParamFun;
    HMap.QskInfoParam = QskInfoParamFun;
}
//---- 此上为 HMap 方法 -----------------------------------------------------
//---------------------------------------------------------------------------
//---------------------------------------------------------------------------


//获取最新id编码
function getIdNum() {
    return (idNum++).toString();
}

//执行监听事件
function doListenerArray(eventId, eventType, eventParam) {
    var eventParams = new Object();
    for (paramObject in eventParam) {
        eventParams[paramObject] = eventParam[paramObject];
    }
    for (var i = 0; i < listenerList[eventType].length; i++) {
        var listener = listenerList[eventType][i];
        if (listener.id == eventId && listener.type == eventType) {
            for (paramObj in listener.param){
                eventParams[paramObj] = listener.param[paramObj];
            }
            listener.callback(eventParams);
        }
    }
}

//执行程序列队
function doProQueue() {
    if (isSystemLoad) {    //判断系统是否已经加载完成
        if (proQueue.length) {
            var contentParm = proQueue[0];      //取出需要执行的程序参数
            webMap[contentParm.type](contentParm.param);    //执行程序
            proQueue.shift();    //把执行过的程序移除
            doProQueue();    //执行下一个程序
        }
    }
}

//执行模块程序列队
function doWidgetQueue(widgetId) {
    if (widgetIsLoad[widgetId]) {    //判断模块是否加载完成
        var widgetQueue = allWidgetQueue[widgetId];
        if (widgetQueue.length) {
            var proParam = widgetQueue[0];  //取出需要执行的模块程序参数
            webMap[proParam.type](proParam.param);    //执行模块程序
            widgetQueue.shift();    //把执行过的程序移除
            doWidgetQueue(widgetId);    //执行下一个程序
        }
    }
}

//单个模块加载完成
//由Flex调用
function widgetLoad(widgetId) {
    widgetIsLoad[widgetId] = true;
    doWidgetQueue(widgetId);
}


//公共事件函数
//由Flex调用
function eventBus(eventId, eventType, eventParam) {
    doListenerArray(eventId, eventType, eventParam);
}


//监听地图加载完成
//由Flex调用
function map_load() {
    isMapLoad = true;
    doListenerArray("HMap", "Load", isMapLoad);
}

//监听许可验证完成
//由Flex调用
function license_Success() {
    isLicenseTrue = true;
}

//监听接口调用完成
//由Flex调用
function system_complete() {
    isSystemLoad = true;
    webMap = swfobject.getObjectById("WebMap")
    doProQueue();    //执行程序列队
}

//加载系统文件
function loadSystem(flashContent) {
    var swfVersionStr = "11.1.0";
    var xiSwfUrlStr = "playerProductInstall.swf";
    var flashvars = {};
    var params = {};
    params.quality = "high";
    params.bgcolor = "#ffffff";
    params.allowscriptaccess = "always";
    params.allowfullscreen = "true";
    params.wmode = "opaque";
    var attributes = {};
    attributes.id = "WebMap";
    attributes.name = "WebMap";
    attributes.align = "middle";
    swfobject.embedSWF(
        sysUrl+"/WebMap.swf", flashContent,
        "100%", "100%",
        swfVersionStr, xiSwfUrlStr,
        flashvars, params, attributes);

    swfobject.createCSS("#flashContent", "display:block;text-align:left;");

}