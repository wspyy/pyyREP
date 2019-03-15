function OnCancel() {
    var _DG = frameElement.lhgDG;
    _DG.cancel();
}


$(document).ready(function() {

    //保存按钮事件
    $(".Button_Save").click(function () {
     
        var _PollutantId = $(this).attr("name");     
        var AQI=document.getElementById("txtAQI"+_PollutantId).value; 
        var FirstP=document.getElementById("txtFirstP"+_PollutantId).value; 
    
        $.ajax({
            url: "../../Data/GetForCastWarnInfoData.ashx",
            data: { mode: "UP",id:_PollutantId, StationCode: $("#regionid" + _PollutantId).val(), AQI: AQI, QualityLevelID: $("#standardid" + _PollutantId).val(), FirstP: FirstP },
            method: 'Get',
            dataType: 'html',
            success: function(result) {
                if (result == "1") {
                    document.getElementById("txtAQI" + _PollutantId).style.display = "none";
                    document.getElementById("txtFirstP" + _PollutantId).style.display = "none";
                 
                    alert("保存成功！");
                    LoadData("");
               

                } else {
                    alert("保存失败！");
                }
            }

        });

    });
    //保存按钮事件
    $(".Button_Del").click(function () {

        var _PollutantId = $(this).attr("name");
       // var _OutPortCodte = $("#hid").val();
        //保存数据
        $.ajax({
            url: "../../Data/GetForCastWarnInfoData.ashx",
            data: { mode: "DEL", id: _PollutantId },
            method: 'Get',
            dataType: 'html',
            success: function(result) {
                if (result == "1") {
                    $("#tr" + _PollutantId).removeClass("StatusEdit");
                    $("#tr" + _PollutantId).addClass("StatusSave");

                    alert("删除成功！");
                 
                    LoadData("");
                } else {
                    alert("删除失败！");
                }
            }

        });
      

    });
    //添加按钮事件
    $(".Button_Add").click(function () {
   
        var _PollutantId = $(this).attr("name");
        var AQI = document.getElementById("txtAQI" + _PollutantId).value;
        var FirstP = document.getElementById("txtFirstP" + _PollutantId).value;

        $.ajax({
            url: "../../Data/GetForCastWarnInfoData.ashx",
            data: { mode: "adddetail", StationCode: $("#regionid" + _PollutantId).val(), AQI: AQI, QualityLevelID: $("#standardid" + _PollutantId).val(), FirstP: FirstP },
            method: 'Get',
            dataType: 'html',
            success: function (result) {
                if (result == "1") {
                    document.getElementById("txtAQI" + _PollutantId).style.display = "none";
                    document.getElementById("txtFirstP" + _PollutantId).style.display = "none";

                    alert("保存成功！");
                    LoadData("");
                } else {
                    alert("保存失败！");
                }
            }

        });

    });
    //编辑按钮事件
    $(".Button_Edit").click(function() {
        var _PollutantId = $(this).attr("name");
        $("#tr" + _PollutantId).removeClass("StatusSave");
        $("#tr" + _PollutantId).addClass("StatusEdit");
    });
    //添加按钮事件
    $(".Button_Add").click(function () {
        var _PollutantId = $(this).attr("name");
        $("#tr" + _PollutantId).removeClass("StatusSave");
        $("#tr" + _PollutantId).addClass("StatusEdit");
    });
    //取消按钮事件
    $(".Button_Closed").click(function() {
        var _PollutantId = $(this).attr("name");
        $("#tr" + _PollutantId).removeClass("StatusEdit");
        $("#tr" + _PollutantId).addClass("StatusSave");
    });

});
