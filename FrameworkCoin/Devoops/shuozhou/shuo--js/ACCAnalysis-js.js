﻿/*ACCAnalysis.html--ResultList, FalseArr, TrueArr, NoArr*/
function getnum(items, ResultList) {
    var AF = 0.0;
    var AT = 0.0;
    var LF = 0.0;
    var LT = 0.0;
    var N = 0.0;
    var dataArr = [];
    for (var i = 0; i < ResultList.length ; i++) {
        if (items == "AQI") {
            var AoL = ResultList[i]["AQI"];
            if (AoL > -1) {
                if (AoL > 30) {
                    AF++;
                }
                if (AoL <= 30) {
                    AT++;
                }
            }

            else if (AoL == -1) {
                N++;
            }
        }
        if (items == "AirLevel") {
            var AoL = ResultList[i]["AirLevel"];
            if (isNaN(AoL)) {
                LT++;
            }
            if (AoL == 1) {
                LF++;
            }
            else if (AoL == -1) {
                N++;
            }
        }
    }
        dataArr.push(AT);
        dataArr.push(AF);
        dataArr.push(LT);
        dataArr.push(LF);
        dataArr.push(N);
        return dataArr;
    
}


/*ACCAnalysis.html--ResultList, FalseArr, TrueArr, NoArr*/
function getArray(items, ResultList) {
    var TrueArr = new Array();
    var FalseArr = new Array();    
    var NoArr = new Array();
    var dataArr = [];
    //dataArr.length = 3;
    for (var i = 0; i < ResultList.length ; i++) {
        if (items == "AQI") {
            var AoL = ResultList[i]["AQI"];
            if (AoL > -1) {
                if (AoL > 30) {
                    //AF++;
                    FalseArr.push(ResultList[i]["MonitorTime"]);
                }
                if (AoL <= 30) {
                    //AT++;
                    TrueArr.push(ResultList[i]["MonitorTime"]);
                }
            }

            else if (AoL == -1) {
                //N++;
                NoArr.push(ResultList[i]["MonitorTime"]);
            }
        }
        if (items == "AirLevel") {
            var AoL = ResultList[i]["AirLevel"];
            if (isNaN(AoL)) {
                //LT++;
                TrueArr.push(ResultList[i]["MonitorTime"]);
            }
            if (AoL == 1) {
                //LF++;
                FalseArr.push(ResultList[i]["MonitorTime"]);
            }
            else if (AoL == -1) {
                //N++;
                NoArr.push(ResultList[i]["MonitorTime"]);
            }
        }
    }
    dataArr.push(TrueArr);
    dataArr.push(FalseArr);
    dataArr.push(NoArr);
    return (dataArr);
}

/*ACCAnalysis.html--calendar*/
//全年日历
function showCalendar0(select_year,  items, ResultList, FalseArr, TrueArr, NoArr) {
    var content = "";
    
        for (var i = 0; i < 12; i++) {
            content += "<div style='float:left;width:25%;border-right:1px solid #dadada' > <div class='c_header'><h1></h1><ol><li>日</li><li>一</li><li>二</li><li>三</li><li>四</li><li>五</li><li>六</li></ol></div><ul class='c_body' id='ul" + i + "'>";

            for (var j = 0; j < 42; j++) {
                content += "<li></li>";
            }
            content += "</ul></div>";
        }
    
  
    debugger;
    $("#calendar").html(content);
    var oCal = document.getElementById("calendar");//id='calendar'的对象
    var oTitle = oCal.getElementsByTagName("h1");//得到h1标签元素
    var oUl = oCal.getElementsByTagName("ul");//得到ul标签元素

    var addIDArray = new Array();//17
    addIDArray.length = 0;//17

    //根据传入的年份重新算时间
    var oDate = new Date();//当前时间的日期对象
    oDate.setYear(select_year);//setYear()用于设置年份

    var iYear = oDate.getFullYear();
    //得到年                        

    var bLeap = false;//是否是闰年
    if (iYear % 4 == 0 && iYear % 100 !== 0 || iYear % 400 == 0) bLeap = true;
    var addIDArray = new Array();
    
        for (var m = 0; m < 12; m++) {
        oDate.setMonth(m);//设置月份（0-11，12个月）
        oDate.setDate(1);//设置一个月的某一天（1-31）
        var iMonth = oDate.getMonth();
        //得到月
        var iDay = oDate.getDay();
        //得到星期几
        var iSum = 0;	//存本月天数
        oTitle[m].innerHTML = iYear + "年" + (iMonth + 1) + "月"; //第一个h1标签元素（*年*月）
        
        switch (iMonth + 1) {
            case 1:
            case 3:
            case 5:
            case 7:
            case 8:
            case 10:
            case 12:
                iSum = 31;
                break;
            case 4:
            case 6:
            case 9:
            case 11:
                iSum = 30;
                break;
            case 2:
                if (bLeap) iSum = 29;
                else iSum = 28;
                break;
        }
         var aLi = oUl[m].getElementsByTagName("li"); //得到第（m+1）个所有li标签元素
        
        //初始化一下
        for (var i = 0; i < aLi.length; i++) {//
            aLi[i].innerHTML = "";//清除内容
            aLi[i].style.height = "40px";
            aLi[i].className = "";
        }
        var month = m < 9 ? "-0" + (m + 1) : "-" + (m + 1);
        for (var i = 1; i <= iSum; i++) {

            var day = i < 10 ? "-0" + i : "-" + i;
            var addID = iYear + month + day;
            addIDArray.push(addID);//17                    

            aLi[iDay].setAttribute("id", "a" + addID); 
            $("#a" + addID).html(i);
            for (var f in FalseArr) {
                if (addID == FalseArr[f]) {
                    //['#90ED7D', '#FFBC75', '#999EFF'],
                    $("#a" + addID).css("backgroundColor", "#FFBC75");
                }
            }
            for (var t in TrueArr) {
                if (addID == TrueArr[t]) {
                    $("#a" + addID).css("backgroundColor", "#90ED7D");
                }
            }
            for (var n in NoArr) {
                if (addID == NoArr[n]) {
                    $("#a" + addID).css("backgroundColor", "#999EFF");
                }
            }
            //17
            for (var r in ResultList) {
                if (addID == ResultList[r]["MonitorTime"]) {
                    var strdiv;
                    var strdiv = ""
                    strdiv += "<div class='tip' id='tip" + addID + "'>" + addID + "<br/>";
                    if (items == "AQI") {
                        strdiv += "预测：" + ResultList[r]["fAQI"] + ";实测：" + ResultList[r]["dAQI"];
                    }
                    if (items == "AirLevel") {
                        strdiv += "预测：" + ResultList[r]["fLevel"] + ";实测：" + ResultList[r]["dLevel"];
                    }
                    strdiv += "</div>";
                    $("#a" + addID).append(strdiv);
                }

            }//17
            //showdata(addID);
            iDay++;//得到这天是周几，依次往后加
        }
        for (var i = 0; i < addIDArray.length; i++) {
            var liID = "#a" + addIDArray[i];
            var divID = "#tip" + addIDArray[i];
            $(liID).mouseover(function (e) {
                var aid = $(e.target)[0].id;
                var tipid = $("#" + aid + " div")[0].id;
                $("#" + tipid).show();
                //$(divID).css("display", "block");
            });
            $(liID).mouseout(function (e) {
                var aid = $(e.target)[0].id;
                var tipid = $("#" + aid + " div")[0].id;
                $("#" + tipid).hide();
                //$(divID).css("display", "none");
            });
        }


    }
    //把没字的格子折叠起来
    for (var i = 0; i < aLi.length; i++) {
        if (aLi[i].innerHTML == "") {
            aLi[i].style.height = "0px";
        }
    }

}



//颜色
//function ACCAnalysis_color(items,AoL) {
//    var b_color;
//    if (items == "airlevel") {
//        if (isNaN(AoL)) {
//            b_color = "#00ff00";
//        }
//        if (AoL == 1) {
//            b_color = "#ff0000";
//        }
//        else if (AoL == -1) {
//            b_color = "#F9F900";
//        }

//    }
//   if (items == "aqi") {
        
//       if (AoL > -1) {
//           if(AoL > 20){
//               b_color = "#ff0000";}
//           if(AoL <= 20){
//               b_color = "#00ff00";
//           }
//        }
        
//       else if (AoL == -1) {
//           b_color = "#F9F900";
//       }
//    }
//    return b_color;
//}

//单个月份日历
function showCalendar1(select_year, month, items, ResultList, FalseArr, TrueArr, NoArr) {
    var content = "";
    if (month == "0") {
        for (var i = 0; i < 12; i++) {
            content += "<div style='float:left;width:25%;border-right:1px solid #dadada' > <div class='c_header'><h1></h1><ol><li>日</li><li>一</li><li>二</li><li>三</li><li>四</li><li>五</li><li>六</li></ol></div><ul class='c_body' id='ul" + i + "'>";

            for (var j = 0; j < 42; j++) {
                content += "<li></li>";
            }
            content += "</ul></div>";
        }
    }
    else if (month != "0") {
        content += "<div style='float:left;width:100%;border-right:1px solid #dadada' > <div class='c_header'><h1></h1><ol><li>日</li><li>一</li><li>二</li><li>三</li><li>四</li><li>五</li><li>六</li></ol></div><ul class='c_body' id='ul" + month + "'>";
        for (var j = 0; j < 42; j++) {
            content += "<li></li>";
        }
        content += "</ul></div>";
    }
    debugger;
    $("#calendar").html(content);
    var oCal = document.getElementById("calendar");//id='calendar'的对象
    var oTitle = oCal.getElementsByTagName("h1");//得到h1标签元素
    var oUl = oCal.getElementsByTagName("ul");//得到ul标签元素

    var addIDArray = new Array();//17
    addIDArray.length = 0;//17

    //根据传入的年份重新算时间
    var oDate = new Date();//当前时间的日期对象
    oDate.setYear(select_year);//setYear()用于设置年份

    var iYear = oDate.getFullYear();
    //得到年                        

    var bLeap = false;//是否是闰年
    if (iYear % 4 == 0 && iYear % 100 !== 0 || iYear % 400 == 0) bLeap = true;
    var addIDArray = new Array();
    var Mbegin = 0;
    var Mend = 12;
    if (month != "0") {
        Mbegin = month - 1;
        Mend = month;
    }
    for (var m = Mbegin; m < Mend; m++) {
        oDate.setMonth(m);//设置月份（0-11，12个月）
        oDate.setDate(1);//设置一个月的某一天（1-31）
        var iMonth = oDate.getMonth();
        //得到月
        var iDay = oDate.getDay();
        //得到星期几
        var iSum = 0;	//存本月天数
        if (month == "0")
        { oTitle[m].innerHTML = iYear + "年" + (iMonth + 1) + "月"; }//第一个h1标签元素（*年*月）
        else if (month != "0")
        { oTitle[0].innerHTML = iYear + "年" + (iMonth + 1) + "月"; }
        switch (iMonth + 1) {
            case 1:
            case 3:
            case 5:
            case 7:
            case 8:
            case 10:
            case 12:
                iSum = 31;
                break;
            case 4:
            case 6:
            case 9:
            case 11:
                iSum = 30;
                break;
            case 2:
                if (bLeap) iSum = 29;
                else iSum = 28;
                break;
        }
        if (month == "0")
        { var aLi = oUl[m].getElementsByTagName("li"); }//得到第（m+1）个所有li标签元素
        else if (month != "0")
        { var aLi = oUl[0].getElementsByTagName("li"); }
        //初始化一下
        for (var i = 0; i < aLi.length; i++) {//
            aLi[i].innerHTML = "";//清除内容
            aLi[i].style.height = "40px";
            aLi[i].className = "";
        }
        var month = m < 9 ? "-0" + (m + 1) : "-" + (m + 1);
        for (var i = 1; i <= iSum; i++) {

            var day = i < 10 ? "-0" + i : "-" + i;
            var addID = iYear + month + day;
            addIDArray.push(addID);//17                    

            aLi[iDay].setAttribute("id", "a" + addID);
            $("#a" + addID).html(i);
            for (var f in FalseArr) {
                //['#90ED7D', '#FFBC75', '#999EFF'],
                if (addID == FalseArr[f]) {
                    $("#a" + addID).css("backgroundColor", "#FFBC75");
                }
            }
            for (var t in TrueArr) {
                if (addID == TrueArr[t]) {
                    $("#a" + addID).css("backgroundColor", "#90ED7D");
                }
            }
            for (var n in NoArr) {
                if (addID == NoArr[n]) {
                    $("#a" + addID).css("backgroundColor", "#999EFF");
                }
            }
            //17
            for (var r in ResultList) {
                if (addID == ResultList[r]["MonitorTime"]) {
                    var strdiv;
                    var strdiv = ""
                    strdiv += "<div class='tip' id='tip" + addID + "'>" + addID + "<br/>";
                    if (items == "AQI") {
                        strdiv += "预测：" + ResultList[r]["fAQI"] + ";实测：" + ResultList[r]["dAQI"];
                    }
                    if (items == "AirLevel") {
                        strdiv += "预测：" + ResultList[r]["fLevel"] + ";实测：" + ResultList[r]["dLevel"];
                    }
                    strdiv += "</div>";
                    $("#a" + addID).append(strdiv);
                }

            }//17
            //showdata(addID);
            iDay++;//得到这天是周几，依次往后加
        }
        for (var i = 0; i < addIDArray.length; i++) {
            var liID = "#a" + addIDArray[i];
            var divID = "#tip" + addIDArray[i];
            $(liID).mouseover(function (e) {
                var aid = $(e.target)[0].id;
                var tipid = $("#" + aid + " div")[0].id;
                $("#" + tipid).show();
                //$(divID).css("display", "block");
            });
            $(liID).mouseout(function (e) {
                var aid = $(e.target)[0].id;
                var tipid = $("#" + aid + " div")[0].id;
                $("#" + tipid).hide();
                //$(divID).css("display", "none");
            });
        }


    }
    //把没字的格子折叠起来
    for (var i = 0; i < aLi.length; i++) {
        if (aLi[i].innerHTML == "") {
            aLi[i].style.height = "0px";
        }
    }

}

