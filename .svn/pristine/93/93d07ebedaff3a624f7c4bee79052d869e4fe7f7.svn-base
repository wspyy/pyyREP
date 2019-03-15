/*ChinaAir.cs
  ChainaAirQuality.html--Graphics_color
*/

function PointAQIDisplay(aqi) {
    
    var ColorCode;
    ColorCode = getAqiColor(aqi);

    return ColorCode;
}



var C1 = parseInt("66CC00",16);//绿色
var C2 = parseInt("FBD12A",16);//黄色
var C3 = parseInt("FFA641",16);//橙色
var C4 = parseInt("EB5B13",16);//橘红色
var C5 = parseInt("960453",16);
var C6 = parseInt("580422",16);
var C0 = parseInt("FFFFFF",16);

//var C1 = 6736896;//#66CC00
//var C2 = 16503082;//#FBD12A
//var C3 = 16754241;//#FFA641
//var C4 = 15424275;//#EB5B13
//var C5 = 9831507;//#960453
//var C6 = 5768226;//#580422
//var C0 = 16777215;//#FFFFFF



function getAqiColor(monitorValue) {
    if (monitorValue >= 0 && monitorValue <= 50) {
        return C1;
    }
    else if (monitorValue > 50 && monitorValue <= 100) {

        return C2;
    }
    else if (monitorValue > 100 && monitorValue <= 150) {

        return C3;
    }
    else if (monitorValue > 150 && monitorValue <= 200) {

        return C4;
    }
    else if (monitorValue > 200 && monitorValue <= 300) {

        return C5;
    }
    else if (monitorValue > 300) {

        return C6;
    }
    else {

        return C0;
    }
}
var l1 = "优";
var l2 = "良";
var l3 = "轻度污染";
var l4 = "中度污染";
var l5 = "重度污染";
var l6 = "严重污染";
function AirLevelAQI(aqi) {

    if (aqi >= 0 && aqi <= 50) {
        return l1;
    }
    else if (aqi > 50 && aqi <= 100) {

        return l2;
    }
    else if (aqi > 100 && aqi <= 150) {

        return l3;
    }
    else if (aqi > 150 && aqi <= 200) {

        return l4;
    }
    else if (aqi > 200 && aqi <= 300) {

        return l5;
    }
    else if (aqi > 300) {

        return l6;
    }
    else {

        return 0;
    }
}


function AirQuality(airlevel) {
    var levlel;
    if (airlevel==1) {
        return l1;
    }
    else if (airlevel == 2) {

        return l2;
    }
    else if (airlevel == 3) {

        return l3;
    }
    else if (airlevel == 4) {

        return l4;
    }
    else if (airlevel == 5) {

        return l5;
    }
    else if (airlevel == 6) {

        return l6;
    }
    else {

        return 0;
    }
    //switch (airlevel) {
    //    case 1:
    //        {
    //           level =l1;
    //            break;
    //        }
    //    case 2:
    //        {
    //            level = l2;
    //            break;
    //        }
    //    case 3:
    //        {
    //            level = l3;
    //            break;
    //        }
    //    case 4:
    //        {
    //            level = l4;
    //            break;
    //        }
    //    case 5:
    //        {
    //            level = l5;
    //            break;
    //        }
    //    case 6:
    //        {
    //            level = l6;
    //            break;
    //        }       
    //    default:
    //        {
    //            break;
    //        }
    //}
    //return levlel;
}