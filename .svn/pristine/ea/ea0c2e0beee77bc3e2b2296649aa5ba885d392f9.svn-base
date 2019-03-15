//地址参数解析 全局变量
var objK;
//SingleTable全局变量
var args = {
    //配置标识
    config_Id: "",
    //唯一字段
    pk_Field: "",
    //唯一字段值
    pk_Value: "",
    //Grid显示页
    pageNum: 1,
    //操作类型【Add、Delete、Detail、DetailNoReturn、Open、Open_Disk、Preview、Preview_Disk、Composite】
    operate_Type: "",
    //联合主键
    Composite_FieldName: "",
    //联合主键值
    Composite_FieldValue: "",
    //文件名称字段
    fileNameColumn: "",
    //文件二进制或路径字段
    imgColumn: "",
    //文件后缀名
    imgFormat: "",
    //排序字段
    sortField: "",
    //排序规则
    sortRole: ""
};

//模块查询过滤
function QueryNode() {
    var txt = $("#txtNode").val();
    var id = $("a.ajaxify:contains('" + txt + "')").first()[0].id;
    $("#" + id).click();
}

function clickMenu(menuId) {
    $("#" + menuId).addClass("active");
    var url = $("#" + menuId)[0].hreflang;
    //将点击的模块保存到session中
    var data = { menuId: menuId };
    $.post("Data/UserLogin.ashx", data, function (msg) { });
    InitializationPage(url);
}

function InitializationPage(url) {
    objK = url.replace(/\'/g, "\"");

    objK = $.parseJSON(objK);

    if (objK.flag != null) {
        //拆分初始化参数
        var pairs = objK.flag.split("&");
        for (var i = 0; i < pairs.length; i++) {
            var argname = pairs[i].split('=')[0];        //提取name   
            var value = pairs[i].split('=')[1];          //提取value   
            args[argname] = unescape(value);             //存为属性   
        }
    }
    args["Composite_FieldName"] = "";
    args["Composite_FieldValue"] = "";
    hideIframe("ParentFrame");
    //查询模块
    refreshQuery();
    //列表模块
    refreshGrid();
    //图表模块
    refreshChart();
}

/*###########################   刷新    ###############################*/
//刷新查询
function refreshQuery() {
    hideIframe("QueryFrame");
    if (objK.Query != null) {
        if (objK.Query == "Default" && args["config_Id"] != "") {
            var url = "../SingleTable/SingleTableQuery.aspx?config_Id=" + args["config_Id"];
            loadDivCK("MainFrame", "QueryFrame", url, 1, 0);
        }
        else {
            loadDivCK("MainFrame", "QueryFrame", objK.Query, 1, 0);
        }
    }
}
//刷新图表
function refreshChart() {
    hideIframe("ChartFrame");
    if (objK.Chart != null) {
        if (objK.Chart == "Default" && args["config_Id"] != "") {
            var url = "../SingleTable/SingleTableChart.aspx?config_Id=" + args["config_Id"];
            loadDivCK("MainFrame", "ChartFrame", url, 0, 1);
        }
        else {
            loadDivCK("MainFrame", "ChartFrame", objK.Chart, 0, 1);
        }
    }
}
//刷新列表
function refreshGrid() {
    hideIframe("FormFrame");
    if (objK.Grid != null) {
        var url = objK.Grid;
        if (objK.Grid == "Default" && args["config_Id"] != "") {
            url = "../SingleTable/SingleTableGrid.aspx?config_Id=" + args["config_Id"] + "&pageNum=" + args["pageNum"];
        }
        if (args["fileNameColumn"] != "") {
            url += "&fileNameColumn=" + args["fileNameColumn"] + "&imgColumn=" + args["imgColumn"] + "&imgFormat=" + args["imgFormat"];
        }
        if (args["Composite_FieldName"] != "" && args["Composite_FieldValue"] != "") {
            url += "&Composite_FieldName=" + args["Composite_FieldName"] + "&Composite_FieldValue=" + args["Composite_FieldValue"];
        }
        if (args["sortField"] != "" && args["sortRole"] != "") {
            url += "&sortField=" + args["sortField"] + "&sortRole=" + args["sortRole"];
        }
        loadDivCK("MainFrame", "GridFrame", url, 0, 1);

        args["sortField"] = "";
        args["sortRole"] = "";
    }
    else {
        hideIframe("GridFrame");
    }
}
//刷新表单
function refreshForm() {
    hideIframe("GridFrame");
    if (args["config_Id"] != "" && args["operate_Type"] != "") {
        var url = "../SingleTable/SingleTableForm.aspx?config_Id=" + args["config_Id"] + "&operate_Type=" + args["operate_Type"];

        if (args["pk_Field"] != "") {
            url += "&pk_Field=" + args["pk_Field"] + "&pk_Value=" + args["pk_Value"];
        }
        if (args["fileNameColumn"] != "") {
            url += "&fileNameColumn=" + args["fileNameColumn"] + "&imgColumn=" + args["imgColumn"] + "&imgFormat=" + args["imgFormat"];
        }
        loadDivCK("MainFrame", "FormFrame", url, 1, 1);
    }
}
//刷新子页面
function refreshSubPage() {
    hideIframe("SubPageFrame");
    if (args["config_Id"] != "") {
        var url = "../SingleTable/SingleTableSubPage.aspx?config_Id=" + args["config_Id"] + "&pageNum=" + args["pageNum"];
        loadDivCK("ParentFrame", "SubPageFrame", url, 1, 0);
    }
}

//用户维护
function ManageUserInfo() {
    args["operate_Type"] = "Update";
    hideIframe("ParentFrame");
    var url = "RightsManagement/UserInformation.aspx";
    loadDivCK("MainFrame", "FormFrame", url, 1, 1);
}

//注销
function exitSign() {
    location.href = "Login.aspx";
}

//登录超时
function sessionTimeOut() {
    bootbox.alert("页面响应超时，请重新登录！", function () {
        location.href = "Sign-In.aspx";
    });
}
