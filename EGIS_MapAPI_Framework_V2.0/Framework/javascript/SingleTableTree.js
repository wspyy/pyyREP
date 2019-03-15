/*###########################   定义变量  ###############################*/
var settingList = {
    //			check: {
    //				enable: true
    //			},
    data: {
        simpleData: {
            enable: true
        }
    },
    callback: {
        onClick: onClick
    }
};

//点击 列表树 后重新绑定 模块树结构
function onClick(event, treeId, treeNode, clickFlag) {
    if (treeNode.children == null) {

        var mainUrl = window.location.href;
        var type = mainUrl.split("?")[1];

        var url = mainUrl.replace(/SingleTableTree/, "MainPage");

        parent.SetMainFrame(url + "&" + treeNode.code); 

    }
}


/*###########################   初始化构建树结构  ###############################*/
function bindList() {
    //var query = $("#txtQuery").val();
    var mainUrl = window.location.href;
    var type = mainUrl.split("?")[1];
//    debugger;
    var query = "";

    $.ajax({
        type: "POST",
        url: "../Data/GetTreeData.ashx?param=T_Sys_CommonTree&pId=" + type + "&query=" + escape(query),
        contentType: "application/json;charset=utf-8;",
        cache: false,
        async: false, //同步方式
        success: function (zNodes) {
            $.fn.zTree.init($("#tableList"), settingList, eval(zNodes));

            //                  var zTree = $.fn.zTree.getZTreeObj("tableList"),
            //		            type = { "Y":"ps", "N":"ps"};
            //		            zTree.setting.check.chkboxType = type;

        }
    });
}

function bindListHtml() {
//    var zNodes = [{ "id": "TM1A", "name": "接口列表", "pId": "TM", "nocheck": "false", "open": "True" }, { "id": "TM2A", "name": "经典案例", "pId": "TM", "nocheck": "false", "open": "True" }, { "id": "BaseMap", "name": "底图", "pId": "TM1A", "nocheck": "false", "open": "False" }, { "id": "MapControler", "name": "地图操作", "pId": "TM1A", "nocheck": "false", "open": "False" }, { "id": "Layer", "name": "图层操作", "pId": "TM1A", "nocheck": "false", "open": "False" }, { "id": "TM1A4B", "name": "图形", "pId": "TM1A", "nocheck": "false", "open": "False" }, { "id": "TM1A5B", "name": "工具", "pId": "TM1A", "nocheck": "false", "open": "False" }, { "id": "Legend", "name": "图例", "pId": "TM1A", "nocheck": "false", "open": "False" }, { "id": "MapChart", "name": "地图图表", "pId": "TM1A", "nocheck": "false", "open": "False" }, { "id": "Graphic_Effect", "name": "在地图上播放图片", "pId": "TM2A", "nocheck": "false", "open": "True" }, { "id": "Graphic_Extent_Search", "name": "图形周边查询", "pId": "TM2A", "nocheck": "false", "open": "True", "code": "Graphic_Extent_Search", "direction":"Graphic_Extent_Search"}, { "id": "MapChart", "name": "在地图上添加统计图表", "pId": "TM2A", "nocheck": "false", "open": "True" }, { "id": "SearchByExtent", "name": "拉框统计", "pId": "TM2A", "nocheck": "false", "open": "True" }, { "id": "Graphic_dataStyle", "name": "获取数据的方式", "pId": "TM1A4B", "nocheck": "false", "open": "True" }, { "id": "Graphic_symbolStyle", "name": "样式", "pId": "TM1A4B", "nocheck": "false", "open": "True" }, { "id": "Graphic_infoWindowStyle", "name": "气泡样式", "pId": "TM1A4B", "nocheck": "false", "open": "True" }, { "id": "Graphic_listenerStyle", "name": "事件类型", "pId": "TM1A4B", "nocheck": "false", "open": "True" }, { "id": "Graphic_Extent_Search", "name": "图形周边查询", "pId": "TM1A4B", "nocheck": "false", "open": "True" }, { "id": "Graphic_Effect", "name": "添加图形效果", "pId": "TM1A4B", "nocheck": "false", "open": "True" }, { "id": "getLayerUrl", "name": "获取图层地址", "pId": "TM1A5B", "nocheck": "false", "open": "True" }, { "id": "Search", "name": "查询", "pId": "TM1A5B", "nocheck": "false", "open": "True" }, { "id": "Buffer", "name": "缓冲", "pId": "TM1A5B", "nocheck": "false", "open": "True" }, { "id": "Relation", "name": "叠加对比", "pId": "TM1A5B", "nocheck": "false", "open": "True" }, { "id": "Draw", "name": "绘制图形", "pId": "TM1A5B", "nocheck": "false", "open": "True"}];
    var zNodes = [{ "id": "TM1A", "name": "接口列表", "pId": "TM", "nocheck": "false", "open": "True" },
 { "id": "TM2A", "name": "经典案例", "pId": "TM", "nocheck": "false", "open": "True" },
 { "id": "BaseMap", "name": "底图", "pId": "TM1A", "nocheck": "false", "open": "False", "code": "BaseMap", "direction": "BaseMap" },
 { "id": "MapControler", "name": "地图操作", "pId": "TM1A", "nocheck": "false", "open": "False", "code": "", "direction": "" },
 { "id": "Layer", "name": "图层操作", "pId": "TM1A", "nocheck": "false", "open": "False", "code": "Layer", "direction": "Layer" },
 { "id": "TM1A4B", "name": "图形", "pId": "TM1A", "nocheck": "false", "open": "False", "code": "", "direction": "" },
 { "id": "TM1A5B", "name": "工具", "pId": "TM1A", "nocheck": "false", "open": "False", "code": "", "direction": "" },
 { "id": "Legend", "name": "图例", "pId": "TM1A", "nocheck": "false", "open": "False", "code": "Legend", "direction": "Legend" },
 { "id": "MapChart", "name": "地图图表", "pId": "TM1A", "nocheck": "false", "open": "False", "code": "", "direction": "" },
 { "id": "Graphic_Effect", "name": "在地图上播放图片", "pId": "TM2A", "nocheck": "false", "open": "True", "code": "Graphic_Effect", "direction": "Graphic_Effect" },
 { "id": "Graphic_Extent_Search", "name": "图形周边查询", "pId": "TM2A", "nocheck": "false", "open": "True", "code": "", "direction": "" },
 { "id": "MapChart", "name": "在地图上添加统计图表", "pId": "TM2A", "nocheck": "false", "open": "True", "code": "", "direction": "" },
 { "id": "SearchByExtent", "name": "拉框统计", "pId": "TM2A", "nocheck": "false", "open": "True", "code": "", "direction": "" },
 { "id": "Graphic_dataStyle", "name": "获取数据的方式", "pId": "TM1A4B", "nocheck": "false", "open": "True", "code": "Graphic_dataStyle", "direction": "Graphic_dataStyle" },
 { "id": "Graphic_symbolStyle", "name": "样式", "pId": "TM1A4B", "nocheck": "false", "open": "True", "code": "Graphic_symbolStyle", "direction": "Graphic_symbolStyle" },
 { "id": "Graphic_infoWindowStyle", "name": "气泡样式", "pId": "TM1A4B", "nocheck": "false", "open": "True", "code": "Graphic_infoWindowStyle", "direction": "Graphic_infoWindowStyle" },
 { "id": "Graphic_listenerStyle", "name": "事件类型", "pId": "TM1A4B", "nocheck": "false", "open": "True", "code": "Graphic_listenerStyle", "direction": "Graphic_listenerStyle" },
 { "id": "Graphic_Extent_Search", "name": "图形周边查询", "pId": "TM1A4B", "nocheck": "false", "open": "True", "code": "", "direction": "" },
 { "id": "Graphic_Effect", "name": "添加图形效果", "pId": "TM1A4B", "nocheck": "false", "open": "True", "code": "Graphic_Effect", "direction": "Graphic_Effect" },
 { "id": "getLayerUrl", "name": "获取图层地址", "pId": "TM1A5B", "nocheck": "false", "open": "True", "code": "", "direction": "" },
 { "id": "Search", "name": "查询", "pId": "TM1A5B", "nocheck": "false", "open": "True", "code": "", "direction": "" },
 { "id": "Buffer", "name": "缓冲", "pId": "TM1A5B", "nocheck": "false", "open": "True", "code": "", "direction": "" },
 { "id": "Relation", "name": "叠加对比", "pId": "TM1A5B", "nocheck": "false", "open": "True", "code": "", "direction": "" },
 { "id": "Draw", "name": "绘制图形", "pId": "TM1A5B", "nocheck": "false", "open": "True", "code": "", "direction": ""}]; 
    $.fn.zTree.init($("#tableList"), settingList, eval(zNodes));
}


$(document).ready(function () {
    $("#tableList").height(document.documentElement.clientHeight - 150);
    $("#mainFrame").height(document.documentElement.clientHeight - 10);
    bindList();
//    bindListHtml();
});
		

		
