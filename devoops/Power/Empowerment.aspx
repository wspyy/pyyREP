<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Empowerment.aspx.cs" Inherits="Framework_RightsManagement_Empowerment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>赋权</title>
    <link href="../Controls/zTree/css/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="row">
        <div class="col-xs-12">
            <div class="box">
                <div class="box-header">
                    <div class="box-name">
                        <i class="fa fa-th"></i>&nbsp;&nbsp;赋权管理
                    </div>
                    <div class="box-icons">
                        <a class="collapse-link">
                            <i class="fa fa-chevron-up"></i>
                        </a>
                        <a class="expand-link">
                            <i class="fa fa-expand"></i>
                        </a>
                        <a class="close-link">
                            <i class="fa fa-times"></i>
                        </a>
                    </div>
                    <div class="no-move"></div>
                </div>
                <div class="box-content">
                    <div class="form-actions top ">
                        <button class="btn btn-primary btn-label-left" onclick="saveRights();" type="button"><span><i class="fa fa-clock-o"></i></span>保 存</button>

                    </div>
                    <div class="row">
                        <div class="col-sm-3">
                            <ul id="treeList" class="ztree">
                            </ul>
                        </div>
                        <div class="col-sm-9">
                            <ul id="treeModule" class="ztree">
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="../Controls/zTree/js/jquery.ztree.core-3.5.min.js" type="text/javascript"></script>
    <script src="../Controls/zTree/js/jquery.ztree.excheck-3.5.min.js" type="text/javascript"></script>
    <script src="../Controls/zTree/js/jquery.ztree.exedit-3.5.js" type="text/javascript"></script>
    <script src="js/Empowerment.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //$("#tableTree").height(document.documentElement.clientHeight - 150);
            bindList();
            bindModule("");
        });
    </script>
</body>
</html>
