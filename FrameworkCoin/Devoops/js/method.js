/*
通用方法
2014-05-30
Coin
*/

/*#########################   提示框  ###############################*/
function showMessage(message) {
    bootbox.alert(message);
}

function showMessageAndReturn(message) {
    bootbox.alert(message, function () {
        jumpReturn();
    });
}

/*#########################   页面跳转  ###############################*/
//添加时间戳
function timestamp(url) {
    var getTimestamp = new Date().getTime();
    if (url.indexOf("?") > -1) {
        url = url + "&timestamp=" + getTimestamp
    } else {
        url = url + "?timestamp=" + getTimestamp
    }
    return url;
}

/*#########################   页面加载  ###############################*/
//需要在那个div下添加div
function checkDiv(pid, divID, sequence) {
    //检查是否有父节点
    if ($("#" + pid).length == 0) {
        var newDiv = $("<div id='"+divID+"' style='padding:5px; padding-bottom:0px;'></div>")
        $("#ParentFrame").append(newDiv);
    }
    //检查是否有子节点
    if ($("#" + divID).length == 0) {
        var newDiv = document.createElement('div');
        newDiv.setAttribute("Id", divID);
        if (sequence == 0) {
            $("#" + pid).prepend(newDiv);
        }
        else {
            $("#" + pid).append(newDiv);
        }
    }
}
//异步加载页面
function loadDivCK(pid, divId, url, animation, sequence) {
    checkDiv(pid, divId, sequence);
    loadDiv(divId, url, animation);

}
//异步加载页面
function loadDiv(divId, url, animation) {
    var el = $("#" + divId);
    App.blockUI(el);

    if (animation == 0) {
        $("#" + divId).show();
        $("#" + divId).load(timestamp(url), function () {
            App.unblockUI(el);
        });
    }
    else {
        $("#" + divId).hide();
        $("#" + divId).load(timestamp(url), function () {
            App.unblockUI(el);
            $("#" + divId).fadeIn("slow");
        });
    }

}
function hideIframe(pid) {
    $("#" + pid).empty();
}

//添加iframe
function CreateIframe(pid, id, url, success) {
    //var newIframe = document.createElement('iframe');
    //newIframe.src = url;
    //newIframe.id = id;
    //newIframe.frameborder = "0";
    //newIframe.width = "100%";
    //newIframe.height = "300px";

    var el = $("#" + pid);
    App.blockUI(el);
    var iframe = $("<iframe id=\"" + id + "\" src=\"" + url + "\" style=\"width:100%;\" scrolling=\"auto\" frameborder=\"0\"></iframe>");
    // document.getElementById(pid).appendChild(iframe);
    $("#" + pid).append(iframe);
    var newIframe = document.getElementById(id);
    //注册iframe加载完成回调事件,取消loading

    if (newIframe.attachEvent) {
        newIframe.attachEvent("onreadystatechange", function () {
            //此事件在内容没有被载入时候也会被触发，所以我们要判断状态
            //有时候会比较怪异 readyState状态会跳过 complete 所以我们loaded状态也要判断
            if (newIframe.readyState === "complete" || newIframe.readyState == "loaded") {
                //代码能执行到这里说明已经载入成功完毕了
                //要清除掉事件
                newIframe.detachEvent("onreadystatechange", arguments.callee);
                if (success != null) {
                    success(id);
                }
                App.unblockUI(el);
            }
        });
    } else {
        newIframe.addEventListener("load", function () {
            //代码能执行到这里说明已经载入成功完毕了
            this.removeEventListener("load", arguments.call, false);
            if (success != null) {
                success(id);
            }
            App.unblockUI(el);
        }, false);
    }


   $("#" + id).height($(window).height() - 85);
    $(window).resize(function () {
    $("#" + id).height($(window).height() - 85);
    });
}

//移除iframe
function RemoveIframe(pid, id) {
    document.getElementById(pid).removeChild(document.getElementById(id));
}

Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(),    //day
        "h+": this.getHours(),   //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter
        "S": this.getMilliseconds() //millisecond
    }
    if (/(y+)/.test(format)) format = format.replace(RegExp.$1,
    (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o) if (new RegExp("(" + k + ")").test(format))
        format = format.replace(RegExp.$1,
        RegExp.$1.length == 1 ? o[k] :
        ("00" + o[k]).substr(("" + o[k]).length));
    return format;
}
Date.prototype.addDays = Date.prototype.addDays || function (days) {
    this.setDate(this.getDate() + days);
    return this;
}