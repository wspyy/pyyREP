/*  对话框操作  */
//普通弹出窗口，默认ID为 _Fixed
function ShowDataFrame(url, width, height, $title, isModal) {
    ShowDataFrame_Id(url, width, height, $title, isModal, '_Fixed');
}

//普通弹出窗口，带ID
function ShowDataFrame_Id(url, frame_width, frame_height, $title, isModal,frame_id) {
    //先关闭打开的对话框
    $.dialog({id: 'DataIFrame_Fixed'}).close();
    
    $.dialog({ 
        id: 'DataIFrame' + frame_id, 
        lock:isModal,
        min: false, 
        //max: false, 
        width: frame_width, 
        height: frame_height, 
        title: $title, 
        content: 'url:'+url+'',
//        ok: function(){
//    	    this.title('2秒后自动关闭').time(2);
//            return false;
//        },
        cancelVal: '关闭',
//        close:function(){
//            var duration = 800, /*动画时长*/
//                api = this,
//                opt = api.config,
//                wrap = api.DOM.wrap,
//                top = $(window).scrollTop() - wrap[0].offsetHeight;
//        	
//            wrap.animate({top:top + 'px', opacity:0}, duration, function(){
//                opt.close = function(){};
//                api.close();
//            });
//            return false;
//        },
        cancel: true  
    });
}

//弹出类似 QQ 的右下角提示框
function ShowLittleBoxMessage(qTitle,qContent)
{
    ShowLittleBoxMessage_Source({
        title: qTitle,
        width: 220,  /*必须指定一个像素宽度值或者百分比，否则浏览器窗口改变可能导致lhgDialog收缩 */
        content:qContent,
        time: 4 //默认 4s 关闭
    });
}

//弹出提示框
function showMessage(message,iconCode)
{
    if(iconCode == "ok")
    {
        iconName = "success.gif";
    }
    else if(iconCode == "alert")
    {
        iconName = "alert.gif";
    }
    else if(iconCode == "error")
    {
        iconName = "error.gif";
    }
    
    $.dialog({
        title:'提示',
        icon: iconName,
        content:message,
        time: 3,
        ok:function(){}
    });
}



//关闭弹出窗口
function CloseDataFrame(zid) {
    $.dialog({id: zid}).close();
}

////页面过渡
//function PageTransition()
//{
//    if(!+[1,]);else 
//    $('body').hide().fadeIn(1500);//"slow"   "normal"   "fast"  或者 毫秒数值如：1000
//}

//显示左边索引的窗口，隐藏索引DIV
function ShowIndexFrame(url) {
    $('#divFrame').css('display', 'block');
    $('#indexFrame').attr('src', encodeURI(url));
}
//显示左边的索引DIV,隐藏索引窗口
function ShowIndexDiv() {
    $('#divFrame').css('display', 'none');
    $('#indexFrame').attr('src', '');
}

//设定内容窗口
function SetMainFrame(url) {
    url = encodeURI(url);
    if (parent != null) {    
        $('#mainFrame').attr('src', url);
        $('#hfMainFrameUrl').val(url);
    }
    else {  
        parent.$('#mainFrame').attr('src', url);
        parent.$('#hfMainFrameUrl').val(url);
    } 
}

//隐藏指定的布局窗口
function HidePanel(panelName) {
    myLayout.hide(panelName);
}
//显示指定的布局窗口
function ShowPanel(panelName) {
    myLayout.show(panelName);
}

//脚本得到url  get提交的参数
function GetUrlParms()    
{
     var args = new Object();   
     var query = location.search.substring(1);          //获取查询串   
     var pairs = query.split("&");                      //在逗号处断开   
     for(var i=0;i<pairs.length;i++)   
     {   
        var pos = pairs[i].indexOf('=');                //查找name=value   
        if(pos == -1)   continue;                       //如果没有找到就跳过   
        var argname = pairs[i].substring(0,pos);        //提取name   
        var value = pairs[i].substring(pos+1);          //提取value   
        args[argname] = unescape(value);                //存为属性   
    }
     return args;
}

/* 扩展窗口外部方法 右下角出现提示框 */
function ShowLittleBoxMessage_Source( options )
{
    var opts = options || {},
        api, aConfig, hide, wrap, top,
        duration = opts.duration || 800;
	
    var config = {
        id: 'Notice',
        left: '100%',
        top: '100%',
        fixed: true,
        min: false, 
        max: false, 
        drag: false,
        resize: false,
        init: function(here){
            api = this;
            aConfig = api.config;
            wrap = api.DOM.wrap;
            top = parseInt(wrap[0].style.top);
            hide = top + wrap[0].offsetHeight;
			
            wrap.css('top', hide + 'px')
            .animate({top: top + 'px'}, duration, function(){
                opts.init && opts.init.call(api, here);
            });
        },
        close: function(here){
            wrap.animate({top: hide + 'px'}, duration, function(){
                opts.close && opts.close.call(this, here);
                aConfig.close = $.noop;
                api.close();
            });
			
            return false;
        }
    };
	
    for(var i in opts)
    {
        if( config[i] === undefined ) config[i] = opts[i];
    }
	
    return $.dialog( config );
};



