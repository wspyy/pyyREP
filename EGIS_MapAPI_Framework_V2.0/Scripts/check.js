/**
使用说明：
1、 将js引入后<script type="text/javascript" src="check.js"></script>
2、 在body中加入onload="allfocus();"通过获得焦点遍历控件，得到填写要求提示。
3、 在需要验证控件中加入 onblur和onchange事件，如onblur="isNull(this);"     onchange="isNull(this);"
4、 在提交按钮控件中，客户端控件加入onclick="isClearMsg();"事件，服务器端控件加入OnClientClick="return isClearMsg();"事件，判断是否已经没有填写错误。


参数说明：
s 为this,t为需要与s比较的文本框， b 为是否可以为空（0 为可以为空）（其他 为不可以）；
函数说明：
/**js函数以及说明信息
setStyle(s):说明设置当前焦点文本框样式 s 为this
outStyle(s)：还原失去焦点文本框样式 s 为this
ClearState(s)：清除错误提示 s 为this
(1)CreatState(s, msg):通用的添加错误提示 s 为this:
(2)CreatState1(s, msg):限定经度的度的值输入范围,只针对沈阳的122-123
(3)CreatState2(s, msg)：限定经度和纬度的分，秒的值输入范围:0-60
(4)CreatState3(s, msg):限定纬度的度的值输入范围41-43
(5)isClearMsg():表单提交前遍历表单判断是否还有显示的错误未修改，在提交服务器端控件中加入OnClientClick="retrun isClearMsg();"
(6)allfocus():在页面加载前遍历表单
(7)trim(str):去掉前置和后置空格
(8)isNull1(s):是否为空；为了与原有isNull()函数区分
(9)isNull2(s):是否为空和输入值为0，都认为是无效
(10)isChinese(s, b):是否全部中文字符； s 为this, b 为是否可以为空（0 为可以 1 为不可以）
(11)isRegisterUserName(s, b):是否合法变量字符串 isRegisterUserName(s) 只能输入5-20个以字母开头、可带数字、“_”、“.”的字串b 为是否可以为空（0 为可以 1 为不可以）
(12)isEnglish(s, b):英文字符；b 为是否可以为空（0 为可以 1 为不可以）
(13)isNaN1(s, b):是否数字；为了与原有isNaN()函数区分 ，b 为是否可以为空（0 为可以 1 为不可以）
(14)isInteger(s, b):是否整数；b 为是否可以为空（0 为可以 1 为不可以）
(15)isDouble(s, b):是否浮点型；b 为是否可以为空（0 为可以 1 为不可以）
(16)isEmail(s, b):Email地址；b 为是否可以为空（0 为可以 1 为不可以）
(17)isURL(s, b):网址；isURL(s)  b 为是否可以为空（0 为可以 1 为不可以）
(18)telCheck(s, b):普通电话（座机）、传真号码 ；telCheck(s) b 为是否可以为空（0 为可以 1 为不可以）  可以“+”开头，除数字外，可含有“-”
(19)mobileCheck(s, b):.手机号码 ；mobileCheck(s) b 为是否可以为空（0 为可以 1 为不可以）  必须以数字开头，除数字外，可含有“-”
(20)PhoneCheck(s, b)
-----电话号码由数字、"("、")"和"-"构成
-----电话号码为3到8位
-----如果电话号码中包含有区号，那么区号为三位或四位
-----区号用"("、")"或"-"和其他部分隔开
-----移动电话号码为11或12位，如果为12位,那么第一位为0
-----11位移动电话号码的第一位和第二位为"13"或"15"
-----12位移动电话号码的第二位和第三位为"13"或"15"
(21)isPostalCode(s, b):邮政编码；b 为是否可以为空（0 为可以 1 为不可以）
(22)isIdCardNo1(s, b):身份证号码验证（不是太严格）；b 为是否可以为空（0 为可以 1 为不可以）
(23)isIdCardNo(s, b):身份证号码验证（严格判定）；b 为是否可以为空（0 为可以 1 为不可以）
(24)isIp(s, b):ip地址  b 为是否可以为空（0 为可以 1 为不可以）
(25)isDate(s, b):日期；isDate(s)（格式：yyyy-mm-dd）b 为是否可以为空（0 为可以 1 为不可以）
(26)isEqually(s, t, b):是否相同；isEqually(s,t)
(27)isBetween(s, Nmin, Nmax, b):数字在最大值和最小值之间；isBetween(s,Nmax,Nmin)
isMin(s, Nmin,b):数字最小值
(28)IsSafe(s, b):是否安全密码； IsSafe(s)   b 为是否可以为空（0 为可以 1 为不可以）
(29)isLongitude(s, b):经度； isLongitude(s)   b 为是否可以为空（0 为可以 1 为不可以）
(30)isLongitude2(s, b):经度中的度的验证
(31)IsLatitude(s, b):纬度； IsLatitude(s)  b 为是否可以为空（0 为可以 1 为不可以）
(32)IsLatitude2(s, b):纬度中的度的验证
(33)IsMinSec(s, b):经纬度中分秒的验证
(34)isAddess(s, b):是否地址（可以包含中文、英文、数字、“，”、“,”、“-”、“#”、空格的字符串，最大长度为200）； s 为this, b 为是否可以为空（0 为可以 1 为不可以）
(35)isName(s, b):是否姓名（包含中文、英文、空格、名称间隔符·的字符串，最大长度为20）； s 为this, b 为是否可以为空（0 为可以 1 为不可以）
(36)fixtime(number):将数字格式化
(37)isRTime(s, t, b):两个日期是否前小后大；isRTime(s,t) 由于日期输入都是采用日期控件，就不再进行日期格式的验证了
(38)isCode(s, b):是否编码（包含英文、数字、“_”，只能以英文和“_”作为开头的字符串，最大长度为20）； s 为this, b 为是否可以为空（0 为可以 1 为不可以）
(39)islength(s, minlength, maxlength):长度限制；
(40)isConcentration(s, b):是否浓度（区间[0,1]的正浮点型或0、1）；b 为是否可以为空（0 为可以 1 为不可以）
(41)isPH(s, b):是否PH值（区间[0,14]的正浮点型或0、14）；b 为是否可以为空（0 为可以 1 为不可以）
(42)CheckFileFormat(imgFile, b):验证文档的类型CheckFileFormat(s,b); 0为可以为空，1为不可空,文档格式：doc,txt,pdf,xls,docx,xlsx
(43)SelectIsNull(s):判断下拉框是否进行了选择
**/


//设置当前焦点文本框样式 s 为this
function setStyle(s) {
    if(s.type != "select-one")
    {
        s.style.backgroundColor = "#D4FFFF";
        s.style.borderStyle = "groove";
    }
}

//还原失去焦点文本框样式 s 为this
function outStyle(s) {
    if(s.type != "select-one")
    {
        s.style.backgroundColor = "#FFFFFF";
        s.style.borderStyle = "groove";
    }
}

//清除错误提示 s 为this
function ClearState(s) {
    var lastNode = s.parentNode.childNodes[s.parentNode.childNodes.length - 1];
    if (lastNode.id == "__ErrorMessagePanel")
        s.parentNode.removeChild(lastNode);
}

//1.通用的添加错误提示 s 为this
function CreatState(s, msg) {
    var span = document.createElement("SPAN");
    span.id = "__ErrorMessagePanel";
    span.style.color = "red";
    s.parentNode.appendChild(span);
    span.innerHTML = " * " + msg;
}
//2.页面加载前给出提示信息，限定经度的度的值输入范围
function CreatState1(s, msg) {
    var span = document.createElement("SPAN");
    span.id = "__ErrorMessagePanel";
    span.style.color = "red";
    s.parentNode.appendChild(span);
    span.innerHTML = "*值在122-123之间" + msg;
}
//3.页面加载前给出提示信息，限定经度和纬度的分，秒的值输入范围
function CreatState2(s, msg) {
    var span = document.createElement("SPAN");
    span.id = "__ErrorMessagePanel";
    span.style.color = "red";
    s.parentNode.appendChild(span);
    span.innerHTML = "*值在0-60之间" + msg;
}
//4.页面加载前给出提示信息，限定纬度的度的值输入范围
function CreatState3(s, msg) {
    var span = document.createElement("SPAN");
    span.id = "__ErrorMessagePanel";
    span.style.color = "red";
    s.parentNode.appendChild(span);
    span.innerHTML = "*值在41-43之间" + msg;
}
//5.表单提交前遍历表单判断是否还有显示的错误未修改，在提交服务器端控件中加入OnClientClick="retrun isClearMsg();"
function isClearMsg() {
    var allitem = document.forms[0];
    for (var i = 1; i < allitem.length - 1; i++) {
        try {
            allitem[i].focus();
            allitem[i].blur();
        } catch (e) { }
    }
    if (window.document.getElementById("__ErrorMessagePanel") == null) {
        return true;
    }
    else {
        $.dialog.alert("红色*处内容为必填项！请检查是否填写正确或完整！");
        //alert("红色*处内容为必填项！请检查是否填写正确或完整！");
        return false;
    }
}
//6.在页面加载前遍历表单
function allfocus() {

    var allitem = document.forms[0];

    for (var i = 1; i < allitem.length -1; i++) {
        try {
            allitem[i].focus();

        } catch (e) { }
    }

    //页面第一个文本框得到焦点  coin  ie8下加载光标闪动比较明显
    $("input[type='button']").first().focus();
    $("input[type='text']").first().focus();

}

//7.去掉前置和后置空格
function trim(str) {
    return str.replace(/(^\s*)|(\s*$)/g, "");
}
//8.是否为空；为了与原有isNull()函数区分
function isNull1(s) {
    ClearState(s);
    if (trim(s.value) == "") {
        CreatState(s, "");
        return true;
    } else {
        return false;
    }
}
//9.是否为空和输入值为0，都认为是无效
function isNull2(s) {
    ClearState(s);
    if (trim(s.value) == "" | trim(s.value) == "0") {
        CreatState(s, "");
        return true;
    } else {
        return false;
    }
}
//10.是否全部中文字符； s 为this, b 为是否可以为空（0 为可以 1 为不可以）
function isChinese(s, b) {

    ClearState(s);
    var pattern = /^[\u4e00-\u9fa5]*$/;
    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState(s, "");
            return false;
        }
    }
    else if (pattern.test(trim(s.value))) {
        return true;
    } else {
        CreatState(s, "包含非中文字符!");
        return false;
    }
}
//11.是否合法变量字符串 isRegisterUserName(s)
//只能输入5-20个以字母开头、可带数字、“_”、“.”的字串
// b 为是否可以为空（0 为可以 1 为不可以）
function isRegisterUserName(s, b) {
    ClearState(s);
    var pattern = /^[a-zA-Z]{1}([a-zA-Z0-9._]){4,19}$/;
    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState(s, "");
            return false;
        }
    }
    else if (pattern.test(trim(s.value))) {
        return true;
    } else {
        CreatState(s, "非法变量字符串,只能输入5-20个以字母开头、可带数字、“_”、“.”的字串!");
        return false;
    }
}

//12.英文字符；b 为是否可以为空（0 为可以 1 为不可以）
function isEnglish(s, b) {
    ClearState(s);
    var pattern = /^[A-Za-z]+$/;
    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState(s, "");
            return false;
        }
    }
    else if (pattern.test(trim(s.value))) {

        return true;
    } else {
        CreatState(s, "包含非英文字符!");
        return false;
    }
}
//13.是否数字；为了与原有isNaN()函数区分 ，b 为是否可以为空（0 为可以 1 为不可以）
function isNaN1(s, b) {
    ClearState(s);
    var pattern = /^\d+$/;
    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState(s, "");
            return false;
        }
    }
    else if (pattern.test(trim(s.value))) {
        return true;
    } else {
        CreatState(s, "包含非数字字符!");
        return false;
    }
}
//14.是否整数；b 为是否可以为空（0 为可以 1 为不可以）
function isInteger(s, b) {
    ClearState(s);
    var pattern = /^[-\+]?\d+$/;
    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState(s, "");
            return false;
        }
    }
    else if (pattern.test(trim(s.value))) {
        return true;
    } else {
        CreatState(s, "不是整数!");
        return false;
    }
}
//15.是否浮点型；b 为是否可以为空（0 为可以 1 为不可以）
function isDouble(s, b) {
    ClearState(s);
    var pattern = /^[-\+]?\d+(\.\d+)?$/;
    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState(s, "");
            return false;
        }
    }
    else if (pattern.test(trim(s.value))) {
        return true;
    } else {
        CreatState(s, "不是双精度浮点型!");
        return false;
    }
}
//16.Email地址；b 为是否可以为空（0 为可以 1 为不可以）
function isEmail(s, b) {
    ClearState(s);
    var re = /^([a-zA-Z0-9]+[_|-|.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|-|.]?)*[a-zA-Z0-9]+.[a-zA-Z]{2,3}$/; 

    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState(s, "");
            return false;
        }
    }
    else if (new RegExp(re).test(trim(s.value))) {
        return true;
    }
    else {
        CreatState(s, "格式有误,如aaa@126.com!");
        return false;
    }
}

//17.网址；isURL(s)  b 为是否可以为空（0 为可以 1 为不可以）
function isURL(s, b) {
    ClearState(s);
    //alert(s.value.toLowerCase());
    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState(s, "");
            return false;
        }
    }
    else if (new RegExp(/^http:\/\/[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\"\"])*$/).test(trim(s.value).toLowerCase())) {
        return true;
    }
    else {
        CreatState(s, "格式有误,如http://aaa.com!");
        return false;
    }
}

//18.普通电话（座机）、传真号码 ；telCheck(s) b 为是否可以为空（0 为可以 1 为不可以）
//可以“+”开头，除数字外，可含有“-”
function telCheck(s, b) {
    ClearState(s);
    //var area = {010: "北京", 021: "上海", 022: "天津", 023: "重庆", 0471: "呼和浩特市", 0479: "二连浩特市", 0478: "临河市", 0477: "东胜市", 0470: "满洲里市", 0476: "赤峰市", 0482: "乌兰浩特市", 0477: "鄂尔多斯市", 0482: "兴安盟", 0474: "乌兰察布盟", 0483: "阿拉善盟", 0474: "集宁市", 0472: "包头市", 0473: "乌海市", 0470: "海拉尔市", 0470: "牙克石市", 0479: "锡林浩特市", 0475: "通辽市", 0470: "呼伦贝尔市", 0479: "锡林郭勒盟", 0478: "巴彦淖尔盟", 0351: "太原市", 0350: "忻州市", 0357: "临汾市", 0359: "运城市", 0355: "长治市", 0349: "朔州市", 0358: "吕梁地区", 0354: "榆次市", 0352: "大同市", 0357: "侯马市", 0353: "阳泉市", 0356: "晋城市", 0354: "晋中市", 0311: "石家庄市", 0311: "辛集市", 0319: "邢台市", 0310: "邯郸市", 0317: "泊头市", 0315: "唐山市", 0316: "廊坊市", 0312: "保定市", 0312: "定州市", 024: "沈阳市", 0410: "铁岭市", 0413: "抚顺市", 0412: "海城市", 0411: "大连市", 0414: "本溪市", 0416: "锦州市", 0429: "兴城市", 0421: "北票市", 0427: "盘锦市", 0431: "长春市", 0432: "吉林市", 0433: "延吉市", 0433: "龙井市", 0435: "通化市", 0439: "浑江市", 0434: "四平市", 0437: "辽源市", 0436: "洮南市", 0438: "松原市", 0451: "哈尔滨市", 0451: "肇东市", 0458: "伊春市", 0468: "鹤岗市", 0469: "双鸭山市", 0453: "牡丹江市", 0467: "鸡西市", 0459: "大庆市", 0456: "黑河市", 0457: "大兴安岭地区", 025: "南京市", 0511: "镇江市", 0519: "常州市", 0510: "宜兴市", 0512: "苏州市", 0516: "徐州市", 0517: "淮阴市", 0527: "宿迁市", 0515: "东台市", 0523: "泰州市", 0513: "南通市", 0551: "合肥市", 0552: "蚌埠市", 0561: "淮北市", 0558: "毫州市", 0565: "巢湖市", 0553: "芜湖市", 0559: "黄山市", 0562: "铜陵市", 0556: "安庆市", 0531: "济南市", 0635: "临清市", 0533: "淄博市", 0546: "东营市", 0536: "诸城市", 0535: "烟台市", 0532: "青岛市", 0634: "莱芜市", 0537: "济宁市", 0530: "荷泽市", 0633: "日照市", 0632: "藤州市", 0571: "杭州市", 0571: "下城区", 0571: "江干区", 0575: "绍兴市", 0573: "嘉兴市", 0574: "宁波市", 0580: "舟山市", 0576: "椒江市", 0579: "兰溪市", 0570: "衢州市", 0577: "温州市", 0579: "东阳市", 0577: "乐清市", 0791: "南昌市", 0798: "景德镇市", 0701: "鹰潭市", 0790: "新余市", 0797: "赣州市", 0796: "井冈山市", 0794: "临川市", 0591: "福州市", 0599: "南平市", 0592: "厦门市", 0595: "石狮市", 0597: "龙岩市", 0598: "永安市", 0731: "长沙市", 0732: "湘乡市", 0737: "益阳市", 0730: "汨罗市", 0736: "津市市", 0744: "大庸市", 0738: "连源市", 0745: "怀化市", 0734: "衡阳市", 0739: "邵阳市", 0746: "永州市", 0744: "张家界市", 027: "武汉市", 0728: "天门市", 0712: "应城市", 0728: "仙桃市", 0716: "沙市市", 0724: "荆门市", 0711: "鄂州市", 0715: "咸宁市", 0715: "蒲昕市", 0710: "老河口市", 0719: "十堰市", 0717: "枝城市", 0718: "利川市", 0713: "黄冈市", 0371: "郑州市", 0391: "焦作市", 0392: "鹤壁市", 0374: "许昌市", 0396: "驻马店市", 0394: "周口市", 0379: "洛阳市", 0398: "义马市", 0378: "开封市", 020: "广州市", 0769: "东莞市", 0753: "梅州市", 0768: "潮州市", 0660: "汕尾市", 0755: "深圳市", 0668: "茂名市", 0757: "佛山市", 0750: "江门市", 0756: "珠海市", 0766: "云浮市", 0898: "海口市", 0898: "通什市", 0771: "南宁地区", 0776: "百色市", 0779: "北海市", 0773: "桂林市", 0760: "柳州地区", 0778: "河池市", 0775: "贵港市", 0851: "贵阳市", 0856: "铜仁地市", 0854: "都匀市", 0859: "兴义市", 0852: "赤水市", 0319: "南宫市", 0318: "衡水市", 0319: "沙河市", 0317: "沧州市", 0317: "任丘市", 0335: "秦皇岛市", 0314: "承德市", 0312: "涿州市", 0313: "张家口市", 0419: "辽阳市", 0410: "铁岭市", 0412: "鞍山市", 0417: "营口市", 0411: "瓦房店市", 0415: "丹东市", 0429: "锦西市", 0421: "朝阳市", 0418: "阜新市", 0429: "葫芦岛市", 0438: "扶余市", 0432: "桦甸市", 0433: "图门市", 0433: "敦化市", 0435: "集安市", 0448: "梅河口市", 0434: "公主岭市", 0436: "白城市", 0439: "白山市", 0433: "延边朝鲜族自治州", 0450: "阿城市", 0455: "绥化市", 0454: "佳木斯市", 0464: "七台河市", 0454: "同江市", 0453: "绥汾河市", 0452: "齐齐哈尔市", 0456: "北安市", 0456: "五大连池市", 0514: "仪征市", 0511: "丹阳市", 0510: "无锡市", 0510: "江阴市", 0520: "常熟市", 0518: "连云港市", 0517: "淮安市", 0515: "盐城市", 0514: "扬州市", 0523: "兴化市", 0554: "淮南市", 0557: "宿州市", 0558: "阜阳市", 0564: "六安市", 0550: "滁州市", 0563: "宣城市", 0555: "马鞍山市", 0566: "池州市", 0635: "聊城市", 0534: "德州市", 0543: "滨州市", 0536: "潍坊市", 0536: "青州市", 0631: "威海市", 0538: "泰安市", 0538: "新泰市", 0537: "曲阜市", 0539: "临沂市", 0632: "枣庄市", 0571: "上城区", 0571: "西湖区", 0571: "萧山市", 0572: "湖州市", 0573: "海宁市", 0574: "余姚市", 0576: "临海市", 0579: "金华市", 0578: "丽水市", 0570: "江山市", 0579: "义乌市", 0577: "瑞安市", 0576: "台州市", 0792: "九江市", 0793: "上饶市", 0795: "宜春市", 0799: "萍乡市", 0796: "吉安市", 0794: "抚州市", 0594: "莆田市", 0599: "邵武市", 0595: "泉州市", 0596: "漳州市", 0598: "三明市", 0593: "宁德市", 0732: "湘潭市", 0733: "株洲市", 0730: "岳阳市", 0736: "常德市", 0743: "吉首市", 0738: "娄底市", 0738: "冷水江市", 0745: "洪江市", 0734: "耒阳市", 0735: "彬州市", 0738: "冷水江市", 0743: "湘西土家族苗族自治州", 0713: "麻城市", 0712: "孝感市", 0712: "安陆市", 0728: "洪湖市", 0716: "石首市", 0714: "黄石市", 0713: "武穴市", 0710: "襄樊市", 0722: "随州市", 0719: "丹江口市", 0717: "宜昌市", 0718: "恩施土家族苗族自治州", 0716: "荆州市", 0373: "新乡市", 0372: "安阳市", 0393: "濮阳市", 0395: "缧河市", 0376: "信阳市", 0375: "平顶山市", 0398: "三门峡市", 0377: "南阳市", 0370: "商丘市", 0763: "清远市", 0751: "韶关市", 0754: "汕头市", 0752: "惠州市", 0762: "河源市", 0759: "湛江市", 0758: "肇庆市", 0760: "中山市", 0662: "阳江市", 0663: "揭阳市", 0898: "三亚市", 0898: "海南省", 0771: "凭祥市", 0777: "钦州市", 0775: "玉林市", 0774: "梧州市", 0760: "合山市", 0770: "防城港市", 0774: "贺州市", 0858: "六盘水市", 0855: "凯里市", 0853: "安顺市", 0852: "遵义市", 0857: "毕节地区", 0855: "黔东南苗族侗族自治州", 028: "都江堰市", 0813: "自贡市", 0830: "泸州市", 0816: "绵阳市", 0825: "遂宁市", 0833: "乐山市", 0833: "眉山市", 0826: "广安市", 0835: "雅安市", 0832: "资阳市", 0836: "甘孜藏族自治州", 0881: "东川市", 0870: "昭通市", 0873: "个旧市", 0878: "楚雄彝族自治州", 0879: "思茅地区", 0883: "临沧地区", 0876: "文山壮族苗族自治州", 0872: "大理白族自治州", 0886: "怒江傈僳族自治州", 0877: "玉溪市", 0910: "咸阳市", 0913: "韩城市", 0917: "宝鸡市", 0919: "铜川市", 0915: "安康市", 0943: "白银市", 0935: "武威市", 0937: "酒泉市", 0937: "玉门市", 0938: "天水市", 0934: "西峰市", 0932: "定西地区", 0941: "甘南藏族自治州", 0953: "吴忠市", 0952: "石嘴山市", 0979: "格尔木市", 0972: "海东地区", 0973: "黄南藏族自治州", 0975: "果洛藏族自治州", 0977: "海西蒙古族藏族自治州", 0994: "昌吉回族自治州", 0992: "奎屯市", 0990: "克拉玛依市", 0999: "伊宁市", 0995: "吐鲁番市", 0996: "库尔勒市", 0998: "喀什地区", 0903: "和田地区", 0908: "克孜勒苏柯尔克孜自治州", 0892: "日喀则地区", 0893: "山南地区", 0897: "阿里地区", 008862: "基隆", 008866: "台南", 008868: "台东", 008862: "台北", 008864: "台中", 008867: "高雄", 000852: "香港特别行政区", 000853: "澳门特别行政区" };
    var area = { "010": "北京", "021": "上海", "022": "天津", "023": "重庆", "0471": "呼和浩特市", "0479": "二连浩特市", "0478": "临河市", "0477": "东胜市", "0470": "满洲里市", "0476": "赤峰市", "0482": "乌兰浩特市", "0477": "鄂尔多斯市", "0482": "兴安盟", "0474": "乌兰察布盟", "0483": "阿拉善盟", "0474": "集宁市", "0472": "包头市", "0473": "乌海市", "0470": "海拉尔市", "0470": "牙克石市", "0479": "锡林浩特市", "0475": "通辽市", "0470": "呼伦贝尔市", "0479": "锡林郭勒盟", "0478": "巴彦淖尔盟", "0351": "太原市", "0350": "忻州市", "0357": "临汾市", "0359": "运城市", "0355": "长治市", "0349": "朔州市", "0358": "吕梁地区", "0354": "榆次市", "0352": "大同市", "0357": "侯马市", "0353": "阳泉市", "0356": "晋城市", "0354": "晋中市", "0311": "石家庄市", "0311": "辛集市", "0319": "邢台市", "0310": "邯郸市", "0317": "泊头市", "0315": "唐山市", "0316": "廊坊市", "0312": "保定市", "0312": "定州市", "024": "沈阳市", "0410": "铁岭市", "0413": "抚顺市", "0412": "海城市", "0411": "大连市", "0414": "本溪市", "0416": "锦州市", "0429": "兴城市", "0421": "北票市", "0427": "盘锦市", "0431": "长春市", "0432": "吉林市", "0433": "延吉市", "0433": "龙井市", "0435": "通化市", "0439": "浑江市", "0434": "四平市", "0437": "辽源市", "0436": "洮南市", "0438": "松原市", "0451": "哈尔滨市", "0451": "肇东市", "0458": "伊春市", "0468": "鹤岗市", "0469": "双鸭山市", "0453": "牡丹江市", "0467": "鸡西市", "0459": "大庆市", "0456": "黑河市", "0457": "大兴安岭地区", "025": "南京市", "0511": "镇江市", "0519": "常州市", "0510": "宜兴市", "0512": "苏州市", "0516": "徐州市", "0517": "淮阴市", "0527": "宿迁市", "0515": "东台市", "0523": "泰州市", "0513": "南通市", "0551": "合肥市", "0552": "蚌埠市", "0561": "淮北市", "0558": "毫州市", "0565": "巢湖市", "0553": "芜湖市", "0559": "黄山市", "0562": "铜陵市", "0556": "安庆市", "0531": "济南市", "0635": "临清市", "0533": "淄博市", "0546": "东营市", "0536": "诸城市", "0535": "烟台市", "0532": "青岛市", "0634": "莱芜市", "0537": "济宁市", "0530": "荷泽市", "0633": "日照市", "0632": "藤州市", "0571": "杭州市", "0571": "下城区", "0571": "江干区", "0575": "绍兴市", "0573": "嘉兴市", "0574": "宁波市", "0580": "舟山市", "0576": "椒江市", "0579": "兰溪市", "0570": "衢州市", "0577": "温州市", "0579": "东阳市", "0577": "乐清市", "0791": "南昌市", "0798": "景德镇市", "0701": "鹰潭市", "0790": "新余市", "0797": "赣州市", "0796": "井冈山市", "0794": "临川市", "0591": "福州市", "0599": "南平市", "0592": "厦门市", "0595": "石狮市", "0597": "龙岩市", "0598": "永安市", "0731": "长沙市", "0732": "湘乡市", "0737": "益阳市", "0730": "汨罗市", "0736": "津市市", "0744": "大庸市", "0738": "连源市", "0745": "怀化市", "0734": "衡阳市", "0739": "邵阳市", "0746": "永州市", "0744": "张家界市", "027": "武汉市", "0728": "天门市", "0712": "应城市", "0728": "仙桃市", "0716": "沙市市", "0724": "荆门市", "0711": "鄂州市", "0715": "咸宁市", "0715": "蒲昕市", "0710": "老河口市", "0719": "十堰市", "0717": "枝城市", "0718": "利川市", "0713": "黄冈市", "0371": "郑州市", "0391": "焦作市", "0392": "鹤壁市", "0374": "许昌市", "0396": "驻马店市", "0394": "周口市", "0379": "洛阳市", "0398": "义马市", "0378": "开封市", "020": "广州市", "0769": "东莞市", "0753": "梅州市", "0768": "潮州市", "0660": "汕尾市", "0755": "深圳市", "0668": "茂名市", "0757": "佛山市", "0750": "江门市", "0756": "珠海市", "0766": "云浮市", "0898": "海口市", "0898": "通什市", "0771": "南宁地区", "0776": "百色市", "0779": "北海市", "0773": "桂林市", "0760": "柳州地区", "0778": "河池市", "0775": "贵港市", "0851": "贵阳市", "0856": "铜仁地市", "0854": "都匀市", "0859": "兴义市", "0852": "赤水市", "0319": "南宫市", "0318": "衡水市", "0319": "沙河市", "0317": "沧州市", "0317": "任丘市", "0335": "秦皇岛市", "0314": "承德市", "0312": "涿州市", "0313": "张家口市", "0419": "辽阳市", "0410": "铁岭市", "0412": "鞍山市", "0417": "营口市", "0411": "瓦房店市", "0415": "丹东市", "0429": "锦西市", "0421": "朝阳市", "0418": "阜新市", "0429": "葫芦岛市", "0438": "扶余市", "0432": "桦甸市", "0433": "图门市", "0433": "敦化市", "0435": "集安市", "0448": "梅河口市", "0434": "公主岭市", "0436": "白城市", "0439": "白山市", "0433": "延边朝鲜族自治州", "0450": "阿城市", "0455": "绥化市", "0454": "佳木斯市", "0464": "七台河市", "0454": "同江市", "0453": "绥汾河市", "0452": "齐齐哈尔市", "0456": "北安市", "0456": "五大连池市", "0514": "仪征市", "0511": "丹阳市", "0510": "无锡市", "0510": "江阴市", "0520": "常熟市", "0518": "连云港市", "0517": "淮安市", "0515": "盐城市", "0514": "扬州市", "0523": "兴化市", "0554": "淮南市", "0557": "宿州市", "0558": "阜阳市", "0564": "六安市", "0550": "滁州市", "0563": "宣城市", "0555": "马鞍山市", "0566": "池州市", "0635": "聊城市", "0534": "德州市", "0543": "滨州市", "0536": "潍坊市", "0536": "青州市", "0631": "威海市", "0538": "泰安市", "0538": "新泰市", "0537": "曲阜市", "0539": "临沂市", "0632": "枣庄市", "0571": "上城区", "0571": "西湖区", "0571": "萧山市", "0572": "湖州市", "0573": "海宁市", "0574": "余姚市", "0576": "临海市", "0579": "金华市", "0578": "丽水市", "0570": "江山市", "0579": "义乌市", "0577": "瑞安市", "0576": "台州市", "0792": "九江市", "0793": "上饶市", "0795": "宜春市", "0799": "萍乡市", "0796": "吉安市", "0794": "抚州市", "0594": "莆田市", "0599": "邵武市", "0595": "泉州市", "0596": "漳州市", "0598": "三明市", "0593": "宁德市", "0732": "湘潭市", "0733": "株洲市", "0730": "岳阳市", "0736": "常德市", "0743": "吉首市", "0738": "娄底市", "0738": "冷水江市", "0745": "洪江市", "0734": "耒阳市", "0735": "彬州市", "0738": "冷水江市", "0743": "湘西土家族苗族自治州", "0713": "麻城市", "0712": "孝感市", "0712": "安陆市", "0728": "洪湖市", "0716": "石首市", "0714": "黄石市", "0713": "武穴市", "0710": "襄樊市", "0722": "随州市", "0719": "丹江口市", "0717": "宜昌市", "0718": "恩施土家族苗族自治州", "0716": "荆州市", "0373": "新乡市", "0372": "安阳市", "0393": "濮阳市", "0395": "缧河市", "0376": "信阳市", "0375": "平顶山市", "0398": "三门峡市", "0377": "南阳市", "0370": "商丘市", "0763": "清远市", "0751": "韶关市", "0754": "汕头市", "0752": "惠州市", "0762": "河源市", "0759": "湛江市", "0758": "肇庆市", "0760": "中山市", "0662": "阳江市", "0663": "揭阳市", "0898": "三亚市", "0898": "海南省", "0771": "凭祥市", "0777": "钦州市", "0775": "玉林市", "0774": "梧州市", "0760": "合山市", "0770": "防城港市", "0774": "贺州市", "0858": "六盘水市", "0855": "凯里市", "0853": "安顺市", "0852": "遵义市", "0857": "毕节地区", "0855": "黔东南苗族侗族自治州", "028": "都江堰市", "0813": "自贡市", "0830": "泸州市", "0816": "绵阳市", "0825": "遂宁市", "0833": "乐山市", "0833": "眉山市", "0826": "广安市", "0835": "雅安市", "0832": "资阳市", "0836": "甘孜藏族自治州", "0881": "东川市", "0870": "昭通市", "0873": "个旧市", "0878": "楚雄彝族自治州", "0879": "思茅地区", "0883": "临沧地区", "0876": "文山壮族苗族自治州", "0872": "大理白族自治州", "0886": "怒江傈僳族自治州", "0877": "玉溪市", "0910": "咸阳市", "0913": "韩城市", "0917": "宝鸡市", "0919": "铜川市", "0915": "安康市", "0943": "白银市", "0935": "武威市", "0937": "酒泉市", "0937": "玉门市", "0938": "天水市", "0934": "西峰市", "0932": "定西地区", "0941": "甘南藏族自治州", "0953": "吴忠市", "0952": "石嘴山市", "0979": "格尔木市", "0972": "海东地区", "0973": "黄南藏族自治州", "0975": "果洛藏族自治州", "0977": "海西蒙古族藏族自治州", "0994": "昌吉回族自治州", "0992": "奎屯市", "0990": "克拉玛依市", "0999": "伊宁市", "0995": "吐鲁番市", "0996": "库尔勒市", "0998": "喀什地区", "0903": "和田地区", "0908": "克孜勒苏柯尔克孜自治州", "0892": "日喀则地区", "0893": "山南地区", "0897": "阿里地区", "008862": "基隆", "008866": "台南", "008868": "台东", "008862": "台北", "008864": "台中", "008867": "高雄", "000852": "香港特别行政区", "000853": "澳门特别行政区" };
    //    alert(area[s.value.split("-")[0]]);
    //    alert(area[parseInt(s.value.split("-")[0])]);
    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState(s, "不能为空!");
            return false;
        }
    }
    //else if ((new RegExp(/^0\d{2,3}[-][1-9]\d{6,7}$/).test(trim(s.value))) && (area[parseInt(trim(s.value).split("-")[0])] != null)) {
    else if ((new RegExp(/^0\d{2,3}[-][1-9]\d{6,7}$/).test(s.value)) && (area[s.value.split("-")[0]] != null)) {
        return true;
    }
    else {
        CreatState(s, "区号或格式有误,如010-12345678!");
        return false;
    }
}

//19.手机号码 ；mobileCheck(s) b 为是否可以为空（0 为可以 1 为不可以）
//必须以数字开头，除数字外，可含有“-”
function mobileCheck(s, b) {
    ClearState(s);
    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState(s, "");
            return false;
        }
    }
    else if (new RegExp(/^0{0,1}1(3|5|8)[0-9]{9}$/).test(trim(s.value))) {
        return true;
    }
    else {
        CreatState(s, "格式有误,如13823456789!");
        return false;
    }
}

//20.电话号码（座机或手机）；PhoneCheck(s) b 为是否可以为空（0 为可以 1 为不可以）
//(1)电话号码由数字、"("、")"和"-"构成
//(2)电话号码为3到8位
//(3)如果电话号码中包含有区号，那么区号为三位或四位
//(4)区号用"("、")"或"-"和其他部分隔开
//(5)移动电话号码为11或12位，如果为12位,那么第一位为0
//(6)11位移动电话号码的第一位和第二位为"13"或"15"
//(7)12位移动电话号码的第二位和第三位为"13"或"15"
function PhoneCheck(s, b) {
    ClearState(s);
    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState(s, "");
            return false;
        }
    }
    else if (new RegExp(/(^[0-9]{3,4}\-[0-9]{7,8}$)|(^[0-9]{7,8}$)|(^\([0-9]{3,4}\)[0-9]{7,8}$)|(^0{0,1}1(3|5|8)[0-9]{9}$)/).test(trim(s.value))) {
        return true;
    }
    else {
        //        CreatState(s, "格式有误,如13823456789,010-12345678,12345678,(010)12345678!");
        CreatState(s, "格式有误,必须是手机、电话号码");
        return false;
    }
}


//21.邮政编码；b 为是否可以为空（0 为可以 1 为不可以）
function isPostalCode(s, b) {
    ClearState(s);
    var patrn = /^[1-9]{1}(\d){5}$/;
    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState(s, "");
            return false;
        }
    }
    else if (!patrn.exec(trim(s.value))) {

        CreatState(s, "格式有误，如100010!");
        return false;
    }
    else {
        return true;
    }
}

//22.身份证号码验证（不是太严格）；b 为是否可以为空（0 为可以 1 为不可以）

function isIdCardNo1(s, b) {
    ClearState(s);
    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState(s, "");
            return false;
        }
    }
    var pattern1 = /^\d+$/;
    var pattern2 = /^[A-Za-z]+$/;
    if (!pattern1.test(trim(s.value).substring(0, s.value.length - 1))) { CreatState(s, "除最后一位外，其余必须为数字！"); return false; }
    if (!pattern1.test(trim(s.value).substring(s.value.length - 1)) && (!pattern2.test(trim(s.value).substring(s.value.length - 1)))) { CreatState(s, "最后一位必须为数字或字母！"); return false; }
    var len = s.value.length, re;
    //alert(len);
    if (len == 15)
        re = new RegExp(/^(\d{6})()?(\d{2})(\d{2})(\d{2})(\d{3})$/);
    else if (len == 18)
        re = new RegExp(/^(\d{6})()?(\d{4})(\d{2})(\d{2})(\d{3})(\d)$/);
    else {
        CreatState(s, "位数不对！");
        return false;
    }
    var a = trim(s.value).match(re);
    if (a != null) {
        if (len == 15) {
            var D = new Date("19" + a[3] + "/" + a[4] + "/" + a[5]);
            var B = D.getYear() == a[3] && (D.getMonth() + 1) == a[4] && D.getDate() == a[5];
        }
        else {
            var D = new Date(a[3] + "/" + a[4] + "/" + a[5]);
            var B = D.getFullYear() == a[3] && (D.getMonth() + 1) == a[4] && D.getDate() == a[5];
        }
        if (!B) { CreatState(s, "输入的身份证号 " + a[0] + " 里出生日期不对！"); return false; }
    }
    return true;
}

//23.身份证号码验证（严格判定）；b 为是否可以为空（0 为可以 1 为不可以）

function isIdCardNo(s, b) {
    ClearState(s);
    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState(s, "不能为空!");
            return false;
        }
    }
    var Errors = new Array(
                "身份证验证通过!",
                "身份证号码位数不对!",
                "身份证号码出生日期超出范围或含有非法字符!",
                "身份证号码校验错误!",
                "身份证地区非法!"
                );
    var area = { 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外" }

    var idcard, Y, JYM;
    var Ss, M;
    var idcard_array = new Array();
    idcard = s.value;
    idcard_array = idcard.split("");
    //地区检验
    if (area[parseInt(idcard.substr(0, 2))] == null) {
        //alert(Errors[4]);
        CreatState(s, Errors[4]);
        return false;
    }
    //身份号码位数及格式检验
    switch (idcard.length) {
        case 15:
            if ((parseInt(idcard.substr(6, 2)) + 1900) % 4 == 0 || ((parseInt(idcard.substr(6, 2)) + 1900) % 100 == 0 && (parseInt(idcard.substr(6, 2)) + 1900) % 4 == 0)) {
                ereg = /^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}$/; //测试出生日期的合法性
            } else {
                ereg = /^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}$/; //测试出生日期的合法性
            }
            if (ereg.test(idcard)) {
                //alert(Errors[0]);
                CreatState(s, Errors[0]);
                return true;
            }
            else {
                //alert(Errors[2]);
                CreatState(s, Errors[2]);
                return false;
            }
            break;
        case 18:
            //18位身份号码检测
            //出生日期的合法性检查
            //闰年月日:((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))
            //平年月日:((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))
            if (parseInt(idcard.substr(6, 4)) % 4 == 0 || (parseInt(idcard.substr(6, 4)) % 100 == 0 && parseInt(idcard.substr(6, 4)) % 4 == 0)) {
                ereg = /^[1-9][0-9]{5}[1-2][0-9][0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}[0-9Xx]$/; //闰年出生日期的合法性正则表达式
            } else {
                ereg = /^[1-9][0-9]{5}[1-2][0-9][0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}[0-9Xx]$/; //平年出生日期的合法性正则表达式
            }
            if (ereg.test(idcard)) {//测试出生日期的合法性
                //计算校验位
                Ss = (parseInt(idcard_array[0]) + parseInt(idcard_array[10])) * 7
                    + (parseInt(idcard_array[1]) + parseInt(idcard_array[11])) * 9
                    + (parseInt(idcard_array[2]) + parseInt(idcard_array[12])) * 10
                    + (parseInt(idcard_array[3]) + parseInt(idcard_array[13])) * 5
                    + (parseInt(idcard_array[4]) + parseInt(idcard_array[14])) * 8
                    + (parseInt(idcard_array[5]) + parseInt(idcard_array[15])) * 4
                    + (parseInt(idcard_array[6]) + parseInt(idcard_array[16])) * 2
                    + parseInt(idcard_array[7]) * 1
                    + parseInt(idcard_array[8]) * 6
                    + parseInt(idcard_array[9]) * 3;
                Y = Ss % 11;
                M = "F";
                JYM = "10X98765432";
                M = JYM.substr(Y, 1); //判断校验位
                if (M == idcard_array[17]) {
                    //alert(Errors[0]);

                    return true;
                } //检测ID的校验位
                else {
                    //alert(Errors[3]);
                    CreatState(s, Errors[3]);
                    return false;
                }

            }
            else {
                //alert(Errors[2]);
                CreatState(s, Errors[2]);
                return false;
            }
            break;
        default:
            //alert(Errors[1]);
            CreatState(s, Errors[1]);
            return false;
            break;
    }

}


//24.ip地址  b 为是否可以为空（0 为可以 1 为不可以）

function isIp(s, b) {
    ClearState(s);
    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState(s, "不能为空!");
            return false;
        }
    }
    var check = function(v) { try { return (v <= 255 && v >= 0) } catch (x) { return false } };
    var re = s.value.split(".");

    if (((re.length == 4) && (re[0] != "") && (re[1] != "") && (re[2] != "") && (re[3] != "")) ? (check(re[0]) && check(re[1]) && check(re[2]) && check(re[3])) : false) {
        return true;
    }
    else {
        CreatState(s, "格式错误，如192.168.1.1!");
        return false;
    }
}


//25.日期；isDate(s)（格式：yyyy-mm-dd）b 为是否可以为空（0 为可以 1 为不可以）

function isDate(s, b) {
    ClearState(s);

    var patrn = /^[12]{1}(\d){3}[-][01]?(\d){1}[-][0123]?(\d){1}$/;
    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState(s, "不能为空!");
            return false;
        }
    }
    else if (!patrn.exec(s.value)) {
        CreatState(s, "格式错误，如2011-01-01!");
        return false;
    }
    else {
        if (parseInt(s.value.substr(0, 4)) % 4 == 0 || (parseInt(s.value.substr(0, 4)) % 100 == 0 && parseInt(s.value.substr(0, 4)) % 4 == 0)) {
            ereg = /^[1-2][0-9][0-9]{2}[-]((01|03|05|07|08|10|12|1|3|5|7|8)([-]0[1-9]|[-][1-2][0-9]|[-]3[0-1]|[-][1-9])|(04|06|09|11|4|6|9)([-]0[1-9]|[-][1-2][0-9]|[-]30|[-][1-9])|(02|2)([-]0[1-9]|[-][1-2][0-9]|[-][1-9]))$/; //闰年出生日期的合法性正则表达式
        } else {
            ereg = /^[1-2][0-9][0-9]{2}[-]((01|03|05|07|08|10|12|1|3|5|7|8)([-]0[1-9]|[-][1-2][0-9]|[-]3[0-1]|[-][1-9])|(04|06|09|11|4|6|9)([-]0[1-9]|[-][1-2][0-9]|[-]30|[-][1-9])|(02|2)([-]0[1-9]|[-]1[0-9]|[-]2[0-8]|[-][1-9]))$/; //平年出生日期的合法性正则表达式
        }
        if (!ereg.test(s.value)) {
            CreatState(s, "格式错误,日期不合法!");
            return false;
        }
        else {
            return true;
        }
    }
}

//26.是否相同；isEqually(s,t)
function isEqually(s, t, b) {
    ClearState(s);
    ClearState(t);
    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState(s, "不能为空!");
            return false;
        }
    }
    else if (s.value == t.value) {
        return true;
    }
    else {
        CreatState(s, "重复值不相同!");
        return false;
    }
}

//27.数字在最大值和最小值之间；isBetween(s,Nmax,Nmin);

function isBetween(s, Nmin, Nmax, b) {
    ClearState(s);
    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState(s, "不能为空!");
            return false;
        }
    }
    else if ((parseFloat(s.value) <= parseFloat(Nmax)) && (parseFloat(s.value) >= parseFloat(Nmin))) {
        return true;
    }
    else {
        CreatState(s, "数值不在" + Nmin + "和" + Nmax + "之间!");
        return false;
    }
}

function isMin(s, Nmin,b) {


    ClearState(s);
    var pattern = /^[-\+]?\d+(\.\d+)?$/;
    if (trim(s.value) == "") {
      
    }
    else if (pattern.test(trim(s.value))) {
        
    } else {
        CreatState(s, "不是双精度浮点型!");
        return false;
    }



    ClearState(s);

    
    
    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState(s, "不能为空!");
            return false;
        }
    }
    else if (parseFloat(s.value) >= parseFloat(Nmin)) {
        return true;
    }
    else {
        CreatState(s, "数值不能小于" + Nmin + "!");
        return false;
    }
}

//28.是否安全密码； IsSafe(s)   b 为是否可以为空（0 为可以 1 为不可以）

function IsSafe(s, b) {
    ClearState(s);

    var patrn = /^(([A-Z]*|[a-z]*|\d*|[-_\~!@#\$%\^&\*\.\(\)\[\]\{\}<>\?\\\/\'\"]*)|.{0,5})$|\s/;
    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState(s, "不能为空!");
            return false;
        }
    }
    else if (s.value.length > 20) {
        CreatState(s, "密码长度不能超过20!");
        return false;
    }
    else if (patrn.exec(s.value)) {
        CreatState(s, "密码安全级别较低!");
        return false;
    }
    else {
        return true;
    }
}


//29.经度； isLongitude(s)   b 为是否可以为空（0 为可以 1 为不可以）
function isLongitude(s, b) {
    ClearState(s);
    var pattern = /^[-\+]?\d+(\.\d+)?$/;
    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState(s, "不能为空!");
            return false;
        }
    }
    else if (!pattern.test(s.value)) {
        CreatState(s, "经度值应为浮点型！");
        return false;
    }
    else if ((s.value < 0) || (s.value > 180)) {
        CreatState(s, "经度值应在0-180之间！");
        return false;
    }
    else if (s.value.indexOf(".") == -1) {
        s.value = s.value + ".000000";
        return true;
    }
    else if ((s.value.substring(s.value.indexOf(".") + 1).length < 6)) {
        for (i = 0; i <= 6 - s.value.substring(s.value.indexOf(".") + 1).length; i++)
            s.value = s.value + "0";
        return true;
    }
    else if ((s.value.substring(s.value.indexOf(".") + 1).length > 6)) {

        s.value = s.value.substring(0, s.value.indexOf(".") + 7);
        return true;
    }
    else {
        return true;
    }
}
//30.经度中的度的验证
function isLongitude2(s, b) {
    ClearState(s);
    var pattern = /^\d+$/;
    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState1(s, "");
            return false;
        }
    }
    else if ((s.value < 122) || (s.value > 123)) {
        CreatState(s, "值应在122-123之间！");
        return false;
    }
    else if (pattern.test(s.value)) {
        return true;
    }
    else {
        CreatState(s, "包含非数字字符!");
        return false;
    }
}

//31.纬度； IsLatitude(s)  b 为是否可以为空（0 为可以 1 为不可以）
function IsLatitude(s, b) {
    ClearState(s);
    var pattern = /^[-\+]?\d+(\.\d+)?$/;
    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState(s, "不能为空!");
            return false;
        }
    }
    else if (!pattern.test(s.value)) {
        CreatState(s, "纬度值应为浮点型!");
        return false;
    }
    else if ((s.value < 0) || (s.value > 90)) {
        CreatState(s, "纬度值应在0-90之间！");
        return false;
    }
    else if (s.value.indexOf(".") == -1) {
        s.value = s.value + ".000000";
        return true;
    }
    else if ((s.value.substring(s.value.indexOf(".") + 1).length < 6)) {
        for (i = 0; i <= 6 - s.value.substring(s.value.indexOf(".") + 1).length; i++)
            s.value = s.value + "0";
        return true;
    }
    else if ((s.value.substring(s.value.indexOf(".") + 1).length > 6)) {

        s.value = s.value.substring(0, s.value.indexOf(".") + 7);
        return true;
    }
    else {
        return true;
    }
}
//32.纬度中的度的验证
function IsLatitude2(s, b) {
    ClearState(s);
    var pattern = /^\d+$/;
    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState3(s, "");
            return false;
        }
    }
    else if ((s.value < 41) || (s.value > 43)) {
        CreatState(s, "值应在41-43之间！");
        return false;
    }
    else if (pattern.test(s.value)) {
        return true;
    }
    else {
        CreatState(s, "包含非数字字符!");
        return false;
    }
}
//33.经纬度中分秒的验证
function IsMinSec(s, b) {
    ClearState(s);
    var pattern = /^\d+$/;
    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState2(s, "");
            return false;
        }
    }
    else if ((s.value < 0) || (s.value > 60)) {
        CreatState(s, "值应在0-60之间！");
        return false;
    }
    else if (pattern.test(s.value)) {
        return true;
    }
    else {
        CreatState(s, "包含非数字字符!");
        return false;
    }
}
//34.是否地址（可以包含中文、英文、数字、“，”、“,”、“-”、“#”、空格的字符串，最大长度为200）； s 为this, b 为是否可以为空（0 为可以 1 为不可以）
function isAddess(s, b) {

    ClearState(s);
    var pattern = /^(\w|[\u4e00-\u9fa5]|\-|\,|，|\s|#)*$/;
    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState(s, "不能为空!");
            return false;
        }
    }
    else if (s.value.length > 200) {
        CreatState(s, "长度超过200!");
        return false;
    }
    else if (pattern.test(s.value)) {
        return true;
    } else {
        CreatState(s, "不能包含中文、英文、数字、空格、逗号、#、-外的字符!");
        return false;
    }

}
//35.是否姓名（包含中文、英文、空格、名称间隔符·的字符串，最大长度为20）； s 为this, b 为是否可以为空（0 为可以 1 为不可以）
function isName(s, b) {
    //alert(s.value.replace(" ", ""));
    ClearState(s);
    // var pattern1 = /^([\u4e00-\u9fa5]|\ |\·)*$/;
    // var pattern2 = /^([A-Za-z]|\ |\·)+$/;

    var pattern1 = /^([\u4e00-\u9fa5]|[A-Za-z]|\ |\·)*$/;
    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState(s, "不能为空!");
            return false;
        }
    }
    else if (s.value.length > 20) {
        CreatState(s, "长度超过20!");
        return false;
    }
    else if ((pattern1.test(s.value))) {
        return true;
    } else {
        CreatState(s, "姓名格式不标准!");
        return false;
    }

}
//36.将数字格式化
function fixtime(number) {
    if (number < 10) {
        number = "0" + number;
    }
    return number;
}

//37.两个日期是否前小后大；isRTime(s,t) 由于日期输入都是采用日期控件，就不再进行日期格式的验证了
function isRTime(s, t, b) {
    var now = new Date();
    var strnow = now.getYear() + "-" + fixtime(now.getMonth() + 1) + "-" + fixtime(now.getDate()) + " " + fixtime(now.getHours()) + ":" + fixtime(now.getMinutes()) + ":" + fixtime(now.getSeconds());
    //alert(strnow);
    //alert(s.value);
    ClearState(s);
    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState(s, "不能为空!");
            return false;
        }
    }
    else if ((s.value > t.value) && (s.value < strnow) && (t.value < strnow)) {
        return true;
    }
    else {
        CreatState(s, "前时间应该小于后时间，并都小于当前时间!");
        return false;
    }
}

//38.是否编码（包含英文、数字、“_”，只能以英文和“_”作为开头的字符串，最大长度为20）； s 为this, b 为是否可以为空（0 为可以 1 为不可以）
function isCode(s, b) {

    ClearState(s);
    var pattern1 = /^([A-Za-z]|\_){1}([A-Za-z]|[0-9]|\_)*$/;

    if (trim(s.value) == "") {
        if (b == 0) {
            return true;
        }
        else {
            CreatState(s, "不能为空!");
            return false;
        }
    }
    else if (s.value.length > 20) {
        CreatState(s, "长度超过20!");
        return false;
    }
    else if (pattern1.test(s.value)) {
        return true;
    } else {
        CreatState(s, "编码格式不标准!");
        return false;
    }

}
//39.长度限制；
function islength(s, minlength, maxlength) {
    if ((s.value.length > maxlength) || (s.value.length < minlength)) {
        CreatState(s, "长度不在" + minlength + "-" + maxlength + "之间!");
        return false;
    }
    else {
        return true;
    }
}
//40.是否浓度（区间[0,1]的正浮点型或0、1）；b 为是否可以为空（0 为可以 1 为不可以）
function isConcentration(s, b) {

    if (isDouble(s, b)) {//if (isDouble(s, b) && (b == 1)) {
        if ((s.value < 0) || (s.value > 1)) {
            CreatState(s, "浓度应在[0,1]区间内!");
            return false;
        }
        else if ((s.value == 0) || (s.value == 1)) {
            return true;
        }
        else if (s.value.substring(s.value.indexOf(".") + 1).length > 2) {
            CreatState(s, "小数点后长度最多保留2位!");
            return false;
        }
        else {
            return true;
        }
    }

}

//41.是否PH值（区间[0,14]的正浮点型或0、14）；b 为是否可以为空（0 为可以 1 为不可以）
function isPH(s, b) {

    if (isDouble(s, b)) { //if (isDouble(s, b) && (b == 1)) {
        if ((s.value < 0) || (s.value > 14)) {
            CreatState(s, "PH值应在[0,14]区间内!");
            return false;
        }
        else if (s.value.substring(s.value.indexOf(".") + 1).length > 2) {
            CreatState(s, "小数点后长度最多保留2位!");
            return false;
        }
        else {
            return true;
        }
    }

}
//42.验证文档的类型CheckFileFormat(s,b); 0为可以为空，1为不可空
function CheckFileFormat(imgFile, b) {
    var fileformat = imgFile.value;
    ClearState(imgFile);
    if (b == 1) {
        if (fileformat != "") {

            var position = fileformat.lastIndexOf(".");
            var format = fileformat.substring(position + 1, fileformat.length)
            format = format.toLowerCase();
            if (format == "doc" | format == "docx" | format == "txt" | format == "pdf" | format == "xls" | format == "xlsx") {
                return true;
            }
            else {

                CreatState(imgFile, "请上传doc,docx,txt,pdf,xls,xlsx格式的文件!");
                return false;
            }
        }
        else {

            CreatState(imgFile, "");
            return false;
        }
    }
    if (b == 0) {
        if (fileformat != "") {

            var position = fileformat.lastIndexOf(".");
            var format = fileformat.substring(position + 1, fileformat.length)
            format = format.toLowerCase();
            if (format == "doc" | format == "docx" | format == "txt" | format == "pdf" | format == "xls" | format == "xlsx") {
                return true;
            }
            else {

                CreatState(imgFile, "请上传doc,docx,txt,pdf,xls,xlsx格式的文件!");
                return false;
            }
        }

    }
}
//43.判断下拉框是否进行了选择
function SelectIsNull(s) {
    ClearState(s);
    for (var i = 0; i < s.length; i++) {
        if (s[i].selected == true) {
            if (trim(s[i].innerText) == "-----请选择-----") {
                CreatState(s, "");
            }
        }
    }

}
//43.判断下拉框是否进行了选择
function SelectIsNull2(s) {
    ClearState(s);
    for (var i = 0; i < s.length; i++) {
        if (s[i].selected == true) {
            if (trim(s[i].value) == -1) {
                CreatState(s, "");
            }
        }
    }

}
function SelectIsCode(s) {
    ClearState(s);
    for (var i = 0; i < s.length; i++) {
        if (s[i].selected == true) {
            if (trim(s[i].innerText) == "市辖区") {
                CreatState(s, "");
            }
        }
    }

}
//44.判断checkbox是否有选中项，必选
//45.为输入框加提示
function AddMsg(s) {
    s.parentNode.getElementsByTagName("span")[0].style.display = "inline";
}
function ClearMsg(s) {
    s.parentNode.getElementsByTagName("span")[0].style.display = "none";
}
