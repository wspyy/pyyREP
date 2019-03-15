/*MapPointAQIDisplayOption.cs
  AirForeCastSSCI.html--show_table_color
*/

function MapPointAQIDisplayOption( dataType, monitorValue)
        {
            var DataType = dataType;
            var MonitorValue = monitorValue;
            var ColorCode="";

            switch (DataType)
            {
                case "AQI":
                    {
                        ColorCode = getAqiColor(monitorValue);
                        break;
                    }
                case "SO2":
                    {
                        ColorCode = getSo2Color(monitorValue);
                        break;
                    }
                case "NO2":
                    {
                        ColorCode = getNo2Color(monitorValue);
                        break;
                    }
                case "O3":
                    {
                        ColorCode = getO3Color(monitorValue);
                        break;
                    }
                case "PM10":
                    {
                        ColorCode = getPm10Color(monitorValue);
                        break;
                    }
                case "PM25":
                    {
                        ColorCode = getPm2_5Color(monitorValue);
                        break;
                    }
                case "CO":
                    {
                        ColorCode = getCoColor(monitorValue);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return ColorCode;
}

/*var C1_1 = "#66CC00";
var C2_2 = "#FBD12A";
var C3_3 = "#FFA641";
var C4_4 = "#EB5B13";
var C5_5 = "#960453";
var C6_6 = "#580422";
var C0_0 = "#FFFFFF";

var C1 = "<style>table td{background-color: " + C1_1 + "}</style>";
var C2 = "<style>table td{background-color: " + C2_2 + "}</style>";
var C3 = "<style>table td{background-color: " + C3_3 + "}</style>";
var C4 = "<style>table td{background-color: " + C4_4 + "}</style>";
var C5 = "<style>table td{background-color: " + C5_5 + "}</style>";
var C6 = "<style>table td{background-color: " + C6_6 + "}</style>";
var C0 = "<style>table td{background-color: " + C0_0 + "}</style>";
*/

var C1 = "#66CC00";
var C2 = "#FBD12A";
var C3 = "#FFA641";
var C4 = "#EB5B13";
var C5 = "#960453";
var C6 = "#580422";
var C0 = "#FFFFFF";

function getCoColor(monitorValue)
{
    if (monitorValue >= 0 && monitorValue <= 5)
    {
        return C1;
    }
    else if (monitorValue > 5 && monitorValue <= 10)
    {
        return C2;
    }
    else if (monitorValue > 10 && monitorValue <= 35)
    {
        return C3;
    }
    else if (monitorValue > 35 && monitorValue <= 60)
    {
        return C4;
    }
    else if (monitorValue > 60 && monitorValue <= 90)
    {
        return C5;
    }
    else if (monitorValue > 90)
    {
        return C6;
    }
    else
    {
        return C0;
    }
}

function getPm2_5Color(monitorValue)
{
    if (monitorValue >= 0 && monitorValue <= 35)
    {
        return C1;
    }
    else if (monitorValue > 35 && monitorValue <= 75)
    {
        return C2;
    }
    else if (monitorValue > 75 && monitorValue <= 115)
    {
        return C3;
    }
    else if (monitorValue > 115 && monitorValue <= 150)
    {
        return C4;
    }
    else if (monitorValue > 150 && monitorValue <= 250)
    {
        return C5;
    }
    else if (monitorValue > 250)
    {
        return C6;
    }
    else
    {
        return C0;
    }
}

function getO3Color(monitorValue)
{
    if (monitorValue >= 0 && monitorValue <= 160)
    {
        return C1;
    }
    else if (monitorValue > 160 && monitorValue <= 200)
    {
        return C2;
    }
    else if (monitorValue > 200 && monitorValue <= 300)
    {
        return C3;
    }
    else if (monitorValue > 300 && monitorValue <= 400)
    {
        return C4;
    }
    else if (monitorValue > 400 && monitorValue <= 800)
    {
        return C5;
    }
    else if (monitorValue > 800)
    {
        return C6;
    }
    else
    {
        return C0;
    }
}

function getPm10Color(monitorValue)
{
    if (monitorValue >= 0 && monitorValue <= 50)
    {
        return C1;
    }
    else if (monitorValue > 50 && monitorValue <= 150)
    {
        return C2;
    }
    else if (monitorValue > 150 && monitorValue <= 250)
    {
        return C3;
    }
    else if (monitorValue > 250 && monitorValue <= 350)
    {
        return C4;
    }
    else if (monitorValue > 350 && monitorValue <= 420)
    {
        return C5;
    }
    else if (monitorValue > 420)
    {
        return C6;
    }
    else
    {
        return C0;
    }
}

function getNo2Color(monitorValue)
{
    if (monitorValue >= 0 && monitorValue <= 100)
    {
        return C1;
    }
    else if (monitorValue > 100 && monitorValue <= 200)
    {
        return C2;
    }
    else if (monitorValue > 200 && monitorValue <= 700)
    {
        return C3;
    }
    else if (monitorValue > 700 && monitorValue <= 1200)
    {
        return C4;
    }
    else if (monitorValue > 1200 && monitorValue <= 2340)
    {
        return C5;
    }
    else if (monitorValue > 2340)
    {
        return C6;
    }
    else
    {
        return C0;
    }
}

function getSo2Color(monitorValue)
{
    if (monitorValue >= 0 && monitorValue <= 150)
    {
        return C1;
    }
    else if (monitorValue > 150 && monitorValue <= 500)
    {
        return C2;
    }
    else if (monitorValue > 500 && monitorValue <= 650)
    {
        return C3;
    }
    else if (monitorValue > 650 && monitorValue <= 800)
    {
        return C4;
    }
    else
    {
        return C0;
    }
}

function getAqiColor(monitorValue)
{
    if (monitorValue >= 0 && monitorValue <= 50)
    {
        return C1;
    }
    else if (monitorValue > 50 && monitorValue <= 100)
    {

        return C2;
    }
    else if (monitorValue > 100 && monitorValue <= 150)
    {

        return C3;
    }
    else if (monitorValue > 150 && monitorValue <= 200)
    {

        return C4;
    }
    else if (monitorValue > 200 && monitorValue <= 300)
    {

        return C5;
    }
    else if (monitorValue > 300)
    {

        return C6;
    }
    else
    {

        return C0;
    }
}
      
