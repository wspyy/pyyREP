

/*###########################   SingleTableGrid.aspx ###############################*/
//SingleTableGrid.aspx 全选、取消全选
function selectAll() {
    //    if ($("#ck_All")[0].checked) {
    //        $(":checkbox").prop("checked", true);
    //    }
    //    else {
    //        $(":checkbox").prop("checked", false);
    //    }

    var checked = $("#ck_All")[0].checked;
    $('#dataTable :checkbox').each(function () {
        if (checked) {
            $(this).prop("checked", true);
            $(this).parents('span').removeClass().addClass("checked");
            $(this).parents('tr').removeClass().addClass("active");
        } else {
            $(this).prop("checked", false);
            $(this).parents('span').removeClass();
            $(this).parents('tr').removeClass();
        }
    });

}

//排序
function Sorting(id) {
    args["sortField"] = id;
    if ($("#th_" + id).attr('class') == "sorting" || $("#th_" + id).attr('class') == "sorting_asc") {
        args["sortRole"] = "desc";
    }
    else {
        args["sortRole"] = "asc";
    }
    refreshGrid();
}

//添加
function jumpAdd() {
    args["operate_Type"] = "Add";
    refreshForm();
}
function jumpAdd_More() {
    if (args["config_Id"] != "") {
        var url = "../Controls/Uploadify/Uploadify.aspx?config_Id=" + args["config_Id"] + "&operate_Type=" + args["operate_Type"];

        if (args["pk_Field"] != "") {
            url += "&pk_Field=" + args["pk_Field"] + "&pk_Value=" + args["pk_Value"];
        }
        if (args["fileNameColumn"] != "") {
            url += "&fileNameColumn=" + args["fileNameColumn"] + "&imgColumn=" + args["imgColumn"] + "&imgFormat=" + args["imgFormat"];
        }
        loadDivCK("MainFrame", "FormFrame", url, 1, 1);
    }
}

//详细、编辑、删除、打开
function jumpPage(config_Id, operate_Type, pk_Field, pk_Value) {
    //删除
    if (operate_Type == "Delete") {
        var data = { param: "Delete", config_Id: config_Id, pk_Field: pk_Field, pk_Value: pk_Value };
        deleteRow("确定要删除该记录吗？", data);
    }
        //打开、预览，模拟A标签点击，打开二进制数据
    else if (operate_Type == "Open" || operate_Type == "Open_Disk" || operate_Type == "Preview" || operate_Type == "Preview_Disk") {

        var url = "../Data/SingleTable.ashx?config_Id=" + config_Id + "&pk_Field=" + pk_Field + "&pk_Value=" + pk_Value + "&param=" + operate_Type;
        if (args["fileNameColumn"] != "") {
            url += "&fileNameColumn=" + args["fileNameColumn"] + "&imgColumn=" + args["imgColumn"] + "&imgFormat=" + args["imgFormat"];
        }
        document.getElementById("aHidden").href = url;
        $('#aHidden').get(0).click();
    }
    else if (operate_Type == "Composite") {

        args["pk_Field"] = pk_Field;
        args["pk_Value"] = pk_Value;
        args["Composite_FieldName"] = pk_Field;
        args["Composite_FieldValue"] = pk_Value;
        //清空全部
        hideIframe("MainFrame");
        //显示子页面
        refreshSubPage();
    }
        //详细、编辑
    else {
        args["operate_Type"] = operate_Type;
        args["pk_Field"] = pk_Field;
        args["pk_Value"] = pk_Value;
        refreshForm();
    }
}

//删除全部
function deleteAll() {
    var value = "";
    var pk_Field = "";
    var config_Id = args["config_Id"];

    //SingleTableGrid.aspx 批量删除
    for (var i = 0; i < $(":checkbox").length; i++) {
        if ($(":checkbox")[i].checked && $(":checkbox")[i].id != "ck_All") {
            value += $(":checkbox")[i].id.replace("ck_", "") + ",";
            pk_Field = $(":checkbox")[i].attributes.getNamedItem("pk_Field").value;
        }
    }
    if (value != "") {
        value = value.replace(/,$/gi, "");
        var data = { param: "Delete", config_Id: config_Id, pk_Field: pk_Field, pk_Value: value };
        deleteRow("确定要删除 " + value.split(',').length + " 条记录吗？", data);
    }
    else {
        bootbox.alert("请选择需要删除的数据行！");
    }
}
//删除
function deleteRow(strMsg, data) {
    bootbox.confirm(strMsg, function (result) {
        if (result) {
            $.post("../Data/SingleTable.ashx", data,
                function (msg) {
                    bootbox.alert(msg, function () {
                        refreshGrid();
                    });
                });
        }
    });
}


//导出
function jumpExport() {
    document.getElementById("aHidden").href = "../Data/SingleTable.ashx?config_Id=" + args["config_Id"] + "&param=Export";
    $('#aHidden').get(0).click();
}


/*###########################   SingleTableImportExcel.aspx  ###############################*/
//弹出导入页面
function jumpImport() {
    var url = "../SingleTable/SingleTableImportExcel.aspx?config_Id=" + args["config_Id"];
    loadDivCK("MainFrame", "FormFrame", url, 1, 1);
}

//下载导入模板
function jumpExportDot() {
    document.getElementById("aHidden").href = "../Data/SingleTable.ashx?config_Id=" + args["config_Id"] + "&param=ExportDot";
    $('#aHidden').get(0).click();
}

//导入后保存
function ImportPage() {
    if ($("#fileUrl").val() == "") {
        bootbox.alert("请选择需要上传的文件！");
    }
    else if ($("#fileUrl").val().indexOf(".xls") == -1) {
        bootbox.alert("文件格式应为.xls 或 .xlsx ！");
    }
    else {
        var options = {
            url: '../SingleTable/SingleTableImportExcel.aspx',
            data: { flag: 'Import', config_Id: args["config_Id"] },
            success: function (msg) {
                if (msg.indexOf("重复") == -1) {
                    showMessageAndReturn(msg);
                }
                else {
                    showMessageCK(msg);
                }
            }
        };
        $('#formImport').ajaxSubmit(options);
    }

}

//提示导入重复数据
function showMessageCK(msg) {
    bootbox.dialog({
        message: msg,
        title: "提示",
        buttons: {
            success: {
                label: "覆盖",
                className: "btn-success",
                callback: function () {
                    var options = {
                        url: '../SingleTable/SingleTableImportExcel.aspx',
                        data: { flag: 'ImportAgain', key: 'cover', config_Id: args["config_Id"] },
                        success: function (msg) {
                            showMessageAndReturn(msg);
                        }
                    };
                    $('#formImport').ajaxSubmit(options);
                }
            },
            danger: {
                label: "导入未重复",
                className: "btn-danger",
                callback: function () {
                    var options = {
                        url: '../SingleTable/SingleTableImportExcel.aspx',
                        data: { flag: 'ImportAgain', key: 'NotRepeated', config_Id: args["config_Id"] },
                        success: function (msg) {
                            showMessageAndReturn(msg);
                        }
                    };
                    $('#formImport').ajaxSubmit(options);
                }
            },
            main: {
                label: "取消",
                className: "btn-primary"
            }
        }
    });
}


/*###########################   SingleTableForm.aspx  ###############################*/
//点击返回后，刷新列表
function jumpReturn() {
    hideIframe("FormFrame");
    refreshGrid();
}
//添加或编辑后保存
function SavePage() {
    $('#formManage').validate({

        errorPlacement: function (error, element) {
            if (element.is(":radio"))
                error.insertAfter(element.parent().next().next());
            else if (element.is(":checkbox"))
                error.insertAfter(element.next());
            else
                error.insertAfter(element);
        },
        submitHandler: function (form) {
            //防止不可编辑的控件不提交后台，清空该字段数据
            $('input').attr("disabled", false);
            var options = {
                url: '../SingleTable/SingleTableForm.aspx',
                data: {
                    flag: 'AddOrUpdate', config_Id: args["config_Id"],
                    Composite_FieldName: args["Composite_FieldName"], Composite_FieldValue: args["Composite_FieldValue"],
                    operate_Type: args["operate_Type"], pk_Field: args["pk_Field"], pk_Value: args["pk_Value"]
                },
                success: function (msg) {
                    bootbox.alert(msg, function () {
                        refreshGrid();
                    });
                }
            };
            $('#formManage').ajaxSubmit(options);
        }
    });

}

/*###########################   SingleTableQuery.aspx  ###############################*/
//重置、刷新
function CleanQuery() {
    $('#formQuery').clearForm();
}
//保存查询条件
function QueryPage() {
    hideIframe("FormFrame");
    var options = {
        url: '../SingleTable/SingleTableQuery.aspx',
        data: { flag: 'Query', config_Id: args["config_Id"] },
        success: function () {
            refreshGrid();
            refreshChart();
        }
    };

    $('#formQuery').ajaxSubmit(options);
}

/*###########################   SingleTableSubPage.aspx  ###############################*/







