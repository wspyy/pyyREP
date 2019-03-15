﻿
//点击“区域预报”，隐藏‘站点预报’
function ShowArea() {
    $("#imgselect").show();
    $("#Station").hide();
}
//点击"站点预报"，隐藏‘区域预报’
function ShowSta() {
    debugger;
    if (playPicWidget != null) {
        playPicWidget.closeAndclear();
    }
    hmap.clear();
    $("#imgselect").hide();
    $("#Station").show();

}
//获取到图片，显示在指定的经纬度的位置
function addWidget(e) {
    var pubdate = $("#txt_Time").val();
    var date = pubdate.replace(/-/g, "");
    debugger;
    var picArr = new Array();
    hmap.clear();//把图层清空
    //playPicWidget = null;
    var url = window.location.host;
    //alert(url);
    $.ajax({
        url: "../../Data/GetAirForeCastData.ashx",
        data: { flag: "picture", item: e, date: date },//.id
        dataType: "json",
        success: function (result) {
            debugger;
            if (result && result.length>0) {
                
                var pictureExtent = new HMap.Extent(109.72, 37.5348, 115.782, 41.4441);
                var picture = "";
                for (var i = 0; i < 72; i++) {
                    picture = new HMap.PlayPic({
                        picId: "pic" + i,
                        picUrl: "http://" + url + "/AirData/AirForeCastData/CMAQ/" + result[i]["imgurl"],
                        fadeInTime: 1000,
                        pauseTime: 1000,
                        alphaMax: 0.8,
                        alphaMin: 0,
                        picExtent: pictureExtent
                    })
                    picArr.push(picture);
                }
                var widgetParam = new HMap.WidgetParam({ bottom: 10, right: 40 });
                playPicWidget = new HMap.PlayPicWidget(picArr, { timeUnit: "h", isReplay: true, widgetParam: widgetParam, widgetWisible: false });
                //widgetWisible: true(图片播放进度条)
                widget = hmap.addControl(playPicWidget);
                //playPicWidget.play();

                hmap.extentTo(109.72, 37.5348, 115.782, 41.4441);
                $("#picture").html("");

            }

            else if (result.length == 0) {                
                //playPicWidget.closeAndclear(); 
                if (playPicWidget != null) {
                    playPicWidget.stop();
                   playPicWidget.closeAndclear();
                    
                }
                //setTimeout(function () { hmap.clear() }, 1000);//把图层清空
                setTimeout(function () { hmap.clear() }, 1500);
                //setTimeout(function () { hmap.clear() }, 2000);
                //hmap.clear();
                //alert("暂无图片");
                $("#picture").html("暂无"+e+"数据！");

            }
        }
    })
}

//播放
/*function PlayImg() {
    if (playPicWidget != null) {
        //点击播放按钮                
        playPicWidget.play();
    }
}*/
//暂停
function PauseImg() {
    if (playPicWidget != null) {
        //点击暂停按钮                
        playPicWidget.pause();
    }
}
//上一张
function PreImg() {
    if (playPicWidget != null) {
        //点击播放按钮                
        playPicWidget.previous();
    }
}
//下一张
function NextImg() {
    if (playPicWidget != null) {
        //点击播放按钮                
        playPicWidget.next();
    }
}
//停止
function StopImg() {
    if (playPicWidget != null) {
        //点击播放按钮                
        playPicWidget.stop();
    }
}

