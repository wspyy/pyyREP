
function toExcel(inTblId, inWindow) {
    if (window.attachEvent) { //如果是IE浏览器
        try {
            var allStr = ""; var curStr = "";
            if (inTblId != null && inTblId != "" && inTblId != "null") {
                curStr = getTblData(inTblId, inWindow);
            }
            if (curStr != null) {
                allStr += curStr;
            }
            else {
                alert("你要导出的表不存在！");
                return;
            }
            var fileName = getExcelFileName();
            doFileExport(fileName, allStr);
        }
        catch (e) {
            alert("导出发生异常:" + e.name + "->" + e.description + "!");
        }
    } else {
        window.open('data:application/vnd.ms-excel,' + encodeURIComponent($('div[id$=divGvData]').html()));
        //e.preventDefault();
    }
} function getTblData(inTbl, inWindow) {
    var rows = 0; var tblDocument = document;
    if (!!inWindow && inWindow != "") {
        if (!document.all(inWindow)) {
            return null;
        } else {
            tblDocument = eval(inWindow).document;
        }
    }
    var curTbl = tblDocument.getElementById(inTbl);
    if (curTbl.rows.length > 65000) {
        alert('源行数不能大于65000行');
        return false;
    }
    if (curTbl.rows.length <= 1) {
        alert('数据源没有数据');
        return false;
    }
    var outStr = "";
    if (curTbl != null) {
        for (var j = 0; j < curTbl.rows.length; j++) {
            for (var i = 0; i < curTbl.rows[j].cells.length; i++) {
                if (i == 0 && rows > 0) {
                    outStr += " \t"; rows -= 1;
                }
                var tc = curTbl.rows[j].cells[i];
                if (j > 0 && tc.hasChildNodes() && tc.firstChild.nodeName.toLowerCase() == "input") {
                    if (tc.firstChild.type.toLowerCase() == "checkbox") {
                        if (tc.firstChild.checked == true) {
                            outStr += "是" + "\t";
                        } else {
                            outStr += "否" + "\t";
                        }
                    }
                } else {
                    outStr += " " + curTbl.rows[j].cells[i].innerText + "\t";
                }
                if (curTbl.rows[j].cells[i].colSpan > 1) {
                    for (var k = 0; k < curTbl.rows[j].cells[i].colSpan - 1; k++) {
                        outStr += " \t";
                    }
                }
                if (i == 0) {
                    if (rows == 0 && curTbl.rows[j].cells[i].rowSpan > 1) {
                        rows = curTbl.rows[j].cells[i].rowSpan - 1;
                    }
                }
            }
            outStr += "\r\n";
        }
    } else {
        outStr = null; alert(inTbl + "不存在!");
    }
    return outStr;
}
function getExcelFileName() {
    var d = new Date(); var curYear = d.getYear(); var curMonth = "" + (d.getMonth() + 1);
    var curDate = "" + d.getDate(); var curHour = "" + d.getHours(); var curMinute = "" +
    d.getMinutes(); var curSecond = "" + d.getSeconds();
    if (curMonth.length == 1) {
        curMonth = "0" + curMonth;
    }
    if (curDate.length == 1) {
        curDate = "0" + curDate;
    }
    if (curHour.length == 1) {
        curHour = "0" + curHour;
    }
    if (curMinute.length == 1) {
        curMinute = "0" + curMinute;
    }
    if (curSecond.length == 1) {
        curSecond = "0" + curSecond;
    }
    var fileName = "设备状态" + curYear + curMonth + curDate + curHour + curMinute + curSecond + ".xls";
    return fileName;
}
function doFileExport(inName, inStr) {
    var xlsWin = null;
    if (!!document.all("glbHideFrm")) {
        xlsWin = glbHideFrm;
    } else {
        var width = 1; var height = 1;
        var openPara = "left=" + (window.screen.width / 2 + width / 2) + ",top=" + (window.screen.height + height / 2) +
        ",scrollbars=no,width=" + width + ",height=" + height;
        xlsWin = window.open("", "_blank", openPara);
    }
    xlsWin.document.write(inStr);
    xlsWin.document.close();
    xlsWin.document.execCommand('Saveas', true, inName);
    xlsWin.close();
}
