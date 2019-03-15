<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SingleTableTree.aspx.cs"
    Inherits="Framework_SingleTableTree" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Controls/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Controls/zTree/css/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="container-fluid">
        <div class="row-fluid">
            <fieldset>
                <legend>接口清单</legend>
               <%-- <input type="text" onblur="bindList();" onkeypress="bindList();" onkeydown="bindList();" id="txtQuery" placeholder="查询节点……" />--%>
<%--                <input type="text" onblur="bindList();" onkeypress="bindList();" onkeydown="bindList();"
                    id="txtQuery" placeholder="查询节点……" />--%>
                <ul id="tableList" class="ztree">
                </ul>
            </fieldset>
        </div>
    </div>
    <form id="form1" runat="server">
    </form>
    <script src="../Scripts/jquery-1.7.2.js" type="text/javascript"></script>
    <script src="../Controls/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../Controls/zTree/js/jquery.ztree.core-3.5.min.js" type="text/javascript"></script>
    <script src="../Controls/zTree/js/jquery.ztree.excheck-3.5.min.js" type="text/javascript"></script>
    <script src="../Controls/zTree/js/jquery.ztree.exedit-3.5.js" type="text/javascript"></script>
    <script src="javascript/SingleTableTree.js" type="text/javascript"></script>
</body>
</html>
